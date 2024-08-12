using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefabLibrary : MonoBehaviour
{
    [SerializeField] EnemyPrefabAssignments[] enemyPrefabAssignments;
    public EnemyPrefabAssignments[] EnemyPrefabAssignments { get { return enemyPrefabAssignments; } }
}
