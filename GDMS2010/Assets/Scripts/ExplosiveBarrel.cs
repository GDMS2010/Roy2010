using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float radius = 3.0f;
    public float force = 200.0f;

    public GameObject explosion;


    // Start is called before the first frame update
    void Start()
    {

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

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in collidersToMove)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }

        Destroy(gameObject);
    }
}
