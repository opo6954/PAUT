using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PAUT{

	public delegate void AssetTriggerHandler(object o, AssetTrigger e);

	public class AssetTrigger
	{
		public readonly Player tPlayer;
		public readonly Asset tAsset;
		public readonly List<Action> tActions;
		public readonly TriggerParam tTriggerParam;

		public AssetTrigger(Player p, Asset a, TriggerParam t, List<PAUT.Action> s)
		{
			tPlayer = p;
			tAsset = a;
			tTriggerParam = t;
            if (s == null)
                tActions = new List<Action>();
            else{
                tActions = new List<PAUT.Action>(s.Count);
                s.ForEach((item) =>
                {
                    tActions.Add((PAUT.Action)item.Clone());
                });
            }            
        }

		public override bool Equals(object obj)
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType())
				return false;

			AssetTrigger p = (AssetTrigger)obj;

			// keydown의 경우 Asset이 없ㅡ
			if (tAsset == null || p.tAsset == null) {
				return (tPlayer.uid == p.tPlayer.uid) && (tTriggerParam.Equals (p.tTriggerParam));
			} else if(tPlayer == null || p.tPlayer == null){
				return (tAsset.Equals (p.tAsset) && (tTriggerParam.Equals (p.tTriggerParam)));
			}
			else {
				return (tPlayer.uid == p.tPlayer.uid) && tAsset.Equals (p.tAsset) && (tTriggerParam.Equals (p.tTriggerParam));
			}
		}

		public override string ToString()
		{
			if (tAsset != null && tActions != null)
				return tPlayer.uid + ", " + tAsset.name + ", " + tTriggerParam.type+ ", " + tActions.Count ;

			else if (tActions != null)
				return tPlayer.uid + ", null, " + ", " + tTriggerParam.type+ ", " + tActions.Count;
			else if (tAsset != null)
				return tPlayer.uid + ", " + tAsset.name + ", null, " + tTriggerParam.type;
			else
				return tPlayer.uid + ", null, null, " + tTriggerParam.type;
		}
	}

	public class AssetTriggerListener
	{
		public void matchCondition(object obj, AssetTrigger e, ref List<AssetTrigger> targetTriggerEvents)
		{
			if (e != null && targetTriggerEvents != null)
			{
                // match the condition
                foreach (AssetTrigger targetTriggerEvent in targetTriggerEvents)
                {
 //                   Debug.Log("\t" + targetTriggerEvent + " \t" + e);

                    if (targetTriggerEvent.tTriggerParam.type == TriggerConditionType.MonitorValueCaptured)
                    {
                        if (e.tTriggerParam is MonitorParam && ((MonitorParam)(e.tTriggerParam)).satisfied)
                        {
                            Debug.Log("TriggerEventListener - matched");

                            //if(targetTriggerEvent.tAction != null)
                            foreach (Action action in targetTriggerEvent.tActions)
                                action.process();
                        }
                    }
                    else
                    {
                        if (e.Equals(targetTriggerEvent))
                        {
//                            Debug.Log("AssetTriggerListener - matched - " + targetTriggerEvent.tActions.Count);

                            //if(targetTriggerEvent.tAction != null)
                            foreach (Action action in targetTriggerEvent.tActions)
                                action.process();
                        }
                    }
                }
			}
		}
	}
}