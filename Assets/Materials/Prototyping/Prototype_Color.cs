using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Prototype_Color : MonoBehaviour
{
    [SerializeField] Color color;

    void Start()
    {
        if (!Application.isPlaying )
        {
            gameObject.layer = LayerMask.NameToLayer("Prototype");
        }
    }

    void Update()
    {
        if (!Application.isPlaying )
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material sharedMaterial = renderer.sharedMaterial;
                if (sharedMaterial != null)
                {
                    sharedMaterial.color = color;
                }
            }
        }
    }
}
