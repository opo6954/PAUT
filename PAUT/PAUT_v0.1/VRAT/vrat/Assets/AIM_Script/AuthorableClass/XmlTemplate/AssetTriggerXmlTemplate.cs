using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    /*
     * asset trigger를 위한 xml template임
     * 
     * 
     * 
     * 구성 요소:
     * //public void OnGetAssetTriggerInfo(string triggerName, string[] attachedActionLst, string[] actionList, string[] triggerParamList, string[] actionParamList)
     * 
    */
    public class AssetTriggerXmlTemplate : XmlTemplate
    {
         
        public string assetTriggerType = "";
        public List<string> attachedAssetList = new List<string>();
        public List<string> actionList = new List<string>();
        public List<string> triggerParamList = new List<string>();
        public List<string> actionParamList = new List<string>();

        public static void CopyValue(AssetTriggerXmlTemplate _origin,  ref AssetTriggerXmlTemplate _dest)
        {
            _dest.clearAllList();




            _dest.assetTriggerType = _origin.assetTriggerType;

            for(int i=0; i<_origin.attachedAssetList.Count; i++)
            {
                _dest.attachedAssetList.Add(_origin.attachedAssetList[i]);
            }
            for(int i=0; i<_origin.actionList.Count; i++)
            {
                _dest.actionList.Add(_origin.actionList[i]);
            }
            for(int i=0; i<_origin.triggerParamList.Count; i++)
            {
                _dest.triggerParamList.Add(_origin.triggerParamList[i]);
            }
            for (int i = 0; i < _origin.actionParamList.Count; i++)
            {
                _dest.actionParamList.Add(_origin.actionParamList[i]);
            }
        }

        public void clearAllList()
        {
            attachedAssetList.Clear();
            actionList.Clear();
            triggerParamList.Clear();
            actionParamList.Clear();


        }
        public void setValue(string _triggerName, string[] _attachedActionLst, string[] _actionList, string[] _triggerParamList, string[] _actionParamList)
        {
            assetTriggerType = _triggerName;
            clearAllList();

            for(int i=0; i<_attachedActionLst.Length; i++)
            {
                attachedAssetList.Add(_attachedActionLst[i]);
            }
            for(int i=0; i<_actionList.Length; i++)
            {
                actionList.Add(_actionList[i]);
            }
            for (int i = 0; i < _triggerParamList.Length; i++)
            {
                triggerParamList.Add(_triggerParamList[i]);
            }
            for (int i = 0; i < _actionParamList.Length; i++)
            {
                actionParamList.Add(_actionParamList[i]);
            }
        }

        public AssetTriggerXmlTemplate(string _name, string _type)
            : base(_name, _type)
        {
            ClassName = "AssetTriggerXmlTemplate";

            assetTriggerType = "";

            attachedAssetList = new List<string>();
            actionList = new List<string>();
            triggerParamList = new List<string>();
            actionParamList = new List<string>();

    }


        

        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            string propertyName = Name;
            string propertyType = Type;
            string propertyTriggerType = assetTriggerType;


            //root 설정
            XmlElement individualProperty = document.CreateElement("vrat.AssetTriggerXmlTemplate");
            parentElement.AppendChild(individualProperty);

            individualProperty.SetAttribute("name", propertyName);
            individualProperty.SetAttribute("type", propertyType);

            //trigger type 설정
            XmlElement triggerTypeRoot = document.CreateElement("TriggerType");
            individualProperty.AppendChild(triggerTypeRoot);
            triggerTypeRoot.SetAttribute("name", "TriggerType");
            triggerTypeRoot.SetAttribute("type", ParameterConversion.str2ParamType("string").ToString());
            triggerTypeRoot.SetAttribute("triggerType", assetTriggerType);
            

            if (triggerParamList.Count < 3)
            {
                triggerTypeRoot.SetAttribute("triggerCondition", "");
                triggerTypeRoot.SetAttribute("triggerOperator", "");
                triggerTypeRoot.SetAttribute("triggerParamStr", "");
            }
            else if (triggerParamList.Count == 4)
            {
                triggerTypeRoot.SetAttribute("triggerCondition", triggerParamList[0]);
                triggerTypeRoot.SetAttribute("triggerOperator", triggerParamList[1]);
                triggerTypeRoot.SetAttribute("triggerParamStr", triggerParamList[2]);
                triggerTypeRoot.SetAttribute("triggerParamFloat", triggerParamList[3]);
            }
            else
            {
                triggerTypeRoot.SetAttribute("triggerCondition", triggerParamList[0]);
                triggerTypeRoot.SetAttribute("triggerOperator", triggerParamList[1]);
                triggerTypeRoot.SetAttribute("triggerParamStr", triggerParamList[2]);
            }

            //action list 설정
            XmlElement actionTypeList = document.CreateElement("ActionList");
            individualProperty.AppendChild(actionTypeList);

            //actionList별로만들기
            for (int i = 0; i < actionList.Count; i++)
            {
                XmlElement actionTypeElement = document.CreateElement("ActionElement");
                actionTypeList.AppendChild(actionTypeElement);

                actionTypeElement.SetAttribute("name", "ActionType");
                actionTypeElement.SetAttribute("type", ParameterConversion.str2ParamType("string").ToString());
                actionTypeElement.SetAttribute("actionType", actionList[i]);
                actionTypeElement.SetAttribute("attachedAsset", attachedAssetList[i]);
                actionTypeElement.SetAttribute("actionParamStr", actionParamList[2 * i]);
                actionTypeElement.SetAttribute("actionParamTh", actionParamList[2 * i + 1]);
            }

            return individualProperty;
        }

        public void deserializeFromXml(XmlNode xmlChildNode)
        {
            clearAllList();
            

            XmlNodeList xnl = xmlChildNode.ChildNodes;
            Name = xmlChildNode.Name;



            for (int i = 0; i < xnl.Count; i++)
            {
                string name = xnl[i].Name;

                if (name == "TriggerType")
                {
                    assetTriggerType = xnl[i].Attributes["triggerType"].InnerText;
                    triggerParamList.Add(xnl[i].Attributes["triggerCondition"].InnerText);
                    triggerParamList.Add(xnl[i].Attributes["triggerOperator"].InnerText);
                    triggerParamList.Add(xnl[i].Attributes["triggerParamStr"].InnerText);
                    triggerParamList.Add(xnl[i].Attributes["triggerParamFloat"].InnerText);
                }
                else if (name == "ActionList")
                {
                    for(int j=0; j<xnl[i].ChildNodes.Count ;j++)
                    {
                        XmlNode childInner = xnl[i].ChildNodes[j];
                        

                        if (childInner.Name == "ActionElement")
                        {
                            actionList.Add(childInner.Attributes["actionType"].InnerText);
                            attachedAssetList.Add(childInner.Attributes["attachedAsset"].InnerText);
                            actionParamList.Add(childInner.Attributes["actionParamStr"].InnerText);
                            actionParamList.Add(childInner.Attributes["actionParamTh"].InnerText);
                        }                        
                        
                    }
                }
                
            }
        }

        
    }
}