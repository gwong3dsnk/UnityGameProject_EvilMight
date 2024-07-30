using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTracking : MonoBehaviour
{
    [SerializeField] Transform playerMesh;
    Vector3 offset = new Vector3(0f, 35f, -10f);

    void Update()
    {
        if (playerMesh != null)
        {
            transform.position = playerMesh.position + offset;
        }

    }
}
