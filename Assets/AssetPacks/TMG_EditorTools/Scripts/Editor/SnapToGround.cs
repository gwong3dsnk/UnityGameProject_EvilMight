using UnityEngine;
using UnityEditor;

namespace TMG_EditorTools
{
    public class SnapToGround : MonoBehaviour
    {

        [MenuItem("Tools/TMG_EditorTools/Snap To Ground %g")]
        public static void Ground()
        {
            foreach (var transform in Selection.transforms)
            {
                var hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 999999f);
                foreach (var hit in hits)
                {
                    if (hit.collider.gameObject == transform.gameObject)
                        continue;

                    transform.position = hit.point;
                    break;
                }
            }
        }

    }

}
