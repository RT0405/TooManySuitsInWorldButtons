using BepInEx;
using LethalConfig;
using System;
using Unity.Netcode;
using UnityEngine;

namespace RT0405.TooManySuitsInWorldButtons;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
[BepInDependency("verity.TooManySuits")]
[BepInDependency("ainavt.lc.lethalconfig")]
public class TooManySuitsInWorldButtons : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "RT0405.TooManySuitsInWorldButtons";
    public const string PLUGIN_NAME = "TooManySuitsInWorldButtons";
    public const string PLUGIN_VERSION = "1.0.0";

    public bool EnableButtonsInNonVR;
    public TooManySuits.PluginLoader tooManySuitsPlugin;
    Transform leftButton;
    Transform rightButton;

    public void Awake()
    {
        Logger.LogError($"Plugin {PLUGIN_GUID} loading...");

        EnableButtonsInNonVR = Config.BindCheckBox("", "Enable suit cycle buttons in flatscreen mode", true, "",
            (b) =>
            {
                EnableButtonsInNonVR = b;
                if(leftButton != null)
                {
                    leftButton.gameObject.SetActive(b);
                    rightButton.gameObject.SetActive(b);
                }
            }).Value;

        gameObject.hideFlags = HideFlags.HideAndDontSave;

        Logger.LogError($"Plugin {PLUGIN_GUID} loaded!");
    }
    public void Update()
    {
        Logger.LogError("tick");
        Logger.LogError((leftButton == null).ToString()+(tooManySuitsPlugin == null).ToString()+(TooManySuits.Helper.LocalPlayer.localPlayer == null).ToString());
        if(leftButton != null && TooManySuits.Helper.LocalPlayer.localPlayer != null)
        {
            Logger.LogError((leftButton.position - TooManySuits.Helper.LocalPlayer.localPlayer.transform.position).ToString());
        }
        if(StartOfRound.Instance == null) return;
        if(leftButton == null)
        {
            if(tooManySuitsPlugin == null && 
                (tooManySuitsPlugin = FindObjectOfType<TooManySuits.PluginLoader>(true)) == null) return;
            leftButton = CreateTrigger(false).transform;
            rightButton = CreateTrigger(true).transform;
        }
    }
    private InteractTrigger CreateTrigger(bool isRight)
    {
        InteractTrigger trigger = new GameObject(isRight ? "RightSuitCycleButton" : "LeftSuitCycleButton", typeof(NetworkObject), typeof(InteractTrigger), typeof(BoxCollider)).GetComponent<InteractTrigger>();
        trigger.gameObject.tag = "InteractTrigger";
        trigger.gameObject.layer = LayerMask.NameToLayer("InteractableObject");
        Logger.LogWarning("InteractableObject Layer: " + (LayerMask.NameToLayer("InteractableObject")));
        trigger.interactable = true;
        trigger.oneHandedItemAllowed = true;
        trigger.timeToHold = 0.25f;
        trigger.hoverTip = isRight ? "Cycle Suits Right" : "Cycle Suits Left";
        trigger.onInteract.AddListener(
            (a) =>
            {
                if(!a.IsLocalPlayer) return;

                if(isRight)
                    tooManySuitsPlugin.MoveRightAction(default);
                else
                    tooManySuitsPlugin.MoveLeftAction(default);
            });
        trigger.transform.parent = TooManySuits.PluginLoader.Hooks.SuitPanel.transform;
        trigger.transform.SetLocalPositionAndRotation(new(isRight ? .1f : -.1f, 0, 0), Quaternion.identity);

        ScanNodeProperties scanNode = new GameObject("Scan Node", typeof(BoxCollider), typeof(ScanNodeProperties)).GetComponent<ScanNodeProperties>();
        scanNode.transform.parent = trigger.transform;
        scanNode.transform.localPosition = Vector3.zero;
        scanNode.gameObject.layer = LayerMask.NameToLayer("ScanNode");
        scanNode.headerText = "Suit Cycle Button";
        scanNode.requiresLineOfSight = false;
        scanNode.maxRange = 1000;
        Logger.LogWarning("ScanNode Layer: " + (LayerMask.NameToLayer("ScanNode")));

        return trigger;
    }
    private static class Hooks
    {
        public static bool Awake()
        {

        }
    }
}