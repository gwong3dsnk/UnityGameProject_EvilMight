using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveController : MonoBehaviour
{
    [SerializeField] EnemyWaveContainer[] enemyWaveContainer;
    public EnemyWaveContainer[] EnemyWaveContainer { get { return enemyWaveContainer; } }
}
