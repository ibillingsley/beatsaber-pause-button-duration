using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using PauseButtonDuration.Configuration;

namespace PauseButtonDuration.UI
{
    internal class SettingsController : NotifiableSingleton<SettingsController>
    {
        [UIValue("enabled")]
        private bool Enabled
        {
            get => PluginConfig.Instance.Enabled;
            set => PluginConfig.Instance.Enabled = value;
        }

        [UIValue("presets")]
        private List<object> Presets = PauseButtonModePresets.Values.Cast<object>().ToList();

        [UIValue("preset")]
        private PauseButtonModePreset Preset
        {
            get => PauseButtonModePresets.FindByMode((PauseButtonMode)PluginConfig.Instance.PauseButtonMode);
            set => PluginConfig.Instance.PauseButtonMode = (int)value.PauseButtonMode;
        }

        [UIValue("presetHint")]
        public virtual string PresetHint
        {
            get => Preset.Description;
        }

        [UIAction("onPresetChange")]
        private void OnPresetChange(PauseButtonModePreset value)
        {
            Preset = value;
            NotifyPropertyChanged(nameof(PresetHint));
            if (value.PauseButtonMode != PauseButtonMode.Custom)
            {
                RequiredTapAmount = value.RequiredTapAmount;
                RequiredPressDuration = value.RequiredPressDuration;
                MultiTapTimeout = value.MultiTapTimeout;
                RequiresDualPress = value.RequiresDualPress;
                RequiresTriggerPress = value.RequiresTriggerPress;
                NotifyPropertyChanged(nameof(RequiredTapAmount));
                NotifyPropertyChanged(nameof(RequiredPressDuration));
                NotifyPropertyChanged(nameof(MultiTapTimeout));
                NotifyPropertyChanged(nameof(RequiresDualPress));
                NotifyPropertyChanged(nameof(RequiresTriggerPress));
            }
        }

        [UIValue("requiredTapAmount")]
        private int RequiredTapAmount
        {
            get => PluginConfig.Instance.RequiredTapAmount;
            set => PluginConfig.Instance.RequiredTapAmount = value;
        }

        [UIValue("requiredPressDuration")]
        private float RequiredPressDuration
        {
            get => PluginConfig.Instance.RequiredPressDuration;
            set => PluginConfig.Instance.RequiredPressDuration = value;
        }

        [UIValue("multiTapTimeout")]
        private float MultiTapTimeout
        {
            get => PluginConfig.Instance.MultiTapTimeout;
            set => PluginConfig.Instance.MultiTapTimeout = value;
        }

        [UIValue("requiresDualPress")]
        private bool RequiresDualPress
        {
            get => PluginConfig.Instance.RequiresDualPress;
            set => PluginConfig.Instance.RequiresDualPress = value;
        }

        [UIValue("requiresTriggerPress")]
        private bool RequiresTriggerPress
        {
            get => PluginConfig.Instance.RequiresTriggerPress;
            set => PluginConfig.Instance.RequiresTriggerPress = value;
        }
    }
}
