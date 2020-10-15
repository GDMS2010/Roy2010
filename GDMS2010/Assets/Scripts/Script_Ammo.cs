using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ammo : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            GunScript gs = GameObject.FindGameObjectWithTag("Weapon").GetComponent<GunScript>();
            gs.bulletsIHave += 10;
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
