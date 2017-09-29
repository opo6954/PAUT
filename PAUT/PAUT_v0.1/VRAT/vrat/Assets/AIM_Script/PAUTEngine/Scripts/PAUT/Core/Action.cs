using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Effect for asset
 * 
 * like water out effect when the extinguisher operation trigger is worked * 
 * */
namespace PAUT
{
    public enum ActionType
    {
        None = 0,
        Create = 1,
        Destroy = 2,
        ActiveRender = 3,
        DeactiveRender = 4,
        Translate = 5,
		SetPosition = 6,
		EnableAssetTrigger = 7,
		DisableAssetTrigger = 8,
        PlaySound = 9,
        StopSound = 10,
        StartParticle = 11,
        StopParticle = 12,
        SetFloatVariable = 13,
        SetBoolVariable = 14,
        IncreaseFloatVariable = 15,
        DecreaseFloatVariable = 16,
        AttachToCamera = 17,
        SetScale = 18
    }

    public class Action : ICloneable
    {
        public string name { get; set; }
        public ActionType type { get; set; }
        public Asset asset { get; set; }
        public float paramFloat { get; set; }
        public bool paramBool { get; set; }
        public Vector3 paramVec { get; set; }
        public string paramString { get; set; }

        public Action() { type = ActionType.None; }
        public Action(ActionType _type, Asset _asset) {
            type = _type; asset = _asset; paramVec = Vector3.zero;
        }
        public Action(ActionType _type, Asset _asset, Vector3 _paramVec) {
            type = _type; asset = _asset; paramVec = _paramVec;
        }
        public Action(ActionType _type, Asset _asset, float _paramFloat)
        {
            type = _type; asset = _asset; paramFloat = _paramFloat;
        }
        public Action(ActionType _type, Asset _asset, bool _paramBool)
        {
            type = _type; asset = _asset; paramBool = _paramBool;
        }

        // this is construction for Increase/Decrease/SetFloatVariable,
        public Action(ActionType _type, Asset _asset, string _paramString, float _paramFloat)
        {
            type = _type; asset = _asset; paramString = _paramString; paramFloat = _paramFloat;
        }
        // this is construction for SetBoolVariable
        public Action(ActionType _type, Asset _asset, string _paramString, bool _paramBool)
        {
            type = _type; asset = _asset; paramString = _paramString; paramBool = _paramBool;
        }

        public void process()
        {
			//Debug.Log (this);
			if (type == ActionType.None)
				return;
			else if (type == ActionType.ActiveRender) {
				//asset.GetComponent<Renderer> ().enabled = true;
				enableRender(asset);
			} else if (type == ActionType.DeactiveRender) {
				//asset.GetComponent<Renderer> ().enabled = false;
				disableRender(asset);
			} else if (type == ActionType.Destroy) {
				asset.gameObject.SetActive (false);
			} else if (type == ActionType.Translate) {
				asset.transform.position += paramVec;
			} else if (type == ActionType.SetPosition) {
				asset.transform.position = paramVec;
			} else if (type == ActionType.EnableAssetTrigger) {
				asset.startTrigger(); // TO BE IMPLEMENTED : 어떤 어셋 트리거를 보여줄지
			} else if (type == ActionType.DisableAssetTrigger) {
				asset.stopTrigger();
			}            
            else if (type == ActionType.PlaySound)
            {
                asset.playSound(paramFloat);
            }
            else if (type == ActionType.StopSound)
            {
                asset.stopSound();
            }
            else if (type == ActionType.StartParticle)
            {
                if(!asset.GetComponent<ParticleSystem>().isPlaying)
                    asset.GetComponent<ParticleSystem>().Play();
            }
            else if (type == ActionType.StopParticle)
            {
                if (asset.GetComponent<ParticleSystem>().isPlaying)
                    asset.GetComponent<ParticleSystem>().Stop();
            }
            else if(type == ActionType.SetFloatVariable)
            {
                asset.dicFloat[paramString] = paramFloat;
            }
            else if (type == ActionType.SetBoolVariable)
            {
                if(asset.dicBool.ContainsKey(paramString))
                    asset.dicBool[paramString] = paramBool;
            }
            else if (type == ActionType.IncreaseFloatVariable)
            {
                asset.dicFloat[paramString] += paramFloat;
            }
            else if (type == ActionType.DecreaseFloatVariable)
            {
                asset.dicFloat[paramString] -= paramFloat;
                Debug.Log("Action - process - "+ paramString+ " =+ "+ asset.dicFloat[paramString]);
            }
            else if (type == ActionType.AttachToCamera)
            {
                asset.transform.parent = GameObject.Find("Inventory").transform;
                asset.transform.localPosition = Vector3.zero;
                asset.transform.localRotation = Quaternion.identity;
            }
            else if (type == ActionType.SetScale)
            {
                asset.transform.localScale = new Vector3(paramFloat, paramFloat, paramFloat);
            }
        }

		public void enableRender(Asset asset){
            if (!paramBool)
            {
                Renderer[] hingeJoints = asset.GetComponentsInChildren<Renderer>();

                foreach (Renderer joint in hingeJoints)
                    joint.enabled = true;
            }
            else
            {
                Renderer hingeJoint = asset.GetComponent<Renderer>();
                hingeJoint.enabled = true;
            }
		}

		public void disableRender(Asset asset){
            if (!paramBool)
            {
                Renderer[] hingeJoints = asset.GetComponentsInChildren<Renderer>();

                foreach (Renderer joint in hingeJoints)
                    joint.enabled = false;
            }
            else
            {
                Renderer hingeJoint = asset.GetComponent<Renderer>();
                hingeJoint.enabled = false;
            }
		}

        public object Clone()
        {
			return new Action(this.type, this.asset, this.paramVec);
        }

        public override string ToString()
        {
            if (type != ActionType.None)
                return type + ", " + asset.name + ", " + paramVec;
            else
                return type.ToString();
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Action p = (Action)obj;
            return (type == p.type) && (asset == p.asset) && (paramVec == p.paramVec);
        }
    }
}
