using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_playerDeath : MonoBehaviour
{
    [SerializeField]
    Canvas death;
    [SerializeField]
    Camera backupCamera;

    private Script_Health health;
    private void Awake()
    {
        death.gameObject.SetActive(false);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Script_Health>();
        health.destroyOnDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.getHealth() <= 0)
        {
            death.gameObject.SetActive(true);
            backupCamera.gameObject.SetActive(true);
            GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
            if (weapon)
                Destroy(weapon);
            Destroy(this.gameObject);
        }

    }
}
