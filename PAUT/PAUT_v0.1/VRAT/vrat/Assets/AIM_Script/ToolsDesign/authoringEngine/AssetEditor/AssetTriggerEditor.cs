using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vrat
{
    /*ad
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

    //assetTriggerInfo를 일단 hard하게 주자
    public class AssetTriggerTriggerInfo
    {
        public int name;

        public List<string> triggerOperatorList = new List<string>();
        public List<string> triggerTypeList = new List<string>();
    }
    public class AssetTriggerActionInfo
    {
        public int name;
    }


    public class AssetTriggerEditor : MonoBehaviour
    {

        [SerializeField]
        List<Button> allAddedTrigger;

        //trigger list
        [SerializeField]
        Dropdown triggerList;

        //가능한 action list
        [SerializeField]
        Dropdown possibleActionList;

        //추가된 action list
        [SerializeField]
        Dropdown addedActionList;

        //연결할수 있는 asset list를 말함
        [SerializeField]
        Dropdown attachedAssetList;

        //trigger type임
        [SerializeField]
        Dropdown triggerType;

        //trigger의 operator임
        [SerializeField]
        Dropdown triggerOperator;

        //trigger의 parameter 문자열임
        [SerializeField]
        InputField triggerParam;

        [SerializeField]
        InputField triggerParam2;

        //action의 parameter 문자열임
        [SerializeField]
        InputField actionParamStr;

        //action의 parameter숫자임
        [SerializeField]
        InputField actionParamThreshold;

        [SerializeField]
        AssetEditor assetEditor;

        [SerializeField]
        AssetListWindowHandler assetListWindowHandler;


        //HARDCODING for property name...
        [SerializeField]
        Text triggerParamNameLabel1;

        [SerializeField]
        Text triggerParamNameLabel2;

        [SerializeField]
        Text actionParamNameLabel1;

        [SerializeField]
        Text actionParamNameLabel2;


        string[] triggerTypeList = { "InputHoldParam", "InputUpParam", "GazeEnterParam", "GazeExitParam",
                                    "InputDownParam", "MonitorDistanceParam", "MonitorFloatVariableParam",
                                    "MonitorBooleanVariableParam", "CollisionParam" };
        string[] triggerTypeListVis = { "InputHold", "InputUp", "GazeEnter", "GazeExit",
                                    "InputDown", "MonitorDistance", "MonitorFloatVariable",
                                    "MonitorBooleanVariable", "Collision" };

        string[] triggerContidionTypeList = {"CollisionEnter", "CollisionHold", "CollisionExit",
                                        "MinTimeElapsed", "MaxTimeElapsed", "MonitorValueCaptured",
                                        "GazeEnter", "GazeHold", "GazeExit", "InventoryOpen",
                                        "InventoryClose", "InputDown", "InputUp", "Cancel", "None" };

        string[] actionTypeList = {"None", "Create", "Destroy", "ActiveRender", "DeactiveRender",
                                "Translate", "SetPosition", "EnableAssetTrigger", "DisableAssetTrigger", "StartParticle", "StopParticle", 
                                "PlaySound", "StopSound", "AttachToCamera", "SetScale","SetFloatVariable","SetBoolVariable", "IncreaseFloatVariable", "DecreaseFloatVariable"};

        string[] triggerOperatorList = { "Larger", "LargerOrEqual", "Equal", "Smaller", "SmallerOrEqual" };

        //모든 action에 대해 param값과 idx를 모두 저장하기




        //addd된 녀석만 저장하자
        Dictionary<string, List<string>> _actionParamList = new Dictionary<string, List<string>>();


        //여기에 걍 저장을 해놓자
        List<AssetTriggerXmlTemplate> currAssetTriggerXmlTemplateList = new List<AssetTriggerXmlTemplate>();

        //각기 asset trigger가 저장이 되었는지 확인하는 녀석임
        bool[] isSetAssetTriggerIdxList = new bool[4];

        //현재 편집하고 있는 assetTriggerOrderIdx임
        int currAssetTriggerOrderIdx = -1;

        bool isNewOne = false;




        void Start()
        {
            gameObject.SetActive(false);
            setUpFixedContents();
            for (int i = 0; i < isSetAssetTriggerIdxList.Length; i++)
            {
                isSetAssetTriggerIdxList[i] = false;
                AssetTriggerXmlTemplate att = new AssetTriggerXmlTemplate("", "");
                att.clearAllList();
                currAssetTriggerXmlTemplateList.Add(att);
            }
        }
        /*
         *     * InputHoldParam
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
         * */
        //각기 다른 trigger, action의 표기만 바꾸기
        public void OnVsisualizationForDifferentTrigger()
        {

            //input 관련
            int idx = triggerList.value;
            if (idx ==0 || idx == 1 || idx == 4)
            {
                triggerParamNameLabel1.text = "KeyName";
                triggerParamNameLabel2.text = "TriggerParam2";
                triggerParam.interactable = true;
                triggerParam2.interactable = false;

                triggerOperator.interactable = false;

            }
            else if(idx == 2 || idx == 3 || idx == 8)
            {
                triggerParamNameLabel1.text = "TriggerParam1";
                triggerParamNameLabel2.text = "TriggerParam2";

                triggerParam.interactable = false;
                triggerParam2.interactable = false;
                triggerOperator.interactable = false;
            }
            else if(idx == 5)
            {
                triggerParam.interactable = true;
                triggerParam2.interactable = false;
                triggerOperator.interactable = true;

                triggerParamNameLabel1.text = "Threshold";
                triggerParamNameLabel2.text = "TriggerParam2";
            }
            else if(idx == 6)
            {
                triggerParam.interactable = true;
                triggerParam2.interactable = true;
                triggerOperator.interactable = true;

                triggerParamNameLabel1.text = "Threshold";
                triggerParamNameLabel2.text = "VariableName";
            }
            else if(idx == 7)
            {
                triggerParam.interactable = true;
                triggerParam2.interactable = true;
                triggerOperator.interactable = false;

                triggerParamNameLabel1.text = "ValueToTurn";
                triggerParamNameLabel2.text = "VariableName";
            }

            

            
            
        }
        public void OnVisualizationForDifferentAction()
        {
            
        }
        public void OnUpdateForAddedAction()
        {
            int idx=0;

            for(int i=0; i < actionTypeList.Length; i++)
            {
                if(addedActionList.options[addedActionList.value].text.Contains(actionTypeList[i]) == true)
                {
                    idx = i;
                }
            }
            
            OnUpdateActionListTmp(idx);
        }

        public void OnUpdateActionListTmp(int idx)
        {
            
            if (idx >= 0 && idx <= 13)
            {
                actionParamNameLabel1.text = "Param1";
                actionParamNameLabel2.text = "Param2";

                actionParamStr.interactable = false;
                actionParamThreshold.interactable = false;
            }
            else
            {
                if (idx >= 14)
                {
                    actionParamNameLabel1.text = "VariableName";
                    actionParamNameLabel2.text = "Threshold";

                    actionParamStr.interactable = true;
                    actionParamThreshold.interactable = true;
                }

            }
        }

        public void OnUpdateforPossibleActionParamName()
        {
            int idx = possibleActionList.value;

            OnUpdateActionListTmp(idx);

           
        }

        void clearActionParam()
        {
            _actionParamList.Clear();
            clearValue2Zero();
            clearTriggerProp();
            clearActionProp();
        }

        void setUpAssetList()
        {
            List<AuthorableAsset> aa = assetListWindowHandler.getAssetList();

            attachedAssetList.ClearOptions();

            for (int i = 0; i < aa.Count; i++)
            {
                attachedAssetList.options.Add(new Dropdown.OptionData(aa[i].ObjectName));
            }
            attachedAssetList.value = 0;
            attachedAssetList.RefreshShownValue();
        }

        //초기 고정된 contents 설정하기
        void setUpFixedContents()
        {
            for (int i = 0; i < triggerTypeList.Length; i++)
            {
                triggerList.options.Add(new Dropdown
                    

                    .OptionData(triggerTypeListVis[i]));
            }
            for (int i = 0; i < triggerContidionTypeList.Length; i++)
            {
                triggerType.options.Add(new Dropdown.OptionData(triggerContidionTypeList[i]));
            }
            for (int i = 0; i < triggerOperatorList.Length; i++)
            {
                triggerOperator.options.Add(new Dropdown.OptionData(triggerOperatorList[i]));
            }
            for (int i = 0; i < actionTypeList.Length; i++)
            {
                possibleActionList.options.Add(new Dropdown.OptionData(actionTypeList[i]));
            }
        }
        //모든 list의 idx의 값을 0으로 하기
        void clearValue2Zero()
        {
            triggerList.value = 0;
            possibleActionList.value = 0;
            attachedAssetList.value = 0;
            addedActionList.value = 0;

            triggerList.RefreshShownValue();
            possibleActionList.RefreshShownValue();
            attachedAssetList.RefreshShownValue();
            addedActionList.RefreshShownValue();

            clearTriggerProp();
            clearActionProp();

            //추가되는 action 역시 clear하기
            addedActionList.ClearOptions();

            addedActionList.options.Add(new Dropdown.OptionData("None"));

            addedActionList.RefreshShownValue();
        }
        void clearTriggerProp()
        {
            triggerType.value = 0;
            triggerOperator.value = 0;
            triggerParam.text = "";
            triggerParam2.text = "";

            triggerType.RefreshShownValue();
            triggerOperator.RefreshShownValue();


        }
        void clearActionProp()
        {
            actionParamStr.text = "";
            actionParamThreshold.text = "";
        }
        //add asset trigger 버튼 누를 경우...
        public void OnAddAssetTriggerButton()
        {
            //여기서 켜는게 될라나?
            gameObject.SetActive(true);
            isNewOne = true;
            OnGetAssetTriggerInfo();


            OnUpdateforPossibleActionParamName();
            //OnUpdateforPossibleActionParamName();

            

        }

        //added action에서 선택시 저장된 parameter를 불러온다
        public void OnSelectAction()
        {

            UpdateActionParam(addedActionList.options[addedActionList.value].text);
        }

        public void OnSelectAsset()
        {
            clearActionParam();

            for (int i = 0; i < isSetAssetTriggerIdxList.Length; i++)
            {
                isSetAssetTriggerIdxList[i] = false;
                AssetTriggerXmlTemplate att = new AssetTriggerXmlTemplate("", "");
                att.clearAllList();
                currAssetTriggerXmlTemplateList.Add(att);
            }
        }

        public void OnValueChange(int idx)
        {
            if (addedActionList.options.Count > 0)
            {
                if (_actionParamList.ContainsKey(addedActionList.options[addedActionList.value].text) == true)
                {
                    Debug.Log("Name is " + addedActionList.options[addedActionList.value].text);
                    var pList = _actionParamList[addedActionList.options[addedActionList.value].text] as List<string>;

                    

                    if (idx == 0)
                    {
                        pList[idx] = actionParamStr.text;
                        Debug.Log(pList[idx]);
                    }

                    else if (idx == 1)
                    {
                        pList[idx] = actionParamThreshold.text;
                        Debug.Log(pList[idx]);
                    }
                }
            }

        }

        /*
         * 일단 update 건드리자
         * */

        //action 추가하는 함수
        public void OnAddAction()
        {
            Debug.Log("On add action...");
            if (possibleActionList.value >= possibleActionList.options.Count ||
                attachedAssetList.value >= attachedAssetList.options.Count)
            {
                Debug.Log("Wron with idx...");
                return;
            }
            string actionName = possibleActionList.options[possibleActionList.value].text;
            string assetName = attachedAssetList.options[attachedAssetList.value].text;
            string fullName = actionName + "+" + assetName;

            if (_actionParamList.ContainsKey(fullName))
            {
                Debug.Log("Already exist same name with " + fullName);
                return;
            }

            addedActionList.value = 0;
            addedActionList.RefreshShownValue();

            //기본 
            actionParamStr.text = "";
            actionParamThreshold.text = "";

            //dictionary에 집어넣기
            _actionParamList.Add(fullName, new List<string>() { actionParamStr.text, actionParamThreshold.text});

            addedActionList.options.Add(new Dropdown.OptionData(fullName));
        }

        //list of assetTriggerXmlTemplate로부터 복사하기
        public void OnSetAssetTriggerListFromAsset(List<AssetTriggerXmlTemplate> _list)
        {
            clearActionParam();
            for (int i = 0; i < allAddedTrigger.Count; i++)
            {
                allAddedTrigger[i].interactable = false;
            }

            for (int i=0; i<_list.Count; i++)
            {
                AssetTriggerXmlTemplate att = new AssetTriggerXmlTemplate("", "");
                AssetTriggerXmlTemplate.CopyValue(_list[i], ref att);

                currAssetTriggerXmlTemplateList[i] = _list[i];

                //값이 들어가 있음을 알려줌
                isSetAssetTriggerIdxList[i] = true;
                allAddedTrigger[i].interactable = true;
            }
        }

        public List<AssetTriggerXmlTemplate> OnPassAssetTriggerListToAsset()
        {
            List<AssetTriggerXmlTemplate> pq = new List<AssetTriggerXmlTemplate>();
            for (int i = 0; i < isSetAssetTriggerIdxList.Length; i++)
            {
                if (isSetAssetTriggerIdxList[i] == true)
                {
                    pq.Add(currAssetTriggerXmlTemplateList[i]);
                }
            }
            return pq;
        }

        

        //아직 설정 안되있을시
        public void OnGetAssetTriggerInfo()
        {
            clearActionParam();
            Debug.Log("Init with null");
            clearValue2Zero();
            setUpAssetList();
        }


        //이미 저장한 경력이 있을 경우
        public void OnGetAssetTriggerInfo(string triggerName, string[] attachedActionLst, string[] actionList, string[] triggerParamList, string[] actionParamList)
        {
            setUpAssetList();
            clearActionParam();
            

            for (int i = 0; i < triggerList.options.Count; i++)
            {
                if (triggerName.Contains(triggerList.options[i].text))
                {
                    triggerList.value = i;
                }
            }

            triggerList.RefreshShownValue();

            //action list update하기
            for(int i=0; i<actionList.Length; i++)
            {
                addedActionList.options.Add(new Dropdown.OptionData(actionList[i]+"+"+attachedActionLst[i]));
                _actionParamList.Add(actionList[i]+"+"+attachedActionLst[i], new List<string>() {actionParamList[2*i], actionParamList[2*i+1]});
            }
            

            //trigger property 설정하기
            triggerType.value = int.Parse(triggerParamList[0]);
            triggerType.RefreshShownValue();

            triggerOperator.value = int.Parse(triggerParamList[1]);
            triggerOperator.RefreshShownValue();

            triggerParam.text = triggerParamList[2];

            //이 부분 수정해야 함 2개 파라미터 더 입력 가능하도록
            triggerParam2.text = triggerParamList[3];


            //걍 첫 번째 action을 보여주기
            addedActionList.value = 0;

            addedActionList.RefreshShownValue();


            //새롭게 action param을 업데이트하기
            UpdateActionParam(addedActionList.options[addedActionList.value].text);
        }
        

        //possible dropdown을 바꿀 경우 clearActionProp()을 하자 무조건 없을테니까
        public void OnValueChangeForPossibleAction()
        {
            clearActionProp();
        }

        //action parameter를 주어진 이름에 맞는 파라미터 찾아서 업데이트하기
        public void UpdateActionParam(string actionName)
        {
            if (_actionParamList.Keys.Count <= 0)
            {
                return;
            }


            if (_actionParamList.ContainsKey(actionName))
            {
                List<string> pList = _actionParamList[actionName];

                actionParamStr.text = pList[0];
                actionParamThreshold.text = pList[1];
            }
            else
            {
                actionParamStr.text = "";
                actionParamThreshold.text = "";
            }
        }
                
        //asset trigger 저장하는 함수
        //넘겨줄 것 list of action of name string
        //trigger type
        //기타 list of object로 주기
        public void OnSaveAssetTrigger()
        {
            Debug.Log("On save asset trigger...");
            //걍 바로 assetrigger를 만들어보자

            if(_actionParamList.Keys.Count == 0)
            {
                return;
            }
            //trigger name
            string triggerName = triggerTypeList[triggerList.value];
            //LHW
            //action list랑 action에 부착된 asset list를 가져옴 
            string[] actionList = new string[_actionParamList.Keys.Count];
            string[] assetList = new string[_actionParamList.Keys.Count];
            string[] actionParamList = new string[_actionParamList.Keys.Count * 2];

            int idx = 0;
            
            

            foreach (string key in _actionParamList.Keys)
            {
                string[] splitInfo = key.Split('+');
                actionList[idx] = splitInfo[0];
                assetList[idx] = splitInfo[1];

                List<string> qList = _actionParamList[key];

                actionParamList[2*idx] = qList[0];
                actionParamList[2*idx+1] = qList[1];

                Debug.Log("Saved action and asset:" + key);
                Debug.Log("Saved action param1: " + qList[0]);
                Debug.Log("Saved action param2: " + qList[1]);

                idx++;
            }

            string[] triggerParameterList = new string[4];

            triggerParameterList[0] = triggerType.value.ToString();
            triggerParameterList[1] = triggerOperator.value.ToString();
            triggerParameterList[2] = triggerParam.text;
            triggerParameterList[3] = triggerParam2.text;





            //assetEditor.OnAddAssetTrigger(triggerName, assetList, actionList, triggerParameterList,actionParamList);


            if (isNewOne == true)
            {

                //새롭게 만들기
                AssetTriggerXmlTemplate att = new AssetTriggerXmlTemplate("", "");
                att.setValue(triggerName, assetList, actionList, triggerParameterList, actionParamList);

                bool isPush = false;

                for (int i = 0; i < isSetAssetTriggerIdxList.Length; i++)
                {
                    if (isSetAssetTriggerIdxList[i] == false)
                    {
                        Debug.Log("Save on " + i.ToString());
                        var atxt = currAssetTriggerXmlTemplateList[i] as AssetTriggerXmlTemplate;
                        AssetTriggerXmlTemplate.CopyValue(att, ref atxt);
                        isPush = true;

                        isSetAssetTriggerIdxList[i] = true;
                        allAddedTrigger[i].interactable = true;
                        currAssetTriggerOrderIdx = i;

                        break;
                    }
                }

                if (isPush == false)
                {
                    Debug.Log("Full of asset Trigger...");
                    OnCancel();
                }

                isNewOne = false;

                clearActionParam();
                this.gameObject.SetActive(false);
            }
            //이미 있는 경우
            else
            {
                var atxt = currAssetTriggerXmlTemplateList[currAssetTriggerOrderIdx] as AssetTriggerXmlTemplate;

                atxt.setValue(triggerName, assetList, actionList, triggerParameterList, actionParamList);

                Debug.Log("Already exist for asset trigger with idx ");
                Debug.Log(currAssetTriggerOrderIdx);


                clearActionParam();
                this.gameObject.SetActive(false);

            }
        }
        
        

        //interactiable
        //특정 trigger 버튼 누를 시의 동작
        public void OnClickSpecificTriggerButton(int idx)
        {
            if(idx < allAddedTrigger.Count)
            {
                clearActionParam();
                currAssetTriggerOrderIdx = idx;
                
                //최근에 수정하고 있는 asset trigger의 order idx를 저장해놓기
                gameObject.SetActive(true);

                OnUpdateAssetTriggerProp(currAssetTriggerXmlTemplateList[idx]);
                OnUpdateForAddedAction();

                OnVsisualizationForDifferentTrigger();


            }
        }

        public void OnUpdateAssetTriggerProp(AssetTriggerXmlTemplate axt)
        {
            string triggerName = axt.assetTriggerType;
            string[] attachedActionList = new string[axt.attachedAssetList.Count];
            string[] actionList = new string[axt.actionList.Count];
            string[] triggerParamList = new string[axt.triggerParamList.Count];
            string[] actionParamList = new string[axt.actionParamList.Count];

            //값 복사하기
            axt.attachedAssetList.CopyTo(attachedActionList);
            axt.actionList.CopyTo(actionList);
            axt.triggerParamList.CopyTo(triggerParamList);
            axt.actionParamList.CopyTo(actionParamList);


            //가져와야 하는 거 아님?

            //update하기
            OnGetAssetTriggerInfo(triggerName, attachedActionList, actionList, triggerParamList, actionParamList);
        }

        //asset trigger를 삭제하기
        public void OnDeleteAssetTrigger()
        {
            if (currAssetTriggerOrderIdx >= 0)
            {
                //만일 값이 설정된 경우
                if (isSetAssetTriggerIdxList[currAssetTriggerOrderIdx] == true)
                {
                    //지우기
                    clearActionParam();

                    currAssetTriggerXmlTemplateList[currAssetTriggerOrderIdx].clearAllList();

                    isSetAssetTriggerIdxList[currAssetTriggerOrderIdx] = false;
                    allAddedTrigger[currAssetTriggerOrderIdx].interactable = false;

                    currAssetTriggerOrderIdx = -1;

                    gameObject.SetActive(false);

                }
                else
                {
                    Debug.Log("Threr's nothing on " + currAssetTriggerOrderIdx.ToString());
                }
                
            }
        }
        

        //선택된 action 지우는 함수
        public void OnDeleteAction()
        {
            Debug.Log("On delete action button");
            //addedActionList로부터 지우기

            string fullName = addedActionList.options[addedActionList.value].text;

            if (!_actionParamList.ContainsKey(fullName))
            {
                Debug.Log("Not exist certain key with " + fullName);
                return;
            }

            addedActionList.options.RemoveAt(addedActionList.value);

            addedActionList.value = 0;
            addedActionList.RefreshShownValue();

            if(addedActionList.options.Count > 0)
                UpdateActionParam(addedActionList.options[addedActionList.value].text);
            
            _actionParamList.Remove(fullName);
        }
        //취소 버튼 누르는 함수
        public void OnCancel()
        {
            clearValue2Zero();
            clearActionParam();
            setUpAssetList();

            currAssetTriggerOrderIdx = -1;
            isNewOne = false;

            this.gameObject.SetActive(false);
        }
    }
}