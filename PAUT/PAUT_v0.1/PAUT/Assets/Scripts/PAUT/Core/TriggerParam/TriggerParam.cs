using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAUT
{
    // this Param classes are defined to pass multiple parameters according to the type of Trigger
    public enum TriggerConditionType
    {
        CollisionEnter = 0,
        CollisionHold = 1,
        CollisionExit = 2,
        MinTimeElapsed = 3,
        MaxTimeElapsed = 4,
        MonitorValueCaptured = 5,
        GazeEnter = 6,
        GazeHold = 7,
        GazeExit = 8,
        InventoryOpen = 9,
        InventoryClose = 10,
        InputDown = 11,
        InputUp = 12,
        Cancel = 16,
        None = 99 
    }

    public enum TriggerOperator
    {
        Larger = 0,
        LargerOrEqual = 1,
        Equal = 2,
        SmallerOrEqual = 3,
        Smaller = 4
    }

    public enum KeyInputMapping
    {

    }

    public class TriggerParam
    {
        public TriggerConditionType type;

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            TriggerParam p = (TriggerParam)obj;
            return (type == p.type);
        }

        public override string ToString()
        {
            return type.ToString();
        }
    }

    public class CollisionParam : TriggerParam
    {
        public CollisionParam(TriggerConditionType _type)
        {
            type = _type;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            CollisionParam p = (CollisionParam)obj;
            return (type == p.type);
        }
    }

    public class GazeEnterParam : TriggerParam
    {
        public GazeEnterParam(TriggerConditionType _type)
        {
            type = _type;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            GazeEnterParam p = (GazeEnterParam)obj;
            return (type == p.type);
        }
    }

    public class GazeExitParam : TriggerParam
    {
        public GazeExitParam(TriggerConditionType _type)
        {
            type = _type;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            GazeExitParam p = (GazeExitParam)obj;
            return (type == p.type);
        }
    }

    public class InputDownParam : TriggerParam
    {
        public InputMapping.InputType inputType;
        public string keyTarget;
        public ulong viveTarget;
        public InputMapping.InputIndex targetInputIdx;

        public InputDownParam(TriggerConditionType _type, InputMapping.InputType _inputType, InputMapping.InputIndex _targetInputIdx)
        {
            type = _type;
            inputType = _inputType;
            targetInputIdx = _targetInputIdx;
            if (inputType == InputMapping.InputType.KEYBOARD)
            {
                keyTarget = InputMapping.getKeyInput(_targetInputIdx);
            }
            else if(inputType == InputMapping.InputType.HTCVIVE)
            {
                viveTarget = InputMapping.getViveInput(_targetInputIdx);
            }
        }

        public InputDownParam(TriggerConditionType _type, InputMapping.InputType _inputType, string _keyTarget)
        {
            type = _type;
            inputType = _inputType;
            keyTarget = _keyTarget;
            targetInputIdx = InputMapping.getInputIndexFromString(keyTarget);
        }

        public InputDownParam(TriggerConditionType _type, InputMapping.InputType _inputType, ulong _viveTarget)
        {
            type = _type;
            inputType = _inputType;
            viveTarget = _viveTarget;
            targetInputIdx = InputMapping.getInputIndexFromUlong(viveTarget);
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            InputDownParam p = (InputDownParam)obj;
            return (type == p.type) && (inputType == p.inputType) && (targetInputIdx == p.targetInputIdx);
        }

        public override string ToString()
        {
            return "\t" + inputType.ToString() + ", " + targetInputIdx.ToString();
        }
    }

    public class InputHoldParam : TriggerParam
    {
        public InputMapping.InputType inputType;
        public string keyTarget;
        public ulong viveTarget;
        public InputMapping.InputIndex targetInputIdx;
        
        public InputHoldParam(TriggerConditionType _type, InputMapping.InputType _inputType, InputMapping.InputIndex _targetInputIdx)
        {
            type = _type;
            inputType = _inputType;
            targetInputIdx = _targetInputIdx;
            if (inputType == InputMapping.InputType.KEYBOARD)
            {
                keyTarget = InputMapping.getKeyInput(_targetInputIdx);
            }
            else if (inputType == InputMapping.InputType.HTCVIVE)
            {
                viveTarget = InputMapping.getViveInput(_targetInputIdx);
            }
        }

        public InputHoldParam(TriggerConditionType _type, InputMapping.InputType _inputType, string _keyTarget)
        {
            type = _type;
            inputType = _inputType;
            keyTarget = _keyTarget;
            targetInputIdx = InputMapping.getInputIndexFromString(keyTarget);
        }

        public InputHoldParam(TriggerConditionType _type, InputMapping.InputType _inputType, ulong _viveTarget)
        {
            type = _type;
            inputType = _inputType;
            viveTarget = _viveTarget;
            targetInputIdx = InputMapping.getInputIndexFromUlong(viveTarget);
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            InputHoldParam p = (InputHoldParam)obj;
            return (type == p.type) && (inputType == p.inputType) && (targetInputIdx == p.targetInputIdx);
        }

        public override string ToString()
        {
            return "\t" + inputType.ToString() + ", " + targetInputIdx.ToString();
        }
    }

    public class InputUpParam : TriggerParam
    {
        public InputMapping.InputType inputType;
        public string keyTarget;
        public ulong viveTarget;
        public InputMapping.InputIndex targetInputIdx;

        public InputUpParam(TriggerConditionType _type, InputMapping.InputType _inputType, InputMapping.InputIndex _targetInputIdx)
        {
            type = _type;
            inputType = _inputType;
            targetInputIdx = _targetInputIdx;
            if (inputType == InputMapping.InputType.KEYBOARD)
            {
                keyTarget = InputMapping.getKeyInput(_targetInputIdx);
            }
            else if (inputType == InputMapping.InputType.HTCVIVE)
            {
                viveTarget = InputMapping.getViveInput(_targetInputIdx);
            }
        }

        public InputUpParam(TriggerConditionType _type, InputMapping.InputType _inputType, string _keyTarget)
        {
            type = _type;
            inputType = _inputType;
            keyTarget = _keyTarget;
            targetInputIdx = InputMapping.getInputIndexFromString(keyTarget);
        }

        public InputUpParam(TriggerConditionType _type, InputMapping.InputType _inputType, ulong _viveTarget)
        {
            type = _type;
            inputType = _inputType;
            viveTarget = _viveTarget;
            targetInputIdx = InputMapping.getInputIndexFromUlong(viveTarget);
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            InputUpParam p = (InputUpParam)obj;
            return (type == p.type) && (inputType == p.inputType) && (targetInputIdx == p.targetInputIdx);
        }

        public override string ToString()
        {
            return "\t" + inputType.ToString() + ", " + targetInputIdx.ToString();
        }
    }

    public class AdjustChangeParam
    {

    }

    public class MonitorDistanceParam : MonitorFloatVariableParam
    {
        // this constructor for creating an event in run-time
        public MonitorDistanceParam(TriggerConditionType _type)
        {
            type = _type;
            satisfied = false;
        }

        public MonitorDistanceParam(TriggerConditionType _type, float _value, TriggerOperator _tOperator)
        {
            type = _type;
            satisfied = false;
            value = _value;
            tOperator = _tOperator;
        }

        public MonitorDistanceParam(TriggerConditionType _type, bool _satisfied)
        {
            type = _type;
            satisfied = _satisfied;
        }
    }

    public class MonitorFloatVariableParam : MonitorParam
    {
        public string key;
        public float value;
        public TriggerOperator tOperator;

        public MonitorFloatVariableParam()
        {
            type = TriggerConditionType.None;
            satisfied = false;
        }

        // this constructor for creating an event in run-time
        public MonitorFloatVariableParam(TriggerConditionType _type)
        {
            type = _type;
            satisfied = false;
        }

        public MonitorFloatVariableParam(TriggerConditionType _type, string _key, float _value, TriggerOperator _tOperator)
        {
            type = _type;
            satisfied = false;
            key = _key;
            value = _value;
            tOperator = _tOperator;
        }

        public MonitorFloatVariableParam(TriggerConditionType _type, bool _satisfied)
        {
            type = _type;
            satisfied = _satisfied;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            MonitorFloatVariableParam p = (MonitorFloatVariableParam)obj;
            return (type == p.type) && (satisfied == p.satisfied) && (value == p.value);
        }

        public override string ToString()
        {
            return base.ToString() + ", " + key + ", " + value + ", " + tOperator;
        }
    }

    public class MonitorBooleanVariableParam : MonitorParam
    {
        public string key;
        public bool value;

        public MonitorBooleanVariableParam()
        {
            type = TriggerConditionType.None;
            satisfied = false;
        }
        
        public MonitorBooleanVariableParam(TriggerConditionType _type, string _key, bool _value)
        {
            type = _type;
            satisfied = false;
            key = _key;
            value = _value;
        }

        public MonitorBooleanVariableParam(TriggerConditionType _type, bool _satisfied)
        {
            type = _type;
            satisfied = _satisfied;
        }
        
        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            MonitorBooleanVariableParam p = (MonitorBooleanVariableParam)obj;
            return (type == p.type) && (satisfied == p.satisfied) && (key.Equals(p.key)) && (value.Equals(p.value));
        }

        public override string ToString()
        {
            return base.ToString() + ", " + key + ", " + value ;
        }
    }

    public class MonitorParam : TriggerParam
    {
        public bool satisfied { get; set; }
    }

    public class AssetTriggerParam: TriggerParam
    {
        public Action action;
        public AssetTriggerParam(TriggerConditionType _type, Action _action)
        {
            type = _type;
            action = _action;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            AssetTriggerParam p = (AssetTriggerParam)obj;
            return (type == p.type) && (action == p.action);
        }
    }
}
