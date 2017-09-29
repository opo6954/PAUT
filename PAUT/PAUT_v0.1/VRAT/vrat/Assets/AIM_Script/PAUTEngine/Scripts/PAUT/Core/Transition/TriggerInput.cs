using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Trigger for asset
 * Containing active trigger like gesture or passive trigger like approach
 * 
 * Component of trigger:
 * 1. Trigger supervisor: Supervise input whether the trigger is worked or not
 * 2. 
  * */

namespace PAUT
{
    public enum TriggerInputType { HTCVIVE, KEYBOARD, JOYPAD, OCUTOUCH };

    // Class that states the use of input devices.
    // Actual mapping would be implemented to 
    public class TriggerInput
    {
        public static TriggerInputType curInput;
        public Player player;
    }
}
