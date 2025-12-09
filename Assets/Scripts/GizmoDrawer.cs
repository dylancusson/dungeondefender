using UnityEngine;

public class GizmoDrawer : MonoBehaviour
{

    //[SerializeField] string ColorName;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
