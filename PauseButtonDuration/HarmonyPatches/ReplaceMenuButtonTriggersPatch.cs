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
using PauseButtonDuration.Configuration;
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

        public static void Postfix(GameCoreSceneSetup __instance)
        {
            if (PluginConfig.Instance.Enabled)
            {
                int requiredTapAmount = PluginConfig.Instance.RequiredTapAmount;
                float requiredPressDuration = PluginConfig.Instance.RequiredPressDuration;
                float multiTapTimeout = PluginConfig.Instance.MultiTapTimeout;
                bool requiresDualPress = PluginConfig.Instance.RequiresDualPress;
                bool requiresTriggerPress = PluginConfig.Instance.RequiresTriggerPress;

                // Setup the configurable menu button trigger with the configured properties
                DiContainer container = BaseContainer(__instance);
                container.Bind<int>().WithId("RequiredTapAmount").FromInstance(requiredTapAmount).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                container.Bind<float>().WithId("RequiredPressDuration").FromInstance(requiredPressDuration).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                container.Bind<float>().WithId("MultiTapTimeout").FromInstance(multiTapTimeout).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                container.Bind<bool>().WithId("RequiresDualPress").FromInstance(requiresDualPress).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                container.Bind<bool>().WithId("RequiresTriggerPress").FromInstance(requiresTriggerPress).WhenInjectedInto<ConfigurableMenuButtonTrigger>();
                container.Unbind(typeof(IMenuButtonTrigger));
                container.Bind(typeof(ITickable), typeof(IMenuButtonTrigger)).To<ConfigurableMenuButtonTrigger>().AsSingle();
            }
        }
    }
}
