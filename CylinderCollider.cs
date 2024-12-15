using UnityEngine;

public class CylinderCollider : MonoBehaviour
{
    public bool isTrigger = false;
    public PhysicsMaterial material = null;
    public Vector3 center = Vector3.zero;
    public float radius = 0.5f;
    public float height = 2;
    public Orientation orientation = Orientation.Y_axis;
    public enum Orientation { X_axis, Y_axis, Z_axis }

    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        BuildCollider();
    }

    public static bool CheckCylinder(Vector3 position, float height, float radius)
    {
        return Physics.CheckCapsule(position - new Vector3(0, height / 2, 0), position + new Vector3(0, height / 2, 0), radius);
    }

    public static bool CheckCylinder(Vector3 position, float height, float radius, Quaternion rotation, LayerMask mask)
    {
        return Physics.CheckCapsule(position - new Vector3(0, height / 2, 0), position + new Vector3(0, height / 2, 0), radius, mask);
    }

    public void BuildCollider()
    {
        ClearCollider();

        capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.center = center;
        capsuleCollider.radius = radius;
        capsuleCollider.height = height;
        capsuleCollider.direction = (int)orientation;
        capsuleCollider.material = material;
        capsuleCollider.isTrigger = isTrigger;
    }

    public void ClearCollider()
    {
        if (capsuleCollider != null)
        {
            DestroyImmediate(capsuleCollider);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + center + Vector3.up * (height / 2 - radius), radius);
        Gizmos.DrawWireSphere(transform.position + center - Vector3.up * (height / 2 - radius), radius);
        Gizmos.DrawLine(transform.position + center + Vector3.up * (height / 2 - radius), transform.position + center - Vector3.up * (height / 2 - radius));
    }
}
