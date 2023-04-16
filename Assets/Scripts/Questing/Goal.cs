using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    //https://www.youtube.com/watch?v=up6HcYph_bo&ab_channel=GameGrind TR -- 2023.03.24 15:30
    public Quest Quest { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public virtual void Init()
    {
        //default init stuff
    }

    public void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }

    public void Complete()
    {
        Quest.CheckGoals();
        Completed = true;
        Debug.Log("Goal completed!");
    }
}
