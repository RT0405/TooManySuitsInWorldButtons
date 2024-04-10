using BepInEx;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace RT0405.TooManySuitsInWorldButtons;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
[BepInDependency("verity.TooManySuits")]
public class TooManySuitsInWorldButtons : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "RT0405.TooManySuitsInWorldButtons";
    public const string PLUGIN_NAME = "TooManySuitsInWorldButtons";
    public const string PLUGIN_VERSION = "1.0.2";

    private static Transform suitPanelTransform;
    private static bool recreateButtons;
    private static Transform leftButton;
    private static Transform rightButton;
    private static readonly Sprite emptySprite = Sprite.Create(new Rect(0, 0, 1, 1), Vector2.zero, 1);

    public void Awake()
    {
        Logger.LogInfo($"Plugin {PLUGIN_GUID} loading...");

        Harmony.CreateAndPatchAll(typeof(TooManySuitsInWorldButtons));

        Logger.LogInfo($"Plugin {PLUGIN_GUID} loaded!");
    }
    [HarmonyPatch(typeof(TooManySuits.PluginLoader), "Update")]
    [HarmonyPostfix]
    public static void Update(TooManySuits.PluginLoader __instance)
    {
        if(recreateButtons)
        {
            recreateButtons = false;
            leftButton = CreateTrigger(false, __instance).transform;
            rightButton = CreateTrigger(true, __instance).transform;
        }
        if(suitPanelTransform != null && leftButton != null)
        {
            Vector3 position = suitPanelTransform.position;
            Quaternion rotation = suitPanelTransform.rotation;
            const float offset = .9f;
            leftButton.transform.SetPositionAndRotation(position + rotation * new Vector3(-offset, 0, 0), rotation);
            rightButton.transform.SetPositionAndRotation(position + rotation * new Vector3(offset, 0, 0), rotation);

            bool active = suitPanelTransform.gameObject.activeInHierarchy;
            leftButton.gameObject.SetActive(active);
            rightButton.gameObject.SetActive(active);
        }
    }
    [HarmonyPatch(typeof(TooManySuits.PluginLoader.Hooks), nameof(TooManySuits.PluginLoader.Hooks.HookStartGame))]
    [HarmonyPostfix]
    private static void HookStartGame()
    {
        if(leftButton != null)
            Destroy(leftButton);
        if(rightButton != null)
            Destroy(rightButton);
        leftButton = rightButton = null;
        suitPanelTransform = TooManySuits.PluginLoader.Hooks.SuitPanel.GetComponentInChildren<Canvas>().transform;
        recreateButtons = true;
    }
    private static InteractTrigger CreateTrigger(bool isRight, TooManySuits.PluginLoader plugin)
    {
        InteractTrigger trigger = new GameObject(isRight ? "RightSuitCycleButton" : "LeftSuitCycleButton", typeof(NetworkObject), typeof(InteractTrigger), typeof(BoxCollider)).GetComponent<InteractTrigger>();
        trigger.gameObject.tag = "InteractTrigger";
        trigger.gameObject.layer = LayerMask.NameToLayer("InteractableObject");
        trigger.interactable = true;
        trigger.oneHandedItemAllowed = true;
        trigger.holdInteraction = false;
        trigger.hoverTip = isRight ? "Cycle Suits Right" : "Cycle Suits Left";
        trigger.interactCooldown = false;
        trigger.onInteract = new();
        trigger.onInteractEarly = new();
        trigger.onCancelAnimation = new();
        trigger.onStopInteract = new();
        trigger.holdingInteractEvent = new();
        trigger.onInteract.AddListener(
            (a) =>
            {
                if (a != TooManySuits.Helper.LocalPlayer.localPlayer) return;
                if (isRight)
                    plugin.MoveRightAction(default);
                else
                    plugin.MoveLeftAction(default);
            });
        trigger.hoverIcon = emptySprite;
        trigger.GetComponent<BoxCollider>().size = new(0.8f, 0.2f, 0.8f);

        ScanNodeProperties scanNode = new GameObject("Scan Node", typeof(BoxCollider), typeof(ScanNodeProperties)).GetComponent<ScanNodeProperties>();
        scanNode.transform.parent = trigger.transform;
        scanNode.transform.localPosition = Vector3.zero;
        scanNode.gameObject.layer = LayerMask.NameToLayer("ScanNode");
        scanNode.headerText = trigger.hoverTip;
        scanNode.requiresLineOfSight = true;
        scanNode.minRange = 1;
        scanNode.maxRange = 8;
        scanNode.GetComponent<BoxCollider>().size = new(0.1f, 0.1f, 0.1f);

        return trigger;
    }
}