/*
 * This file is part of BeatSaber-PauseButtonDuration.
 * Copyright (c) 2021 Bart Toersche
 *
 * BeatSaber-PauseButtonDuration is licensed under a MIT License (MIT).
 *
 * You should have received a copy of the MIT License along with
 * BeatSaber-PauseButtonDuration. If not, see <https://opensource.org/licenses/MIT>.
 */

using System;
using System.Reflection;
using BeatSaberMarkupLanguage.Settings;
using IPA;
using IPA.Config.Stores;
using PauseButtonDuration.UI;
using IPALogger = IPA.Logging.Logger;

namespace PauseButtonDuration
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public const string HarmonyId = "com.github.btoersche.beatsaber.PauseButtonDuration";
        internal static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(HarmonyId);
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        public void InitWithConfig(IPALogger logger, IPA.Config.Config config)
        {
            Instance = this;
            Log = logger;
            Log.Debug("PauseButtonDuration initialized.");

            Configuration.PluginConfig.Instance = config.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded.");
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("Applying Harmony patches.");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            BSMLSettings.instance.AddSettingsMenu("<size=85%>Pause Button Duration</size>", "PauseButtonDuration.UI.Settings.bsml", SettingsController.instance);
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("Removing Harmony patches.");
            harmony.UnpatchSelf();
        }
    }
}
