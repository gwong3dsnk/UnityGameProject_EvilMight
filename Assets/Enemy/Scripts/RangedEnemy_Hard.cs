using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyHard : RangedEnemy
{
    void Start()
    {
        SetClassAndDifficulty(EnemyClass.Range, EnemyDifficulty.Hard);
    }
}
