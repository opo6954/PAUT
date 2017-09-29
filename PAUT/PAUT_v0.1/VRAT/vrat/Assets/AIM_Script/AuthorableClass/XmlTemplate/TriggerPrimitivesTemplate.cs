using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace vrat
{
    //trigger를 위한 것임
    //걍 죄다 string으로 넣자
    //읽을 때 고생하겠지만

    public class TriggerPrimitivesTemplate : PAUTPrimitivesTemplate
    {
        public TriggerPrimitivesTemplate(string _name, string _type) : base(_name, _type)
        {
            ClassName = "TriggerPrimitivesTemplate";
        }

        public void exampleMonitorBooleanvariableParam()
        {
            Name = "MonitorBooleanVariableParam";
            ownedAssetName = "";

            ParameterConversion pc = new ParameterConversion();

            pc.setParameterType(PARAMTYPE.CHOICE);
            pc.setParameterName("TriggerType");

            pc.setAdditionalInfo("MonitorValueCaptured");


            ParameterConversion pc_var = new ParameterConversion();

            pc_var.setParameterType(PARAMTYPE.STRING);
            pc_var.setParameterName("VariableName");

            




            ParameterConversion pc_th = new ParameterConversion();

            pc_th.setParameterType(PARAMTYPE.BOOL);
            pc_th.setParameterName("Threshold");
            pc_th.setParameter("0");

            paramValue.Add(pc);
            paramValue.Add(pc_th);
            paramValue.Add(pc_var);
        }

        public void exampleInputUp()
        {
            Name = "InputUpParam";
            
            ParameterConversion pc = new ParameterConversion();

            ownedAssetName = "";

            pc.setParameterType(PARAMTYPE.CHOICE);
            pc.setParameterName("TriggerType");

            pc.setAdditionalInfo("InputUp");



            ParameterConversion pc_Key = new ParameterConversion();

            pc_Key.setParameterType(PARAMTYPE.STRING);
            pc_Key.setParameterName("KeyName");

            pc_Key.setParameter("e");


            paramValue.Add(pc);
            paramValue.Add(pc_Key);

        }


        public void exampleInputHold()
        {
            Name = "InputHoldParam";

            ParameterConversion pc = new ParameterConversion();

            ownedAssetName = "";

            pc.setParameterType(PARAMTYPE.CHOICE);
            pc.setParameterName("TriggerType");

            pc.setAdditionalInfo("InputDown");
            pc.setAdditionalInfo("InputUp");

            paramValue.Add(pc);

            ParameterConversion pc2 = new ParameterConversion();

            ownedAssetName = "";
            pc2.setParameterType(PARAMTYPE.STRING);
            pc2.setParameterName("KeyName");
            pc2.setParameter("p");

            paramValue.Add(pc2);
        }




        public void exampleGazeTrigger()
        { 
            Name = "GazeEnter";
             
            ParameterConversion pc = new ParameterConversion();

            ownedAssetName = "";

            pc.setParameterType(PARAMTYPE.CHOICE);
            pc.setParameterName("TriggerType");


            pc.setAdditionalInfo("GazeEnter");
            pc.setAdditionalInfo("GazeHold");
            pc.setAdditionalInfo("GazeExit");


            paramValue.Add(pc);

        }

        public void exampleCollisionTrigger()
        {
            Name = "CollisionParam";

            ParameterConversion pc = new ParameterConversion();

            ownedAssetName = "";

            pc.setParameterType(PARAMTYPE.CHOICE);
            pc.setParameterName("TriggerType");

            
            pc.setAdditionalInfo("CollisionEnter");            
            pc.setAdditionalInfo("CollisionHold");            
            pc.setAdditionalInfo("CollisionExit");

            paramValue.Add(pc);
        }

        public void exampleInputDownTrigger()
        {
            Name = "InputDownParam";

            ParameterConversion pc = new ParameterConversion();

            ownedAssetName = "";

            pc.setParameterType(PARAMTYPE.CHOICE);
            pc.setParameterName("TriggerType");

            pc.setAdditionalInfo("InputDown");

            

            ParameterConversion pc_Key = new ParameterConversion();

            pc_Key.setParameterType(PARAMTYPE.STRING);
            pc_Key.setParameterName("KeyName");

            pc_Key.setParameter("x");


            paramValue.Add(pc);
            paramValue.Add(pc_Key);
        }

        public void exampleMonitorFloatVariableParam()
        {
            Name = "MonitorFloatVariableParam";

            ownedAssetName = "";

            ParameterConversion pc = new ParameterConversion();

            pc.setParameterType(PARAMTYPE.CHOICE);
            pc.setParameterName("TriggerType");
            pc.setAdditionalInfo("MonitorValueCaptured");

            ParameterConversion pc_Operator = new ParameterConversion();

            pc_Operator.setParameterType(PARAMTYPE.CHOICE);
            pc_Operator.setParameterName("TriggerOperator");
            pc_Operator.setAdditionalInfo("Larger");
            pc_Operator.setAdditionalInfo("LargerOrEqual");
            pc_Operator.setAdditionalInfo("Equal");
            pc_Operator.setAdditionalInfo("SmallerOrEqual");
            pc_Operator.setAdditionalInfo("Smaller");



            ParameterConversion pc_var = new ParameterConversion();

            pc_var.setParameterType(PARAMTYPE.STRING);
            pc_var.setParameterName("VariableName");






            ParameterConversion pc_th = new ParameterConversion();

            pc_th.setParameterType(PARAMTYPE.BOOL);
            pc_th.setParameterName("Threshold");
            pc_th.setParameter("0");

            paramValue.Add(pc);
            paramValue.Add(pc_Operator);
            paramValue.Add(pc_th);
            paramValue.Add(pc_var);

        }

        public void exampleMonitorDistanceCapturedTrigger()
        {
            Name = "MonitorDistanceParam";
            ownedAssetName = "";

            ParameterConversion pc = new ParameterConversion();

            pc.setParameterType(PARAMTYPE.CHOICE);
            pc.setParameterName("TriggerType");

            pc.setAdditionalInfo("MonitorValueCaptured");

            ParameterConversion pc_Operator = new ParameterConversion();

            pc_Operator.setParameterType(PARAMTYPE.CHOICE);
            pc_Operator.setParameterName("TriggerOperator");
            pc_Operator.setAdditionalInfo("Larger");
            pc_Operator.setAdditionalInfo("LargerOrEqual");
            pc_Operator.setAdditionalInfo("Equal");
            pc_Operator.setAdditionalInfo("SmallerOrEqual");
            pc_Operator.setAdditionalInfo("Smaller");

            ParameterConversion pc_th = new ParameterConversion();

            pc_th.setParameterType(PARAMTYPE.FLOAT);
            pc_th.setParameterName("Threshold");
            pc_th.setParameter("0");

            paramValue.Add(pc);
            paramValue.Add(pc_Operator);
            paramValue.Add(pc_th);




        }


        




        

        public override System.Xml.XmlElement XmlSerialize(System.Xml.XmlDocument document, System.Xml.XmlElement parentElement, bool isRoot)
        {
            return base.XmlSerialize(document, parentElement, isRoot);
        }
    }
}