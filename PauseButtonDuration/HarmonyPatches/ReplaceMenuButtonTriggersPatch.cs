using PauseButtonDuration.MenuButtonTriggers;
using HarmonyLib;
using System;
using System.Runtime.CompilerServices;
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
			uint pauseButtonMode = Configuration.PluginConfig.Instance.PauseButtonMode;
			uint requiredPressAmount = Configuration.PluginConfig.Instance.RequiredPressAmount;
			float requiredPressDuration = Configuration.PluginConfig.Instance.RequiredPressDuration;

			// Fetch a few protected fields through reflection.
			ScreenCaptureAfterDelay screenCaptureAfterDelayPrefab = (ScreenCaptureAfterDelay)AccessTools.Field(typeof(GameCoreSceneSetup), "_screenCaptureAfterDelayPrefab").GetValue(__instance);
			MainSettingsModelSO mainSettingsModel = (MainSettingsModelSO)AccessTools.Field(typeof(GameCoreSceneSetup), "_mainSettingsModel").GetValue(__instance);
			BloomFogSO bloomFog = (BloomFogSO)AccessTools.Field(typeof(GameCoreSceneSetup), "_bloomFog").GetValue(__instance);

			// Select a menu button trigger according to the mode (MultiPress/Delayed/DualHold).
			if (pauseButtonMode == 0)
			{
				BaseContainer(__instance).Bind<uint>().FromInstance(requiredPressAmount).WhenInjectedInto<MultiTapMenuButtonTrigger>();
				BaseContainer(__instance).Bind<float>().FromInstance(requiredPressDuration).WhenInjectedInto<MultiTapMenuButtonTrigger>();
				BaseContainer(__instance).Bind(new Type[]
				{
					typeof(ITickable),
					typeof(IMenuButtonTrigger)
				}).To<MultiTapMenuButtonTrigger>().AsSingle();
			}
			else if (pauseButtonMode == 1)
			{
				BaseContainer(__instance).Bind<float>().FromInstance(requiredPressDuration).WhenInjectedInto<DelayedMenuButtonTrigger>();
				BaseContainer(__instance).Bind(new Type[]
				{
				typeof(ITickable),
				typeof(IMenuButtonTrigger)
				}).To<DelayedMenuButtonTrigger>().AsSingle();
			}
			else if (pauseButtonMode == 2)
			{
				BaseContainer(__instance).Bind<float>().FromInstance(requiredPressDuration).WhenInjectedInto<DualPressMenuButtonTrigger>();
				BaseContainer(__instance).Bind(new Type[]
				{
				typeof(ITickable),
				typeof(IMenuButtonTrigger)
				}).To<DualPressMenuButtonTrigger>().AsSingle();
			}
			else if (pauseButtonMode == 3)
			{
				BaseContainer(__instance).Bind<float>().FromInstance(requiredPressDuration).WhenInjectedInto<TriggerAndMenuButtonTrigger>();
				BaseContainer(__instance).Bind(new Type[]
				{
				typeof(ITickable),
				typeof(IMenuButtonTrigger)
				}).To<TriggerAndMenuButtonTrigger>().AsSingle();
			}

			// Remaining initialization which is performed as normal.
			BaseContainer(__instance).Bind<BloomFogSO>().FromScriptableObject(bloomFog).AsSingle();
			BaseContainer(__instance).Bind<NoteCutter>().AsSingle();
			if (mainSettingsModel.createScreenshotDuringTheGame)
			{
				BaseContainer(__instance).Bind<ScreenCaptureAfterDelay.InitData>().FromInstance(new ScreenCaptureAfterDelay.InitData(ScreenCaptureCache.ScreenshotType.Game, 5f, 1920, 1080));
				BaseContainer(__instance).Bind<ScreenCaptureAfterDelay>().FromComponentInNewPrefab(screenCaptureAfterDelayPrefab).AsSingle().NonLazy();
			}

			return false;
		}
	}
}
