using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endlevel : MonoBehaviour
{
    public bool unlocked = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (unlocked)
                SceneManager.LoadScene("Camp");
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
