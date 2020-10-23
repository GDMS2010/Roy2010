using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public int currHealth = 100;
    public int MaxHealth = 100;
    public bool IsAlive = true;
    public UnityEvent<GameObject> HitEvent;
    public UnityEvent DeathEvent;
    // Start is called before the first frame update
    public void Hit(GameObject attacker, int value)
    {
        currHealth -= value;
        if(currHealth <= 0)
        {
            Death();
        }
        else
        {
            HitEvent.Invoke(attacker);
        }
    }

    void Death()
    {
        IsAlive = false;
        DeathEvent.Invoke();
    }
}
