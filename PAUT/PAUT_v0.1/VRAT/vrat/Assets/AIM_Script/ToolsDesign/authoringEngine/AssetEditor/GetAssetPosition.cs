using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    /*
     * 이미 배치된 asset의 position, rotation을 가지고 와서 insitu에 저장하기
    */
    public class GetAssetPosition : MonoBehaviour {

        string fileSavePath = "";

        void Start()
        {
            fileSavePath = Application.dataPath + "/Resources/AssetFiles/";


            //LoadAssetNSavePos();




        }

        public void testScenario()
        {
            AuthorableScenario testScenario = new AuthorableScenario();

            testScenario.initialize();

            testScenario.testDeserialize("POAPAUT.scenario");


            /*
             *  //timeline 불러오기
            //원래는 여러명이라서 각기 다른 사람별로 eventlist 줬었는데 지금은 한 명이니 걍 0번째idx로eventlist 불러와도 됨

            List<AuthorableTimeline> _timelineList = testScenario.timelineList;

            List<AuthorableEvent> _eventList = _timelineList[0].eventList;
             * */

            List<AuthorableTimeline> _timeline = testScenario.timelineList;

            List<AuthorableEvent> _eventLIst = _timeline[0].eventList;

            
            //trigger 가져오기
            foreach (AuthorableEvent ae in _eventLIst)
            {
                Debug.Log("Attached Asset name: " + (ae.variableContainer.getParameters(0) as PrimitiveXmlTemplate).getVariable());

                TriggerPrimitivesTemplate tpt = ae.variableContainer.getParameters(1) as TriggerPrimitivesTemplate;

                Debug.Log("Trigger Name: " + tpt.Name);
                Debug.Log("Trigger Parameter: ");

                for(int i=0; i<tpt.getNumberOfParameter(); i++)
                {
                    Debug.Log(i.ToString() + "th parameer name: ");
                    ParameterConversion pc = tpt.getParameterValue(i);

                    Debug.Log("Param Name: " + pc.getParamName());
                    Debug.Log("Param Type: " + pc.getParameterType());
                    //만일 getParameterType()이 CHOICE일 경우 addional info 존재함
                    //Debug.Log("Param Addional info" + pc.getAdditionalInfo());
                    Debug.Log("Param Value: " + pc.getParameter());
                }
            }

            //before action list 가져오기
            foreach (AuthorableEvent ae in _eventLIst)
            {


                ListOfXmlTemplate lxt = ae.variableContainer.getParameters("BeforeActionList") as ListOfXmlTemplate;



                for(int j=0; j<lxt.getLengthofList(); j++)
                {
                    ActionPrimitivesTemplate apt = lxt.getXmlTemplate(j) as ActionPrimitivesTemplate;

                    Debug.Log("Action Name: " + apt.Name);

                    Debug.Log("Action Parameter: ");

                    for(int k=0; k<apt.getNumberOfParameter(); k++)
                    {

                        Debug.Log(k.ToString() + "th parameter name: ");

                        ParameterConversion pc = apt.getParameterValue(k);

                        Debug.Log("Param Name: " + pc.getParamName());
                        Debug.Log("Param Type: " + pc.getParameterType());
                        //만일 getParameterType()이 CHOICE일 경우 addional info 존재함
                        //Debug.Log("Param Addional info" + pc.getAdditionalInfo());
                        Debug.Log("Param Value: " + pc.getParameter());
                    }
                }
            }




        }

        public bool LoadAssetNSavePos()
        {
            Debug.Log("Update Directory View...");

            if (System.IO.Directory.Exists(fileSavePath) == false)
            {
                Debug.Log("No path of " + fileSavePath + ", please check out the asset file path");

                return false;
            }
            

            //assetSavePath에 있는 모든 파일 list를 가져오기
            string[] p = System.IO.Directory.GetFiles(fileSavePath);

            //.asset file만 추출
            for (int i = 0; i < p.Length; i++)
            {
                string fileName = "";
                string extension = "";

                fileStructure.getFileNameNExtension(p[i], ref extension, ref fileName);

                //.asset이 아닐 경우 제외함
                if (extension != "asset")
                    continue;
                Debug.Log(fileName);

                //바로 deserialize하기
                AuthorableAsset aa = new AuthorableAsset();
                aa.testDeserialize(p[i]);

                GameObject go = GameObject.Find(fileName);

                if (go != null)
                {
                    (aa.variableContainer.getParameters("Location") as LocationXmlTemplate).setParameter(new Location(go.transform.position, go.transform.rotation.eulerAngles));
                }

                aa.testSerialize(p[i]);

            }
            return true;
        }

    }
}