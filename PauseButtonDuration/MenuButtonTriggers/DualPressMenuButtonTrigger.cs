using System;
using UnityEngine;
using UnityEngine.XR;
using Zenject;
using PauseButtonDuration.Controllers;

namespace PauseButtonDuration.MenuButtonTriggers
{
	public class DualPressMenuButtonTrigger : ITickable, IMenuButtonTrigger
	{
		public event Action menuButtonTriggeredEvent;

		[Inject]
		protected VRControllersInputManager VRControllersInputManager;
		[Inject]
		protected float RequiredPressDuration = 0.0f;
		protected float CurrentPressDuration = 0.0f;
		protected bool BothMenuButtonsPressed = false;

		public virtual void Tick()
		{
			// The original input manager is wrapped in this project's input manager implementation.
			// This is required to distinguish between the left and right menu buttons.
			ExtendedVRControllersInputManager inputManager = new ExtendedVRControllersInputManager(VRControllersInputManager);
			bool bothMenuButtonsHeld = inputManager.MenuButton(XRNode.LeftHand) && inputManager.MenuButton(XRNode.RightHand);
			if (bothMenuButtonsHeld && !BothMenuButtonsPressed)
			{
				CurrentPressDuration += Time.deltaTime;
				if (CurrentPressDuration > RequiredPressDuration)
				{
					BothMenuButtonsPressed = true;
					menuButtonTriggeredEvent?.Invoke();
				}
			}
			else
			{
				BothMenuButtonsPressed = false;
				CurrentPressDuration = 0.0f;
			}
		}

	}
}
