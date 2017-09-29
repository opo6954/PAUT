using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAUT;

public class TestDemo : Timeline {

	// Use this for initialization
	new void Start () {
        base.Start();

        // test
        Demo();
    }

    void Demo()
    {
        if (player == null)
            Debug.Log("Please select Player object for this Timeline.");

        player.name = "Hyeopwoo Lee";
        player.myTimeline = this;

        addFirstEvent("the first event", "approach to the lifeboat", GameObject.Find("life_boat").gameObject.GetComponent<Asset>(), new GazeEnterParam(TriggerConditionType.GazeEnter));

        addAndLinkEvent("the second event", "collide with the lifeboat", GameObject.Find("life_boat").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 5.0f, TriggerOperator.SmallerOrEqual));

        addAndLinkEvent("the third event", "collide with the lifeboat", GameObject.Find("life_boat").gameObject.GetComponent<Asset>(), new CollisionParam(TriggerConditionType.CollisionEnter));
        
    }
}
