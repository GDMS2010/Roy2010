using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Script_Health health = collision.transform.GetComponent<Script_Health>();
            health.Heal(10);
            Destroy(this.gameObject);
        }
    }
}
