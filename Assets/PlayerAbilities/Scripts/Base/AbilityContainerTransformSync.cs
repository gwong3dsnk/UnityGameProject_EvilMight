using UnityEngine;

/// <summary>
/// Belongs on PlayerAbilityContainer gameobject and contains all player instantiated abilities.
/// Matches player mesh's position and rotation transforms.
/// </summary>
public class AbilityContainerTransformSync : MonoBehaviour
{
    [SerializeField] Transform player;

    private void Update() 
    {
        if (player != null)
        {
            transform.position = player.position;
            transform.rotation = player.rotation;
        }
    }
}