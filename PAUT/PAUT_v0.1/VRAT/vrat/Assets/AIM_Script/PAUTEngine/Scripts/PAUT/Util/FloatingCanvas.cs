using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingCanvas : MonoBehaviour
{
    Vector3 targetPosition;
    Vector3 targetRotation;

    public bool isFixed = false;
    public float distance = 3.0f;
    // Use this for initialization
    void Start()
    {
        //RepositionCanvas();
    }

    public void Deactivate()
    {

    }

    public void AdjustRotation()
    {
        // Get a Vector that points from the target to the main camera.
        Vector3 directionToTarget = Camera.main.transform.position - transform.position;

        // If we are right next to the camera the rotation is undefined. 
        if (directionToTarget.sqrMagnitude < 0.001f)
        {
            return;
        }

        // Calculate and apply the rotation required to reorient the object
        transform.rotation = Quaternion.LookRotation(-directionToTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFixed)
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, 0.2f);

        //this.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3.0f;
        //this.transform.forward = Vector3.Lerp(this.transform.forward, targetRotation, 0.2f);
        RepositionCanvas();
        AdjustRotation();
    }
    
    public void RenderSelect(GameObject obj, bool turnOn)
    {
        foreach (Transform child in obj.transform)
        {
            child.gameObject.SetActive(turnOn);
        }
    }

    public void ActivateModeSelect()
    {
        //objectSelectUI.SetActive(false);
        //modeSelectUI.SetActive(true);
        //showingMode = 1;
        //RenderSelect(objectSelectUI, false);
        //RenderSelect(modeSelectUI, true);
    }

    public void RepositionCanvas()
    {
        if(!isFixed)
            targetPosition = (Camera.main.transform.position + Camera.main.transform.forward * distance);
        //targetRotation = new Vector3(0, 0, 1);//Camera.main.transform.forward;
    }

}
