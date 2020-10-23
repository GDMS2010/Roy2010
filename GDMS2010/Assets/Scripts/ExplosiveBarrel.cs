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
            Animator anim = collider.GetComponent<Animator>();

            if (anim != null)
            {
                StartCoroutine(RagDoll(anim));
            }

            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.enabled = false;
            }

            NavMeshAgent navMesh = collider.GetComponent<NavMeshAgent>();

            if (navMesh != null)
            {
                navMesh.enabled = false;
            }

            Script_Health _Health = collider.GetComponent<Script_Health>();


            if (_Health != null)
            {
                _Health.takeDamage(50);
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
