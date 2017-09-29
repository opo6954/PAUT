using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.IO;


namespace vrat
{
      
    /*
     * Asset serialize를 위한 class로써 asset의 모든 정보를 저장할 수 있음
     * */
    public class AuthorableAsset : ObjectTemplate
    { 
        //xml 형식으로 serialize하기
        //모든 obj별로 override하면 됨

        public List<AssetTriggerXmlTemplate> assetTriggerXmlTemplateList = new List<AssetTriggerXmlTemplate>();


        public override void initialize()
        {
            base.initialize();

            ObjectType = OBJTYPE.ASSET;

            assetTriggerXmlTemplateList.Clear();
            

            
            


            /*
             * common asset에 대한 trigger임
             * */
        }
         
        void Start()
        {
        }

        public override void testSerialize(string xmlName)
        {
            XmlDocument document = new XmlDocument();
            serialize2Xml(document, null, true);
            document.Save(xmlName);
             
        }

        public override bool serialize2Xml(XmlDocument document, XmlElement rootElement, bool isRoot)
        {
            XmlElement root = document.CreateElement(ObjectType.ToString());
            root.SetAttribute("name", ObjectName);

            if (isRoot == true)
            {
                document.AppendChild(root);
            }
            else
            {
                rootElement.AppendChild(root);
            }

            XmlElement xe = serializeCustomizedProperty2Xml(document, root);

            //이 부분에서 assetTriggerSerialize를 해야 함

            for (int i = 0; i < assetTriggerXmlTemplateList.Count; i++)
            {
                assetTriggerXmlTemplateList[i].XmlSerialize(document, xe, false);
            }

            return true;
        }
        protected override bool deserializeChildEachInside(XmlNodeList xnList)
        {
            

            foreach (XmlNode xn in xnList)
            {
                
                XmlAttributeCollection xac = xn.Attributes;

                //기본 primitiveXmlTemplate일 시
                if (xn.Name == "vrat.ListOfXmlTemplate")
                {
                    variableContainer.addParameter(deserializeChildEachForList(xn));
                }
                else
                {
                    XmlTemplate xt = createInstanceListElement(xn);
                    //만일 assetTrigger일 경우
                    if (xt.Name == "vrat.AssetTriggerXmlTemplate")
                    {
                        
                        //copy 진행하기
                        //읽어온거임
                        AssetTriggerXmlTemplate fromXml = xt as AssetTriggerXmlTemplate;
                        AssetTriggerXmlTemplate instanceNew = new AssetTriggerXmlTemplate("", "");

                        
                        instanceNew.clearAllList();

                        instanceNew.assetTriggerType = fromXml.assetTriggerType;
                        
                        
                        for(int i=0; i<fromXml.attachedAssetList.Count; i++)
                        {
                            instanceNew.attachedAssetList.Add(fromXml.attachedAssetList[i]);
                        }
                        for (int i = 0; i < fromXml.actionList.Count; i++)
                        {
                            instanceNew.actionList.Add(fromXml.actionList[i]);
                        }
                        for (int i = 0; i < fromXml.triggerParamList.Count; i++)
                        {
                            instanceNew.triggerParamList.Add(fromXml.triggerParamList[i]);
                        }
                        for (int i = 0; i < fromXml.actionParamList.Count; i++)
                        {
                            instanceNew.actionParamList.Add(fromXml.actionParamList[i]);
                        }

                        //asset trigger list에 집어넣기
                        assetTriggerXmlTemplateList.Add(instanceNew);

                    }
                    else
                    {

                        if (variableContainer.checkParameter(xt.Name) == true)
                        {
                            Debug.Log("Already exist for " + xt.Name);
                        }

                        variableContainer.addParameter(xt);
                    }
                }
            }

            return true;
        }
        
        


        //일단 임시로 각 asset별로의 example xml serialize를 제공해보자
        public void exampleFireSerialize()
        {
            ObjectName = "fire";

            variableContainer.addParameter(new PrimitiveXmlTemplate("IsGraspable","false", "bool"));
            variableContainer.addParameter(new LocationXmlTemplate("Location", "location", new Location(new Vector3(10, 20, 30), new Vector3(50, 70, 210))));
            variableContainer.addParameter(new VariableXmlTemplate("Life", "float"));//explicit된 형태의 variable임
            variableContainer.addParameter(new VariableXmlTemplate("isDetected", "bool"));//explicit된 형태의 variable임
        }
        public void exampleExtinguisherSerialize()
        {
            ObjectName = "extinguisher";

            variableContainer.addParameter(new PrimitiveXmlTemplate("IsGraspable","true","bool"));
            variableContainer.addParameter(new LocationXmlTemplate("Location", "location", new Location(new Vector3(10, 20, 30), new Vector3(50, 70, 210))));
            ListOfXmlTemplate holdGesture = new ListOfXmlTemplate("HoldGestureType", "ListOfXmlTemplate", 0);
            ListOfXmlTemplate extinguishGesture = new ListOfXmlTemplate("ExtinguishGestureType", "ListOfXmlTemplate", 0);

            holdGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownZ", "z", "string"));
            holdGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownX", "x", "string"));
            holdGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownC", "c", "string"));

            extinguishGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownZ", "z", "string"));
            extinguishGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownX", "x", "string"));
            

            variableContainer.addParameter(holdGesture);
            variableContainer.addParameter(extinguishGesture);

        }
        public void examplePhoneSerialize()
        {
            ObjectName = "phone";

            variableContainer.addParameter(new PrimitiveXmlTemplate("IsGraspable","false", "bool"));
            variableContainer.addParameter(new LocationXmlTemplate("Location", "location", new Location(new Vector3(10, 20, 30), new Vector3(50, 70, 210))));
            ListOfXmlTemplate reportGesture = new ListOfXmlTemplate("ReportGestureType", "ListOfXmlTemplate", 0);

            
            reportGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownX", "x", "string"));
            reportGesture.addList(new PrimitiveXmlTemplate("KeyboardButtonDownC", "c", "string"));

            variableContainer.addParameter(reportGesture);
        }
    }
}
