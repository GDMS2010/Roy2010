using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarrelAI : CompanionAIOption
{
    public GameObject Ammo;
    public GameObject HPPack;
    public override void AffecsionP99()
    {
        GameObject go = HPPack;
        go.GetComponent<HealthPack>().Amount = 20;
        Instantiate(go, transform.position, transform.rotation);
    }
    public override void AffecsionP50()
    {
        GameObject go = HPPack;
        go.GetComponent<HealthPack>().Amount = 10;
        Instantiate(go, transform.position, transform.rotation);
    }
    public override void AffecsionP25()
    {
        Instantiate(Ammo, transform.position, transform.rotation);
    }
    public override void Affecsion0()
    {
        throw new System.NotImplementedException();
    }
    public override void AffecsionN25()
    {
        throw new System.NotImplementedException();
    }
    public override void AffecsionN50()
    {
        throw new System.NotImplementedException();
    }

}
