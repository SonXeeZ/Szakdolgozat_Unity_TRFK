using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour
{
    //https://www.youtube.com/watch?v=jN-27UawCgU&list=PLivfKP2ufIK6ToVMtpc_KTHlJRZjuE1z0&index=25&ab_channel=GameGrind TR -- 2023.03.25 18:31
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    //public Item ItemReward { get; set; }
    public bool Completed { get; set; }

    public void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
    }

}
