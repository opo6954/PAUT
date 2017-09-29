using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using System;
using System.ComponentModel;

/*
 * Asset template
 * Only asset cannot do training... asset collaborated event can be possible in training stage
 * 
 * 
 * 
 * */
namespace PAUT
{
    // Dummy will be ignored by collision and gaze trigger
    public enum AssetType { MODEL, LOCATION, EFFECT, USER, DUMMY, GUI };

    public class Asset : Placable
    {
        public AssetType assetType;

        //attached role information
        protected string roleInfo; // authority of users
        
        //attached event template
        protected Event myEvent;

        //protected TransitionTrigger assetTrigger; // the trigger that changes a variable of the asset

        //Collider for boundingBox
        protected Collider boundingBox;

        //Is Inventory-able
        protected bool isInventory;
        
        //Possible after effect list
        //protected List<Effect> afterEffectList=new List<Effect>();

        //Possible before effect list
        //protected List<Effect> beforeEffectList=new List<Effect>();

		// this should be a Dictionary of some structure
        protected List<AssetTrigger> targetAssetTriggers;
        
		public static event AssetTriggerHandler eventHandler;
		private event AssetTriggerHandler tempHandler;
        public bool isTriggering;

        //Location info. of Asset;(from in-situ authoring)
        protected Transform location;

        [Serializable]
        public class DictionaryFloat
        {
            public string key;
            public float value;
        }
        /*
        public class DictionaryFloat : INotifyPropertyChanged
        {
            private object _lock;
            public string key;
            public float keyValue
            {
                get
                {
                    return keyValue;
                }
                set
                {
                    lock (_lock)
                    {
                        //The property changed event will get fired whenever
                        //the value changes. The subscriber will do work if the value is
                        //1. This way you can keep your business logic outside of the setter
                        if (value != keyValue)
                        {
                            keyValue = value;
                            NotifyPropertyChanged("keyValue");
                        }
                    }
                }
            }
            private void NotifyPropertyChanged(string propertyName)
            {
                //Raise PropertyChanged event
                AssetTrigger e1 = new AssetTrigger(null, null, new AdjustChangeParam(TriggerConditionType.GazeEnter), null);
                Asset.OnEventReceive(e1);
            }
        }
        */

        [Serializable]
        public class DictionaryBool
        {
            public string key;
            public bool value;
        }

        [SerializeField]
        private List<DictionaryFloat> floatVariables;
        [SerializeField]
        private List<DictionaryBool> booleanVariables;

        public Dictionary<string, float> dicFloat;
        public Dictionary<string, bool> dicBool;

        private void Awake()
        {
            if(dicFloat==null)
                dicFloat = new Dictionary<string, float>();
            if (dicBool == null)
                dicBool = new Dictionary<string, bool>();
            if (dicFloat.Count != floatVariables.Count)
            {
                foreach (DictionaryFloat entry in floatVariables)
                {
                    dicFloat.Add(entry.key, entry.value);
                }
            }
            if (dicBool.Count != booleanVariables.Count)
            {
                foreach (DictionaryBool entry in booleanVariables)
                {
                    dicBool.Add(entry.key, entry.value);
                }
            }

            textMesh = GetComponentInChildren<TextMesh>();
        }

        private void Start()
        {
            targetAssetTriggers = new List<AssetTrigger>();
            isTriggering = false;
            // if the tag is already set, we will let them leave (e.g. Dummy asset)
            if(gameObject.tag== "Untagged")
                gameObject.tag = "Asset";
            else if(gameObject.tag == "Dummy")
            {
                gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            instruction = new Instruction(gameObject.name, "", "");

            //////////////////////////// VR SUPPORT ///////////////////////////////
            if (UnityEngine.VR.VRSettings.enabled && assetType == AssetType.MODEL)
            {
                gameObject.AddComponent<Interactable>();
            }
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Asset p = (Asset)obj;
            return instruction.name == p.instruction.name;
        }

        public void playSound(float volume)
        {
            if (GetComponent<RandomSound>() != null)
            {
                RandomSound rs = GetComponent<RandomSound>();
                if (!rs.isPlaying())
                {
                    rs.PlayRandom();
                }
                rs.setVolume(volume);
            }
            else if(GetComponent<AudioSource>() != null)
            {
                AudioSource audio = GetComponent<AudioSource>();
                if(!audio.isPlaying)
                    audio.Play(); 
                audio.volume = volume;
            }
        }

        public void stopSound()
        {
            if (GetComponent<AudioSource>() != null)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Stop();
            }
        }

        public void startTrigger()
        {
			Debug.Log ("Asset - startTrigger - " + instruction.name);
            // we will register event listener with target trigger arguments
            AssetTriggerListener dbsl = new AssetTriggerListener();
			tempHandler = new AssetTriggerHandler((sender, e) => dbsl.matchCondition(sender, e, ref targetAssetTriggers));
            eventHandler += tempHandler;
            isTriggering = true;
        }

        public TriggerParam getTriggerParam(int index)
        {
            return targetAssetTriggers[index].tTriggerParam;
        }

		public void addAssetTrigger(Player player, Asset targetAsset, TriggerParam triggerParam, List<Action> actions)
        {
			targetAssetTriggers.Add(new AssetTrigger(player, targetAsset, triggerParam, actions));
        }

		public void addAndStartAssetTrigger(Player player, Asset targetAsset, TriggerParam triggerParam, List<Action> actions)
        {
            addAssetTrigger(player, targetAsset, triggerParam, actions);
            startTrigger();
        }

        public void stopTrigger()
        {
            if (tempHandler != null)
                eventHandler -= tempHandler;
        }

        public static void OnEventReceive(AssetTrigger e)
        {
            if (eventHandler != null)
                eventHandler(new object(), e);
        }

        public bool addParameter(string _parameterName, object _parameterValue)
        {
            //parameterName에 있는 지 확인하고 없으면 false
            if (parameterName.Contains(_parameterName) == false)
            {
                return false;
            }
            //parameter dictionary에 이미 존재하는 경우 false
            if (parameters.ContainsKey(_parameterName) == true || parameters.ContainsValue(_parameterValue) == true)
            {
                return false;
            }

            parameters.Add(_parameterName, _parameterValue);

            return true;
        }

        //get the parameter in asset...
        public T getParameter<T>(string _parameterName)
        {
            //No parametername found...
            if (parameterName.Contains(_parameterName) == false)
            {
                return default(T);
            }

            if (parameters.ContainsKey(_parameterName) == false)
            {
                return default(T);
            }

            return (T)parameters[_parameterName];
        }
        /*
        public bool setRoleInfo(string roleName)
        {
            if (RoleInfoTemplate.isRoleNameExist(roleName) == true)
            {
                roleInfo = roleName;
                return true;
            }
            return false;
        }
        */

        ////////////////////////////////////////////////
        // not implemented yet
        public object Clone()
        {
            Asset clone = new Asset();
            clone.assetType = this.assetType;
            clone.instruction = this.instruction;
            clone.roleInfo = this.roleInfo;
            
            return clone;
        }
        ////////////////////////////////////////////////

        //event 설정 함수
        public void setEvent(Event _eventTemplate)
        {
            //Event = _eventTemplate;
        }

        void OnParticleCollision(GameObject other)
        {
            Debug.Log("Asset - OnParticleCollision - " + other.name);

            if (other.GetComponent<Asset>() != null)
            {
                Debug.Log("Asset - OnParticleCollision - " + other.GetComponent<Asset>().name);
            }
        }

        //////////////////////////////////////// START OF VR SUPPORT //////////////////////////////////////// 

        private TextMesh textMesh;
        private Vector3 oldPosition;
        private Quaternion oldRotation;
        private Transform envTrans;

        private float attachTime;

        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);

        //-------------------------------------------------
       

        //-------------------------------------------------
        // Called when a Hand starts hovering over this object
        //-------------------------------------------------
        private void OnHandHoverBegin(Hand hand)
        {
            if (instruction.toDoIdx > 0)
                textMesh.text = instruction.toDoList[instruction.toDoIdx];
            else
                textMesh.text = "Instruction - sample";
        }


        //-------------------------------------------------
        // Called when a Hand stops hovering over this object
        //-------------------------------------------------
        private void OnHandHoverEnd(Hand hand)
        {
            textMesh.text = instruction.name;
        }


        //-------------------------------------------------
        // Called every Update() while a Hand is hovering over this object
        //-------------------------------------------------
        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
            {
                if (hand.currentAttachedObject != gameObject)
                {
                    // Save our position/rotation so that we can restore it when we detach
                    oldPosition = transform.position;
                    oldRotation = transform.rotation;

                    // Call this to continue receiving HandHoverUpdate messages,
                    // and prevent the hand from hovering over anything else
                    hand.HoverLock(GetComponent<Interactable>());

                    // Attach this object to the hand
                    hand.AttachObject(gameObject, attachmentFlags);
                }
                else
                {
                    // Detach this object from the hand
                    hand.DetachObject(gameObject);

                    // Call this to undo HoverLock
                    hand.HoverUnlock(GetComponent<Interactable>());

                    // Restore position/rotation
                    
                    transform.position = oldPosition;
                    transform.rotation = oldRotation;
                }
            }
        }


        //-------------------------------------------------
        // Called when this GameObject becomes attached to the hand
        //-------------------------------------------------
        private void OnAttachedToHand(Hand hand)
        {
            textMesh.text = "Attached to hand: " + hand.name;
            attachTime = Time.time;
        }


        //-------------------------------------------------
        // Called when this GameObject is detached from the hand
        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
            textMesh.text = "Detached from hand: " + hand.name;
        }


        //-------------------------------------------------
        // Called every Update() while this GameObject is attached to the hand
        //-------------------------------------------------
        private void HandAttachedUpdate(Hand hand)
        {
            textMesh.text = "HandAttached";
        }


        //-------------------------------------------------
        // Called when this attached GameObject becomes the primary attached object
        //-------------------------------------------------
        private void OnHandFocusAcquired(Hand hand)
        {
        }


        //-------------------------------------------------
        // Called when another attached GameObject becomes the primary attached object
        //-------------------------------------------------
        private void OnHandFocusLost(Hand hand)
        {
        }
        //////////////////////////////////////// END OF VR SUPPORT //////////////////////////////////////// 

    }
}
/*
 * 소화기 Asset
3D model: 전체 소화기 mesh(수정 불가)
Bounding box: 전체 소화기 bounding box(수정 불가)
Effect list:(Timeline authoring시 effect list와 trigger list에서 trigger와 effect 선택 가능)
Highlight
Informing
소화기 들기
노즐 위로 이동
물 분사
Trigger list:(Timeline authoring시 effect list와 trigger list에서 trigger와 effect 선택 가능)
Asset의 기본 trigger
(Asset 발견, asset이 시야에 들어올 시 등)
밑의 파트 asset에서의 trigger 생성 가능(Asset list를 만들 시 asset list에서 각 asset의 trigger를 어떻게 합칠 지 선택 가능(sequential or parallel, AND or OR)
New trigger:
전체 분사 trigger: 먼저 안전핀의 trigger  손잡이의  trigger AND trigger  바디의 trigger를 설정
전체 순서대로 동작 trigger: 안전핀의 순서대로동작 trigger  손잡이의 순서대로동작 trigger 노즐의 순서대로 동작 trigger  바디의 순서대로동작 trigger
Location:
In-situ에서 변경 가능
List of asset: 안전핀, 손잡이, 노즐, 바디

 * */
