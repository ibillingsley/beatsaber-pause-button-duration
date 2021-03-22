using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

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

		public virtual bool MenuButton(XRNode node)
		{
			if (_vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusRift || _vrPlatformHelper.currentXRDeviceModel == XRDeviceModel.OculusQuestLink)
			{
				if (_vrPlatformHelper.vrPlatformSDK == VRPlatformSDK.Oculus)
				{
					return (node == XRNode.LeftHand || node == XRNode.RightHand) && Input.GetButton("MenuButtonOculusTouch");
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
					return (node == XRNode.LeftHand || node == XRNode.RightHand) && Input.GetButton("MenuButtonOculusTouch");
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
