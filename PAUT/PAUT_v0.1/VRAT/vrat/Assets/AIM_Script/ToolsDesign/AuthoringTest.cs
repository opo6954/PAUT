using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{

    public class AuthoringTest : MonoBehaviour {

	    // Use this for initialization
	    void Start () {

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
            /*
            AuthorableScenario testScenario = new AuthorableScenario();

            testScenario.initialize();

            testScenario.testDeserialize("POAPAUT.scenario");
            */
            /*
            //asset 불러오기

            List<AuthorableAsset> _assetList = testScenario.assetList;

            foreach(AuthorableAsset aa in _assetList)
            {
                foreach(AssetTriggerXmlTemplate atxt in aa.assetTriggerXmlTemplateList)
                {
                    Debug.Log("Trigger의 attached asset은 설정 안되어 있음, 가져온 asset의 이름으로 대체해야");

                    Debug.Log("붙여진 triggerParam 이름: " + atxt.assetTriggerType);

                    for(int i=0; i<atxt.triggerParamList.Count; i++)
                    {
                        Debug.Log("Trigger의 paramList는 차례대로 triggerCondition, triggerOperator, triggerParamTh로 구성됨");
                        Debug.Log("triggerCondition, triggerOperator는 idx를 가지고 있고 총 list는 위에 있음");
                    }
                    
                        

                    for(int i=0; i<atxt.actionList.Count; i++)
                    {
                        //action name list임
                        Debug.Log("Action 이름: " + atxt.actionList[i]);
                        Debug.Log("각 action의 순서별로 attached된 asset: " + atxt.attachedAssetList[i]);

                        //action parameter list임
                        //강제적으로 2개씩 가지고 있음
                        //action 이름에 따라 다르게 가져와야 함
                        Debug.Log("처음 파라미터: " + atxt.actionParamList[2*i]);
                        Debug.Log("두번째 파라미터: " + atxt.actionParamList[2 * i+1]);
                    }

                    Debug.Log("Action list6: " + atxt.actionList);

                    Debug.Log(atxt.actionParamList);

                }
            }


            //timeline 불러오기
            //원래는 여러명이라서 각기 다른 사람별로 eventlist 줬었는데 지금은 한 명이니 걍 0번째idx로eventlist 불러와도 됨

            List<AuthorableTimeline> _timelineList = testScenario.timelineList;

            List<AuthorableEvent> _eventList = _timelineList[0].eventList;

            foreach(AuthorableEvent ae in _eventList)
            {
                //trigger 불러오기
                TriggerPrimitivesTemplate tpt = ae.variableContainer.getParameters("Trigger") as TriggerPrimitivesTemplate;

                //trigger의 모든 parameter 이름 가져오기 tpt.getAllParameterName()

                //transition trigger의 이름
                Debug.Log("transition name: " + tpt.ClassName);

                //transition trigger에 붙은 asset 가져오기
                Debug.Log("transition attached asset name: " + tpt.getAttachedAssetName());

                //transition trigger의param 가져오기 순서대로 가져올 수 있음



                //가져온 ParameterConversion을 통해서 어느 type인지 확인 가능
                //bool이나 string, choice가 있음
                //choice의 경우 idx만 getParamterValue를 통해 가져올 수 있고
                //array 데이터는 pc.getAdditionalInfo()를 통해 string[]으로 가져올 수 있음d
                //예시
                ParameterConversion pc = tpt.getParameterValue(0);
                PARAMTYPE pm = pc.getParameterType();
                string paramValue = pc.getParameter();
                string[] arrayData = pc.getAdditionalInfo();
                
                

                //에서 무슨 타입이고 무슨 값인지 읽어올 수 있음

                
                 

                //triggertpt.getAllParameterName()



                //before action /after action 불러오기

                //차례대로 action 불러오기 가능
                //일단 첫 번쨰 action 불러오고(list로 구성)

                ActionPrimitivesTemplate apt = (ae.variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate).getXmlTemplate(0) as ActionPrimitivesTemplate;

                //action 역시 위의 trigger와 마찬가지 방법으로 불러올 수 있음
                ParameterConversion pc2 = apt.getParameterValue(0);
                PARAMTYPE pm2 = pc2.getParameterType();
                string paramValue2 = pc2.getParameter();
                string[] arrayData2 = pc2.getAdditionalInfo();




                //instruction 불러오기 역시 before action과 같이 list로 구성됨

                //(variableContainer.getParameters("InstructionList") as ListOfXmlTemplate).addList(instructionBase);

                ///지금은 instruction이 1개씩만 들어가 있으므로 걍 첫 value만 부르면 됨
                InstPrimitivesTemplate ipt = ((ae.variableContainer.getParameters("InstructionList") as ListOfXmlTemplate) as ListOfXmlTemplate).getXmlTemplate(0) as InstPrimitivesTemplate;

                //마찬가지로 똑같은 방법으로 부를 수 있는데 instruction의 경우 처음에 instruction value가 있어서 설정하면 됨
                ParameterConversion pc3 = ipt.getParameterValue(0);
                PARAMTYPE pm3 = pc3.getParameterType();
                string paramValue3 = pc3.getParameter();
                


    */
            }
        
	
	    // Update is called once per frame
	    void Update () {
		
	    }
    }
}

