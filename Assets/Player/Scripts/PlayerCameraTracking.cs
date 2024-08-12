using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTracking : MonoBehaviour
{
    [SerializeField] Transform playerMesh;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position;
    }

    void Update()
    {
        if (playerMesh != null)
        {
            transform.position = playerMesh.position + offset;
        }

    }
}
