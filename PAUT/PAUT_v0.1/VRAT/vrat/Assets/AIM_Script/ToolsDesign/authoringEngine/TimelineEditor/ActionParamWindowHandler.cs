using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vrat
{
    //action param window handler
    public class ActionParamWindowHandler : MonoBehaviour
    {

        //붙여진 asset 이름
        [SerializeField]
        UnityEngine.UI.Text assetName;
         
        //action type dropdown에 대한 instance
        [SerializeField]
        UnityEngine.UI.Dropdown actionType;

        //ui가 보일 곳
        [SerializeField]
        GameObject uiPropertyPosition; 

        eventUIManager currEventUIManager;

        List<ActionPrimitivesTemplate> actionTemplate = new List<ActionPrimitivesTemplate>();

        List<PropertyVisualizeHandler> propertyVisList = new List<PropertyVisualizeHandler>();

        string propertyUIPrefabPath = "TimelineEditor/propertyUIPrimTemplate";
        Object propertyUIPrefab;

        //최근의 template
        ActionPrimitivesTemplate currTemplate;

        //저장된 template
        ActionPrimitivesTemplate savedTemplate;

        //최근 주어진 action의 idx
        public int currActionIdx = -1;

        public bool isBefore = false;

        public void OnSetAssetName(string _name)
        {
            assetName.text = _name;
        }

        public void initialize(List<ActionPrimitivesTemplate> _origin)
        {
            actionTemplate = _origin;

            for (int i = 0; i < _origin.Count; i++)
            {
                actionType.options.Add(new Dropdown.OptionData(_origin[i].Name));
            }
        }
        
        public void OnSave()
        {
            //값을 가져오고
            OnGetValueFromUI();
            //set action callback 함수 부르기
            currEventUIManager.OnCallbackSetAction(currTemplate, currActionIdx, isBefore);
        }

        public void OnDelete()
        {
            currEventUIManager.OnCallbackDeleteAction(currTemplate, currActionIdx, isBefore);
        }

        //time에서 설정하기
        public void OnSetEventUIManager(eventUIManager _eum)
        {
            currEventUIManager = _eum;
            currActionIdx = _eum.currSelectedActionIdx;
            isBefore = _eum.currIsSelectedActionBefore;
            
            if (isBefore == true)
                savedTemplate = currEventUIManager.beforeActionList[currActionIdx];
            else
                savedTemplate = currEventUIManager.afterActionList[currActionIdx];
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

                currTemplate.setAttachedAssetName(assetName.text);

            }
        }

        public void OnClearAction()
        {
            if(isBefore == true)
            {
                if(currEventUIManager.isBeforeActionSet[currActionIdx] == false)
                {
                    actionType.value = 0;
                    actionType.RefreshShownValue();
                }
                else if(currEventUIManager.isBeforeActionSet[currActionIdx] == true)
                {
                    string paramName = savedTemplate.Name;

                    for (int i = 0; i < actionType.options.Count; i++)
                    {
                        if (actionType.options[i].text == paramName)
                        {
                            actionType.value = i;
                            actionType.RefreshShownValue();
                        }
                    }
                }
            }

        }

        public void clearProperty()
        {
            propertyVisList.Clear();

            for (int i = 0; i < uiPropertyPosition.transform.childCount; i++)
            {
                GameObject.Destroy(uiPropertyPosition.transform.GetChild(i).gameObject);
            }
        }

        public void OnVisualizePropertiesInit()
        {
            OnClearAction();
            OnVisualizeProperties();
        }

        public void OnVisualizeProperties()
        {
            clearProperty();

            Debug.Log("For " + currEventUIManager.uniqueName);

            if (isBefore == true)
            {
                if (currEventUIManager.isBeforeActionSet[currActionIdx] == true)
                {
                    if (savedTemplate.Name == actionType.options[actionType.value].text)
                    {
                        Debug.Log("Detect same...");
                        currTemplate = new ActionPrimitivesTemplate("", "");

                        var ct = currTemplate as PAUTPrimitivesTemplate;

                        PAUTPrimitivesTemplate.CopyPAUT(savedTemplate, ref ct);
                    }
                }
                else
                {
                    Debug.Log("For isSetTransitioNOnce...");
                    //일단 새롭게 만들기

                    currTemplate = new ActionPrimitivesTemplate("", "");

                    var ct = currTemplate as PAUTPrimitivesTemplate;

                    //기본값 가지고 복사하기
                    PAUTPrimitivesTemplate.CopyPAUT(actionTemplate[actionType.value], ref ct);
                }
            }
            else if (isBefore == false)
            {
                if (currEventUIManager.isAfterActionSet[currActionIdx] == true)
                {
                    if (savedTemplate.Name == actionType.options[actionType.value].text)
                    {
                        currTemplate = new ActionPrimitivesTemplate("", "");

                        var ct = currTemplate as PAUTPrimitivesTemplate;

                        PAUTPrimitivesTemplate.CopyPAUT(savedTemplate, ref ct);
                    }
                }
                else
                {
                    Debug.Log("For isSetTransitioNOnce...");
                    //일단 새롭게 만들기

                    currTemplate = new ActionPrimitivesTemplate("", "");

                    var ct = currTemplate as PAUTPrimitivesTemplate;

                    //기본값 가지고 복사하기
                    PAUTPrimitivesTemplate.CopyPAUT(actionTemplate[actionType.value], ref ct);
                }
            }
            
            for (int i = 0; i < currTemplate.getNumberOfParameter(); i++)
            {
                ParameterConversion pc = currTemplate.getParameterValue(i);

                string paramName = pc.getParamName();
                PARAMTYPE pm = pc.getParameterType();

                GameObject go = (GameObject)GameObject.Instantiate(propertyUIPrefab, uiPropertyPosition.transform);

                Debug.Log(go.name);

                go.name = paramName;

                if (pm == PARAMTYPE.BOOL)
                {
                    go.GetComponent<PropertyVisualizeHandler>().visualizePropertyRaw(paramName, VISUALIZEPROPTYPE.TOGGLE, pc.getParameter(), null, i+1);
                }
                else
                {
                    go.GetComponent<PropertyVisualizeHandler>().visualizePropertyRaw(paramName, VISUALIZEPROPTYPE.TEXTINPUT, pc.getParameter(), null, i+1);
                }

                propertyVisList.Add(go.GetComponent<PropertyVisualizeHandler>());
            }
        }



        public void OnCancel()
        {
            
        }

        void Start()
        {
            propertyUIPrefab = Resources.Load(propertyUIPrefabPath);
        }


    }
}
