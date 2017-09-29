using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PAUT
{
	public class Placable : ObjectTemplate, ICloneable
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public object Clone()
        {
            Placable clone = new Placable();
            return clone;
        }
    }
}
