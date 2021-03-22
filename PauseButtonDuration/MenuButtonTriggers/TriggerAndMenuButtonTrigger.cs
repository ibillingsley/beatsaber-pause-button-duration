using PauseButtonDuration.Controllers;
using System;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

namespace PauseButtonDuration.MenuButtonTriggers
{
	public class TriggerAndMenuButtonTrigger : ITickable, IMenuButtonTrigger
	{
		private class MenuButtonState
		{
			public float CurrentPressDuration { get; set; } = 0.0f;
			public bool MenuButtonPressed { get; set; } = false;
		}

		public event Action menuButtonTriggeredEvent;

		[Inject]
		protected VRControllersInputManager VRControllersInputManager;
		[Inject]
		protected float RequiredPressDuration = 0.0f;
		private readonly float _requiredTriggerValue = 0.5f;
		private readonly MenuButtonState _leftMenuButtonState = new MenuButtonState();
		private readonly MenuButtonState _rightMenuButtonState = new MenuButtonState();

		public virtual void Tick()
		{
			// The original input manager is wrapped in this project's input manager implementation.
			// This is required to distinguish between the left and right menu buttons.
			ExtendedVRControllersInputManager inputManager = new ExtendedVRControllersInputManager(VRControllersInputManager);
			bool leftTriggerAndMenuButtonHeld = inputManager.MenuButton(XRNode.LeftHand) && inputManager.TriggerValue(XRNode.LeftHand) >= _requiredTriggerValue;
			bool rightTriggerAndMenuButtonHeld = inputManager.MenuButton(XRNode.RightHand) && inputManager.TriggerValue(XRNode.RightHand) >= _requiredTriggerValue;
			bool menuTriggered = UpdateMenuButtonState(_leftMenuButtonState, leftTriggerAndMenuButtonHeld, false);
			UpdateMenuButtonState(_rightMenuButtonState, rightTriggerAndMenuButtonHeld, menuTriggered);
		}

		private bool UpdateMenuButtonState(MenuButtonState state, bool menuButtonHeld, bool menuTriggered)
		{
			if (menuButtonHeld && !state.MenuButtonPressed)
			{
				state.CurrentPressDuration += Time.deltaTime;
				if (state.CurrentPressDuration > RequiredPressDuration)
				{
					state.MenuButtonPressed = true;
					if (!menuTriggered) // Only trigger the action if it hasn't been triggered yet during this tick.
					{
						menuButtonTriggeredEvent?.Invoke();
					}
					return true;
				}
			}
			else
			{
				state.MenuButtonPressed = false;
				state.CurrentPressDuration = 0.0f;
			}
			return false;
		}
	}
}
