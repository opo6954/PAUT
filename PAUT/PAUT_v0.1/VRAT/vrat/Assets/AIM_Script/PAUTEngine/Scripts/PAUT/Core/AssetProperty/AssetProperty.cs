using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetProperty : MonoBehaviour {

	public string name = "noname";
	public string description;
	public BoxCollider boundingBox;
	public string ownUserName;
	/*/
	Constraint (Operable range of Object)
	Range of orientation, position.. etc
	Weak constraint (이동은 가능하지만 동작은 X)
	Visualization properties
	//*/

	public void SetOwner(string ownerName){
		ownUserName = ownerName;
	}

	public bool isHeOwner(string name){
		return name == ownUserName;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
