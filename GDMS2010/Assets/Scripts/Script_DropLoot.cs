using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class Script_DropLoot : MonoBehaviour
{
    [System.Serializable]
    private class Lootable
    {
        [NotNull]
        public GameObject item = null;
        [Tooltip("Range 0 - 100% chance of dropping")]
        [Range(0, 100)]
        public float chance = 0;
    }
    [SerializeField]
    List<Lootable> lootTable;
    [SerializeField]
    bool multipleDrops = false;
    // Start is called before the first frame update
    void Start()
    {
        if (lootTable.Count == 0)
            lootTable = new List<Lootable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        for (int i = 0; i < lootTable.Count; i++)
        {
            float randomChance = Random.Range(0, 100);
            if (randomChance < lootTable[i].chance)
            {
                if (!lootTable[i].item)
                    Debug.LogError("item on loot table needs to be set");
                Instantiate(lootTable[i].item, this.transform.position, this.transform.rotation);
                if (!multipleDrops)
                    return;
            }
        }
    }
}
