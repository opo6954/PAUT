using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

/*
 * 
 * timeline editor임
 * timeline을 수정할 수 있음
 * 
 * 일단 가능한 trigger list, action list, instrution list를 읽는 작업이 필요함
 * 
 * 이후에 onclickevent 만들고
 * 
 * properties에 파라미터 넣는 것도 필요하고 + Name
 *  
 * 그리고 properties와 name을 실시간 동기화 하자
 * 
 * */
namespace vrat
{
       
    public class TimelineEditor :  WindowTemplate
    { 
        //asset list의 instance임
        [SerializeField]
        AssetListWindowHandler assetListWindowHandler;
        

        //timeline에서 uiTemplate이 노이는 부분임
        [SerializeField]
        GameObject eventUITemplatePosition;


        //action property window의 instance임
        [SerializeField]
        ActionParamWindowHandler actionPropertyWindow;


        //transition property window의 instance임
        [SerializeField]
        TransitionParamWindowHandler transitionPropertyWindow;



        [SerializeField]
        NodeLineManager beginNode;

        [SerializeField]
        NodeLineManager endNode;
        
               

        public AuthorableAsset currAsset;
        public Texture2D currPreviewImg;

        PAUTPrimitivesTemplate currPrimTemplate;

        //prim에서 property에서의 ui prefab임
        Object propertyUIPrimPrefab;


        //event의 prefab임
        Object eventUIPrefab;


        //file로부터 load한 trigger list
        List<TriggerPrimitivesTemplate> triggerList = new List<TriggerPrimitivesTemplate>();
        //file로부터 load한 action list
        List<ActionPrimitivesTemplate> actionList = new List<ActionPrimitivesTemplate>();



        List<PropertyVisualizeHandler> propertyVisList = new List<PropertyVisualizeHandler>();
        
        //eventUI에 대한 prefab임
        string eventUIPrefabPath = "TimelineEditor/eventUITemplate";

        string possibleTriggerPath = "/Resources/EventFiles/trigger/";
        string possibleActionPath = "/Resources/EventFiles/action/";

        //현재 respawn된 eventUIManagerList임
        Dictionary<string, eventUIManager> eventuiManagerDic = new Dictionary<string, eventUIManager>();
        

        //drag start가 assetlist로부터 왔는지 체크하기
        bool isDragFromAssetList = false;
        
        //가장 최근 timeline window에서 drop을 한 positio을 저장함
        Vector2 currPointDropPosTimeline = new Vector2();




        //현재 가지고 있는 trigger이름
        string[] triggerNameList;

        //현재 가지고 있는 action이름
        string[] actionNameList;


        //처음 시작되는 event 이름임
        string firstEventName;
        
        





        public void initialize()
        {
            eventUIPrefab = Resources.Load(eventUIPrefabPath);

            //.trigger, .action 파일 불러오기
            OnLoadOriginPrimitivesFromFiles();
            currPreviewImg = new Texture2D(2, 2);

            actionPropertyWindow.gameObject.SetActive(false);
            transitionPropertyWindow.gameObject.SetActive(false);
        }

        
        void Start()
        {
            //초기화하기
            initialize();
            
        }

        public void OnDeleteEvent(string _uniqueName)
        {
            if (eventuiManagerDic.ContainsKey(_uniqueName) == true)
            {
                eventUIManager eum = eventuiManagerDic[_uniqueName];

                if(eum.uniqueNameFromEvent != "")
                {
                    if(eventuiManagerDic.ContainsKey(eum.uniqueNameFromEvent) == true)
                        eventuiManagerDic[eum.uniqueNameFromEvent].clearToConnect();
                }
                else if(eum.isBegenEvent == true)
                {
                    beginNode.OnClearNode();
                }

                if(eum.uniqueNameToEvent != "")
                {
                    if(eventuiManagerDic.ContainsKey(eum.uniqueNameToEvent))
                        eventuiManagerDic[eum.uniqueNameToEvent].clearFromConnect();
                }
                else if(eum.isFinalEvent == true)
                {
                    endNode.OnClearNode();
                    
                }
                



                GameObject.Destroy(eventuiManagerDic[_uniqueName].gameObject);

                

                eventuiManagerDic.Remove(_uniqueName);

                
            }
            else
            {
                Debug.Log("Not exist for given unique name " + _uniqueName);
            }
        }


        //일단 이름만 저장해놓자
        public void OnSetFirstEvent(string _uniqueName)
        {
            firstEventName = _uniqueName;
        }

        //original trigger, action, instruction을 저장된 file로부터 얻는다
        public void OnLoadOriginPrimitivesFromFiles()
        {
            //load trigger

            triggerList.Clear();
            actionList.Clear();

            if (System.IO.Directory.Exists(Application.dataPath + possibleTriggerPath) == false)
            {
                Debug.Log("No path of " + possibleTriggerPath + ", please check trigger file path...");
            }

            //모든 file을 읽어서 .trigger 파일을 확인합시다
            string[] p = System.IO.Directory.GetFiles(Application.dataPath + possibleTriggerPath);

            
            

            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";
                
                fileStructure.getFileNameNExtension(p[i], ref extension, ref fileName);

                //triggerfile일 경우 바로 deserialize해서 불러오자
                if (extension == "trigger")
                {
                    TriggerPrimitivesTemplate tpt = new TriggerPrimitivesTemplate("", "");


                    tpt.testDeserialize(Application.dataPath + possibleTriggerPath + fileName + "." + extension);

                    //list에 집어넣기
                    triggerList.Add(tpt);
                }
            }

            //load action

            if (System.IO.Directory.Exists(Application.dataPath + possibleActionPath) == false)
            {
                Debug.Log("No path of " + possibleActionPath + ", please check action file path...");
            }

            p = System.IO.Directory.GetFiles(Application.dataPath + possibleActionPath);

            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";

                fileStructure.getFileNameNExtension(p[i], ref extension, ref fileName);

                if (extension == "action")
                {
                    ActionPrimitivesTemplate apt = new ActionPrimitivesTemplate("", "");

                    apt.testDeserialize(Application.dataPath + possibleActionPath + fileName + "." + extension);

                    actionList.Add(apt);
                                        
                }
            }


            triggerNameList = new string[triggerList.Count];
            actionNameList = new string[actionList.Count];

            string[] triggerDesList = new string[triggerList.Count];
            string[] actionDesList = new string[actionList.Count];

            //triggerList를 통해서 모든 가능한 trigger의 이름, description을 넣기
            for (int i = 0; i < triggerList.Count; i++)
            {
                triggerNameList[i] = triggerList[i].Name;

                //일단 description은 이름을 넣자
                triggerDesList[i] = triggerList[i].Name;

                
            }

            for (int i = 0; i < actionList.Count; i++)
            {
                actionNameList[i] = actionList[i].Name;

                //일단 description은 이름을 넣자
                actionDesList[i] = actionList[i].Name;

            }



            transitionPropertyWindow.initialize(triggerList);
            actionPropertyWindow.initialize(actionList);
            


        }


       
        

        //저장할 때의 버튼임
        public void OnSaveProperties()
        {
            /*
            Debug.Log("Save...");
            OnGetValueFromProperty();
            */
        }

        public TriggerPrimitivesTemplate getTriggerPrimWithName(List<TriggerPrimitivesTemplate> _list, string _name)
        {
            
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Name == _name)
                {
                    return _list[i];
                }
            }

            

            //만일 없을 경우 걍 null을 return함
            return new TriggerPrimitivesTemplate("", "");
        }

        public ActionPrimitivesTemplate getActionPrimWithName(List<ActionPrimitivesTemplate> _list, string _name)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Name == _name)
                {
                    return _list[i];
                }
            }

            //만일 없을 경우 걍 null을 return함
            return new ActionPrimitivesTemplate("","");

        }


        //event를 respawn하는거임

        //몇 번 째 primitive idx를 가지고 계산함
        //일단 기본적으로 trigger가 있기 때문에 trigger 관련된 update를 해줘야 할텐데..

            //asset 붙이기 N 그림 업데이트하기
        public void OnRespawnEvent()
        {
            Debug.Log("Respawn for event...");

            GameObject go = (GameObject)Instantiate(eventUIPrefab, eventUITemplatePosition.transform);
            go.GetComponent<RectTransform>().anchoredPosition = currPointDropPosTimeline;

            eventUIManager eum = go.GetComponent<eventUIManager>();

            eum.initialize();

            //unique한 event 이름 만들기
            for (int i = 0; i < 100; i++)
            {
                string generatedName = "event" + i.ToString();
                if (eventuiManagerDic.ContainsKey(generatedName) == false)
                {
                    eum.uniqueName = generatedName;
                    break;
                }
            }

            eum.initEvent();

            eum.OnUpdateAttachedAssetInfo(currAsset.ObjectName, currPreviewImg);
            eum.setTimelineEditor(this);
            //eum.setOnDragListener(OnDragFromTimeline);
            eventuiManagerDic.Add(eum.uniqueName, eum);
        }

        


        //timeline window에서 drop이 detect될 경우에 불려짐
        public void OnDetectDropOnTimeline(PointerEventData data)
        {

            Debug.Log("Drop on timeline");

            RectTransformUtility.ScreenPointToLocalPointInRectangle(eventUITemplatePosition.GetComponent<RectTransform>(), new Vector2(data.position.x, data.position.y), Camera.main, out currPointDropPosTimeline);

            if (isDragFromAssetList == true)
            {
                //event에 한 경우 만일 before에 하거나 after에 한 경우

                DETECTDROPREGION dropSign = DETECTDROPREGION.AFTERACTION;
                string eventUniqueName = "";


                if (isDropOnAnyEventUI(data, ref dropSign, ref eventUniqueName) == true)
                {
                    eventUIManager eum = eventuiManagerDic[eventUniqueName];

                    if (dropSign == DETECTDROPREGION.BEFOREACTION)
                        eum.currIsSelectedActionBefore = true;
                    else
                        eum.currIsSelectedActionBefore = false;

                    OnOpenActionWindow(eventuiManagerDic[eventUniqueName]);
                    //actionPropertyWindow.gameObject.SetActive(true);
                }
                else//빈공간에 한 경우 event 생성
                {
                    OnRespawnEvent();
                }
                isDragFromAssetList = false;
            }
        }

        //window 열시
        public void OnOpenTransitionWindow(eventUIManager eum)
        {
            transitionPropertyWindow.OnSetEventUIManager(eum);
            
            transitionPropertyWindow.OnVisualizePropertiesInit();
            transitionPropertyWindow.gameObject.SetActive(true);
        }

        //그림을 클릭했을 때에 뜨는 창
        public void OnOpenActionWindowWithClick(eventUIManager eum, bool isBefore, int selectedIdx)
        {
            //idx는 주어짐
            //만일 비어 있을 경우 실행 안함

            if (isBefore == true)
            {
                if (eum.isBeforeActionSet[selectedIdx] == false)
                    return;
            }
            else if (isBefore == false)
            {
                if (eum.isAfterActionSet[selectedIdx] == false)
                    return;
            }


            eum.currSelectedActionIdx = selectedIdx;
            eum.currIsSelectedActionBefore = isBefore;



            actionPropertyWindow.OnSetEventUIManager(eum);
            actionPropertyWindow.OnVisualizePropertiesInit();
            actionPropertyWindow.gameObject.SetActive(true);

            if (isBefore == true)
                actionPropertyWindow.OnSetAssetName(eum.beforeActionList[selectedIdx].getAttachedAssetName());
            else
                actionPropertyWindow.OnSetAssetName(eum.afterActionList[selectedIdx].getAttachedAssetName());

        }

        public void OnOpenActionWindow(eventUIManager eum)
        {
            if (eum.OnOpenActionParamWindow() == true)
            {
                actionPropertyWindow.OnSetEventUIManager(eum);

                actionPropertyWindow.OnVisualizePropertiesInit();
                //창 보이기
                actionPropertyWindow.gameObject.SetActive(true);

                actionPropertyWindow.OnSetAssetName(currAsset.ObjectName);
            }
        }



        



        public bool isDropOnAnyEventUI(PointerEventData _data, ref DETECTDROPREGION _dropSign, ref string _uniqueName)
        {
            foreach(eventUIManager eum in eventuiManagerDic.Values)
            {
                if(eum.isDropOnEvent(_data.position, ref _dropSign) == true)
                {
                    _uniqueName = eum.uniqueName;
                    return true;
                }
            }
            return false;
        }

        //drag n drop을 timeline있는 상태에서 했을 경우 발생함
        public void OnSelectAsset(string _fileName, AuthorableAsset _currAssetInfo, Texture2D _currPreviewImg)
        {
            currAsset = _currAssetInfo;
            currPreviewImg = _currPreviewImg;
            isDragFromAssetList = true;
        }

        public void OnExportAll()
        {
            //최종 저장하는 거 짜자

            string startName = firstEventName;

            Debug.Log(startName);

            string currName = startName;

            AuthorableTimeline at = new AuthorableTimeline();
            at.initialize();

            (at.variableContainer.getParameters("Player") as PrimitiveXmlTemplate).setparameter("Sailor");

            for(int i=0; i<100; i++)
            {
                if (eventuiManagerDic.ContainsKey(currName) == true)
                {
                    eventUIManager eum = eventuiManagerDic[currName];
                    eum.OnUpdateAllActions();
                    eum.OnUpdateInstruction();
                    currName = eum.uniqueNameToEvent;

                    at.eventList.Add(eum.eventInfo);
                }
                else
                    break;
            }

            AuthorableScenario authorableScenario = new AuthorableScenario();

            authorableScenario.initialize();

            authorableScenario.ObjectName = "FireExtinguisherTraining";


            //역할 정보
            ListOfXmlTemplate lxtRole = authorableScenario.variableContainer.getParameters("TraineeRoleInfo") as ListOfXmlTemplate;

            lxtRole.addList(new PrimitiveXmlTemplate("Role1", "Captain", "string"));
            lxtRole.addList(new PrimitiveXmlTemplate("Role2", "Sailor1", "string"));

            //훈련 환경 정보(VR/AR/Desktop)
            ListOfXmlTemplate lxtCondition = authorableScenario.variableContainer.getParameters("TrainCondition") as ListOfXmlTemplate;

            lxtCondition.addList(new PrimitiveXmlTemplate("Platform1", "VR", "string"));
            lxtCondition.addList(new PrimitiveXmlTemplate("Platform2", "AR", "string"));
            lxtCondition.addList(new PrimitiveXmlTemplate("Platform3", "Desktop", "string"));


            //room 집어 넣기
            if (EnvironmentEditor._currRoomGlobal != null)
            {
                authorableScenario.roomList.Add(EnvironmentEditor._currRoomGlobal);

                Debug.Log("curr room global name is " + EnvironmentEditor._currRoomGlobal.ObjectName);

            }

            //asset 집어넣기
            for (int i = 0; i < AssetListWindowHandler.authorableAssetListGlobal.Count; i++)
            {
                authorableScenario.assetList.Add(AssetListWindowHandler.authorableAssetListGlobal[i]);
            }

            authorableScenario.timelineList.Add(at);

            //일단 임시로 걍 저장하자

            //.scenario 파일로 serailize하기
            authorableScenario.testSerialize("example.scenario");

            SubwindowManager.globalInstance.displayStatus("Save with example.scenario");






            /*
           
            //이제 모든 timeline 만들기

            AuthorableTimeline at = new AuthorableTimeline();
            at.initialize();
            //걍 1명에 대해서 수행하기(Sailor)
            (at.variableContainer.getParameters("Player") as PrimitiveXmlTemplate).setparameter("Sailor");

            for (int i = 0; i < eventListWithIdx.Count; i++)
            {
                //event 넣기
                //업데이트하기

                //eventuiManagerList[eventListWidthIdx[i]].eventInfo
                //LHWLHW

                eventuiManagerList[eventListWithIdx[i]].OnUpdateVariables(uiManagerList);

                at.eventList.Add(eventuiManagerList[eventListWithIdx[i]].eventInfo);
                
                
            }
            authorableScenario = new AuthorableScenario();

            authorableScenario.initialize();

            //임시로 이렇게 넣자
            authorableScenario.ObjectName = "FireExtinguisherTraining";

            //역할 정보
            ListOfXmlTemplate lxtRole = authorableScenario.variableContainer.getParameters("TraineeRoleInfo") as ListOfXmlTemplate;
            
            lxtRole.addList(new PrimitiveXmlTemplate("Role1", "Captain", "string"));
            lxtRole.addList(new PrimitiveXmlTemplate("Role2", "Sailor1", "string"));

            //훈련 환경 정보(VR/AR/Desktop)
            ListOfXmlTemplate lxtCondition = authorableScenario.variableContainer.getParameters("TrainCondition") as ListOfXmlTemplate;

            lxtCondition.addList(new PrimitiveXmlTemplate("Platform1", "VR", "string"));
            lxtCondition.addList(new PrimitiveXmlTemplate("Platform2", "AR", "string"));
            lxtCondition.addList(new PrimitiveXmlTemplate("Platform3", "Desktop", "string"));

            //room 집어 넣기
            if (EnvironmentEditor._currRoomGlobal != null)
            {
                authorableScenario.roomList.Add(EnvironmentEditor._currRoomGlobal);

                Debug.Log("curr room global name is " + EnvironmentEditor._currRoomGlobal.ObjectName);

            }

            //asset 집어넣기
            for (int i = 0; i < AssetListWindowHandler.authorableAssetListGlobal.Count; i++)
            {
                authorableScenario.assetList.Add(AssetListWindowHandler.authorableAssetListGlobal[i]);
            }

            authorableScenario.timelineList.Add(at);

            //일단 임시로 걍 저장하자

            //.scenario 파일로 serailize하기
            authorableScenario.testSerialize("power.scenario");



    */
        }
    }
}
