using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollController : MonoBehaviour
{
    public Animator anim;
    public Rigidbody EnemyRB;
    public NavMeshAgent navAgent;
    public Enemy enemy;
    public BoxCollider boxCollider;

    private Rigidbody[] rigidbodies;
    private Collider[] colliders;

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ActivateRagdoll();
        }
    }
    private void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider col in colliders)
        {
            col.enabled = enabled;
        }
    }

    private void SetRigidbodiesKinematic(bool kinematic)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = kinematic;
        }
    }

    public void ActivateRagdoll()
    {
        boxCollider.enabled = false;
        //EnemyRB.isKinematic = true;
        navAgent.enabled = false;
        enemy.enabled = false;
        anim.enabled = false;

        SetCollidersEnabled(true);
        SetRigidbodiesKinematic(false);
    }
}
