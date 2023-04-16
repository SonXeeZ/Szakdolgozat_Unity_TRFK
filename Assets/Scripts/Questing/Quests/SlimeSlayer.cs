using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeSlayer : Quest
{
    // Start is called before the first frame update


    void Start()
    {
        QuestName = "Slime Slayer";
        Description = "Kill 5 slimes";
        ExperienceReward = 100;

        Goals.Add(new KillGoal(this, 0, "Kill 5 slimes", false, 0, 5));

        Goals.ForEach(g => g.Init());
    }

}
