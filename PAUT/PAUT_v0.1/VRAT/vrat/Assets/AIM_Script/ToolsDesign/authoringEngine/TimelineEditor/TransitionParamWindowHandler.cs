using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vrat
{
    //window handler for transitionParam

    public class TransitionParamWindowHandler : MonoBehaviour
    {

        //trigger parma의 종류를 말함
        [SerializeField]
        Dropdown triggerParam;


        //triggerCondition의 종류를 말함
        [SerializeField]
        Dropdown triggerConditionType;

        //ui가 보일 곳
        [SerializeField]
        GameObject uiPropertyPosition;

        //가장 최근의 eventUIManager임
        eventUIManager currEventUIManager;

        List<TriggerPrimitivesTemplate> triggerTemplate = new List<TriggerPrimitivesTemplate>();

        List<PropertyVisualizeHandler> propertyVisList = new List<PropertyVisualizeHandler>();

        string propertyUIPrefabPath = "TimelineEditor/propertyUIPrimTemplate";
        Object propertyUIUPrefab;

        //ui에 나오는 template
        TriggerPrimitivesTemplate currTemplate;

        //최종 저장되는 template
        TriggerPrimitivesTemplate savedTemplate;

        



        //초기화해주기 transition 종류별로
        public void initialize(List<TriggerPrimitivesTemplate> _origin)
        {
            triggerTemplate = _origin;

            for (int i = 0; i < _origin.Count; i++)
            {
                triggerParam.options.Add(new Dropdown.OptionData(_origin[i].Name));
            }
        }
        public void OnSetEventUIManager(eventUIManager eum)
        {
            currEventUIManager = eum;
            //저장해놓기
            savedTemplate = eum.transitionValue;
        }


        public void OnSave()
        {
            OnGetValueFromUI();
            currEventUIManager.isTransitionSet = true;
            
            currEventUIManager.OnCallbackSetTransitionParam(currTemplate);
            Debug.Log("On Save...");
        }

        public void OnDelete()
        {
            currEventUIManager.isTransitionSet = false;
            
            currEventUIManager.OnCallbackDeleteTransitionParam();

            Debug.Log("On Delete...");
        }

        //trigger param이 바뀔때마다 list update하기

        public void OnUpdateProperties()
        {
            Debug.Log("update for " + currEventUIManager.uniqueName);
                        
            OnVisualizeProperties();
        }

        public void OnGetValueFromUI()
        {
            if (currTemplate != null)
            {
                for (int i = 0; i < propertyVisList.Count; i++)
                {
                    string paramName = "";
                    string paramValue = "";

                    paramValue = propertyVisList[i].getValueNParamName(ref paramName);

                    if (paramName == "" || paramValue == "")
                    {
                        Debug.Log("Empty with param Name and value...");
                        return;
                    }

                    currTemplate.setParameterValue(paramName, paramValue);
                }
            }
        }

        

        public void clearProperty()
        {
            propertyVisList.Clear();

            for (int i = 0; i < uiPropertyPosition.transform.childCount; i++)
            {
                Debug.Log("On destory chid..");
                GameObject.Destroy(uiPropertyPosition.transform.GetChild(i).gameObject);
            }
        }

        public void OnClearTrigger()
        {
            if (currEventUIManager.isTransitionSet == false)
            {
                triggerParam.value = 0;
                triggerParam.RefreshShownValue();
            }
            else if(currEventUIManager.isTransitionSet == true)
            {
                string paramName = savedTemplate.Name;

                Debug.Log(paramName);
                

                for (int i=0; i<triggerParam.options.Count; i++)
                {
                    Debug.Log(triggerParam.options[i].text);
                    if (triggerParam.options[i].text == paramName)
                    {
                        Debug.Log("Found true...");
                        triggerParam.value = i;
                        triggerParam.RefreshShownValue();
                    }
                }

            }
        }
        //초기에만 불리는 경우
        public void OnVisualizePropertiesInit()
        {
            OnClearTrigger();
            OnVisualizeProperties();
        }

                  

        public void OnVisualizeProperties()
        {
            triggerConditionType.ClearOptions();
            clearProperty();

            Debug.Log("For " + currEventUIManager.uniqueName);

            Debug.Log(currEventUIManager.isTransitionSet);

            if(currEventUIManager.isTransitionSet == true)
            {
                //save tempate으로 대체
                if (savedTemplate.Name == triggerParam.options[triggerParam.value].text)
                {
                    currTemplate = new TriggerPrimitivesTemplate("", "");

                    var ppt = currTemplate as PAUTPrimitivesTemplate;

                    //미리 저장된 template을 가지고 복사하기
                    PAUTPrimitivesTemplate.CopyPAUT(savedTemplate, ref ppt);
                }
            }
            //참일경우 문제가 됨 이미 설정된 경우에...d

            //만일 setTransition이 event에서 되지 않았을 경우 새롭게 만들기
            if(currEventUIManager.isTransitionSet == false)
            {
                Debug.Log("For isSetTransitioNOnce...");
                //일단 새롭게 만들기

                currTemplate = new TriggerPrimitivesTemplate("", "");

                var ppt = currTemplate as PAUTPrimitivesTemplate;
                
                //기본값 가지고 복사하기
                PAUTPrimitivesTemplate.CopyPAUT(triggerTemplate[triggerParam.value], ref ppt);
            }
            else
            {
                Debug.Log("For isSetTransitioNOnce...");
                //일단 새롭게 만들기

                currTemplate = new TriggerPrimitivesTemplate("", "");

                var ppt = currTemplate as PAUTPrimitivesTemplate;

                //기본값 가지고 복사하기
                PAUTPrimitivesTemplate.CopyPAUT(triggerTemplate[triggerParam.value], ref ppt);
                //init 할 때 update랑 tirrger type 고를 떄의 update랑 다르게 하기
            }
            for(int i=0; i<currTemplate.getNumberOfParameter(); i++)
            {
                ParameterConversion pc = currTemplate.getParameterValue(i);

                string paramName = pc.getParamName();
                PARAMTYPE pm = pc.getParameterType();
                
                if(paramName == "TriggerType")
                {
                    Debug.Log("in triggerType...");
                    string[] addionalInfo = pc.getAdditionalInfo();
                    for(int j=0; j<addionalInfo.Length; j++)
                    {
                        triggerConditionType.options.Add(new Dropdown.OptionData(addionalInfo[j]));
                    }

                    triggerConditionType.value = 0;
                    triggerConditionType.RefreshShownValue();
                }
                else
                {
                    
                    GameObject go = (GameObject)GameObject.Instantiate(propertyUIUPrefab, uiPropertyPosition.transform);

                    Debug.Log(go.name);
                    
                    go.name = paramName;

                    if (pm == PARAMTYPE.BOOL)
                    {
                        go.GetComponent<PropertyVisualizeHandler>().visualizePropertyRaw(paramName, VISUALIZEPROPTYPE.TOGGLE, pc.getParameter(), null, i);
                    }
                    else if(pm == PARAMTYPE.CHOICE)
                    {
                        go.GetComponent<PropertyVisualizeHandler>().visualizePropertyRaw(paramName, VISUALIZEPROPTYPE.DROPDOWN, pc.getParameter(), pc.getAdditionalInfo(), i);
                    }
                    else
                    {
                        go.GetComponent<PropertyVisualizeHandler>().visualizePropertyRaw(paramName, VISUALIZEPROPTYPE.TEXTINPUT, pc.getParameter(), null, i);
                    }

                    propertyVisList.Add(go.GetComponent < PropertyVisualizeHandler>());
                    

                }
            }



            Debug.Log(triggerParam.value);

        }        

        public void OnCancel()
        {
            //Cancel할 경우 
            //만일 set되지 않은 상태라면 선없애기
            //만일 set된 상태라면걍지우기


            if ( currEventUIManager.isTransitionSet == false)
            {
                currEventUIManager.clearToConnect();
            }
            

            

            Debug.Log("On Cancel...");
        }
        
        // Use this for initialization
        void Start()
        {
            propertyUIUPrefab = Resources.Load(propertyUIPrefabPath);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}