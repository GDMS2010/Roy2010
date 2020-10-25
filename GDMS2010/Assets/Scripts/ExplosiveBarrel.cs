using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ExplosiveBarrel : MonoBehaviour
{
    public float radius = 3.0f;
    public float force = 200.0f;
    public int[] test;

    public GameObject explosion;


    // Start is called before the first frame update
    void Start()
    {
        explosion.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }
    }

    void Explode()
    {
        //Instantiate(explosion, transform.position, transform.rotation);

        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in collidersToDestroy)
        {
            Destructable destructable = collider.GetComponent<Destructable>();

            if (destructable != null)
            {
                destructable.Destroy();
            }
        }
        Collider[] collidersToDeactivate = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in collidersToDeactivate)
        {
            RagdollController ragdoll = collider.GetComponent<RagdollController>();

            if (ragdoll != null)
            {
                ragdoll.ActivateRagdoll();
            }


            Script_Health _Health = collider.GetComponent<Script_Health>();


            if (_Health != null)
            {
                _Health.Hit(transform.gameObject, 100);
            }
        }


        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in collidersToMove)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }

        Destroy(gameObject);
    }
    IEnumerator RagDoll(Animator anim)
    {
        anim.enabled = false;

        yield return new WaitForSeconds(1);

        anim.enabled = true;
    }

}
