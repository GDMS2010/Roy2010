using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCompanion : MonoBehaviour
{
    public enum Statistic
    {
        Affection,
        Dialouge
    }

    [SerializeField]
    private int affection = 0;
    [SerializeField]
    private int dialouge = 0;
    [SerializeField]
    private string npcName = null;
    [SerializeField]
    public string[] Filenames;

    public void ModifyStat(Statistic _statistic, int _number)
    {
        switch (_statistic)
        {
            default:
                return;

            case Statistic.Affection:
                this.affection += _number;
                break;

            case Statistic.Dialouge:
                this.dialouge += _number;
                break;
        }
    }

    public int GetStat(Statistic _statistic)
    {
        switch (_statistic)
        {
            default:
                return 0;

            case Statistic.Affection:
                return this.affection;

            case Statistic.Dialouge:
                return this.dialouge;
        }
    }

    public string GetName()
    {
        return this.npcName;
    }
}
