using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    Collider c;
    RaycastHit hit;
    RaycastHit[] hits;
    public LayerMask ignoreLayer;
    public void Attack()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, c.bounds.size.x, ~ignoreLayer))
        {
            if (hit.transform.tag == "Player")
                Debug.Log("Enemy has hit");
        }
        hits = Physics.SphereCastAll(transform.position, 0.8f, transform.forward, 2.1f);
        foreach(RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Player")
            {
                Debug.Log("Enemy has hit");
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        c = this.GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
