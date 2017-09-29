using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PAUT
{
    public class AssetGroup : Placable
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // to be implemented
        public object Clone()
        {
            AssetGroup clone = new AssetGroup();
            return clone;
        }
    }
}
