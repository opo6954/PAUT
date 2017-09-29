using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace PAUT
{
    public class Scenario : MonoBehaviour
    {
        public string scenarioName;
        public List<Timeline> timelines = new List<Timeline>();
        public List<GameObject> timelineObjs = new List<GameObject>();
        private Timeline tempTimeline;
        public bool VRMode = false;

        public void AddTimeline()
        {
            GameObject tempTimelineObj = new GameObject("Timeline");
            //Add Components
            tempTimelineObj.AddComponent<Timeline>();
            tempTimelineObj.transform.parent = this.transform;
            timelineObjs.Add(tempTimelineObj);
            timelines.Add(tempTimelineObj.GetComponent<Timeline>());
        }

        // Use this for initialization
        void Start()
        {
            VRMode = VRSettings.enabled;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
