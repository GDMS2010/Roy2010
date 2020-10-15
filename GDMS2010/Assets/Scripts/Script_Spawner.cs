using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Spawner : MonoBehaviour
{

    [SerializeField]
    float timer = 1;
    [SerializeField]
    GameObject spawnPrefab;

    float maxTimer;
    // Start is called before the first frame update
    void Start()
    {
        maxTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (timer < 0)
        {
            Instantiate(spawnPrefab, this.transform.position, this.transform.rotation);
            timer = maxTimer;
        }
    }
}
