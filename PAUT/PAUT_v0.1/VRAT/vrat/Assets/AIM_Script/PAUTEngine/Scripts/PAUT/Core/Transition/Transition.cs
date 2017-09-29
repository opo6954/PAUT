using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Trigger for asset
 * Containing active trigger like gesture or passive trigger like approach
 * 
 * Component of trigger:
 * 1. Trigger supervisor: Supervise input whether the trigger is worked or not
 * 2. 
  * */

namespace PAUT
{    
    public class Transition : MonoBehaviour
    {
		public Trigger targetTriggerEvent;

        // the minimum required time for the trigger
        // this variable can be used to move to next event after few seconds.
        public float minimumTimeConstraint;

        // the maximum required time for the trigger
        // thie variable can be used to force transition even if the user doesn't fulfill the condition.
        public float maximumTimeConstraint;

		public Event envolvedEvent;
        public Event nextEvent;

        public static event EventTriggerHandler eventHandler;
		private event EventTriggerHandler tempHandler;
        
        //trigger에 필요한 parameterName
        protected List<string> parameterNames = new List<string>();

        //초기화 함수
		public Transition(Player _player, Event envEvent)
        {
            minimumTimeConstraint = -1;
            maximumTimeConstraint = -1;
            
			envolvedEvent = envEvent;
        }

        public void startEventListen()
        {
            // we will register event listener with target trigger arguments
            EventTriggerListener dbsl = new EventTriggerListener();
			tempHandler = new EventTriggerHandler((sender, e) => dbsl.matchCondition(sender, e, ref targetTriggerEvent));
            eventHandler += tempHandler;
        }

        public TriggerParam getTriggerParam()
        {
            return targetTriggerEvent.tTriggerParam;
        }

		public void setTargetTrigger(Player player, Asset targetAsset, TriggerParam triggerParam){
            targetTriggerEvent = new Trigger(player, targetAsset, envolvedEvent, triggerParam);
		}

        public void setAndStartTargetTrigger(Player player, Asset targetAsset, TriggerParam triggerParam)
        {
            setTargetTrigger(player, targetAsset, triggerParam);
            startEventListen();
        }
        
		public void destroy(){
			if (tempHandler!=null)
				eventHandler -= tempHandler;
		}

        public static void OnEventReceive(Trigger e)
        {
			if (eventHandler != null)
				eventHandler(new object(), e);
        }

        public void setNextEvent(Event ev)
        {
            nextEvent = ev;
        }

        public override string ToString()
        {
            return targetTriggerEvent.ToString();
        }
    }
}
 