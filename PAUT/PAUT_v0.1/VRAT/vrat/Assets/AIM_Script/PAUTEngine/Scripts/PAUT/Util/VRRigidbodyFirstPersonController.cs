using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class VRRigidbodyFirstPersonController : RigidbodyFirstPersonController {
    public PAUT.Hand primaryHand;
    public PAUT.Hand secondaryHand;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        if (m_RigidBody == null)
        {
            m_RigidBody = GetComponentInChildren<Rigidbody>();
        }
        m_Capsule = m_RigidBody.GetComponent<CapsuleCollider>();
        if (m_Capsule == null)
        {
            m_Capsule = GetComponentInChildren<CapsuleCollider>();
        }
        mouseLook.Init(transform, cam.transform);
    }

    public override Vector2 GetInput()
    {
        movementSettings.CurrentTargetSpeed = primaryHand.walkingSpeed + secondaryHand.walkingSpeed;
        Vector2 input = new Vector2(0, 1);
        return input;
    }
}
