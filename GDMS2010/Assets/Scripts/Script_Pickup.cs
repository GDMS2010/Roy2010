using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Script_Pickup : MonoBehaviour
{
    [SerializeField]
    Sprite itemSprite;
    [SerializeField]
    Image spriteContainer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!spriteContainer)
                spriteContainer = GameObject.Find("KeyItemSprite").GetComponent<Image>(); //If not assigned, hard look for it
            if (!spriteContainer)
                Debug.LogError("No container for sprite was found on Pickup Script");

            spriteContainer.sprite = itemSprite;
            spriteContainer.color = Color.white;
            this.transform.parent.gameObject.SetActive(false); //instead of destroying, set to inactive and let unity handle garbage
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SphereCollider sc = this.GetComponent<SphereCollider>();
        if (!sc)
            Debug.LogError("Pickup script requires a sphere collider to work!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
