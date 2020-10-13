using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int gold;
    public int metal;
    public List<Weapon> weaponList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void earnGold(int amount)
    {
        gold += amount;
    }

    public void earnMetal(int amount)
    {
        metal += amount;
    }
}
