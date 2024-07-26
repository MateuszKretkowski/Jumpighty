using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class MeshColliderVisualizer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireMesh(meshCollider.sharedMesh, transform.position, transform.rotation, transform.localScale);
        }
    }
}
