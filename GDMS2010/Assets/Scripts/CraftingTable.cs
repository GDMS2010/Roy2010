using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : MonoBehaviour
{
    Inventory inventory;
    public Text Currency_t;
    public Text noRes;
    int currentWeapon = -1;
    public GameObject FR_Panel;
    public GameObject DMG_Panel;
    public GameObject Mag_Panel;
    public Text Weapon_t;

    GameObject Craft_Panel;
    GameObject Upgrade_Panel;
    Color old;
    // Start is called before the first frame update
    void Start()
    {
        old = Currency_t.color;
        inventory = FindObjectOfType<Inventory>().GetComponent<Inventory>();
        if (!inventory)
            Debug.Log("Inventory Cant find");

        Craft_Panel = transform.Find("Craft Panel").gameObject;
        Upgrade_Panel = transform.Find("Upgrade Panel").gameObject;

        iconUpdate();
        currencyUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            inventory.earnGold(100);
            currencyUpdate();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            inventory.earnMetal(100);
            currencyUpdate();
        }
    }

    void currencyUpdate()
    {
        Currency_t.text = string.Format("Gold : {0}   Metal : {1}", inventory.gold, inventory.metal);
    }

    public void upgrade(string type)
    {
        Weapon weapon = inventory.weaponList[currentWeapon];
        if (type == "FR")
        {
            if (inventory.gold < weapon.getFR_Upgrade_Cost().x || inventory.metal < weapon.getFR_Upgrade_Cost().y)
                StartCoroutine(notEnoughCur());
            else
            {
                inventory.earnGold(-(int)weapon.getFR_Upgrade_Cost().x);
                inventory.earnMetal(-(int)weapon.getFR_Upgrade_Cost().y);
                inventory.weaponList[currentWeapon].FR_Upgrade();
            }
        }
        else if (type == "DMG")
        {
            if (inventory.gold < weapon.getDMG_Upgrade_Cost().x || inventory.metal < weapon.getDMG_Upgrade_Cost().y)
                StartCoroutine(notEnoughCur());
            else
            {
                inventory.earnGold(-(int)weapon.getDMG_Upgrade_Cost().x);
                inventory.earnMetal(-(int)weapon.getDMG_Upgrade_Cost().y);
                inventory.weaponList[currentWeapon].DMG_Upgrade();
            }
        }
        else if (type == "MAG")
        {
            if (inventory.gold < weapon.getMag_Upgrade_Cost().x || inventory.metal < weapon.getMag_Upgrade_Cost().y)
                StartCoroutine(notEnoughCur());
            else
            {
                inventory.earnGold(-(int)weapon.getMag_Upgrade_Cost().x);
                inventory.earnMetal(-(int)weapon.getMag_Upgrade_Cost().y);
                inventory.weaponList[currentWeapon].Mag_Upgrade();
            }
        }
        else
            Debug.Log("Craft table say: ???");
        currencyUpdate();
        visualUpdate();
    }

    public void craft()
    {
        if (currentWeapon == -1) return;
        Weapon weapon = inventory.weaponList[currentWeapon];
        if (weapon.unlocked) return;
        if (inventory.gold < weapon.CraftCost.x || inventory.metal < weapon.CraftCost.y)
            StartCoroutine(notEnoughCur());
        else
        {
            inventory.earnGold(-(int)weapon.CraftCost.x);
            inventory.earnMetal(-(int)weapon.CraftCost.y);
            inventory.weaponList[currentWeapon].Craft();
            currencyUpdate();
            iconUpdate();
            visualUpdate();
            Craft_Panel.transform.Find("Cost").GetComponent<Text>().text = "";
            Craft_Panel.transform.Find("Description").GetComponent<Text>().text = string.Format("You have crafted {0}!", weapon.name);
        }
    }

    void visualUpdate()
    {
        if(currentWeapon == -1)
        {
            FR_Panel.SetActive(false);
            DMG_Panel.SetActive(false);
            Mag_Panel.SetActive(false);
            Craft_Panel.transform.Find("Description").GetComponent<Text>().text = "";
            Craft_Panel.transform.Find("Cost").GetComponent<Text>().text = "";
            Weapon_t.text = "";
        }
        else
        {
            FR_Panel.SetActive(true);
            DMG_Panel.SetActive(true);
            Mag_Panel.SetActive(true);
            Weapon weapon = inventory.weaponList[currentWeapon];
            if(weapon.unlocked==false)
            {
                Craft_Panel.transform.Find("Description").GetComponent<Text>().text = string.Format("{0}\nDamage : {1}\nFiring Rate : {2}\nMagazine Size : {3}", weapon.name, weapon.Damage, weapon.FireRate, weapon.MagazineSize);
                Craft_Panel.transform.Find("Cost").GetComponent<Text>().text = string.Format("Require :\nGold : {0}\nMetal : {0}", weapon.CraftCost.x, weapon.CraftCost.y);
            }

            if (weapon.FireRateLv != Weapon.MAXLV)
            {
                FR_Panel.transform.Find("Title").GetComponent<Text>().text = string.Format("Lv {0} Firing Rate", weapon.FireRateLv);
                FR_Panel.transform.Find("Description").GetComponent<Text>().text = string.Format("Next Level : \n{0}", weapon.NextFR_stat());
                FR_Panel.transform.Find("Require").GetComponent<Text>().text = string.Format("Require :\nGold : {0}\nMetal : {0}", weapon.getFR_Upgrade_Cost().x, weapon.getFR_Upgrade_Cost().y);
                FR_Panel.transform.Find("Upgrade").gameObject.SetActive(true);
            }
            else
            {
                FR_Panel.transform.Find("Title").GetComponent<Text>().text = string.Format("Lv {0} Firing Rate", weapon.FireRateLv);
                FR_Panel.transform.Find("Description").GetComponent<Text>().text = "Reached Max Level!";
                FR_Panel.transform.Find("Require").GetComponent<Text>().text = "";
                FR_Panel.transform.Find("Upgrade").gameObject.SetActive(false);
            }

            if (weapon.DamageLv != Weapon.MAXLV)
            {
                DMG_Panel.transform.Find("Title").GetComponent<Text>().text = string.Format("Lv {0} Damage", weapon.DamageLv);
                DMG_Panel.transform.Find("Description").GetComponent<Text>().text = string.Format("Next Level : \n{0}", weapon.NextDMG_stat());
                DMG_Panel.transform.Find("Require").GetComponent<Text>().text = string.Format("Require :\nGold : {0}\nMetal : {0}", weapon.getDMG_Upgrade_Cost().x, weapon.getDMG_Upgrade_Cost().y);
                DMG_Panel.transform.Find("Upgrade").gameObject.SetActive(true);
            }
            else
            {
                DMG_Panel.transform.Find("Title").GetComponent<Text>().text = string.Format("Lv {0} Damage", weapon.DamageLv);
                DMG_Panel.transform.Find("Description").GetComponent<Text>().text = "Reached Max Level!";
                DMG_Panel.transform.Find("Require").GetComponent<Text>().text = "";
                DMG_Panel.transform.Find("Upgrade").gameObject.SetActive(false);
            }

            if (weapon.MagazineLv != Weapon.MAXLV)
            {
                Mag_Panel.transform.Find("Title").GetComponent<Text>().text = string.Format("Lv {0} Magazine", weapon.MagazineLv);
                Mag_Panel.transform.Find("Description").GetComponent<Text>().text = string.Format("Next Level : \n{0}", weapon.NextMag_stat());
                Mag_Panel.transform.Find("Require").GetComponent<Text>().text = string.Format("Require :\nGold : {0}\nMetal : {0}", weapon.getMag_Upgrade_Cost().x, weapon.getMag_Upgrade_Cost().y);
                Mag_Panel.transform.Find("Upgrade").gameObject.SetActive(true);
            }
            else
            {
                Mag_Panel.transform.Find("Title").GetComponent<Text>().text = string.Format("Lv {0} Damage", weapon.DamageLv);
                Mag_Panel.transform.Find("Description").GetComponent<Text>().text = "Reached Max Level!";
                Mag_Panel.transform.Find("Require").GetComponent<Text>().text = "";
                Mag_Panel.transform.Find("Upgrade").gameObject.SetActive(false);
            }

            Weapon_t.text = string.Format("{0}\nDamage : {1}\nFiring Rate : {2}\nMagazine Size : {3}", weapon.name, weapon.Damage, weapon.FireRate, weapon.MagazineSize);
        }
    }
    public void changeWeapon(int index)
    {
        currentWeapon = index;
        visualUpdate();
    }

    IEnumerator notEnoughCur()
    {
        Currency_t.color = Color.red;
        noRes.gameObject.SetActive(true);
        //Beep sound
        yield return new WaitForSeconds(0.5f);
        Currency_t.color = old;
        noRes.gameObject.SetActive(false);
    }

    void iconUpdate()
    {
        List<GameObject> Cicons = new List<GameObject>();
        for (int i = 0; i < Craft_Panel.transform.Find("GunIconList").childCount; i++)
        {
            Cicons.Add(Craft_Panel.transform.Find("GunIconList").GetChild(i).gameObject);
        }
        List<GameObject> Uicons = new List<GameObject>();
        for (int i = 0; i < Upgrade_Panel.transform.Find("GunIconList").childCount; i++)
        {
            Uicons.Add(Upgrade_Panel.transform.Find("GunIconList").GetChild(i).gameObject);
        }

        foreach (var item in inventory.weaponList)
        {
            foreach (var icon in Cicons)
            {
                if(icon.name == item.name)
                {
                    icon.SetActive(!item.unlocked);
                }
            }

            foreach (var icon in Uicons)
            {
                if (icon.name == item.name)
                {
                    icon.SetActive(item.unlocked);
                }
            }
        }
    }

    public void tabPressed()
    {
        currentWeapon = -1;
        visualUpdate();
    }
}
