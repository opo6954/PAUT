using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAUT;

public class ClosedAreaDemo : Timeline {

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
        Asset ladder = GameObject.Find("Ladder").gameObject.GetComponent<Asset>();
        List<Action> ladderActions = new List<Action>();
        ladderActions.Add(new Action(ActionType.Translate, GameObject.Find("FakePlane").gameObject.GetComponent<Asset>(), new Vector3(0, -0.2f, 0)));
        ladderActions.Add(new Action(ActionType.PlaySound, GameObject.Find("LadderSoundEffect").gameObject.GetComponent<Asset>(), 0.2f));
        ladder.addAssetTrigger(player, GameObject.Find("FakePlane").gameObject.GetComponent<Asset>(), new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2), ladderActions);
        
        // setting event list
        addFirstEvent("event 0", "밀폐구역 입구 주변과 내부의 상황을 확인하십시오.", GameObject.Find("dead_fatguy").gameObject.GetComponent<Asset>(), new GazeEnterParam(TriggerConditionType.GazeEnter));
        
		addAndLinkEvent("event 1", "밀폐구역 진입 전 가스를 확인하세요. \n" + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT1) + "을 누르시면 Gas detector가 나옵니다.", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1));
        eventList[1].afterActions.Add(new Action(ActionType.ActiveRender, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>(), true));

		addAndLinkEvent("event 2", "현재 산소농도는 20.8%입니다. 밀폐구역 진입이 가능합니다. \n"+InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT1) + "을 눌러 Gas detector를 넣으세요.", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1), new Action(ActionType.DeactiveRender, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>()));
        eventList[2].beforeActions.Add(new Action(ActionType.ActiveRender, GameObject.Find("20.8").gameObject.GetComponent<Asset>(), true));

        addAndLinkEvent("event 3", "진입 전 자신의 개인보호장구(PPE) 착용 상태를 확인하세요.\n확인: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));
        eventList[3].beforeActions.Add(new Action(ActionType.ActiveRender, GameObject.Find("PPE").gameObject.GetComponent<Asset>()));
        eventList[3].afterActions.Add(new Action(ActionType.DeactiveRender, GameObject.Find("PPE").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 4", "선교에 트랜시버를 이용하여 밀폐구역 진입보고를 실시합니다.\n보고: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT1), null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1));
        eventList[4].beforeActions.Add(new Action(ActionType.ActiveRender, GameObject.Find("Tranceiver").gameObject.GetComponent<Asset>()));
        eventList[4].afterActions.Add(new Action(ActionType.DeactiveRender, GameObject.Find("Tranceiver").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 5", "선교에서 밀폐구역 진입을 허가하였습니다.\n사다리를 이용해 내려가기 위해 사다리 위로 올라갑니다.", GameObject.Find("FakePlane").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 1.0f, TriggerOperator.Smaller)); //, EffectParam.Destroy, GameObject.Find("FakePlane").gameObject.GetComponent<Asset>());
        
		addAndLinkEvent("event 6", "" + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "을 눌러 사다리를 내려가세요.", GameObject.Find("dead_fatguy").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 6.0f, TriggerOperator.Smaller), GameObject.Find("Ladder").gameObject.GetComponent<Asset>());
		eventList [6].beforeActions.Add(new Action (ActionType.EnableAssetTrigger, GameObject.Find ("Ladder").gameObject.GetComponent<Asset> ()));
		eventList [6].afterActions.Add(new Action (ActionType.DisableAssetTrigger, GameObject.Find ("Ladder").gameObject.GetComponent<Asset> ()));

		addAndLinkEvent("event 7", "알람이 발생하였습니다. Gas detector를 확인하세요.\n확인: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT1), null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1), new Action(ActionType.ActiveRender, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>(), true));
        eventList[7].beforeActions.Add(new Action(ActionType.PlaySound, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>(), 0.2f));
        eventList[7].afterActions.Add(new Action(ActionType.PlaySound, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>(), 1.0f));
        eventList[7].afterActions.Add(new Action(ActionType.ActiveRender, GameObject.Find("19.5").gameObject.GetComponent<Asset>()));
        eventList[7].afterActions.Add(new Action(ActionType.DeactiveRender, GameObject.Find("9").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 8", "현재 산소의 농도가 19.5% 입니다.\n자장식호흡구와 같은 호흡구를 착용하지 않은 경우 밀폐구역으로의 진입은 중단하여야 합니다.  \n" + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT1) + "을 눌러 Gas detector를 넣으세요.", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1), new Action(ActionType.DeactiveRender, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>()));
        eventList[8].beforeActions.Add(new Action(ActionType.DeactiveRender, GameObject.Find("9").gameObject.GetComponent<Asset>()));
        eventList[8].afterActions.Add(new Action(ActionType.PlaySound, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>(), 0.2f));

        addAndLinkEvent("event 9", "현재 호흡장구를 착용한 상태 이므로 계속 진입하도록 하겠습니다.\n" + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "을 눌러 사다리를 내려가세요.", GameObject.Find("dead_fatguy").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 4.0f, TriggerOperator.Smaller), GameObject.Find("Ladder").gameObject.GetComponent<Asset>());
		eventList [9].beforeActions.Add(new Action (ActionType.EnableAssetTrigger, GameObject.Find ("Ladder").gameObject.GetComponent<Asset> ()));
		eventList [9].afterActions.Add(new Action (ActionType.DisableAssetTrigger, GameObject.Find ("Ladder").gameObject.GetComponent<Asset> ()));

		addAndLinkEvent("event 10", "알람이 계속 울립니다. Gas detector를 확인하세요.\n확인 : " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT1) + "", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1), new Action(ActionType.ActiveRender, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>(), true));
        eventList[10].afterActions.Add(new Action(ActionType.PlaySound, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>(), 1.0f));
        eventList[10].afterActions.Add(new Action(ActionType.ActiveRender, GameObject.Find("9").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 11", "산소의 농도가 9%입니다. 산소농도 10%미만에서 사람은 사망하게 됩니다. \n" + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT1) + "을 눌러 Gas detector를 넣으세요.", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1), new Action(ActionType.DeactiveRender, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>()));
        eventList[11].afterActions.Add(new Action(ActionType.PlaySound, GameObject.Find("GasDetector").gameObject.GetComponent<Asset>(), 0.2f));
        eventList[11].afterActions.Add(new Action(ActionType.DeactiveRender, GameObject.Find("9").gameObject.GetComponent<Asset>()));

        addAndLinkEvent("event 12", "호흡장구를 착용한 상태 이므로 계속 진입하도록 하겠습니다.\n" + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "을 눌러 바닥까지 내려가세요.", GameObject.Find("dead_fatguy").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 3.0f, TriggerOperator.Smaller), GameObject.Find("Ladder").gameObject.GetComponent<Asset>());
		eventList [12].beforeActions.Add(new Action (ActionType.EnableAssetTrigger, GameObject.Find ("Ladder").gameObject.GetComponent<Asset> ()));
		eventList [12].afterActions.Add(new Action (ActionType.Destroy, GameObject.Find ("FakePlane").gameObject.GetComponent<Asset> ()));

		addAndLinkEvent("event 13", "바닥에 쓰러진 사망자가 있습니다. \n사망자의 보호장비는 무엇이 있는지 확인하십시요.\n확인: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT1) , null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT1));

        eventList [13].beforeActions.Add(new Action (ActionType.DisableAssetTrigger, GameObject.Find ("Ladder").gameObject.GetComponent<Asset> ()));
        
		// TO BE IMPLEMENTED : 사용자 움직임 제어하는 컴포넌트 
		// TO BE IMPLEMENTED : 사용자가 특정 물체에 접근했을 때 버튼을 누르면 동작하도록 
		addAndLinkEvent("event 14", "사망자는 아무런 보호장비 없이 진입하였습니다.\n밀폐구역 진입시에는 반드시 다음과 같은 장비가 필요합니다.\n확인: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));
		eventList[14].beforeActions.Add(new Action (ActionType.ActiveRender, GameObject.Find ("GDTC").gameObject.GetComponent<Asset> ()));
		eventList[14].afterActions.Add(new Action (ActionType.DeactiveRender, GameObject.Find ("GDTC").gameObject.GetComponent<Asset> ()));
		// >>> Gas detector : 산소부족 폭발성 가스를 인지하기 위해 꼭 필요합니다. >>> Tranceiver(트랜시버) : 밀폐구역 내 발생할 수 있는 상황들을 외부 작업 대기자와 통신하기 위하여 필요합니다. (종료조건 : 진입시 필요한한 장비를 확인하였습니까? 에 대한 Yes 시)

		addAndLinkEvent("event 15", "밀폐구역 내부에 환풍장치 및 조명장치가 준비되어 있는지 확인하십시요.\n확인: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));

		addAndLinkEvent("event 16", "밀폐구역 내부에는 환풍장치와 조명장치가 없었습니다.\n밀폐구역 진입 시 환풍장치와 방폭형 조명장치는 사전에 준비되어야 합니다.\n확인: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));
		eventList[16].beforeActions.Add(new Action (ActionType.ActiveRender, GameObject.Find ("Venculation").gameObject.GetComponent<Asset> ()));
		eventList[16].afterActions.Add(new Action (ActionType.DeactiveRender, GameObject.Find ("Venculation").gameObject.GetComponent<Asset> ()));

		addAndLinkEvent("event 17", "밀폐구역에 진입하는 진입구에는 어떤 준비가 있었는지 확인하십시오.\n확인: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));
		eventList[17].beforeActions.Add(new Action (ActionType.SetPosition, GameObject.Find("PlayerComponent").gameObject.GetComponent<Asset>(), initialPosition));

		addAndLinkEvent("event 18", "진입구에는 어떤 준비도 되어있지 않았습니다. \n밀폐구역 진입시에는 반드시 다음과 같은 준비가 필요합니다.\n확인: " + InputMapping.getString(player.inputType, InputMapping.InputIndex.INPUT2) + "", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));
		eventList[18].beforeActions.Add(new Action (ActionType.ActiveRender, GameObject.Find ("Prepared").gameObject.GetComponent<Asset> ()));
		eventList[18].afterActions.Add(new Action (ActionType.DeactiveRender, GameObject.Find ("Prepared").gameObject.GetComponent<Asset> ()));
		//>>> Watch man: 밀폐구역 진입자를 모니터링 및 통신을 담당하는 보조자가 진입구 주변에 위치하여 긴급한 상황에 대응할 수 있어야 합니다.
		//>>> Rescue equipment: 밀폐구역 진입자에게 사고 발생 시 필요한 구조장비들이 준비되어 있어야 합니다.
		//>>> Entry permit: 사전확인을 통해 해당 밀폐구역에 진입이 가능하다고 승인된 허가서가 준비되어야합니다.
		//(종료조건: 진입구에 필요한 준비를 확인하였습니까 ? 에 대한 Yes 시)

    }
}
