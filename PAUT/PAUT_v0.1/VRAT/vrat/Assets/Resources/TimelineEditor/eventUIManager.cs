using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace vrat
{
    /*
     * timeline window에서event ui manager를 담당함
     * 
     * */

    public enum DETECTDROPREGION
    {
        BEFOREACTION, AFTERACTION
    } 
     
    public class eventUIManager : FileUITemplateManager
    {
          
        //붙여진 asset의 image
        [SerializeField]
        UnityEngine.UI.RawImage attachedAssetPrevImg;

        //붙여진 asset의 text
        [SerializeField]
        UnityEngine.UI.Text attachedAssetNameUI;


        //instruction 영역임
        [SerializeField]
        UnityEngine.UI.InputField instField;

        [SerializeField]
        UnityEngine.UI.Image instButton;

        [SerializeField]
        UnityEngine.UI.Image transitionbutton;

        [SerializeField]
        UnityEngine.UI.Image beforeButton;

        [SerializeField]
        UnityEngine.UI.Image afterbutton;

        [SerializeField]
        List<UnityEngine.UI.Button> beforeActionImg;

        [SerializeField]
        List<UnityEngine.UI.Button> afterActionImg;

        

        


         
        public bool isFinalEvent = false;

        public bool isBegenEvent = false;

        //부모임

        Transform parent;

        
        TimelineEditor timelineEditor;


        public List<ActionPrimitivesTemplate> beforeActionList = new List<ActionPrimitivesTemplate>();
        public List<ActionPrimitivesTemplate> afterActionList = new List<ActionPrimitivesTemplate>();

        public int currSelectedActionIdx = 0;
        public bool currIsSelectedActionBefore = false;


        public TriggerPrimitivesTemplate transitionValue = new TriggerPrimitivesTemplate("","");


        public InstPrimitivesTemplate instructionValue;


        string instText;

        //선택 안되었을 시의 색깔
        Color unselectedColor = new Color(0.5f,0.5f,0.5f);

        Color selectedInst = new Color(0.39f,0.39f,0);
        Color selectedTransition = new Color(0,0.58f,1);
        Color seletedAction = new Color(1,0.4f,0);

        public bool isTransitionSet = false;

        //각 list별로 설정이 되어 있는지 확인
        public bool[] isBeforeActionSet = new bool[3] { false,false,false};
        public bool[] isAfterActionSet = new bool[3] { false,false,false};


        //버튼을 눌러서 action property를 보여줄 경우에 나타나는 효과
        public void OnClickBeforeAction( int idx)
        {
            timelineEditor.OnOpenActionWindowWithClick(this, true, idx);
        }
        public void OnClickAfterAction( int idx)
        {
            timelineEditor.OnOpenActionWindowWithClick(this, false, idx);
        }

        //이미지 바꾸기
        public void OnSetActionPrevImg(bool isBefore, Texture2D _texture)
        {
            if (isBefore == true)
            {
                if (currSelectedActionIdx >= 0 && currSelectedActionIdx < 3)
                {
                    if(beforeActionImg[currSelectedActionIdx].GetComponent<RawImage>().texture == null)
                        beforeActionImg[currSelectedActionIdx].GetComponent<RawImage>().texture = _texture;
                }
            }
            else if (isBefore == false)
            {
                if (currSelectedActionIdx >= 0 && currSelectedActionIdx < 3)
                {
                    if(afterActionImg[currSelectedActionIdx].GetComponent<RawImage>().texture == null)
                        afterActionImg[currSelectedActionIdx].GetComponent<RawImage>().texture = _texture;
                }
            }
        }
        
        public void OnRmoveActionPrevImg(bool isBefore)
        {
            if (isBefore == true)
            {
                if (currSelectedActionIdx >= 0 && currSelectedActionIdx < 3)
                    beforeActionImg[currSelectedActionIdx].GetComponent<RawImage>().texture = null;
            }
            else if (isBefore == false)
            {
                if (currSelectedActionIdx >= 0 && currSelectedActionIdx < 3)
                    afterActionImg[currSelectedActionIdx].GetComponent<RawImage>().texture = null;
            }
        }

        public void OnClickTransitionParamButton()
        {
            if(isTransitionSet == true)
            {
                
                OnOpenTransitionParamWindow();
            }
            else
            {
                Debug.Log("No transition param...");
            }
        }

        //지워진 경우
        public void OnCallbackDeleteTransitionParam()
        {
            isTransitionSet = false;

            OnColorChangeForButton(transitionbutton, false, new Color());
            
        }

        public void OnColorChangeForButton(UnityEngine.UI.Image img, bool isSet, Color c)
        {
            if(isSet == false)
            {
                img.color = unselectedColor;
            }
            else
            {
                img.color = c;
            }
        }

        public void OnCallbackDeleteAction(ActionPrimitivesTemplate apt, int idx, bool isBefore)
        {
            Debug.Log("Delete for idx " + idx.ToString());

            if(isBefore == true)
            {
                if(isBeforeActionSet[idx] == true)
                {
                    isBeforeActionSet[idx] = false;
                    beforeActionList[idx] = new ActionPrimitivesTemplate("", "");
                }

                bool isSetAll = false;

                for (int i = 0; i < isBeforeActionSet.Length; i++)
                {
                    if(isBeforeActionSet[i] == true)
                    {
                        isSetAll = true;
                    }
                }
                if(isSetAll == false)
                {
                    OnColorChangeForButton(beforeButton, false, unselectedColor);
                }
                else
                {
                    OnColorChangeForButton(beforeButton, true, seletedAction);
                }

                OnRmoveActionPrevImg(isBefore);


            }
            else if(isBefore == false)
            {

                if (isAfterActionSet[idx] == true)
                {
                    isAfterActionSet[idx] = false;
                    afterActionList[idx] = new ActionPrimitivesTemplate("", "");
                }

                bool isSetAll = false;

                for (int i = 0; i < isAfterActionSet.Length; i++)
                {
                    if (isAfterActionSet[i] == true)
                    {
                        isSetAll = true;
                    }
                }
                if (isSetAll == false)
                {
                    OnColorChangeForButton(afterbutton, false, unselectedColor);
                }
                else
                {
                    OnColorChangeForButton(afterbutton, true, seletedAction);
                }
                OnRmoveActionPrevImg(isBefore);

            }
            
        }

        public void OnUpdateInstruction()
        {
            InstPrimitivesTemplate ipt = new InstPrimitivesTemplate("","");
            ipt.setAttachedAssetName(attachedAssetNameStr);
            ipt.setParameterValue("Instruction", instText);

            setInstruction(ipt);

        }

        public void OnUpdateAllActions()
        {
            //일단 임시로 여기서 attatched asset 이름을 설정하자

            (eventInfo.variableContainer.getParameters("AttachedAsset") as PrimitiveXmlTemplate).setparameter(attachedAssetNameStr);
            

            for(int i=0; i<beforeActionList.Count; i++)
            {
                if (isBeforeActionSet[i] == true)
                    setBeforeAction(beforeActionList[i]);
            }
            for(int i=0; i<afterActionList.Count; i++)
            {
                if (isAfterActionSet[i] == true)
                    setAfterAction(afterActionList[i]);
            }
        }



        public void OnCallbackSetAction(ActionPrimitivesTemplate apt, int idx, bool isBefore)
        {
            if(isBefore == true)
            {
                
                var temp = beforeActionList[idx] as PAUTPrimitivesTemplate;

                var apt_var = apt as PAUTPrimitivesTemplate;

                isBeforeActionSet[idx] = true;

                OnColorChangeForButton(beforeButton, true, seletedAction);
                
                PAUTPrimitivesTemplate.CopyPAUT(apt_var, ref temp);

                OnSetActionPrevImg(isBefore, timelineEditor.currPreviewImg);

                //저장하기
            }
            else if(isBefore == false)
            {
                var temp = afterActionList[idx] as PAUTPrimitivesTemplate;
                var apt_var = apt as PAUTPrimitivesTemplate;

                isAfterActionSet[idx] = true;

                OnColorChangeForButton(afterbutton, true, seletedAction);

                OnSetActionPrevImg(isBefore, timelineEditor.currPreviewImg);

                PAUTPrimitivesTemplate.CopyPAUT(apt_var, ref temp);
            }

            //최종적으로 update하기
        }


        


        //선택하기
        public void OnCallbackSetTransitionParam(TriggerPrimitivesTemplate tpt)
        {
            PAUTPrimitivesTemplate tpt_base = tpt as PAUTPrimitivesTemplate;

            var transitionValue_base = transitionValue as PAUTPrimitivesTemplate;

            PAUTPrimitivesTemplate.CopyPAUT(tpt_base, ref transitionValue_base);

            //transitionValue = tpt;

            //isTransitionSet true로 바꾸기
            isTransitionSet = true;

            //연결된 asset 알려주기
            tpt.setAttachedAssetName(attachedAssetNameStr);

            //trigger 설정하기
            setTrigger(tpt);

            //eventInfo.addTrigger(tpt);

            OnColorChangeForButton(transitionbutton, true, selectedTransition);
        }
        
         //line drop시 transition param 설정하는 창이 나오도록 하도록 timeline에 callback 보내기
        public void OnOpenTransitionParamWindow()
        {
            timelineEditor.OnOpenTransitionWindow(this);
        }
        //actoin param window 열기 전에 설정해줘야 할 것임

            //이 부분은 걍 추가했을 떄로 하자
        public bool OnOpenActionParamWindow()
        {
            //만일 before가 열렸을 경우
            if (currIsSelectedActionBefore == true)
            {
                int idx = getCurrAvailableActionIdx(isBeforeActionSet);

                if (idx < 0)
                {
                    Debug.Log("Before action list is full...");
                    return false;
                }
                else
                {
                    currSelectedActionIdx = idx;
                    return true;
                }
            }
            else
            {
                int idx = getCurrAvailableActionIdx(isAfterActionSet);

                if (idx < 0)
                {
                    Debug.Log("After action list is full...");
                    return false;
                }
                else
                {
                    currSelectedActionIdx = idx;
                    return true;
                }
            }
        }

        public int getCurrAvailableActionIdx(bool[] isActionList)
        {
            for(int i=0; i<isActionList.Length; i++)
            {
                Debug.Log(isActionList[i]);

                if (isActionList[i] == false)
                    return i;
            }
            //-1일시 자리 없음
            return -1;
        }

        public string uniqueName = "";
        public string attachedAssetNameStr = "";

        bool isInstSet = false;


        [SerializeField]
        NodeLineManager inNodeLine;

        [SerializeField]
        NodeLineManager outNodeLine;

        //from event의 이름
        public string uniqueNameFromEvent="";

        //to event의 이름
        public string uniqueNameToEvent="";

        public void OnValueChangedForInst()
        {
           instText = instField.text;

            //turn 바꾸기
            if (instText == "")
            {
                instButton.color = unselectedColor;
                isInstSet = false;
            }
            else
            {
                instButton.color = selectedInst;
                isInstSet = true;
            }
        }



        //from connect를 clear하기
        public void clearFromConnect()
        {
            uniqueNameFromEvent = "";

            inNodeLine.OnClearNode();
               
        }
        //to connet를 clear하기
        public void clearToConnect()
        {
            uniqueNameToEvent = "";
            outNodeLine.OnClearNode();
        }

        //연결될 경우 불리는 callback
        public void OnConnectSuccess()
        {
            Debug.Log("On connect sucess with " + uniqueName);
            OnOpenTransitionParamWindow();
        }


        //asset 이름과 texture 업데이트하기
        public void OnUpdateAttachedAssetInfo(string _name, Texture2D _texture)
        {
            OnUpdateAssetName(_name);
            OnUpdateAssetImg(_texture);
        }

        void OnUpdateAssetName(string _name)
        {
            attachedAssetNameStr = _name;
            attachedAssetNameUI.text = _name;
        }

        void OnUpdateAssetImg(Texture2D _texture)
        {
            attachedAssetPrevImg.texture = _texture;
        }

        public void OnPressDeleteEvent()
        {
            timelineEditor.OnDeleteEvent(uniqueName);



        }


        void Start()
        {
            //parent 찾기    

            //3개로 미리 정해져 있음
            beforeActionList.Add(new ActionPrimitivesTemplate("",""));
            beforeActionList.Add(new ActionPrimitivesTemplate("", ""));
            beforeActionList.Add(new ActionPrimitivesTemplate("", ""));

            afterActionList.Add(new ActionPrimitivesTemplate("", ""));
            afterActionList.Add(new ActionPrimitivesTemplate("", ""));
            afterActionList.Add(new ActionPrimitivesTemplate("", ""));


            parent = transform.parent;
            
        }


        //event 이름 입력하는 곳
        [SerializeField]
        UnityEngine.UI.InputField eventName;

        [SerializeField]
        eventInnerTypeManager triggerManager;

        [SerializeField]
        eventInnerTypeManager beforeActionManager;

        [SerializeField]
        eventInnerTypeManager afterActionManager;

        [SerializeField]
        eventInnerTypeManager instManager;

        int fromIdx = -1;
        int toIdx = -1;

        

        public void setFromIdx(int _fromIdx)
        {
            fromIdx = _fromIdx;
        }

        public void setToIdx(int _toIdx)
        {
            toIdx = _toIdx;
        }

        public int getFromIdx()
        {
            return fromIdx;
        }
        public int getToIdx()
        {
            return toIdx;
        }

        //before action list에 들어간 element 개수
        int beforeActionCount = 0;
        //after action list에 들어간 element 개수
        int afterActionCount = 0;

        //primitves list에서의 trigger의 idx임
        //이거 설정해야 추후에 update를 원활하게 할 수 있음
        int attachedTriggerIdx = -1;
        int attachedInstIdx = -1;
        int[] attachedBeforeActionIdx = new int[3];
        int[] attachedAfterActionIdx = new int[3];

        //properties를 업데이트하는 함수
        public delegate void OnUpdateProperties(int primIdx);
        public OnUpdateProperties callback;
        
        
        //update variable하기
        public void OnUpdateVariables(List<primitivesUIManager> _list)
        {
            if (attachedTriggerIdx >=0)
            {
                setTrigger(_list[attachedTriggerIdx].getPAUTPrimitiveTemplate());
            }
            if (attachedInstIdx >= 0)
            {
                setInstruction(_list[attachedInstIdx].getPAUTPrimitiveTemplate());
            }
            for(int i=0; i<attachedBeforeActionIdx.Length; i++)
            {
                if (attachedBeforeActionIdx[i] >= 0)
                {
                    //LHWLHWLHW
                    pushBeforeAction(_list[attachedBeforeActionIdx[i]].getPAUTPrimitiveTemplate());
                    //BEFORE< AFTERACTION의 업데이트 하기
                }
            }
            for(int i=0; i<attachedAfterActionIdx.Length; i++)
            {
                if (attachedAfterActionIdx[i] >= 0)
                {
                    pushAfterAction(_list[attachedAfterActionIdx[i]].getPAUTPrimitiveTemplate());
                }
            }
        }


        

        

        

       




        //본 eventUIManager와 연결된 authorableEvent를 저장함
        public AuthorableEvent eventInfo = new AuthorableEvent();

        public bool isInitEvent = false;

        

        public void initEvent()
        {
            if (isInitEvent == false)
            {
                eventInfo.initialize();
                isInitEvent = true;
                for (int i = 0; i < 3; i++)
                {
                    attachedBeforeActionIdx[i] = -1;
                    attachedAfterActionIdx[i] = -1;
                }

                

            }
        }

        public void setTimelineEditor(TimelineEditor _timelineEditor)
        {
            timelineEditor = _timelineEditor;
        }

        public void initialize()
        {
            GetComponent<DragMeEvent>().OnSetCallbackDragEnd(OnDragEnd);
        }

        //여기에서는 걍 위치 이동하면 됨
        public void OnDragEnd(PointerEventData eventData)
        {
            Vector2 pos = eventData.position;

            OnMoveToMousePos(pos);
            

            
        }

        public void OnMoveToMousePos(Vector2 pos)
        {
            var rt = GetComponent<RectTransform>();

            float width = rt.rect.width;
            float height = rt.rect.height;

            Vector2 mousePosOnLocal = new Vector2();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent.GetComponent<RectTransform>(), pos, Camera.main, out mousePosOnLocal);


            Vector2 newPos = new Vector2();

            newPos.x = mousePosOnLocal.x - rt.rect.width / 2;
            newPos.y = mousePosOnLocal.y + rt.rect.height / 2;
            
            rt.anchoredPosition = newPos;

            inNodeLine.OnUpdateConnect();
            outNodeLine.OnUpdateConnect();
            


        //연결된 line에 대해서 update해줘야 함


    }




        
        //event에 drop 되었는지 확인하기
        public bool isDropOnEvent(Vector2 position, ref DETECTDROPREGION dropSign)
        {
            dropSign = DETECTDROPREGION.AFTERACTION;
            
            if (beforeActionManager.isDetectDrop(position) == true)
            {
                dropSign = DETECTDROPREGION.BEFOREACTION;
                return true;
            }
            if (afterActionManager.isDetectDrop(position) == true)
            {
                dropSign = DETECTDROPREGION.AFTERACTION;
                return true;
            }

            Debug.Log("No drop on inner component...");
            return false;
        }

        public void setTrigger(PAUTPrimitivesTemplate _origin)
        {
            //일단 삭제한 후
            deleteTrigger();
            //다시 집어넣기
            eventInfo.addTrigger(_origin);
        }
        public void deleteTrigger()
        {
            (eventInfo.variableContainer.getParameters(1) as PAUTPrimitivesTemplate).printAllParameter();

            eventInfo.deleteTrigger();
            eventInfo.initialize();
        }
        public void setInstruction(PAUTPrimitivesTemplate _origin)
        {
            deleteInstruction();
            eventInfo.addInstruction(_origin);
            Debug.Log("Instruction is set...");
        }
        public void deleteInstruction()
        {
            eventInfo.deleteInstruction();
            Debug.Log("Instruction is deleted...");
        }

        public void setBeforeAction(PAUTPrimitivesTemplate _action )
        {
            eventInfo.addBeforeAction(_action);
        }
        public void setAfterAction(PAUTPrimitivesTemplate _action)
        {
            eventInfo.addAfterAction(_action);
        }
        


        public void pushBeforeAction(PAUTPrimitivesTemplate _origin)
        {
            if (beforeActionCount == 3)
            {
                Debug.Log("Already full of before action list...");
                return;
            }
            beforeActionCount++;

            //eventInfo.addBeforeAction(_origin);
        }
        
        public void pushAfterAction(PAUTPrimitivesTemplate _origin)
        {
            if (afterActionCount == 3)
            {
                Debug.Log("Already full of after action list...");
                return;
            }
            afterActionCount++;

            //eventInfo.addAfterAction(_origin);
        }
        public void popBeforeAction(PAUTPrimitivesTemplate _origin)
        {
            if (beforeActionCount > 0)
            {
                //eventInfo.deleteBeforeAction(_origin);
                beforeActionCount--;
                return;
            }
            Debug.Log("No element in beforeactionlist...");

        }
        public void popAfterAction(PAUTPrimitivesTemplate _origin)
        {
            if (afterActionCount > 0)
            {
                //eventInfo.deleteAfterAction(_origin);
                afterActionCount--;
                return;
            }
            Debug.Log("No element in afteractionlist...");
        }
    }
}