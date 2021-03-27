/*
 * This file is part of BeatSaber-PauseButtonDuration.
 * Copyright (c) 2021 Bart Toersche
 * 
 * BeatSaber-PauseButtonDuration is licensed under a MIT License (MIT).
 * 
 * You should have received a copy of the MIT License along with
 * BeatSaber-PauseButtonDuration. If not, see <https://opensource.org/licenses/MIT>.
 */

using IPA.Config.Stores;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace PauseButtonDuration.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        // Config properties need to be 'virtual' for BSIPA to detect a value change and save the config automatically.
        public virtual bool Enabled { get; set; } = true;

        private int _pauseButtonMode = (int)PauseButtonDuration.PauseButtonMode.Instant;
        public virtual int PauseButtonMode {
            get => _pauseButtonMode;
            set => _pauseButtonMode = Enum.IsDefined(typeof(PauseButtonMode), value) ? value : (int)PauseButtonDuration.PauseButtonMode.Instant;
        }

        private int _requiredTapAmount = 1;
        public virtual int RequiredTapAmount {
            get => _requiredTapAmount;
            set => _requiredTapAmount = Math.Max(1, value);
        }

        private float _requiredPressDuration = 0.0F;
        public virtual float RequiredPressDuration {
            get => _requiredPressDuration;
            set => _requiredPressDuration = Math.Max(0.0F, value);
        }

        private float _multiTapTimeout = 0.5F;
        public virtual float MultiTapTimeout
        {
            get => _multiTapTimeout;
            set => _multiTapTimeout = Math.Max(0.0F, value);
        }

        public virtual bool RequiresDualPress { get; set; } = false;

        public virtual bool RequiresTriggerPress { get; set; } = false;
    }
}
