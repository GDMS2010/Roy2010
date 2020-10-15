﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_Health : MonoBehaviour
{

    [SerializeField]
    float health;
    [SerializeField][Tooltip("Assign a display to show current health")]
    Text display;

    public bool destroyOnDeath = true;

    float maxHealth = 0;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth();
    }

    public void takeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            die();
    }

    private void die()
    {
        //run death animation

        //destroy game object after death animation
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
}
