using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    GameObject current = null;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Initialize();
        SoundManager.PlaySound(SoundManager.Sound.BackgroundMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Player")
            {
                Debug.Log("Player Hit");
                current = hit.collider.gameObject;

                current.GetComponent<BaseCompanion>().ModifyStat(BaseCompanion.Statistic.Affection, 1);
                Debug.Log(current.GetComponent<BaseCompanion>().GetStat(BaseCompanion.Statistic.Affection));

                SoundManager.PlaySound(SoundManager.Sound.Boop, hit.collider.transform);
            }
        }
    }
}
