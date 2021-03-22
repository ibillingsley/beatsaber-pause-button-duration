using PauseButtonDuration.Controllers;
using System;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

namespace PauseButtonDuration.MenuButtonTriggers
{
	public class MultiTapMenuButtonTrigger : ITickable, IMenuButtonTrigger
	{
		private class MenuButtonState
		{
			public uint CurrentPressAmount { get; set; } = 0;
			public float CurrentPressDuration { get; set; } = 0.0f;
			public bool MenuButtonPressed { get; set; } = false;
		}

		public event Action menuButtonTriggeredEvent;

		[Inject]
		protected VRControllersInputManager VRControllersInputManager;
		[Inject]
		protected uint RequiredPressAmount = 1;
		[Inject]
		protected float MultiPressTimeout = 0.5f; // Amount of time allowed between each button press.
		private readonly MenuButtonState _leftMenuButtonState = new MenuButtonState();
		private readonly MenuButtonState _rightMenuButtonState = new MenuButtonState();

		public virtual void Tick()
		{
			// The original input manager is wrapped in this project's input manager implementation.
			// This is required to distinguish between the left and right menu buttons.
			ExtendedVRControllersInputManager inputManager = new ExtendedVRControllersInputManager(VRControllersInputManager);
			bool leftMenuButtonHeld = inputManager.MenuButton(XRNode.LeftHand);
			bool rightMenuButtonHeld = inputManager.MenuButton(XRNode.RightHand);
			bool menuTriggered = UpdateMenuButtonState(_leftMenuButtonState, leftMenuButtonHeld, false);
			UpdateMenuButtonState(_rightMenuButtonState, rightMenuButtonHeld, menuTriggered);
		}

		private bool UpdateMenuButtonState(MenuButtonState state, bool menuButtonDown, bool menuTriggered)
        {
			// Updates the menu button press count.
			if (menuButtonDown && !state.MenuButtonPressed)
			{
				state.MenuButtonPressed = true;
				state.CurrentPressAmount += 1;
				state.CurrentPressDuration = 0.0f;
			}
			else if (!menuButtonDown)
			{
				state.MenuButtonPressed = false;
			}

			// Reset the amount of presses when too much time has elapsed between button presses.
			if (state.CurrentPressAmount > 0)
			{
				if (state.CurrentPressDuration > MultiPressTimeout)
				{
					state.CurrentPressAmount = 0;
					state.CurrentPressDuration = 0.0f;
				}
				else
				{
					state.CurrentPressDuration += Time.deltaTime;
				}
			}

			// Checks number of presses matches the required. If so, invokes the 'menuButtonTriggeredEvent' action.
			if (state.CurrentPressAmount == RequiredPressAmount)
			{
				state.CurrentPressAmount = 0;
				state.CurrentPressDuration = 0.0f;
				if (!menuTriggered) // Only trigger the action if it hasn't been triggered yet during this tick.
				{
					menuButtonTriggeredEvent?.Invoke();
				}
				return true;
			}
			return false;
		}
	}
}
