using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAUT
{
    public enum TimelineEventParam { Proceed = 1, MoveNext = 2, Pause = 3, Cancel = 4};
    
    public delegate void TimelineHandler(object o, TimelineArgs e);

    public class TimelineArgs : EventArgs
    {
        public readonly Player tPlayer;
        public readonly Event tNextEvent;
        public readonly TimelineEventParam tParam;

        public TimelineArgs(Player p, Event e, TimelineEventParam t)
        {
            tPlayer = p;
            tNextEvent = e;
            tParam = t;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            TimelineArgs p = (TimelineArgs)obj;
            return (tPlayer.uid == p.tPlayer.uid) && (tParam == p.tParam);
        }
    }

    public class Timeline : MonoBehaviour
    {
        public static TimelineArgs targetTriggerArgs;
        
        public class TimelineListener
        {
            public void matchCondition(object o, TimelineArgs e)
            {
                if (targetTriggerArgs != null)
                {
                    // match the condition
                    if (targetTriggerArgs.Equals(e))
                    {
                        // let event know that the condition was satisfied
                        Debug.Log("TimelineListener - MatchCondition - " + e.tNextEvent);

                        // activate the next event
                        e.tPlayer.myTimeline.move2NextEvent(e.tPlayer, e.tNextEvent);
                    }
                }
            }
            
        }

        public Player player;
        public List<Event> eventList;
        public bool isFinished;
        public int curEventIdx;

        // accessing current proceding event
        public Event curEvent;
        public event TimelineHandler curHandler;

        public static event TimelineHandler eventHandler;

        // Use this for initialization
        protected void Start()
        {
            curEventIdx = 0;
            eventList = new List<Event>();
            isFinished = false;

            TimelineListener dbsl = new TimelineListener();
            curHandler = new TimelineHandler(dbsl.matchCondition);
            eventHandler += curHandler;
            
        }

        public void moveToNextEvent()
        {
            Debug.Log("Timeline - moveToNextEvent");
            curEventIdx++;
            curEvent = eventList[curEventIdx];
            
        }

        public static void OnEventReceive(TimelineArgs e)
        {
            if (eventHandler != null)
                eventHandler(new object(), e);
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            if (isFinished)
            {
                // finished

            }
            else
            {
                if (eventList.Count > 0)
                {
                    eventList[curEventIdx].process();
                }
            }
        }

        public void terminate()
        {
            isFinished = true;
            Debug.Log("Timeline - terminate - The training has completed.");
        }

        public void move2NextEvent(Player player, Event nextEvent)
        {
            
            if (curEventIdx +1 >= eventList.Count) {
                terminate();
                return;
            }
            nextEvent.activate(player);
            setNextConditionArgs(nextEvent, TimelineEventParam.MoveNext);
            player.curEvent = nextEvent;
            curEventIdx += 1;
        }

        public void setNextConditionArgs(Event nextEvent, TimelineEventParam triggerConditionType)
        {
            targetTriggerArgs = new TimelineArgs(player, nextEvent,  triggerConditionType);
        }

        public void addFirstEvent(string eventName, string guidance, Asset asset, TriggerParam arguments)
        {
            // create event and activate it
            PAUT.Event tempEvent1 = new PAUT.Event();
            tempEvent1.initialize(eventName, guidance, player);
            if (arguments == null)
                return;
            tempEvent1.activateAndSetTrigger(player, asset, arguments);
            eventList.Add(tempEvent1);

            //set as current event
            curEvent = eventList[curEventIdx];
            player.curEvent = curEvent;
        }

        public void addAndLinkEvent(string eventName, string guidance, Asset asset, TriggerParam arguments)
        {
            // set the new event
            // activation should be performed after execution of this function
            PAUT.Event tempEvent2 = new PAUT.Event();
            tempEvent2.initialize(eventName, guidance, player);
            if (arguments == null)
                return;
            tempEvent2.transition.setTargetTrigger(player, asset, arguments);

            // make a link between tempEvent1 and tempEvent2
            if(eventList.Count == 0)
            {
                Debug.Log("Timeline - addAndLinkEvent - eventList should have more than one element");
            }
            eventList[eventList.Count-1].transition.setNextEvent(tempEvent2);
            setNextConditionArgs(tempEvent2, TimelineEventParam.MoveNext);
            eventList.Add(tempEvent2);
        }

        // with inside event
        public void addAndLinkEvent(string eventName, string guidance, Asset asset, TriggerParam arguments, Asset triggingAsset)
        {
            // set the new event
            // activation should be performed after execution of this function
            PAUT.Event tempEvent2 = new PAUT.Event();
            Debug.Log("Timeline - addAndLinkEvent - " + triggingAsset);
            tempEvent2.initialize(eventName, guidance, player, triggingAsset);
            if (arguments == null)
                return;
            tempEvent2.transition.setTargetTrigger(player, asset, arguments);

            // make a link between tempEvent1 and tempEvent2
            if (eventList.Count == 0)
            {
                Debug.Log("Timeline - addAndLinkEvent - eventList should have more than one element");
            }
            eventList[eventList.Count - 1].transition.setNextEvent(tempEvent2);
            setNextConditionArgs(tempEvent2, TimelineEventParam.MoveNext);
            eventList.Add(tempEvent2);
        }

        // with after effect and asset
        public void addAndLinkEvent(string eventName, string guidance, Asset asset, TriggerParam arguments, Action afterEffect)
        {
            // set the new event
            // activation should be performed after execution of this function
            PAUT.Event tempEvent2 = new PAUT.Event();
            tempEvent2.initialize(eventName, guidance, player, afterEffect);
            if (arguments == null)
                return;
            tempEvent2.transition.setTargetTrigger(player, asset, arguments);

            // make a link between tempEvent1 and tempEvent2
            if (eventList.Count == 0)
            {
                Debug.Log("Timeline - addAndLinkEvent - eventList should have more than one element");
            }
            eventList[eventList.Count - 1].transition.setNextEvent(tempEvent2);
            setNextConditionArgs(tempEvent2, TimelineEventParam.MoveNext);
            eventList.Add(tempEvent2);
        }
    }
}
