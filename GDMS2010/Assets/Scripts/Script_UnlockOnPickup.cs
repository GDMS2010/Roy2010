using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_UnlockOnPickup : MonoBehaviour
{
    private void OnDisable()
    {
        endlevel el =  GameObject.FindObjectOfType<endlevel>();
        if (el) el.unlocked = true;
        else
            Debug.LogError("No Endlevel is Found");
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
