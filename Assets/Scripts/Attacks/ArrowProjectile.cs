using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float destroyDistance = 0.5f;
    public float maxDistance = 20f;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the arrow if it reaches target or goes to max distance
        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance ||
            Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
    
    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }
}
