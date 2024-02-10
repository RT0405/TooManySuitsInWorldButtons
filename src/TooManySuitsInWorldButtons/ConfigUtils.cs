using LethalConfig;
using LethalConfig.ConfigItems.Options;
using LethalConfig.ConfigItems;
using BepInEx.Configuration;
using System;

namespace RT0405.TooManySuitsInWorldButtons;

public static class ConfigUtils
{
    public static ConfigEntry<bool> BindCheckBox(this ConfigFile config, string catagory, string name, bool defaultValue, string description, Action<bool> settingChangedCallback = null, bool requiresRestart = false, BaseOptions.CanModifyDelegate canModifyCallback = null)
    {
        ConfigEntry<bool> configEntry = config.Bind(catagory, name, defaultValue, description);
        BoolCheckBoxConfigItem configItem = new(configEntry, new BoolCheckBoxOptions
        {
            RequiresRestart = requiresRestart,
            CanModifyCallback = canModifyCallback,
        });
        LethalConfigManager.AddConfigItem(configItem);
        if (settingChangedCallback != null)
            configEntry.SettingChanged += (_, _) => settingChangedCallback.Invoke(configEntry.Value);
        return configEntry;
    }
    public static ConfigEntry<int> BindIntInputField(this ConfigFile config, string catagory, string name, int defaultValue, string description, int min = int.MinValue, int max = int.MaxValue, Action<int> settingChangedCallback = null, bool requiresRestart = false, BaseOptions.CanModifyDelegate canModifyCallback = null)
    {
        ConfigEntry<int> configEntry = config.Bind(catagory, name, defaultValue, description);
        IntInputFieldConfigItem configItem = new(configEntry, new IntInputFieldOptions
        {
            Min = min,
            Max = max,
            RequiresRestart = requiresRestart, 
            CanModifyCallback = canModifyCallback,
        });
        LethalConfigManager.AddConfigItem(configItem);
        if (settingChangedCallback != null)
            configEntry.SettingChanged += (_, _) => settingChangedCallback.Invoke(configEntry.Value);
        return configEntry;
    }
    public static ConfigEntry<float> BindFloatInputField(this ConfigFile config, string catagory, string name, float defaultValue, string description, float min = float.MinValue, float max = float.MaxValue, Action<float> settingChangedCallback = null, bool requiresRestart = false, BaseOptions.CanModifyDelegate canModifyCallback = null)
    {
        ConfigEntry<float> configEntry = config.Bind(catagory, name, defaultValue, description);
        FloatInputFieldConfigItem configItem = new(configEntry, new FloatInputFieldOptions
        {
            Min = min,
            Max = max,
            RequiresRestart = requiresRestart, 
            CanModifyCallback = canModifyCallback,
        });
        LethalConfigManager.AddConfigItem(configItem);
        if (settingChangedCallback != null)
            configEntry.SettingChanged += (_, _) => settingChangedCallback.Invoke(configEntry.Value);
        return configEntry;
    }
    public static ConfigEntry<string> BindTextInputField(this ConfigFile config, string catagory, string name, string defaultValue, string description, int charLimit = int.MaxValue, Action<string> settingChangedCallback = null, bool requiresRestart = false, BaseOptions.CanModifyDelegate canModifyCallback = null)
    {
        ConfigEntry<string> configEntry = config.Bind(catagory, name, defaultValue, description);
        TextInputFieldConfigItem configItem = new(configEntry, new TextInputFieldOptions
        { 
            CharacterLimit = charLimit,
            RequiresRestart = requiresRestart,  
            CanModifyCallback = canModifyCallback, 
        });
        LethalConfigManager.AddConfigItem(configItem);
        if (settingChangedCallback != null)
            configEntry.SettingChanged += (_, _) => settingChangedCallback.Invoke(configEntry.Value);
        return configEntry;
    }
    public static ConfigEntry<T> BindEnumDropdown<T>(this ConfigFile config, string catagory, string name, T defaultValue, string description, Action<T> settingChangedCallback = null, bool requiresRestart = false, BaseOptions.CanModifyDelegate canModifyCallback = null) where T : Enum
    {
        ConfigEntry<T> configEntry = config.Bind(catagory, name, defaultValue, description);
        EnumDropDownConfigItem<T> configItem = new(configEntry, new EnumDropDownOptions
        {
            RequiresRestart = requiresRestart,  
            CanModifyCallback = canModifyCallback, 
        });
        LethalConfigManager.AddConfigItem(configItem);
        if (settingChangedCallback != null)
            configEntry.SettingChanged += (_, _) => settingChangedCallback.Invoke(configEntry.Value);
        return configEntry;
    }
    public static ConfigEntry<int> BindIntSlider(this ConfigFile config, string catagory, string name, int defaultValue, string description, int min, int max, Action<int> settingChangedCallback = null, bool requiresRestart = false, BaseOptions.CanModifyDelegate canModifyCallback = null)
    {
        ConfigEntry<int> configEntry = config.Bind(catagory, name, defaultValue, description);
        IntSliderConfigItem configItem = new(configEntry, new IntSliderOptions
        {
            Min = min,
            Max = max,
            RequiresRestart = requiresRestart,
            CanModifyCallback = canModifyCallback,
        });
        LethalConfigManager.AddConfigItem(configItem);
        if (settingChangedCallback != null)
            configEntry.SettingChanged += (_, _) => settingChangedCallback.Invoke(configEntry.Value);
        return configEntry;
    }
    public static ConfigEntry<float> BindFloatSlider(this ConfigFile config, string catagory, string name, float defaultValue, string description, float min, float max, Action<float> settingChangedCallback = null, bool requiresRestart = false, BaseOptions.CanModifyDelegate canModifyCallback = null)
    {
        ConfigEntry<float> configEntry = config.Bind(catagory, name, defaultValue, description);
        FloatSliderConfigItem configItem = new(configEntry, new FloatSliderOptions
        {
            Min = min,
            Max = max,
            RequiresRestart = requiresRestart,
            CanModifyCallback = canModifyCallback,
        });
        LethalConfigManager.AddConfigItem(configItem);
        if (settingChangedCallback != null)
            configEntry.SettingChanged += (_, _) => settingChangedCallback.Invoke(configEntry.Value);
        return configEntry;
    }
    public static ConfigEntry<float> BindFloatStepSlider(this ConfigFile config, string catagory, string name, float defaultValue, string description, float min, float max, float step, Action<float> settingChangedCallback = null, bool requiresRestart = false, BaseOptions.CanModifyDelegate canModifyCallback = null)
    {
        ConfigEntry<float> configEntry = config.Bind(catagory, name, defaultValue, description);
        FloatStepSliderConfigItem configItem = new(configEntry, new FloatStepSliderOptions
        {
            Min = min, 
            Max = max,
            Step = step,
            RequiresRestart = requiresRestart,
            CanModifyCallback = canModifyCallback,
        });
        LethalConfigManager.AddConfigItem(configItem);
        if (settingChangedCallback != null)
            configEntry.SettingChanged += (_, _) => settingChangedCallback.Invoke(configEntry.Value);
        return configEntry; 
    }
    public static void BindGenericButton(string catagory, string name, string description, string buttonText, GenericButtonOptions.GenericButtonHandler buttonPressCallback)
    {
        GenericButtonConfigItem configItem = new(catagory, name, description, buttonText, buttonPressCallback);
        LethalConfigManager.AddConfigItem(configItem);
    }
}