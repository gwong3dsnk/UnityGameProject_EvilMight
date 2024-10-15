using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyEasy : RangedEnemy
{
    void Start()
    {
        SetClassAndDifficulty(EnemyClass.Range, EnemyDifficulty.Easy);
    }
}
