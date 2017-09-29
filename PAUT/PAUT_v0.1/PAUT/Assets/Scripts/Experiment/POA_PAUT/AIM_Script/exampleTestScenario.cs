using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    public class AuthoringTest2 : MonoBehaviour
    {
        void Start()
        {
            testScenario();
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

                for (int i = 0; i < tpt.getNumberOfParameter(); i++)
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



                for (int j = 0; j < lxt.getLengthofList(); j++)
                {
                    ActionPrimitivesTemplate apt = lxt.getXmlTemplate(j) as ActionPrimitivesTemplate;

                    Debug.Log("Action Name: " + apt.Name);

                    Debug.Log("Action Parameter: ");

                    for (int k = 0; k < apt.getNumberOfParameter(); k++)
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
    }
}