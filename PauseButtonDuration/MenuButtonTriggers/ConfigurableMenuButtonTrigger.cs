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
using PauseButtonDuration.Controllers;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

namespace PauseButtonDuration.MenuButtonTriggers
{
    public class ConfigurableMenuButtonTrigger : ITickable, IMenuButtonTrigger
    {
        private class MenuButtonState
        {
            public int CurrentTapAmount { get; set; } = 0;
            public float CurrentPressDuration { get; set; } = 0.0F;
            public float CurrentMultiTapDuration { get; set; } = 0.0F;
            public bool MenuButtonPressed { get; set; } = false;
        }

        public event Action menuButtonTriggeredEvent;

        [Inject]
        protected VRControllersInputManager VRControllersInputManager;
        [Inject(Id = "RequiredTapAmount")]
        protected int RequiredTapAmount = 1;
        [Inject(Id = "RequiredPressDuration")]
        protected float RequiredPressDuration = 1.0F;
        [Inject(Id = "MultiTapTimeout")]
        protected float MultiTapTimeout = 0.5F; // Amount of time allowed between each button press.
        [Inject(Id = "RequiresDualPress")]
        protected bool RequiresDualPress = false;
        [Inject(Id = "RequiresTriggerPress")]
        protected bool RequiresTriggerPress = false;
        private readonly float _requiredTriggerValue = 0.5F;
        private readonly MenuButtonState _leftMenuButtonState = new MenuButtonState();
        private readonly MenuButtonState _rightMenuButtonState = new MenuButtonState();
        private ExtendedVRControllersInputManager inputManager = null;

        public virtual void Tick()
        {
            // The original input manager is wrapped in this project's input manager implementation.
            // This is required to distinguish between the left and right menu buttons.
            if (inputManager == null)
            {
                inputManager = new ExtendedVRControllersInputManager(VRControllersInputManager);
            }
            if (RequiresDualPress)
            {
                bool leftControllerConnected = inputManager.ControllerConnected(XRNode.LeftHand); // Check if a controller is connected and if not, exclude it from the dual press check.
                bool rightControllerConnected = inputManager.ControllerConnected(XRNode.RightHand); // This is to make sure players can still pause when for instance, one of their controllers' battery is empty.
                bool atleastOneControllerConnected = (leftControllerConnected || rightControllerConnected);
                bool bothMenuButtonsHeld = (!leftControllerConnected || inputManager.MenuButton(XRNode.LeftHand)) && (!rightControllerConnected || inputManager.MenuButton(XRNode.RightHand));
                bool leftTriggerHeld = !RequiresTriggerPress || !leftControllerConnected || inputManager.TriggerValue(XRNode.LeftHand) >= _requiredTriggerValue;
                bool rightTriggerHeld = !RequiresTriggerPress || !rightControllerConnected || inputManager.TriggerValue(XRNode.RightHand) >= _requiredTriggerValue;
                bool bothTriggersHeld = leftTriggerHeld && rightTriggerHeld;
                bool bothTriggersAndMenuButtonsHeld = atleastOneControllerConnected && bothMenuButtonsHeld && bothTriggersHeld;
                UpdateMenuButtonState(_rightMenuButtonState, bothTriggersAndMenuButtonsHeld, false);
            }
            else
            {
                bool leftMenuButtonHeld = inputManager.MenuButton(XRNode.LeftHand);
                bool rightMenuButtonHeld = inputManager.MenuButton(XRNode.RightHand);
                bool leftTriggerHeld = !RequiresTriggerPress || inputManager.TriggerValue(XRNode.LeftHand) >= _requiredTriggerValue;
                bool rightTriggerHeld = !RequiresTriggerPress || inputManager.TriggerValue(XRNode.RightHand) >= _requiredTriggerValue;
                bool leftTriggerAndMenuButtonHeld = leftMenuButtonHeld && leftTriggerHeld;
                bool rightTriggerAndMenuButtonHeld = rightMenuButtonHeld && rightTriggerHeld;
                bool menuTriggered = UpdateMenuButtonState(_leftMenuButtonState, leftTriggerAndMenuButtonHeld, false);
                UpdateMenuButtonState(_rightMenuButtonState, rightTriggerAndMenuButtonHeld, menuTriggered);
            }
        }

        private bool UpdateMenuButtonState(MenuButtonState state, bool menuButtonDown, bool menuTriggered)
        {
            // Updates the button press duration.
            if (menuButtonDown && !state.MenuButtonPressed)
            {
                state.CurrentPressDuration += Time.deltaTime;
                // Updates the menu button press count.
                if (state.CurrentPressDuration >= RequiredPressDuration)
                {
                    state.MenuButtonPressed = true;
                    state.CurrentTapAmount += 1;
                    state.CurrentMultiTapDuration = 0.0F;
                }
            }
            else if (!menuButtonDown)
            {
                state.MenuButtonPressed = false;
                state.CurrentPressDuration = 0.0F;
            }

            // Reset the amount of presses when too much time has elapsed between button presses.
            if (state.CurrentTapAmount > 0)
            {
                if (state.CurrentMultiTapDuration > MultiTapTimeout)
                {
                    state.CurrentTapAmount = 0;
                    state.CurrentMultiTapDuration = 0.0F;
                }
                else if (state.CurrentPressDuration >= RequiredPressDuration)
                {
                    state.CurrentMultiTapDuration += Time.deltaTime;
                }
            }

            // Checks number of presses matches the required. If so, invokes the 'menuButtonTriggeredEvent' action.
            if (state.CurrentTapAmount == RequiredTapAmount)
            {
                state.CurrentTapAmount = 0;
                state.CurrentPressDuration = 0.0F;
                state.CurrentMultiTapDuration = 0.0F;
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
