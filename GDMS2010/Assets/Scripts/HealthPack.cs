using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Range(0,999)]
    public int Amount = 10;
    public float pos;
    GameObject body;
    // Start is called before the first frame update
    void Start()
    {
        body = transform.Find("Body").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.transform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(Time.time)), 0);
        body.transform.Rotate(0, 0.2f, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<Script_Health>().Heal(Amount);
            Destroy(this.gameObject);
        }
    }
}
