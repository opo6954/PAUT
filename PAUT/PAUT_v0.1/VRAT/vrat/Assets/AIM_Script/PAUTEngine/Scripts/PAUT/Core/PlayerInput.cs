using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PAUT
{
    public enum PlayerInputType { Controller, Camera, Body };

    public class PlayerInput : Placable
    {
        public PlayerInputType inputType;
        public Player player;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.GetComponent<Asset>() != null)
            {
                // get player component from parent node
                Player player = this.gameObject.transform.root.GetComponent<Player>();
				if (player == null || col.collider.tag == "Dummy")
					return;
                Debug.Log("PlayerInput - OnCollisionEnter - " + player.name);
                // create an event
                Trigger e1 = new Trigger(player, col.gameObject.GetComponent<Asset>(), player.curEvent, new CollisionParam(TriggerConditionType.CollisionEnter));
				Transition.OnEventReceive (e1);
            }
        }

		void OnCollisionExit(Collision col)
		{
            Debug.Log("PlayerInput - OnCollisionExit");
            if (col.gameObject.GetComponent<Asset>() != null)
			{
				// get player component from parent node
				Player player = this.gameObject.transform.parent.GetComponent<Player>();
				if (player == null || col.collider.tag == "Dummy")
					return;

                // create an event
                Trigger e1 = new Trigger(player, col.gameObject.GetComponent<Asset>(), player.curEvent, new CollisionParam(TriggerConditionType.CollisionEnter));
                Transition.OnEventReceive (e1);

			}
		}
    }
}
