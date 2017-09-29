using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    /*
    * asset의 variable을 explicit 시키기 위한 요소
    */
    public class VariableXmlTemplate : XmlTemplate
    {
        string propertyValue;

        public void setValue(string _value)
        {
            propertyValue = _value;
        }

        public string getValue()
        {
            return propertyValue;
        }

        public override XmlElement XmlSerialize(XmlDocument document, XmlElement parentElement, bool isRoot)
        { 
            string propertyName = Name;
            string propertyType = Type;
            

            
            
            
            
            XmlElement individualProperty = document.CreateElement("vrat.VariableXmlTemplate");
            parentElement.AppendChild(individualProperty);

            individualProperty.SetAttribute("name", propertyName);
            individualProperty.SetAttribute("type", propertyType);
            individualProperty.SetAttribute("value", propertyValue);

            

            return individualProperty;

        }

        public VariableXmlTemplate(string _name, string _type) : base(_name, _type)
        {
            ClassName = "VariableXmlTemplate";
        }
    }
}

