using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace vrat
{
    /*
     * node line을 관리하는 녀석
     * */

    public enum NODETYPE
    {
        IN_NODE, OUT_NODE
    }
     
    public class NodeLineManager : MonoBehaviour
    {

        //이 node의 line renderer임
        [SerializeField]
        LineRenderer lineRenderer;

        //현재 이 node의 type, in과 out으로 나눠짐
        [SerializeField]
        NODETYPE _myNodeType;

        //연결된 eventUIManager임
        [SerializeField]
        eventUIManager attachedEventUIManager;

        //반대편 node line manager를 가지고 있자
        [SerializeField]
        NodeLineManager oppositeNodeLineManager;

        [SerializeField]
        TimelineEditor timelineEditor;

        [SerializeField]
        Transform endPosition;
        

        [SerializeField]
        bool isStart = false;

        [SerializeField]
        bool isEnd = false;


        public static bool onConnecting = false;

        //from node의 lineManager를 global로 가지고 있자
        public static NodeLineManager onFromLineManager;

        //연결된 connectLineManager임
        public NodeLineManager connectLineManager;
        
        

        public bool isPressedInNode = false;

        //현재 conect를 하고 있는 지 여부
        bool isConnectingNow=false;

        


        public void OnClearNode()
        {
            isConnectingNow = false;

            if (connectLineManager != null)
            {
                connectLineManager.isConnectingNow = false;
            }

            if (_myNodeType == NODETYPE.OUT_NODE)
            {
                lineRenderer.SetPosition(0, new Vector3());
                lineRenderer.SetPosition(1, new Vector3());
            }
            //초기 시작 node일 시
            if(isStart == true)
            {
                Debug.Log("For Start Node...");
                lineRenderer.SetPosition(0, new Vector3());
                lineRenderer.SetPosition(1, new Vector3());
            }
        }

        //node 부분을 누를 경우
        public void OnClickNode()
        {
            //현재 연결중인지 확인해야 함
            //onConnecting인지 확인해야 함

            //처음 눌리는 경우
            if (NodeLineManager.onConnecting == false)
            {
                if (_myNodeType == NODETYPE.OUT_NODE)
                {
                    if (isConnectingNow == false)
                    {
                        NodeLineManager.onConnecting = true;
                        NodeLineManager.onFromLineManager = this;
                        isPressedInNode = true;
                    }
                }
            }
                //이미 다른 node에서 눌려진 경우, 이 node은 끝 node가 되겠지?
            else if (NodeLineManager.onConnecting == true)
            {
                //본인의 end node를 누르진 않았는지 확인해야함
                if (oppositeNodeLineManager.isPressedInNode == false)
                {

                    //in node로 연결된 경우
                    if (_myNodeType == NODETYPE.IN_NODE)
                    {
                        if (isConnectingNow == false)
                        {
                            //여기서 from에 대한 callback을 보내야 되는데
                            
                            NodeLineManager.onConnecting = false;
                            isPressedInNode = false;
                            NodeLineManager.onFromLineManager.isPressedInNode = false;
                            
                            isConnectingNow = true;
                            NodeLineManager.onFromLineManager.isConnectingNow = true;

                            connectLineManager = NodeLineManager.onFromLineManager;

                            NodeLineManager.onFromLineManager.connectLineManager = this;

                            
                            //isStart: 시작점
                            if (NodeLineManager.onFromLineManager.isStart == true)
                            {
                                NodeLineManager.onFromLineManager.timelineEditor.OnSetFirstEvent(attachedEventUIManager.uniqueName);
                                attachedEventUIManager.isBegenEvent = true;

                            }
                            else
                            {
                                //선의 시작점 eventUI에 다음 event 이름으로 이녀석의 이름을 저장하기

                                if (isEnd == false)
                                {
                                    NodeLineManager.onFromLineManager.attachedEventUIManager.uniqueNameToEvent = attachedEventUIManager.uniqueName;
                                    attachedEventUIManager.uniqueNameFromEvent = NodeLineManager.onFromLineManager.attachedEventUIManager.uniqueName;
                                    NodeLineManager.onFromLineManager.attachedEventUIManager.isFinalEvent = false;
                                }
                                else
                                {
                                    NodeLineManager.onFromLineManager.attachedEventUIManager.isFinalEvent = true;
                                }
                                


                                NodeLineManager.onFromLineManager.attachedEventUIManager.OnConnectSuccess();

                                //이전의 다음 line manager를 끝점의 linemanager로 설정하기
                            }

                            //시작 event의 다음 event 이름을 이 event 이름으로 설정한다

                            //connect될 경우 transition window를 연다d.

                            


                            OnUpdateConnect();

                            NodeLineManager.onFromLineManager = null;
                        }
                    }
                }
            }
        }

        //일단 대충 Connect update함수임
        public void OnUpdateConnect()
        {
            

            if (isConnectingNow == true)
            {
                if (_myNodeType == NODETYPE.IN_NODE)
                {
                    if(connectLineManager != null)
                    {
                        Vector2 newPos = Camera.main.WorldToScreenPoint(endPosition.transform.position);
                        Vector2 mousePosOnTimeEditor = new Vector2();
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(connectLineManager.GetComponent<RectTransform>(), newPos, Camera.main, out mousePosOnTimeEditor);

                        connectLineManager.lineRenderer.SetPosition(1, mousePosOnTimeEditor);
                    }
                    else
                    {
                        Debug.Log("no connect line manager exist for" + _myNodeType.ToString());
                    }
                }
                //out node일 경우
                else
                {
                    if (connectLineManager != null)
                    {
                        
                        Vector2 newPos = Camera.main.WorldToScreenPoint(connectLineManager.endPosition.transform.position);
                        Vector2 mousePosOnTimeEditor = new Vector2();
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), newPos, Camera.main, out mousePosOnTimeEditor);
                        lineRenderer.SetPosition(1, mousePosOnTimeEditor);



                        
                        newPos = Camera.main.WorldToScreenPoint(endPosition.transform.position);

                        mousePosOnTimeEditor = new Vector2();
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), RectTransformUtility.WorldToScreenPoint(Camera.main, endPosition.transform.position), Camera.main, out mousePosOnTimeEditor);
                        lineRenderer.SetPosition(0, mousePosOnTimeEditor);
                        




                    }
                    else
                    {
                        Debug.Log("no connect line manager exist for" + _myNodeType.ToString());
                    }
                }

                

            }
        }

        void Update()
        {
            if (NodeLineManager.onConnecting == true && isPressedInNode == true)
            {
                //이제 제대로 mouse position을 hooking해야 함

                Vector2 mousePosOnTimeEditor = new Vector2();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out mousePosOnTimeEditor);

                Vector3 linePos = new Vector3(mousePosOnTimeEditor.x, mousePosOnTimeEditor.y);
                

                lineRenderer.SetPosition(1, linePos);
            }
                
        }

    }
}
