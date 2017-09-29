using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PAUT;

namespace vrat
{

    public class AuthoringTest : PAUT.Timeline {

        public AuthorableScenario testScenario;

        // Use this for initialization
        void Start () {
            base.Start();
            /*
             *   /*
             *   
             *   차례대로 idx를 의미함
             *   
     * Asset Trigger editor를 관리함
     * 
     * 가능한 trigger type:
     * InputHoldParam
     * InputUpParam
     * GazeEnterParam
     * GazeExitParam
     * InputDownParam
     * MonitorDistanceParam
     * MonitorFloatVariableParam
     * MonitorBooleanVariableParam
     * CollisionParam
     * 
     * 가능한 triggerConditio type:
    
        CollisionEnter
        CollisionHold
        CollisionExit
        MinTimeElapsed
        MaxTimeElapsed
        MonitorValueCaptured
        GazeEnter
        GazeHold
        GazeExit
        InventoryOpen
        InventoryClose
        InputDown
        InputUp
        Cancel
        None
     * 
     * 가능한 triggerOperator
     * Larger
     * LargerOrEqual
     * Equal
     * Smaller
     * SmallerOrEqual
     * 
     * 가능한 action type:
     *  None
        Create
        Destroy
        ActiveRender
        DeactiveRender
        Translate
		SetPosition
		EnableAssetTrigger
		DisableAssetTrigger
        PlaySound
        StopSound
        StartParticle
        StopParticle
        SetFloatVariable
        SetBoolVariable
        IncreaseFloatVariable
        DecreaseFloatVariable
        AttachToCamera
        SetScale
     * */

            loadAsset();

			loadScenario ();

        }

        // Update is called once per frame
        void Update () {
		
	    }

		private void loadScenario(){
			/*
			// setting event list
			addFirstEvent("event 0", "공간을 순찰하며 내부의 상황을 확인하십시오.", GameObject.Find("Fire").gameObject.GetComponent<Asset>(), new MonitorBooleanVariableParam(TriggerConditionType.MonitorValueCaptured, "isDetected", true));
			eventList[0].beforeActions.Add(new PAUT.Action(ActionType.EnableAssetTrigger, GameObject.Find("Fire").gameObject.GetComponent<Asset>()));

			addAndLinkEvent("event 1", "화재가 발견되었습니다. 소화기가 있는 위치로 이동하세요.", GameObject.Find("FE").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 1.0f, TriggerOperator.Smaller));

			addAndLinkEvent("event 2", "X 버튼을 눌러 소화기를 들어 올리세요.", null, new InputDownParam(TriggerConditionType.InputDown, player.inputType, InputMapping.InputIndex.INPUT2));
			eventList[2].afterActions.Add(new PAUT.Action(ActionType.DeactiveRender, GameObject.Find("FE").gameObject.GetComponent<Asset>()));
			eventList[2].afterActions.Add(new PAUT.Action(ActionType.AttachToCamera, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>()));
			eventList[2].afterActions.Add(new PAUT.Action(ActionType.ActiveRender, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>()));

			addAndLinkEvent("event 3", "화재 장소로 이동하여 소화를 준비하세요.", GameObject.Find("Fire").gameObject.GetComponent<Asset>(), new MonitorDistanceParam(TriggerConditionType.MonitorValueCaptured, 5.0f, TriggerOperator.Smaller));
			eventList[3].beforeActions.Add(new PAUT.Action(ActionType.EnableAssetTrigger, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>()));

			addAndLinkEvent("event 4", "Z 버튼을 눌러 소화기를 동작시키세요.", GameObject.Find("Fire").gameObject.GetComponent<Asset>(), new MonitorFloatVariableParam(TriggerConditionType.MonitorValueCaptured, "HP", 0, TriggerOperator.SmallerOrEqual));
			eventList[4].afterActions.Add(new PAUT.Action(ActionType.DisableAssetTrigger, GameObject.Find("FE_Hose").gameObject.GetComponent<Asset>()));
			eventList[4].afterActions.Add(new PAUT.Action(ActionType.StopParticle, GameObject.Find("FE_Effect").gameObject.GetComponent<Asset>()));
			//eventList[4].afterActions.Add(new Action(ActionType.DeactiveRender, GameObject.Find("Fire").gameObject.GetComponent<Asset>()));

			addAndLinkEvent("event 5", "화재가 진화되었습니다. 훈련을 종료합니다.", null, null);

			*/

			List<AuthorableTimeline> _timelineList = testScenario.timelineList;

			List<AuthorableEvent> _eventList = _timelineList[0].eventList;

			foreach (AuthorableEvent ae in _eventList)
			{
				// create and add event to eventList
				TriggerPrimitivesTemplate tpt = ae.variableContainer.getParameters(1) as TriggerPrimitivesTemplate;

				PAUT.TriggerConditionType triggerConditionType = PAUT.TriggerConditionType.Cancel;
				string paramString="";
				bool paramBool=false;
				float paramFloat=0.0f;
				int paramOpeIdx = 0;
				for (int i = 0; i < tpt.getNumberOfParameter(); i++)
				{
					ParameterConversion pc = tpt.getParameterValue(i);
					if (pc.getParamName () == "TriggerType" && pc.getParameterType() == PARAMTYPE.CHOICE) {
						if (pc.getAdditionalInfo ()[0].Length > 0)
							triggerConditionType = (PAUT.TriggerConditionType)Enum.Parse (typeof(PAUT.TriggerConditionType), pc.getAdditionalInfo () [0]);
						else
							triggerConditionType = PAUT.TriggerConditionType.Cancel;
					} else if (pc.getParameterType () == PARAMTYPE.STRING) {
						paramString = pc.getParameter ();
					} else if (pc.getParameterType () == PARAMTYPE.BOOL) {
						paramBool = bool.Parse (pc.getParameter ().ToLower ());
					} else if (pc.getParameterType () == PARAMTYPE.FLOAT) {
						paramFloat = float.Parse (pc.getParameter ());
					} else if (pc.getParamName () == "TriggerOperator") {
						paramOpeIdx = int.Parse(pc.getParameter ());
					}
					//Debug.Log("Param Name: " + pc.getParamName());
					//Debug.Log("Param Type: " + pc.getParameterType());
					//父老 getParameterType()捞 CHOICE老 版快 addional info 粮犁窃
					//Debug.Log("Param Value: " + pc.getParameter());
				}
				TriggerParam triggerParam = new TriggerParam();
				if(tpt.Name == "MonitorBooleanVariableParam")
					triggerParam= (PAUT.TriggerParam)System.Activator.CreateInstance(Type.GetType("PAUT." + tpt.Name), triggerConditionType, paramString, paramBool);
				else if(tpt.Name == "MonitorDistanceParam")
					triggerParam= (PAUT.TriggerParam)System.Activator.CreateInstance(Type.GetType("PAUT." + tpt.Name), triggerConditionType, paramFloat, paramOpeIdx);
				else if(tpt.Name == "InputDownParam")
					triggerParam= (PAUT.TriggerParam)System.Activator.CreateInstance(Type.GetType("PAUT." + tpt.Name), triggerConditionType, player.inputType, (PAUT.InputMapping.InputIndex)Enum.Parse (typeof(PAUT.InputMapping.InputIndex), paramString)); 
				else if(tpt.Name == "MonitorFloatVariableParam")
					triggerParam= (PAUT.TriggerParam)System.Activator.CreateInstance(Type.GetType("PAUT." + tpt.Name), triggerConditionType, paramString, paramFloat, paramOpeIdx); 

				Debug.Log ("Event : " + ae.ObjectName + ", " + ae.getInstruction () + ", " + tpt.ownedAssetName + ", " + tpt.Name + ", " + triggerParam.type + ", " + paramFloat + ", " + paramString);
				if (eventList.Count == 0) {
					addFirstEvent (ae.ObjectName, ae.getInstruction (), GameObject.Find (tpt.ownedAssetName).gameObject.GetComponent<Asset> (), triggerParam);
				} else {
					if(tpt.ownedAssetName.Length==0)
						addAndLinkEvent(ae.ObjectName, ae.getInstruction (), null, triggerParam);
					else
						addAndLinkEvent(ae.ObjectName, ae.getInstruction (), GameObject.Find (tpt.ownedAssetName).gameObject.GetComponent<Asset> (), triggerParam);
				}

				// create before action list
				ListOfXmlTemplate lxt = ae.variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate;

				for (int j = 0; j < lxt.getLengthofList(); j++)
				{
					ActionPrimitivesTemplate apt = lxt.getXmlTemplate(j) as ActionPrimitivesTemplate;
					ActionType actionType = (PAUT.ActionType)Enum.Parse (typeof(PAUT.ActionType), apt.Name.Substring(0,apt.Name.Length-6));
					PAUT.Action action = new PAUT.Action ();

					Debug.Log (eventList.Count - 1 + ": " +actionType + ", " + apt.ownedAssetName);
					action = new PAUT.Action (actionType, GameObject.Find (apt.ownedAssetName).gameObject.GetComponent<Asset> ());

					eventList [eventList.Count - 1].beforeActions.Add (action);
				}

				// create after action list
				lxt = ae.variableContainer.getParameters("AfterActionList") as ListOfXmlTemplate;

				for (int j = 0; j < lxt.getLengthofList(); j++)
				{
					ActionPrimitivesTemplate apt = lxt.getXmlTemplate(j) as ActionPrimitivesTemplate;
					ActionType actionType = (PAUT.ActionType)Enum.Parse (typeof(PAUT.ActionType), apt.Name.Substring(0,apt.Name.Length-6));
					PAUT.Action action = new PAUT.Action ();

					Debug.Log (eventList.Count - 1 + ": " +actionType + ", " + apt.ownedAssetName);
					action = new PAUT.Action (actionType, GameObject.Find (apt.ownedAssetName).gameObject.GetComponent<Asset> ());

					eventList [eventList.Count - 1].afterActions.Add (action);
				}

			}
		}

        private void loadAsset()
        {
            player = GameObject.Find("Player").GetComponent<PAUT.Player>();

            testScenario = new AuthorableScenario();

            testScenario.initialize();

            testScenario.testDeserialize("POAPAUT.scenario");

            List<AuthorableAsset> _assetList = testScenario.assetList;

            // creation of Asset component
            foreach (AuthorableAsset aa in _assetList)
            {
                GameObject foundAssetByName = GameObject.Find(aa.ObjectName);
                if (foundAssetByName == null)
                {
                    Debug.Log("AuthoringTest - Start - Error in accessing asset by name.");
                }
                else
                {
                    foundAssetByName.AddComponent<PAUT.Asset>();
                }
            }

            // setting parameter of Asset
            foreach (AuthorableAsset aa in _assetList)
            {
                Debug.Log("Asset name : " + aa.ObjectName);
                GameObject foundAssetByName = GameObject.Find(aa.ObjectName);
                PAUT.Asset asset = foundAssetByName.GetComponent<PAUT.Asset>();
                asset.intialize();
                asset.setupDictionaries();
                asset.assetType = PAUT.AssetType.MODEL;

                // creating variables
                for (int i = 0; i < aa.variableContainer.getVariableCount(); i++)
                {
                    if (!(aa.variableContainer.getParameters(i) is VariableXmlTemplate))
                        continue;
                    VariableXmlTemplate variable = (VariableXmlTemplate)aa.variableContainer.getParameters(i);
                    if (variable.Type.Equals("bool"))
                    {
                        asset.dicBool.Add(variable.Name, bool.Parse(variable.getValue()));
                        asset.booleanVariables.Add(new PAUT.Asset.DictionaryBool(variable.Name, bool.Parse(variable.getValue())));
                        Debug.Log("\tvariables : " + variable.Name + ", " + variable.getValue());
                    }
                    else if (variable.Type.Equals("float"))
                    {
                        asset.dicFloat.Add(variable.Name, float.Parse(variable.getValue()));
                        asset.floatVariables.Add(new PAUT.Asset.DictionaryFloat(variable.Name, float.Parse(variable.getValue())));
                        Debug.Log("\tvariables : " + variable.Name + ", " + variable.getValue());
                    }
                }

                // creating asset trigger
                foreach (AssetTriggerXmlTemplate atxt in aa.assetTriggerXmlTemplateList)
                {
                    PAUT.TriggerParam triggerParam;
                    if (atxt.assetTriggerType.Equals("GazeEnterParam"))
                    {
                        triggerParam = (PAUT.TriggerParam)System.Activator.CreateInstance(Type.GetType("PAUT." + atxt.assetTriggerType), int.Parse(atxt.triggerParamList[0]));
                        Debug.Log("\tTriggerParam - PAUT." + atxt.assetTriggerType + ", " + atxt.triggerParamList[0]);
                    }
                    else if (atxt.assetTriggerType.Equals("MonitorFloatVariableParam"))
                    {
                        triggerParam = (PAUT.TriggerParam)System.Activator.CreateInstance(Type.GetType("PAUT." + atxt.assetTriggerType), int.Parse(atxt.triggerParamList[0]), atxt.triggerParamList[2], int.Parse(atxt.triggerParamList[3]), int.Parse(atxt.triggerParamList[1]));
                        Debug.Log("\tTriggerParam - PAUT." + atxt.assetTriggerType + ", " + atxt.triggerParamList[0]);
                    }
                    else
                    {
                        triggerParam = (PAUT.TriggerParam)System.Activator.CreateInstance(Type.GetType("PAUT." + atxt.assetTriggerType), int.Parse(atxt.triggerParamList[0]), player.inputType, int.Parse(atxt.triggerParamList[1]));
                        Debug.Log("\tTriggerParam - PAUT." + atxt.assetTriggerType + ", " + atxt.triggerParamList[0]);
                    }
                    List<PAUT.Action> actions;
                    actions = new List<PAUT.Action>();

                    for (int i = 0; i < atxt.actionList.Count; i++)
                    {
                        if (atxt.actionList[i].Equals("DecreaseFloatVariable"))
                        {
                            actions.Add(new PAUT.Action((PAUT.ActionType)Enum.Parse(typeof(PAUT.ActionType), atxt.actionList[i]), GameObject.Find(atxt.attachedAssetList[i]).gameObject.GetComponent<PAUT.Asset>(), atxt.actionParamList[2 * i], float.Parse(atxt.actionParamList[2 * i + 1])));
                            Debug.Log("\t\tAction - " + atxt.actionList[i] + ", " + atxt.attachedAssetList[i] + ", "+ atxt.actionParamList[2 * i] +", " + atxt.actionParamList[2 * i+1]);
                        }
                        else if (atxt.actionList[i].Equals("SetBoolVariable"))
                        {
                            actions.Add(new PAUT.Action((PAUT.ActionType)Enum.Parse(typeof(PAUT.ActionType), atxt.actionList[i]), GameObject.Find(atxt.attachedAssetList[i]).gameObject.GetComponent<PAUT.Asset>(), atxt.actionParamList[2 * i], bool.Parse(atxt.actionParamList[2 * i + 1])));
                            Debug.Log("\t\tAction - " + atxt.actionList[i] + ", " + atxt.attachedAssetList[i] + ", " + atxt.actionParamList[2 * i] + ", " + atxt.actionParamList[2 * i + 1]);
                        }
                        else
                        {
                            actions.Add(new PAUT.Action((PAUT.ActionType)Enum.Parse(typeof(PAUT.ActionType), atxt.actionList[i]), GameObject.Find(atxt.attachedAssetList[i]).gameObject.GetComponent<PAUT.Asset>()));
                            Debug.Log("\t\tAction - " + atxt.actionList[i] + ", " + atxt.attachedAssetList[i]);
                        }
                    }
                    asset.addAssetTrigger(player, asset, triggerParam, actions);
                    Debug.Log("---------addition of Asset trigger---------");
                }
            }
        }
    }
}

