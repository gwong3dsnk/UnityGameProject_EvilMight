using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTransforms : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;

    void Update()
    {
        transform.position = objectToFollow.transform.position;
        transform.rotation = objectToFollow.transform.rotation;
    }
}
