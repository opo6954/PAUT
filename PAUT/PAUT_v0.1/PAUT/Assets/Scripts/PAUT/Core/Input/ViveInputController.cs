
namespace VRTK.Examples
{
    using UnityEngine;
    using UnityStandardAssets.Characters.FirstPerson;

    public class ViveInputController : MonoBehaviour
    {
        /*
        public FirstPersonController fpc;
        public GameObject gripBalloon;
        public bool gripped = false;
        public bool fired = false;
        public bool triggered = false;
        public Vector3 prevPosition;
        public Vector3 prevLRotation;
        public Quaternion prevControllerRotation;
        public Vector3 prevControllerPosition;
        public ViveGesture targetGesture;
        public Transform cameraTransform;
        public Vector3 diffPose;

        private bool touchPadPressed = false;
        private Vector3 pullDownGesture = Vector3.down;
        private Vector3 pullUpGesture = Vector3.up;
        private Vector3 pullBackGesture = Vector3.back;
        private float pullDownThreshold = 0.2f;
        private float pullBackThreshold = 0.2f;

        private void Start()
        {
            if (GetComponent<VRTK_ControllerEvents>() == null)
            {
                Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
                return;
            }

            //Setup controller event listeners
            GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
            GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

            GetComponent<VRTK_ControllerEvents>().TriggerTouchStart += new ControllerInteractionEventHandler(DoTriggerTouchStart);
            GetComponent<VRTK_ControllerEvents>().TriggerTouchEnd += new ControllerInteractionEventHandler(DoTriggerTouchEnd);

            GetComponent<VRTK_ControllerEvents>().TriggerHairlineStart += new ControllerInteractionEventHandler(DoTriggerHairlineStart);
            GetComponent<VRTK_ControllerEvents>().TriggerHairlineEnd += new ControllerInteractionEventHandler(DoTriggerHairlineEnd);

            GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
            GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);

            GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);

            GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuPressed);
            GetComponent<VRTK_ControllerEvents>().ApplicationMenuReleased += new ControllerInteractionEventHandler(DoApplicationMenuReleased);

            GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
            GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
            GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

            GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
            GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

            GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

            GetComponent<VRTK_ControllerEvents>().ControllerEnabled += new ControllerInteractionEventHandler(DoControllerEnabled);
            GetComponent<VRTK_ControllerEvents>().ControllerDisabled += new ControllerInteractionEventHandler(DoControllerDisabled);

            fpc.m_Input = new Vector2(0,0);
            prevPosition = Vector3.zero;
            prevControllerRotation = this.transform.localRotation;
            prevControllerPosition = this.transform.position;
        }

        public void Update()
        {
            this.fired = false;

            float rotDiff = Quaternion.Angle(this.transform.localRotation, prevControllerRotation);
            Vector3 posDiff = this.transform.position - prevControllerPosition;
            prevControllerRotation = this.transform.localRotation;
            prevControllerPosition = this.transform.position;

            if (triggered)
            {
                Vector2 front = new Vector2(0, 1);
                fpc.m_Input = front;
                fpc.walkingSpeed += rotDiff*rotDiff * 0.1f;
            }
            else if (touchPadPressed)
            {
                Vector2 back = new Vector2(0, -1);
                fpc.m_Input = back;
                fpc.walkingSpeed += rotDiff * rotDiff * 0.1f;
            }
        }

        private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
        {
            if (touchPadPressed)
            {
                //Debug.Log("Controller on index '" + index + "' " + button + " has been " + action + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
                if (transform.Find("ControllerTooltips")==null) { 
                    //fpc.m_Input = e.touchpadAxis;
                }
            }
            else {
                fpc.m_Input = Vector2.zero;
                //fpc.m_YRotation = 0;
            }
            if (triggered)
            {
                if(targetGesture.activeState == ViveGesture.Gesture.TRIGGER)
                {
                    Debug.Log("Trigger gesture");
                    fired = true;
                }
            }
            if (gripped)
            {
                fired = false;
                
                if (targetGesture.activeState == ViveGesture.Gesture.GRIP || targetGesture.activeState == ViveGesture.Gesture.GRIPCONTINUE)
                {
                    fired = true;
                }
                else if (targetGesture.activeState == ViveGesture.Gesture.GRIPPULLDOWN)
                {
                    if(prevPosition != Vector3.zero && ViveGesture.IsPullDown(pullDownGesture, prevPosition, gripBalloon.transform.position, pullDownThreshold))
                    {
                        Debug.Log("Pull down gesture");
                        fired = true;
                        prevPosition = Vector3.zero;
                    }
                }
                else if (targetGesture.activeState == ViveGesture.Gesture.GRIPPULLUP)
                {                    
                    if (prevPosition != Vector3.zero && ViveGesture.IsPullUp(pullUpGesture, prevPosition, gripBalloon.transform.position, pullDownThreshold))
                    {
                        prevPosition = Vector3.zero;
                        Debug.Log("Pull up gesture");
                        fired = true;
                        targetGesture.rollbackState = targetGesture.activeState;
                        targetGesture.activeState = ViveGesture.Gesture.GRIPCONTINUE;
                    }
                }
                else if (targetGesture.activeState == ViveGesture.Gesture.GRIPTWIST)
                {
                    if (ViveGesture.IsTwist(pullUpGesture, prevLRotation, gripBalloon.transform.localEulerAngles, 20))
                    {
                        Debug.Log("Twist gesture");
                        fired = true;
                        targetGesture.rollbackState = targetGesture.activeState;
                        targetGesture.activeState = ViveGesture.Gesture.GRIPCONTINUE;
                    }
                }
                else if (targetGesture.activeState == ViveGesture.Gesture.CLIMB)
                {
                    if (ViveGesture.IsClimb(pullUpGesture, prevLRotation, gripBalloon.transform.localEulerAngles, 20))
                    {
                        diffPose = gripBalloon.transform.position - prevPosition;
                        Debug.Log("Climb gesture");
                        fired = true;
                        targetGesture.rollbackState = targetGesture.activeState;
                        targetGesture.activeState = ViveGesture.Gesture.GRIPCONTINUE;
                    }
                }
            }

        }

        private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "pressed", e);
            triggered = true;
        }

        private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "released", e);
            triggered = false;
        }

        private void DoTriggerTouchStart(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "touched", e);
        }

        private void DoTriggerTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "untouched", e);
        }

        private void DoTriggerHairlineStart(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "hairline start", e);
        }

        private void DoTriggerHairlineEnd(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "hairline end", e);
        }

        private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "clicked", e);
            
        }

        private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "unclicked", e);
            
        }

        private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TRIGGER", "axis changed", e);
        }

        private void DoApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "APPLICATION MENU", "pressed down", e);
        }

        private void DoApplicationMenuReleased(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "APPLICATION MENU", "released", e);
        }

        private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "GRIP", "pressed down", e);
            gripBalloon.GetComponent<MeshRenderer>().enabled = true;
            gripped = true;
            prevPosition = gripBalloon.transform.position;
            prevLRotation = gripBalloon.transform.eulerAngles;
        }

        private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "GRIP", "released", e);
            gripBalloon.GetComponent<MeshRenderer>().enabled = false;
            gripped = false;
            prevPosition = Vector3.zero;
            if(targetGesture.activeState == ViveGesture.Gesture.GRIPCONTINUE)
            {
                targetGesture.activeState = targetGesture.rollbackState;
                targetGesture.rollbackState = ViveGesture.Gesture.NONE;
            }
        }

        private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TOUCHPADDDDDD", "pressed down", e);
            touchPadPressed = true;
        }

        private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TOUCHPADDDDDD", "released", e);
            touchPadPressed = false;
        }

        private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TOUCHPADDDDDD", "touched", e);
        }

        private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TOUCHPADDDDDD", "untouched", e);
        }

        private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TOUCHPADDDDDD", "axis changed", e);
        }

        private void DoControllerEnabled(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "CONTROLLER STATE", "ENABLED", e);
        }

        private void DoControllerDisabled(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "CONTROLLER STATE", "DISABLED", e);
        }
        */
    }
    
}