//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: The hands used by the player in the vr interaction system
//
//=============================================================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Valve.VR.InteractionSystem;

namespace PAUT
{
	//-------------------------------------------------------------------------
	// Links with an appropriate SteamVR controller and facilitates
	// interactions with objects in the virtual world.
	//-------------------------------------------------------------------------
	public class Hand : Valve.VR.InteractionSystem.Hand
	{
        public Player player;
        private Quaternion prevControllerRotation;
        public float walkingSpeed;
        public float walkingSpeedWeight = 0.2f;

        private InputMapping.InputIndex previousInput = InputMapping.InputIndex.NONE;

        //-------------------------------------------------
        void FixedUpdate()
		{
			UpdateHandPoses();
            GetRotationDifference();
            if(player.inputType == InputMapping.InputType.HTCVIVE)
                getViveInput();
        }
        
        void getViveInput()
        {
            if (controller != null && controller.GetPressUp(InputMapping.getViveInput(InputMapping.InputIndex.INPUT1)) && previousInput == InputMapping.InputIndex.NONE)
            {
                AssetTrigger e1 = new AssetTrigger(player, null, new InputDownParam(TriggerConditionType.InputDown, InputMapping.InputType.HTCVIVE, InputMapping.InputIndex.INPUT1), null);
                Asset.OnEventReceive(e1);

                Trigger e2 = new Trigger(player, null, player.curEvent, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1));
                Transition.OnEventReceive(e2);

                previousInput = InputMapping.InputIndex.INPUT1;
            }
            else if (controller != null && controller.GetPressUp(InputMapping.getViveInput(InputMapping.InputIndex.INPUT2)) && previousInput == InputMapping.InputIndex.NONE)
            {
                AssetTrigger e1 = new AssetTrigger(player, null, new InputDownParam(TriggerConditionType.InputDown, InputMapping.InputType.HTCVIVE, InputMapping.InputIndex.INPUT2), null);
                Asset.OnEventReceive(e1);

                Trigger e2 = new Trigger(player, null, player.curEvent, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));
                Transition.OnEventReceive(e2);

                previousInput = InputMapping.InputIndex.INPUT2;
            }
            else
                previousInput = InputMapping.InputIndex.NONE;
        }

        private void GetRotationDifference()
        {
            if (controller != null && controller.GetPress(InputMapping.getViveInput(InputMapping.InputIndex.MOVE)))
            {
                float rotDiff = Quaternion.Angle(this.transform.localRotation, prevControllerRotation);
                walkingSpeed = rotDiff * rotDiff * walkingSpeedWeight;
                if (walkingSpeed > 8)
                    walkingSpeed = 8;
                prevControllerRotation = this.transform.localRotation;
            }
            else
                walkingSpeed = 0;
        }
        
	}

#if UNITY_EDITOR
	//-------------------------------------------------------------------------
	[UnityEditor.CustomEditor( typeof( Hand ) )]
	public class HandEditor : UnityEditor.Editor
	{
		//-------------------------------------------------
		// Custom Inspector GUI allows us to click from within the UI
		//-------------------------------------------------
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			Hand hand = (Hand)target;

			if ( hand.otherHand )
			{
				if ( hand.otherHand.otherHand != hand )
				{
					UnityEditor.EditorGUILayout.HelpBox( "The otherHand of this Hand's otherHand is not this Hand.", UnityEditor.MessageType.Warning );
				}

				if ( hand.startingHandType == Hand.HandType.Left && hand.otherHand.startingHandType != Hand.HandType.Right )
				{
					UnityEditor.EditorGUILayout.HelpBox( "This is a left Hand but otherHand is not a right Hand.", UnityEditor.MessageType.Warning );
				}

				if ( hand.startingHandType == Hand.HandType.Right && hand.otherHand.startingHandType != Hand.HandType.Left )
				{
					UnityEditor.EditorGUILayout.HelpBox( "This is a right Hand but otherHand is not a left Hand.", UnityEditor.MessageType.Warning );
				}

				if ( hand.startingHandType == Hand.HandType.Any && hand.otherHand.startingHandType != Hand.HandType.Any )
				{
					UnityEditor.EditorGUILayout.HelpBox( "This is an any-handed Hand but otherHand is not an any-handed Hand.", UnityEditor.MessageType.Warning );
				}
			}
		}
	}
#endif
}
