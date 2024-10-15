using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyNormal : RangedEnemy
{
    void Start()
    {
        SetClassAndDifficulty(EnemyClass.Range, EnemyDifficulty.Normal);
    }
}
