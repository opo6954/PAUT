using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAUT
{
    public class EventParam : ICloneable
    {
        public TriggerParam triggerParam;
        public Asset asset;
        public Action afterEffect;

        public EventParam(TriggerParam _triggerParam, Asset _asset, Action _afterEffect)
        {
            triggerParam = _triggerParam;
            asset = _asset;
            afterEffect = _afterEffect;

            Debug.Log(this);
        }

        public object Clone()
        {
            EventParam clone = new EventParam(this.triggerParam, this.asset, this.afterEffect.Clone() as Action);
            return clone;
        }

        public override string ToString()
        {
            return "EventParam : " + triggerParam + ", " + asset + ", " + afterEffect;
        }
    }

    public class Event : Arrangeable
    {
        public float startTime;
        
        //Attached Asset on Event... only one asset can be attached to event...
        public Asset linkedAsset;

		public Player player;

        public Transition transition;
        
        public EventParam insideEventParam;

		public List<Action> beforeActions { get; set; }

		public List<Action> afterActions { get; set; }

        public bool isMonitoringEvent;

		public bool conditionSatisfied;

        private bool isEntered; // whether the onEventEnter executed or not

        private bool isTerminated; // for preventing execution of terminated events

        public void initialize(Instruction inst, Player _player)
        {
            player = _player;
            instruction = inst;
            transition = new Transition(player, this);
            isMonitoringEvent = false;
            beforeActions = new List<Action>();
            afterActions = new List<Action>(); // ???? 왜 이렇게 생성해줘야만 할까
            ////////////////////////////////////////////////////////////////////////////////////////////////
            /// 파라미터 세 팅 이 필 요 함
            /// 
            /// 
        }

        public void initialize(string _name, string _instruction, Player _player)
        {
            player = _player;
            instruction = new Instruction(_name, "", _instruction);
            transition = new Transition(player, this);
            isMonitoringEvent = false;
            beforeActions = new List<Action>();
            afterActions = new List<Action>(); // ???? 왜 이렇게 생성해줘야만 할까
            ////////////////////////////////////////////////////////////////////////////////////////////////
            /// 파라미터 세 팅 이 필 요 함
            /// 
            /// 
        }

        // inside event can be initialized only through this function
        public void initialize(string _name, string _instruction, Player _player, Asset triggingAsset)
        {
            initialize(_name, _instruction, _player);


            ////////////////////////////////////////////////////////////////////////////////////////////////
            // for asset trigger
            //insideEvent.initialize(name + "`s inside event", "insideEvent", player, insideEventParam.afterEffect);
            //insideEvent.setTrigger(insideEventParam.asset, insideEventParam.triggerParam);

            ////////////////////////////////////////////////////////////////////////////////////////////////


            
            ////////////////////////////////////////////////////////////////////////////////////////////////
            /// 파라미터 세 팅 이 필 요 함
            /// 
            /// 
        }

        // with after effects
        public void initialize(string _name, string _instruction, Player _player, Action _afterAction)
        {
            initialize(_name, _instruction, _player);

            // set after effect
            afterActions.Add(_afterAction);
            Debug.Log("Event - initialize - "+ _afterAction);
            ////////////////////////////////////////////////////////////////////////////////////////////////
            /// 파라미터 세 팅 이 필 요 함
            /// 
            /// 
        }

        public void activate( Player _player)
        {
            if(player.uid != _player.uid)
            {
                Debug.Log("Error - Event - activate - does not match players");
                return;
            }
            isTerminated = false;
            isEntered = false;
            startTime = Time.time;
			conditionSatisfied = false;
            player.curEvent = this;
            transition.startEventListen();
            
            // activate asset trigger
            
        }

        // execute 'activate' and 'setTransitionCondition' function with one function
        public void activateAndSetTrigger( Player _player, Asset _targetAsset, TriggerParam _t)
        {
            activate(_player);
            setTrigger(_targetAsset, _t);
        }
        
        public override string ToString()
        {
            return instruction.name;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Event p = (Event)obj;
            return name == p.name;
        }

        // set transition trigger
        public void setTrigger(Asset targetAsset, TriggerParam t)
        {
            transition.setTargetTrigger(player, targetAsset, t);
            if( t is MonitorDistanceParam)
            {
                isMonitoringEvent = true;
                Debug.Log("Event - setTransitionCondition - MonitoringEvent");
            }
        }

        public float getElapsedTime() { return Time.time - startTime; }

		public void setConditionSatisfied(bool value) {
            //Debug.Log("Event - setConditionSatisfied - "+ name);
            conditionSatisfied = value;
            isMonitoringEvent = false;
		}

        //다음 event 설정하기
        public virtual void setTransitionTrigger(Transition _transition)
        {
            transition = _transition;
        }

        private Vector3 getPlayerPosition()
        {
            return player.GetComponentInChildren<Rigidbody>().transform.position;
        }

        protected void monitorBooleanValue()
        {
            if (transition.getTriggerParam() is MonitorBooleanVariableParam)
            {
                MonitorBooleanVariableParam mbvp = (MonitorBooleanVariableParam)transition.targetTriggerEvent.tTriggerParam;
                if (!transition.targetTriggerEvent.tAsset.dicBool.ContainsKey(mbvp.key))
                {
                    Debug.Log("Event - monitorBooleanValue - Not valid key value is detected.");
                }
                else
                {
                    if(transition.targetTriggerEvent.tAsset.dicBool[mbvp.key] == mbvp.value)
                    {
                        Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorBooleanVariableParam(TriggerConditionType.MonitorValueCaptured, true));
                        Transition.OnEventReceive(e1);
                    }
                }
            }
        }

        // this function is for implementing monitoring the distance between player and asset
        protected void monitorDistance()
        {
            if (transition.getTriggerParam() is MonitorDistanceParam)
            {
                float targetValue = Vector3.Distance(getPlayerPosition(), transition.targetTriggerEvent.tAsset.transform.position);
                //Debug.Log("Event - monitorValue - " + targetValue);
                float threshold = ((MonitorDistanceParam)transition.getTriggerParam()).value;
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
        }

        protected void monitorFloatValue()
        {
            if (transition.getTriggerParam() is MonitorFloatVariableParam && !(transition.getTriggerParam() is MonitorDistanceParam))
            {
                MonitorFloatVariableParam mfvp = (MonitorFloatVariableParam)transition.getTriggerParam();
                if (!transition.targetTriggerEvent.tAsset.dicFloat.ContainsKey(mfvp.key))
                {
                    Debug.Log("Event - monitorFloatValue - Not valid key value is detected.");
                }
                float targetValue = transition.targetTriggerEvent.tAsset.dicFloat[mfvp.key];
                //Debug.Log("Event - monitorValue - " + targetValue);
                float threshold = mfvp.value;
                if (((MonitorFloatVariableParam)transition.getTriggerParam()).tOperator == TriggerOperator.Equal)
                {
                    if (targetValue == threshold)
                    {
                        Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorFloatVariableParam(TriggerConditionType.MonitorValueCaptured, true));
                        Transition.OnEventReceive(e1);
                    }
                }
                else if (((MonitorFloatVariableParam)transition.getTriggerParam()).tOperator == TriggerOperator.Larger)
                {
                    if (targetValue > threshold)
                    {
                        Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorFloatVariableParam(TriggerConditionType.MonitorValueCaptured, true));
                        Transition.OnEventReceive(e1);
                    }
                }
                else if (((MonitorFloatVariableParam)transition.getTriggerParam()).tOperator == TriggerOperator.LargerOrEqual)
                {
                    if (targetValue >= threshold)
                    {
                        Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorFloatVariableParam(TriggerConditionType.MonitorValueCaptured, true));
                        Transition.OnEventReceive(e1);
                    }
                }
                else if (((MonitorFloatVariableParam)transition.getTriggerParam()).tOperator == TriggerOperator.Smaller)
                {
                    if (targetValue < threshold)
                    {
                        Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorFloatVariableParam(TriggerConditionType.MonitorValueCaptured, true));
                        Transition.OnEventReceive(e1);
                    }
                }
                else if (((MonitorFloatVariableParam)transition.getTriggerParam()).tOperator == TriggerOperator.SmallerOrEqual)
                {
                    if (targetValue <= threshold)
                    {
                        //Debug.Log("Event - Trigger Captured");
                        Trigger e1 = new Trigger(player, transition.targetTriggerEvent.tAsset, this, new MonitorFloatVariableParam(TriggerConditionType.MonitorValueCaptured, true));
                        Transition.OnEventReceive(e1);
                    }
                }
            }
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
                    Trigger e1 = new Trigger(player, hit.collider.GetComponent<Asset>(), this, new GazeEnterParam(TriggerConditionType.GazeEnter));
                    Transition.OnEventReceive(e1);
                }
            }
        }

        public void getKeyInput()
        {
            // for Keyboard input trigger
            // To be modified : keydown에 해당되는 상위 구현 부분(바이브 컨트롤러 포함)을 구현해야 함.
            if (Input.anyKeyDown)
            {
                // reject mouse input
                if (Input.GetMouseButtonDown(0)
                 || Input.GetMouseButtonDown(1)
                 || Input.GetMouseButtonDown(2))
                    return; //Do Nothing
                            //Debug.Log("PlayerInput - FixedUpdate - KeyDown : " + Input.inputString);
                Trigger e1 = new Trigger(player, null, this, new InputDownParam(TriggerConditionType.InputDown, player.inputType, Input.inputString));
                Transition.OnEventReceive(e1);
            }
        }

		public void executeBeforeActions()
		{
            foreach(Action action in beforeActions)
            {
                action.process();
            }
		}

        public void executeAfterActions()
        {
            foreach (Action action in afterActions)
            {
                action.process();
            }
        }

        public void onEventEnter()
        {
            Debug.Log("Event - onEventEnter - " + instruction.name);
            Debug.Log(instruction);
            executeBeforeActions();
        }

        public void onEventStay()
        {
            //Debug.Log("Event - onEventStay");

            // perform asset trigger here
            monitorBooleanValue();
            monitorDistance();
            monitorFloatValue();
            getGazeInput();
            if(player.inputType == InputMapping.InputType.KEYBOARD)
                getKeyInput();
        }

        public void onEventExit()
        {
            Debug.Log("Event - onEventExit - " + this);
            
            executeAfterActions();

            // create an event to the timeline for transition
            TimelineArgs e1 = new TimelineArgs(player, transition.nextEvent, TimelineEventParam.MoveNext);
            Timeline.OnEventReceive(e1);
            
            // remove handler
            transition.destroy();

            isTerminated = true;

            // destroy self
            Destroy(this);
        }

        public virtual void process()
        {
            if (isTerminated)
                return;
            if (!isEntered)
            {
                onEventEnter();
                isEntered = true;
            }
            else
            {
                if (conditionSatisfied)
                {
                    onEventExit();
                }
                else
                {
                    onEventStay();
                }
            }
        }

    }
}