using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vrat
{
    
    public enum ACTIONTYPE
    {
        ACTIVERENDER, DEACTIVERENDER, TRANSLATE, SETPOSITION, ENABLEASSETTRIGGER, DISABLEASSETTRIGGER
    }

    /*
     * Action 종류
     * 
     * DeactiveRender
     * ActiveRender
     * 
     * EnableAssetTrigger
     * DisableAssetTrigger
     * 
     * StartParticle
     * StopParticle
     * 
     * 
     * */
    
     


    public class ActionPrimitivesTemplate : PAUTPrimitivesTemplate
    {
        //붙일 수 있는 특수한 asset type을 여기서 명시하자

        public ActionPrimitivesTemplate(string _name, string _type)
            : base(_name, _type)
        {
            ClassName = "ActionPrimitivesTemplate";
        }


        public void exampleDecreaseFloatVariable()
        {
            Name = "DecreaseFloatVariable";

            ParameterConversion pc = new ParameterConversion();

            ownedAssetName = "";

            pc.setParameterName("VariableName");
            pc.setParameterType(PARAMTYPE.STRING);
            paramValue.Add(pc);



            ParameterConversion pc2 = new ParameterConversion();

            pc2.setParameterName("Threshold");
            pc2.setParameterType(PARAMTYPE.FLOAT);
            pc2.setParameter("true");
            paramValue.Add(pc2);

        }

        public void exampleSetBoolVariable()
        {
            Name = "SetBoolVariable";

            ParameterConversion pc = new ParameterConversion();

            ownedAssetName = "";

            pc.setParameterName("VariableName");
            pc.setParameterType(PARAMTYPE.STRING);
            paramValue.Add(pc);



            ParameterConversion pc2 = new ParameterConversion();

            pc2.setParameterName("Threshold");
            pc2.setParameterType(PARAMTYPE.BOOL);
            pc2.setParameter("true");
            paramValue.Add(pc2);
        }

        public void exampleAttachToCamera()
        {
            Name = "AttachToCamera";
        }

        public void exampleDeactiveRenderAction()
        {
            Name = "DeactiveRenderAction";

            
        }
        public void exampleActiveRenderAction()
        {
            Name = "ActiveRenderAction";
        }
        public void exampleEnableAssetTriggerAction()
        {
            Name = "EnableAssetTriggerAction";
        }
        public void exampleDisableAssetTriggerAction()
        {
            Name = "DisableAssetTriggerAction";
        }
        public void exampleStartParticleTriggerAction()
        {
            Name = "StartParticleTriggerAction";
        }
        public void exampleStopParticleTriggerAction()
        {
            Name = "StopParticleTriggerAction";
        }

        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            return base.XmlSerialize(document, parentElement, isRoot);
        }
    }
}