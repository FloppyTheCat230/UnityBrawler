using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemyDetector : MonoBehaviour
{
    public Transform playerTransform;
    public Transform closestEnemy;
    public PlayerMovement playerScript;
    public CameraInfor ci;

    private void Update()
    {
        if (!playerScript.isAttacking)
        {
            float minDistance = float.MaxValue;
            foreach (var enemy in FindObjectsOfType<VERYQuickEnemScript>())
            {
                if (enemy.isAttackable)
                {
                    float distance = Vector3.Distance(enemy.transform.position, ci.centerScreenPoint);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestEnemy = enemy.transform;
                    }
                }
            }
        }
    }

}
