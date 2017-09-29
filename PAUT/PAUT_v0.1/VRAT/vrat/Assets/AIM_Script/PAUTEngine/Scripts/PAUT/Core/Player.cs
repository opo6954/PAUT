using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PAUT{
	
	public class Player : Asset {
		public string uid;
        public Event curEvent;
        public PlayerInput primaryHand;
        public PlayerInput secondaryHand;
        public PlayerInput head;
        public PlayerInput body;
        public Asset gazedPreviously;
        public InputMapping.InputType inputType  = InputMapping.InputType.KEYBOARD;

        //////////////////////////////////////////////////////////////////////
        // this should be fixed....
        public Timeline myTimeline;
        //////////////////////////////////////////////////////////////////////

        private bool pressedPreviously = false;
        private string stringPreviously = "";

        // Use this for initialization
        void Start () {
            gazedPreviously = null;
		}
		
		private void Update(){
			monitorValue();
			getGazeInput();
            if (inputType == InputMapping.InputType.KEYBOARD)
                getKeyInput();
        }

		// this function is for implementing monitoring a value
		protected void monitorValue()
		{
			/*
			if (transition.getTriggerParam() is MonitorDistanceParam)
			{
				float targetValue = Vector3.Distance(getPlayerPosition(), transition.targetTriggerEvent.tAsset.transform.position);
				//Debug.Log("Event - monitorValue - " + targetValue);
				float threshold = ((MonitorDistanceParam)transition.getTriggerParam()).thresholdValue;
				if (((MonitorDistanceParam)transition.getTriggerParam()).tOperator == TriggerOperator.Equal){
					if(targetValue == threshold)
					{
						Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, true));
						Transition.OnEventReceive(e1);
					}
				}
				else if (((MonitorDistanceParam)transition.getTriggerParam()).tOperator == TriggerOperator.Larger)
				{
					if (targetValue > threshold)
					{
						Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, true));
						Transition.OnEventReceive(e1);
					}
				}
				else if (((MonitorDistanceParam)transition.getTriggerParam()).tOperator == TriggerOperator.LargerOrEqual)
				{
					if (targetValue >= threshold)
					{
						Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, true));
						Transition.OnEventReceive(e1);
					}
				}
				else if (((MonitorDistanceParam)transition.getTriggerParam()).tOperator == TriggerOperator.Smaller)
				{
					if (targetValue < threshold)
					{
						Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, true));
						Transition.OnEventReceive(e1);
					}
				}
				else if (((MonitorDistanceParam)transition.getTriggerParam()).tOperator == TriggerOperator.SmallerOrEqual)
				{
					if (targetValue <= threshold)
					{
						//Debug.Log("Event - Trigger Captured");
						Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, true));
						Transition.OnEventReceive(e1);
					}
				}
			}
			*/
		}

		public void getGazeInput()
		{
			// for Gaze Trigger
			Vector3 headPosition = Camera.main.transform.position;
			Vector3 gazeDirection = Camera.main.transform.forward;

			RaycastHit hit;

			if (Physics.Raycast(headPosition, gazeDirection, out hit, 30.0f))
			{
				if (hit.collider.GetComponent<Asset>() != null && hit.collider.tag != "Dummy")
				{
					//Debug.Log("PlayerInput - FixedUpdate - Gaze Entered");
					AssetTrigger e1 = new AssetTrigger(this, hit.collider.GetComponent<Asset>(), new GazeEnterParam(TriggerConditionType.GazeEnter), null);
					Asset.OnEventReceive(e1);
                    gazedPreviously = hit.collider.GetComponent<Asset>();
                }
                else
                {
                    if(gazedPreviously != null)
                    {
                        //Debug.Log("PlayerInput - FixedUpdate - Gaze Exited");
                        AssetTrigger e1 = new AssetTrigger(this, gazedPreviously, new GazeExitParam(TriggerConditionType.GazeExit), null);
                        Asset.OnEventReceive(e1);
                        gazedPreviously = null;
                    }                    
                }
			}
		}

		public void getKeyInput()
		{
			// for Keyboard input trigger
			if (Input.anyKeyDown)
			{
				// reject mouse input
				if (Input.GetMouseButtonDown(0)
					|| Input.GetMouseButtonDown(1)
					|| Input.GetMouseButtonDown(2))
					return; //Do Nothing
                            //Debug.Log("PlayerInput - FixedUpdate - KeyDown : " + Input.inputString);
                AssetTrigger e1 = new AssetTrigger(this, null, new InputDownParam(TriggerConditionType.InputDown, InputMapping.InputType.KEYBOARD, Input.inputString), null);
                Asset.OnEventReceive(e1);
                pressedPreviously = true;
                stringPreviously = Input.inputString;
                if (stringPreviously.Length > 0)
                    Debug.Log("Key down - " + stringPreviously);
            }
            else if (Input.anyKey)
            {
                AssetTrigger e1 = new AssetTrigger(this, null, new InputHoldParam(TriggerConditionType.InputDown, InputMapping.InputType.KEYBOARD, Input.inputString), null);
                Asset.OnEventReceive(e1);
                pressedPreviously = true;
                stringPreviously = Input.inputString;
                if(stringPreviously.Length>0)
                    Debug.Log("Key hold - " + stringPreviously);
            }
            if (pressedPreviously) { 
                if (stringPreviously.Length>0 && Input.GetKeyUp(stringPreviously))
                {
                    Debug.Log("Key up - " + stringPreviously);
                    AssetTrigger e1 = new AssetTrigger(this, null, new InputUpParam(TriggerConditionType.InputUp, InputMapping.InputType.KEYBOARD, stringPreviously), null);
                    Asset.OnEventReceive(e1);
                    pressedPreviously = false;
                    stringPreviously = "";
                }
                
            }
		}
        
	}

}