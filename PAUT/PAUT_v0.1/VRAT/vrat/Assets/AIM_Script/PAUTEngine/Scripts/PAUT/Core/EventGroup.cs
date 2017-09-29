using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PAUT
{
    public class EventGroup : Arrangeable
    {
        List<Event> eventGroup;

        // Use this for initialization
        void Start()
        {
            eventGroup = new List<Event>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
