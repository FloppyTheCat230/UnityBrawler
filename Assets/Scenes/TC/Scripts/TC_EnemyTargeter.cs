using UnityEngine;
using Cinemachine;

public class TC_EnemyTargeter : MonoBehaviour
{
    [Header("Detection Settings")]
    public float DetectionDistanceMax = 100f;
    public float DetectionSphereRadius = 1f; 
    public LayerMask TargetingLayer; 

    private Vector3 lastSphereCastOrigin;
    private Vector3 lastSphereCastDirection;
    private float lastSphereCastDistance;

    private Camera cam;

    private Transform enemyTransform;
    public Transform EnemyTarget { get => enemyTransform; }

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        DetectLookAt();
    }

    private void DetectLookAt()
    {
        if (cam == null)
            return;

        // our direction is the camera's forward vector, but we ignore the Y so it's not "looking down" at the ground
        var direction = cam.transform.forward;
        direction.y = 0;

        // Adjust origin to be behind the player so the sphere cast can hit close objects
        var origin = transform.position - direction * DetectionSphereRadius;

        // Cast a sphere in our direction. this is "wider" than a raycast and can detect objects not directly in the aim of the camera
        if (Physics.SphereCast(origin, DetectionSphereRadius, direction, out RaycastHit hit, DetectionDistanceMax, TargetingLayer))
        {
            Debug.Log($"Player target: {hit.transform.name}");
            enemyTransform = hit.transform;
            enemyTransform.GetComponent<TC_Enemy>().ToggleHighlight(true);
        }
        else
        {             
            enemyTransform?.GetComponent<TC_Enemy>().ToggleHighlight(false);
            enemyTransform = null;
        }

        // Store the last sphere cast parameters for gizmo drawing
        lastSphereCastOrigin = origin;
        lastSphereCastDirection = direction;
        lastSphereCastDistance = hit.distance > 0 ? hit.distance : DetectionDistanceMax;
    }

    private void OnDrawGizmos()
    {
        // Draw a line from the camera to the maximum distance of the sphere cast
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(lastSphereCastOrigin, lastSphereCastOrigin + lastSphereCastDirection * lastSphereCastDistance);

        // Draw a sphere at the end of the cast to represent the sphere cast's area
        Gizmos.color = Color.red;
        Vector3 sphereEndPosition = lastSphereCastOrigin + lastSphereCastDirection * lastSphereCastDistance;
        Gizmos.DrawWireSphere(sphereEndPosition, DetectionSphereRadius);
    }
}
