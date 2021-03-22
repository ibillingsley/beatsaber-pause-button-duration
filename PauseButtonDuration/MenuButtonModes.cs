using System;
using System.Collections.Generic;
using System.Linq;

namespace PauseButtonDuration
{
    public enum Mode
    {
        Instant,
        Short,
        Medium,
        Long,
        DoubleTap,
        DualPress,
        ButtonAndTrigger
    }

    public class MenuButtonMode
    {
        public Mode PauseButtonMode { get; }
        public String Name { get; }
        public String Description { get; }
        public uint RequiredPressAmount { get; }
        public float RequiredPressDuration { get; }

        public MenuButtonMode(Mode pauseButtonMode, String name, String description, uint requiredPressAmount, float requiredPressDuration)
        {
            PauseButtonMode = pauseButtonMode;
            Name = name;
            Description = description;
            RequiredPressAmount = requiredPressAmount;
            RequiredPressDuration = requiredPressDuration;
        }
    }

    public static class MenuButtonModes
    {
        public static readonly MenuButtonMode Instant = new MenuButtonMode(Mode.Instant, "Instant", "Press pause to immediately open the menu.", 1, 0.0f);
        public static readonly MenuButtonMode Short = new MenuButtonMode(Mode.Short, "Short", "Hold pause for a short amount of time (250 ms) to open the menu.", 1, 0.25f);
        public static readonly MenuButtonMode Medium = new MenuButtonMode(Mode.Medium, "Medium", "Hold pause for a medium amount of time (500 ms) to open the menu.", 1, 0.5f);
        public static readonly MenuButtonMode Long = new MenuButtonMode(Mode.Long, "Long", "Hold pause for a long amount of time (750 ms) to open the menu.", 1, 0.75f);
        public static readonly MenuButtonMode DoubleTap = new MenuButtonMode(Mode.DoubleTap, "Double Tap", "Tap pause twice in quick succession to open the menu.", 2, 0.0f);
        public static readonly MenuButtonMode DualPress = new MenuButtonMode(Mode.DualPress, "Dual Press", "Hold pause on both controllers to open the menu.", 1, 0.0f);
        public static readonly MenuButtonMode ButtonAndTrigger = new MenuButtonMode(Mode.ButtonAndTrigger, "Button+Trigger", "Hold pause and press the trigger to open the menu.", 1, 0.0f);

        public static readonly IList<MenuButtonMode> Values = Array.AsReadOnly(new MenuButtonMode[] {
            Instant,
            Short,
            Medium,
            Long,
            DoubleTap,
            DualPress,
            ButtonAndTrigger
        });

        public static MenuButtonMode FindByMode(Mode mode) => Values.First(m => m.PauseButtonMode == mode);
    }
}
