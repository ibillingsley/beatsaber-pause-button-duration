/*
 * This file is part of BeatSaber-PauseButtonDuration.
 * Copyright (c) 2021 Bart Toersche
 *
 * BeatSaber-PauseButtonDuration is licensed under a MIT License (MIT).
 *
 * You should have received a copy of the MIT License along with
 * BeatSaber-PauseButtonDuration. If not, see <https://opensource.org/licenses/MIT>.
 */

using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace PauseButtonDuration.Controllers
{
    class ExtendedVRControllersInputManager
    {
        private readonly VRControllersInputManager _inputManager;
        private readonly IVRPlatformHelper _vrPlatformHelper;

        public ExtendedVRControllersInputManager(VRControllersInputManager inputManager)
        {
            _inputManager = inputManager;
            _vrPlatformHelper = (IVRPlatformHelper)AccessTools.Field(typeof(VRControllersInputManager), "_vrPlatformHelper").GetValue(inputManager);
        }

        public virtual float HorizontalAxisValue(XRNode node) => _inputManager.HorizontalAxisValue(node);
        public virtual bool MenuButton() => _inputManager.MenuButton();
        public virtual bool MenuButtonDown() => _inputManager.MenuButtonDown();
        public virtual float TriggerValue(XRNode node) => _inputManager.TriggerValue(node);
        public virtual float VerticalAxisValue(XRNode node) => _inputManager.VerticalAxisValue(node);

        public virtual bool ControllerConnected(XRNode node)
        {
            if (_vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusRift || _vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuestLink)
            {
                if (_vrPlatformHelper.vrPlatformSDK == VRPlatformSDK.Oculus)
                {
                    return (node == XRNode.LeftHand || node == XRNode.RightHand) && OVRInput.IsControllerConnected(OVRInput.Controller.Touch);
                }
                if (node == XRNode.LeftHand)
                {
                    return OVRInput.IsControllerConnected(OVRInput.Controller.LTouch);
                }
                else if (node == XRNode.RightHand)
                {
                    return OVRInput.IsControllerConnected(OVRInput.Controller.RTouch);
                }
            }
            else
            {
                if (_vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuest)
                {
                    return (node == XRNode.LeftHand || node == XRNode.RightHand) && OVRInput.IsControllerConnected(OVRInput.Controller.Touch);
                }
                CVRSystem system = OpenVR.System;
                if (node == XRNode.LeftHand)
                {
                    uint index = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
                    return system.IsTrackedDeviceConnected(index);
                }
                else if (node == XRNode.RightHand)
                {
                    uint index = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);
                    return system.IsTrackedDeviceConnected(index);
                }
            }
            return false;
        }

        public virtual bool MenuButton(XRNode node)
        {
            if (_vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusRift || _vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuestLink)
            {
                if (_vrPlatformHelper.vrPlatformSDK == VRPlatformSDK.Oculus)
                {
                    return Input.GetButton("MenuButtonOculusTouch");
                }
                if (node == XRNode.LeftHand)
                {
                    return Input.GetButton("MenuButtonLeftHandOculusTouch");
                }
                else if (node == XRNode.RightHand)
                {
                    return Input.GetButton("MenuButtonRightHandOculusTouch");
                }
            }
            else
            {
                if (_vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuest)
                {
                    return Input.GetButton("MenuButtonOculusTouch");
                }
                if (node == XRNode.LeftHand)
                {
                    return Input.GetButton("MenuButtonLeftHand");
                }
                else if (node == XRNode.RightHand)
                {
                    return Input.GetButton("MenuButtonRightHand");
                }
            }
            return false;
        }
    }
}
