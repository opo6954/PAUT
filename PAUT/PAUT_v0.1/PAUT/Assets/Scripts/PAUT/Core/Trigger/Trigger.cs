using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAUT
{
    public delegate void EventTriggerHandler(object o, Trigger e);

    public class Trigger
    {
        public readonly Player tPlayer;
        public readonly Asset tAsset;
        public readonly Event tEvent;
        public readonly TriggerParam tTriggerParam;

        public Trigger(Player p, Asset a, Event e, TriggerParam t)
        {
            tPlayer = p;
            tAsset = a;
            tEvent = e;
            tTriggerParam = t;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Trigger p = (Trigger)obj;
            if (tAsset == null && tEvent == null)
            {
                return (tPlayer.uid == p.tPlayer.uid) && (tTriggerParam.Equals(p.tTriggerParam));
            }
            else if (tAsset == null)
            {
                return (tPlayer.uid == p.tPlayer.uid) && tEvent.Equals(p.tEvent) && (tTriggerParam.Equals(p.tTriggerParam));
            }
            else if(tEvent == null)
            {
                return (tPlayer.uid == p.tPlayer.uid) && tAsset.Equals(p.tAsset) && (tTriggerParam.Equals(p.tTriggerParam));
            }
            else
                return (tPlayer.uid == p.tPlayer.uid) && tEvent.Equals(p.tEvent) && tAsset.Equals(p.tAsset) && (tTriggerParam.Equals(p.tTriggerParam));
        }

        public override string ToString()
        {
            if (tAsset != null && tEvent != null)
                return tPlayer.uid + ", " + tAsset.name + ", " + tEvent.name + ", " + tTriggerParam.type;

            else if (tEvent != null)
                return tPlayer.uid + ", null, " + tEvent.name + ", " + tTriggerParam.type;
            else if (tAsset != null)
                return tPlayer.uid + ", " + tAsset.name + ", null, " + tTriggerParam.type;
            else
                return tPlayer.uid + ", null, null, " + tTriggerParam.type;
        }
    }

    public class EventTriggerListener
    {
        public void matchCondition(object obj, Trigger e, ref Trigger targetTriggerEvent)
        {
            if (e != null && targetTriggerEvent != null)
            {
                //Debug.Log(targetTriggerArgs + " \t" + e);
                //Debug.Log("\t"+ targetTriggerEvent.tTriggerParam + " \t" + e.tTriggerParam);

                // match the condition
                if (targetTriggerEvent.tTriggerParam.type == TriggerConditionType.MonitorValueCaptured)
                {
                    if (e.tTriggerParam is MonitorParam && ((MonitorParam)(e.tTriggerParam)).satisfied)
                    {
                        //Debug.Log("EventTriggerListener - matched - " + targetTriggerEvent.tEvent + " \t" + e.tEvent);

                        // let event know that the condition was satisfied
                        e.tEvent.setConditionSatisfied(true);
                    }
                }
                else
                {
                    if (e.Equals(targetTriggerEvent))
                    {
                        //Debug.Log("EventTriggerListener - matched - " + targetTriggerEvent.tEvent + " \t" + e.tEvent);

                        // let event know that the condition was satisfied
                        e.tEvent.setConditionSatisfied(true);
                    }
                }
            }
        }
    }

    
}