using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCompanion : MonoBehaviour
{
    public enum Statistic
    {
        Affection
    }

    [SerializeField]
    private int affection = 0;

    public void ModifyStat(Statistic _statistic, int _number)
    {
        switch (_statistic)
        {
            default:
                return;

            case Statistic.Affection:
                this.affection += _number;
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
        }
    }
}
