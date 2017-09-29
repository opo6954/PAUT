using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputMapping : MonoBehaviour {
    public enum InputIndex{ MOVE=0, INPUT1=1, INPUT2=2, INPUT3=3, NONE = 99};
	public enum InputType { KEYBOARD=0, HTCVIVE=1, OCULUSRIFT=2, JOYPAD=3};

    public static string getKeyInput(InputIndex idx)
    {
        if (idx == InputIndex.INPUT1)
            return "Z";
        else if (idx == InputIndex.INPUT2)
            return "X";
        else
            return "";
    }

    public static ulong getViveInput(InputIndex idx)
    {
        if (idx == InputIndex.MOVE)
            return SteamVR_Controller.ButtonMask.Touchpad;
        if (idx == InputIndex.INPUT1)
            return SteamVR_Controller.ButtonMask.Trigger;
        else if (idx == InputIndex.INPUT2)
            return SteamVR_Controller.ButtonMask.Grip;
        else
            return SteamVR_Controller.ButtonMask.Axis0;
    }

    // get InputIndex
    public static InputIndex getInputIndexFromString(string str)
    {
        string checkInput = str.ToUpper();
        if(checkInput.Equals(getKeyInput(InputIndex.INPUT1)))
            return InputIndex.INPUT1;
        else if (checkInput.Equals(getKeyInput(InputIndex.INPUT2)))
            return InputIndex.INPUT2;
        else
            return InputIndex.NONE;
    }

    public static InputIndex getInputIndexFromUlong(ulong checkInput)
    {
        if (checkInput.Equals(getViveInput(InputIndex.INPUT1)))
            return InputIndex.INPUT1;
        else if (checkInput.Equals(getViveInput(InputIndex.INPUT2)))
            return InputIndex.INPUT2;
        else
            return InputIndex.NONE;
    }

    // get string for instruction
    public static string getString(InputType type, InputIndex idx)
    {
        if (type == InputType.KEYBOARD)
        {
            if (idx == InputIndex.INPUT1)
                return getKeyInput(idx) + " 버튼";
            else if (idx == InputIndex.INPUT2)
                return getKeyInput(idx) + " 버튼";
            else
                return "";
        }
        else if (type == InputType.HTCVIVE)
        {
            if (idx == InputIndex.MOVE)
                return "터치패드";
            if (idx == InputIndex.INPUT1)
                return "트리거 버튼";
            else if (idx == InputIndex.INPUT2)
                return "그립 버튼";
            else
                return "";
        }
        else
            return "";
    }

}
