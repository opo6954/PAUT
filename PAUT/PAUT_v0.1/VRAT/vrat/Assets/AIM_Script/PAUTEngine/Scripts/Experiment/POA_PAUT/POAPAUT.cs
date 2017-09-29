using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAUT;

public class POAPAUT : Timeline {

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
		Vector3 initialPosition = GameObject.Find("PlayerComponent").gameObject.GetComponent<Asset>().transform.position;

        // setting asset trigger
        Asset FE = GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>();
        List<Action> FEActions = new List<Action>();
        FEActions.Add(new Action(ActionType.StartParticle, GameObject.Find("FE_Effect").gameObject.GetComponent<Asset>()));
        FEActions.Add(new Action(ActionType.DecreaseFloatVariable, GameObject.Find("Fire").gameObject.GetComponent<Asset>(), "life", 10.0f));
        FE.addAssetTrigger(player, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>(), new InputHoldParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1), FEActions);

        List<Action> FEUpActions = new List<Action>();
        FEUpActions.Add(new Action(ActionType.StopParticle, GameObject.Find("FE_Effect").gameObject.GetComponent<Asset>()));
        FE.addAssetTrigger(player, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>(), new InputUpParam(TriggerConditionType.InputUp, player.inputType, InputMapping.InputIndex.INPUT1), FEUpActions);

        Asset fire = GameObject.Find("Fire").gameObject.GetComponent<Asset>();
        List<Action> fireLookActions = new List<Action>();
        fireLookActions.Add(new Action(ActionType.SetBoolVariable, GameObject.Find("Fire").gameObject.GetComponent<Asset>(), "isDetected", true));
        fire.addAssetTrigger(player, GameObject.Find("Fire").gameObject.GetComponent<Asset>(), new GazeEnterParam(TriggerConditionType.GazeEnter), fireLookActions);

        // setting event list
        addFirstEvent("event 0", "공간을 순찰하며 내부의 상황을 확인하십시오.", GameObject.Find("Fire").gameObject.GetComponent<Asset>(), new MonitorBooleanVariableParam(TriggerConditionType.MonitorValueCaptured, "isDetected", true));
        eventList[0].beforeActions.Add(new Action(ActionType.EnableAssetTrigger, GameObject.Find("Fire").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 1", "화재가 발견되었습니다. 소화기가 있는 위치로 이동하세요.", GameObject.Find("FE").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 1.0f, TriggerOperator.Smaller));

		addAndLinkEvent("event 2", "X 버튼을 눌러 소화기를 들어 올리세요.", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));
        eventList[2].afterActions.Add(new Action(ActionType.DeactiveRender, GameObject.Find("FE").gameObject.GetComponent<Asset>()));
        eventList[2].afterActions.Add(new Action(ActionType.AttachToCamera, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>()));
        eventList[2].afterActions.Add(new Action(ActionType.ActiveRender, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 3", "화재 장소로 이동하여 소화를 준비하세요.", GameObject.Find("Fire").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 5.0f, TriggerOperator.Smaller));
        eventList[3].beforeActions.Add(new Action(ActionType.EnableAssetTrigger, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 4", "Z 버튼을 눌러 소화기를 동작시키세요", GameObject.Find("Fire").gameObject.GetComponent<Asset>(), new MonitorFloatVariableParam(TriggerConditionType.MonitorValueCaptured, "life", 0, TriggerOperator.Smaller));
        eventList[4].afterActions.Add(new Action(ActionType.DisableAssetTrigger, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>()));
        eventList[4].afterActions.Add(new Action(ActionType.StopParticle, GameObject.Find("FE_Effect").gameObject.GetComponent<Asset>()));
        eventList[4].afterActions.Add(new Action(ActionType.DeactiveRender, GameObject.Find("Fire").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 5", "화재가 진화되었습니다. 훈련을 종료합니다", null, null);
        
    }
}
