using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int gold;
    public int metal;
    public List<Weapon> weaponList;
    protected static Inventory i;
    public static Inventory Instance
    {
        get
        {
            if (i != null)
                return i;

            i = FindObjectOfType<Inventory>();
            if (i != null)
                return i;

            GameObject tm = new GameObject("Inventory");
            i = tm.AddComponent<Inventory>();
            return i;
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

    public void earnGold(int amount)
    {
        gold += amount;
    }

    public void earnMetal(int amount)
    {
        metal += amount;
    }

    public int getWeaponIndex(string name)
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i].name == name)
                return i;
        }
        return -1;
    }
}
