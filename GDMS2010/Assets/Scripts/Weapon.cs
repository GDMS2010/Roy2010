using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public const int MAXLV = 5;
    public string Name;
    public bool unlocked = false;
    [Range(1,99)]
    public int Damage;
    [Range(0.1f,99f)]
    public float FireRate;
    [Range(1,99)]
    public int MagazineSize;

    [Range(0,5)]
    public int DamageLv;
    [Range(0,5)]
    public int FireRateLv;
    [Range(0,5)]
    public int MagazineLv;


    [Header("Craft Cost")]
    [Space(1)]
    public Vector2 CraftCost;
    [Header ("Upgrade Cost")]
    [Space(1)]
    //ignore index 0 
    //index 1 will be upgrade to lv1 cost, etc
    public List<Vector2> Dmg_UpgradeCost;
    public List<Vector2> FR_UpgradeCost;
    public List<Vector2> M_UpgradeCost;
    [Header("Upgrade Stat")]
    [Space(1)]
    public List<int> DmgPerLv;
    public List<float> FRPerLv;
    public List<int> MagPerLv;
    // Start is called before the first frame update
    void Start()
    {
        //DamageLv = FireRateLv = MagazineLv = 0;
        Damage = DmgPerLv[DamageLv];
        FireRate = FRPerLv[FireRateLv];
        MagazineSize = MagPerLv[MagazineLv];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Craft()
    {
        unlocked = true;
    }

    public Vector2 getDMG_Upgrade_Cost()
    {
        if (DamageLv == MAXLV)
            return new Vector2(-1, -1);
        return Dmg_UpgradeCost[DamageLv];
    }
    public Vector2 getFR_Upgrade_Cost()
    {
        if (FireRateLv == MAXLV)
            return new Vector2(-1, -1);
        return FR_UpgradeCost[FireRateLv];
    }
    public Vector2 getMag_Upgrade_Cost()
    {
        if (MagazineLv == MAXLV)
            return new Vector2(-1, -1);
        return M_UpgradeCost[MagazineLv];
    }

    public void DMG_Upgrade()
    {
        DamageLv++;
        Damage = DmgPerLv[DamageLv];
    }
    public void FR_Upgrade()
    {
        FireRateLv++;
        FireRate = FRPerLv[FireRateLv];
    }
    public void Mag_Upgrade()
    {
        MagazineLv++;
        MagazineSize = MagPerLv[MagazineLv];
    }

    public int NextDMG_stat()
    {
        if (DamageLv == MAXLV)
            return -1;
        return DmgPerLv[DamageLv+1];
    }
    public float NextFR_stat()
    {
        if (FireRateLv == MAXLV)
            return -1;
        return FRPerLv[FireRateLv + 1];
    }
    public int NextMag_stat()
    {
        if (MagazineLv == MAXLV)
            return -1;
        return MagPerLv[MagazineLv + 1];
    }
}
