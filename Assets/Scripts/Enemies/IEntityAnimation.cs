using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityAnimation
{
    void SetTrigger(string name);
    void PlayAnimation(string name);
}
