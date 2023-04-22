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
using UnityEngine;
using IPA;
using IPA.Config.Stores;
using PauseButtonDuration.UI;
using IPALogger = IPA.Logging.Logger;

namespace PauseButtonDuration
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public const string HarmonyId = "com.github.btoersche.beatsaber.PauseButtonDuration";
        internal static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(HarmonyId);
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static PauseButtonDurationController PluginController { get { return PauseButtonDurationController.Instance; } }

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public Plugin(IPALogger logger)
        {
            Instance = this;
            Plugin.Log = logger;
            Plugin.Log?.Debug("Logger initialized.");
        }

        #region BSIPA Config
        [Init]
        public void InitWithConfig(IPA.Config.Config config)
        {
            Configuration.PluginConfig.Instance = config.Generated<Configuration.PluginConfig>();
            Plugin.Log?.Debug("Config loaded.");

            BSMLSettings.instance.AddSettingsMenu("<size=85%>Pause Button Duration</size>", "PauseButtonDuration.UI.Settings.bsml", SettingsController.instance);
        }
        #endregion


        #region Disableable
        /// <summary>
        /// Called when the plugin is enabled (including when the game starts if the plugin is enabled).
        /// </summary>
        [OnEnable]
        public void OnEnable()
        {
            new GameObject("PauseButtonDurationController").AddComponent<PauseButtonDurationController>();
            ApplyHarmonyPatches();
        }

        /// <summary>
        /// Called when the plugin is disabled and on Beat Saber quit. It is important to clean up any Harmony patches, GameObjects, and Monobehaviours here.
        /// The game should be left in a state as if the plugin was never started.
        /// Methods marked [OnDisable] must return void or Task.
        /// </summary>
        [OnDisable]
        public void OnDisable()
        {
            if (PluginController != null)
            {
                GameObject.Destroy(PluginController);
            }
            RemoveHarmonyPatches();
        }
        #endregion

        #region Harmony
        /// <summary>
        /// Attempts to apply all the Harmony patches in this assembly.
        /// </summary>
        internal static void ApplyHarmonyPatches()
        {
            try
            {
                Plugin.Log?.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error applying Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }

        /// <summary>
        /// Attempts to remove all the Harmony patches that used our HarmonyId.
        /// </summary>
        internal static void RemoveHarmonyPatches()
        {
            try
            {
                Plugin.Log?.Debug("Removing Harmony patches.");
                harmony.UnpatchSelf(); // Removes all patches with this HarmonyId
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error removing Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }
        #endregion
    }
}
