using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{ // -- FK -- 2023.02.10
    private int health;

    private int maxHealth;

    private int damage;

    public Character(int health, int maxHealth, int damage)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        this.damage = damage;
    }
}
