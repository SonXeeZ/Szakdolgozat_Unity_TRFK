using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvents : MonoBehaviour
{
    //https://github.com/GameGrind/Simple-RPG-in-Unity/blob/master/Assets/Scripts/CombatEvents.cs TR -- 2023.03.25 17:40
    public delegate void EnemyEventHandler(IEnemy enemy);
    public static event EnemyEventHandler OnEnemyDeath;

    public static void EnemyDied(IEnemy enemy)
    {
        if (OnEnemyDeath != null)
            OnEnemyDeath(enemy);
    }
}
