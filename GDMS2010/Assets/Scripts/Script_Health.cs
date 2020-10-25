using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Script_Health : MonoBehaviour
{

    [SerializeField]
    int health;
    [SerializeField][Tooltip("Assign a display to show current health")]
    Text display;

    GameHUD hud;
    public UnityEvent<GameObject> HitEvent;
    public UnityEvent DeathEvent;

    public bool destroyOnDeath = true;
    public bool IsAlive = true;
    int maxHealth = 0;
    int companionID;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        if(this.gameObject.tag == "Player")
        {
            hud = FindObjectOfType<GameHUD>();
            hud.setMaxHealth(maxHealth);
            hud.setCurrentHealth(health);
        }
        if (this.gameObject.tag == "Companion")
        {
            hud = FindObjectOfType<GameHUD>();
            companionID = hud.addCompanion(maxHealth, transform.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth();
    }

    public void Heal(int value)
    {
        health += value;
        health = health > maxHealth ? maxHealth : health;
        HUD_HPupdate();
    }

    public void Hit(GameObject attacker, int value)
    {
        health -= value;
        HUD_HPupdate();
        if (health <= 0)
        {
            die();
        }
        else
        {
            HitEvent.Invoke(attacker);
        }
    }

    private void die()
    {
        IsAlive = false;
        DeathEvent.Invoke();
        //run death animation

        //destroy game object after death animation
        Script_DropLoot hasLoot = this.transform.GetComponent<Script_DropLoot>();
        if (hasLoot)
        {
            hasLoot.Drop();
            Inventory inv =
            FindObjectOfType<Inventory>();
            inv.earnGold(10);
            inv.earnMetal(10);
        }
        if (destroyOnDeath)
            Destroy(this.gameObject);
    }

    private void DisplayHealth()
    {
        if (display)
        {
            display.text = health + " / " + maxHealth;
        }
    }

    public float getHealth()
    {
        return health;
    }

    void HUD_HPupdate()
    {
        if (this.gameObject.tag == "Player")
            hud.setCurrentHealth((int)health);
        if (this.gameObject.tag == "Companion")
        {
            hud.setCompanionHP(health, companionID);
        }
    }
}
