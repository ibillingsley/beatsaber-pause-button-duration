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
using System.Collections.Generic;
using System.Linq;

namespace PauseButtonDuration
{
    public enum PauseButtonMode : int
    {
        Custom,
        Instant,
        Short,
        Medium,
        Long,
        DoubleTap,
        DualPress,
        ButtonAndTrigger
    }

    public class PauseButtonModePreset
    {
        public PauseButtonMode PauseButtonMode { get; }
        public String Name { get; }
        public String Description { get; }
        public int RequiredTapAmount { get; }
        public float RequiredPressDuration { get; }
        public float MultiTapTimeout { get; }
        public bool RequiresDualPress { get; }
        public bool RequiresTriggerPress { get; }

        public PauseButtonModePreset(
            PauseButtonMode pauseButtonMode,
            String name,
            String description,
            int requiredTapAmount,
            float requiredPressDuration,
            float multiTapTimeout,
            bool requiresDualPress,
            bool requiresTriggerPress
        )
        {
            PauseButtonMode = pauseButtonMode;
            Name = name;
            Description = description;
            RequiredTapAmount = requiredTapAmount;
            RequiredPressDuration = requiredPressDuration;
            MultiTapTimeout = multiTapTimeout;
            RequiresDualPress = requiresDualPress;
            RequiresTriggerPress = requiresTriggerPress;
        }

        public override String ToString() => Name;
    }

    public static class PauseButtonModePresets
    {
        public static readonly PauseButtonModePreset Custom = new PauseButtonModePreset(PauseButtonMode.Custom, "Custom", "Configure a custom mode to open the menu.", 1, 0.0F, 0.0F, false, false);
        public static readonly PauseButtonModePreset Instant = new PauseButtonModePreset(PauseButtonMode.Instant, "Instant", "Press pause to immediately open the menu.", 1, 0.0F, 0.0F, false, false);
        public static readonly PauseButtonModePreset Short = new PauseButtonModePreset(PauseButtonMode.Short, "Short", "Hold pause for a short amount of time (250 ms) to open the menu.", 1, 0.25F, 0.0F, false, false);
        public static readonly PauseButtonModePreset Medium = new PauseButtonModePreset(PauseButtonMode.Medium, "Medium", "Hold pause for a medium amount of time (500 ms) to open the menu.", 1, 0.5F, 0.0F, false, false);
        public static readonly PauseButtonModePreset Long = new PauseButtonModePreset(PauseButtonMode.Long, "Long", "Hold pause for a long amount of time (750 ms) to open the menu.", 1, 0.75F, 0.0F, false, false);
        public static readonly PauseButtonModePreset DoubleTap = new PauseButtonModePreset(PauseButtonMode.DoubleTap, "Double Tap", "Tap pause twice in quick succession to open the menu.", 2, 0.0F, 0.5F, false, false);
        public static readonly PauseButtonModePreset DualPress = new PauseButtonModePreset(PauseButtonMode.DualPress, "Dual Press", "Hold pause on both controllers to open the menu.", 1, 0.0F, 0.0F, true, false);
        public static readonly PauseButtonModePreset ButtonAndTrigger = new PauseButtonModePreset(PauseButtonMode.ButtonAndTrigger, "Button+Trigger", "Hold pause and press the trigger to open the menu.", 1, 0.0F, 0.0F, false, true);

        public static readonly IList<PauseButtonModePreset> Values = Array.AsReadOnly(new PauseButtonModePreset[] {
            Custom,
            Instant,
            Short,
            Medium,
            Long,
            DoubleTap,
            DualPress,
            ButtonAndTrigger
        });

        public static PauseButtonModePreset FindByMode(PauseButtonMode mode) => Values.First(m => m.PauseButtonMode == mode);
    }
}
