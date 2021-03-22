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

        private uint _pauseButtonMode = (uint)Mode.Instant;
        public virtual uint PauseButtonMode {
            get => _pauseButtonMode;
            set => _pauseButtonMode = Enum.IsDefined(typeof(Mode), value) ? value : (uint)Mode.Instant; }

        private uint _requiredPressAmount = 1;
        public virtual uint RequiredPressAmount {
            get => _requiredPressAmount;
            set => _requiredPressAmount = Math.Max(1, value);
        }

        private float _requiredPressDuration = 0.0f;
        public virtual float RequiredPressDuration {
            get => _requiredPressDuration;
            set => _requiredPressDuration = Math.Max(0.0f, value);
        }
    }
}
