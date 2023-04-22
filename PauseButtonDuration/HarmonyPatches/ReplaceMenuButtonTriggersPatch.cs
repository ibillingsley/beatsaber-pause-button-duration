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
using System.Runtime.CompilerServices;
using HarmonyLib;
using PauseButtonDuration.MenuButtonTriggers;
using Zenject;

namespace PauseButtonDuration.HarmonyPatches
{
    [HarmonyPatch(typeof(GameCoreSceneSetup))]
    [HarmonyPatch(nameof(GameCoreSceneSetup.InstallBindings), MethodType.Normal)]
    public class ReplaceMenuButtonTriggersPatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(MonoInstallerBase))]
        [HarmonyPatch("Container", MethodType.Getter)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static DiContainer BaseContainer(GameCoreSceneSetup _)
        {
            return null; // Will be replaced by Harmony with 'Container' in 'MonoInstallerBase'.
        }

        public static bool Prefix(GameCoreSceneSetup __instance)
        {
            bool enabled = Configuration.PluginConfig.Instance.Enabled;
            if (enabled)
            {
                int requiredTapAmount = Configuration.PluginConfig.Instance.RequiredTapAmount;
                float requiredPressDuration = Configuration.PluginConfig.Instance.RequiredPressDuration;
                float multiTapTimeout = Configuration.PluginConfig.Instance.MultiTapTimeout;
                bool requiresDualPress = Configuration.PluginConfig.Instance.RequiresDualPress;
                bool requiresTriggerPress = Configuration.PluginConfig.Instance.RequiresTriggerPress;

                // Fetch a few protected fields through reflection.
                ScreenCaptureAfterDelay screenCaptureAfterDelayPrefab = (ScreenCaptureAfterDelay)AccessTools.Field(typeof(GameCoreSceneSetup), "_screenCaptureAfterDelayPrefab").GetValue(__instance);
                MainSettingsModelSO mainSettingsModel = (MainSettingsModelSO)AccessTools.Field(typeof(GameCoreSceneSetup), "_mainSettingsModel").GetValue(__instance);
                BloomFogSO bloomFog = (BloomFogSO)AccessTools.Field(typeof(GameCoreSceneSetup), "_bloomFog").GetValue(__instance);

                // Setup the configurable menu button trigger with the configured properties
                BaseContainer(__instance).Bind<int>().WithId("RequiredTapAmount").FromInstance(requiredTapAmount).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                BaseContainer(__instance).Bind<float>().WithId("RequiredPressDuration").FromInstance(requiredPressDuration).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                BaseContainer(__instance).Bind<float>().WithId("MultiTapTimeout").FromInstance(multiTapTimeout).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                BaseContainer(__instance).Bind<bool>().WithId("RequiresDualPress").FromInstance(requiresDualPress).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                BaseContainer(__instance).Bind<bool>().WithId("RequiresTriggerPress").FromInstance(requiresTriggerPress).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                BaseContainer(__instance).Bind(new Type[] { typeof(ITickable), typeof(IMenuButtonTrigger) }).To<ConfigurableMenuButtonTrigger>().AsSingle();

                // Remaining initialization which is performed as normal.
                BaseContainer(__instance).Bind<BloomFogSO>().FromScriptableObject(bloomFog).AsSingle();
                BaseContainer(__instance).Bind<NoteCutter>().AsSingle();
                if (mainSettingsModel.createScreenshotDuringTheGame)
                {
                    BaseContainer(__instance).Bind<ScreenCaptureAfterDelay.InitData>().FromInstance(new ScreenCaptureAfterDelay.InitData(ScreenCaptureCache.ScreenshotType.Game, 5f, 1920, 1080));
                    BaseContainer(__instance).Bind<ScreenCaptureAfterDelay>().FromComponentInNewPrefab(screenCaptureAfterDelayPrefab).AsSingle().NonLazy();
                }
            }
            return !enabled;
        }
    }
}
