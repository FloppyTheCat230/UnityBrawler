using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC_PlayerAttack : MonoBehaviour
{
    [Header("Attack Distances")]
    [SerializeField]
    private float shortRange = 5f;
    [SerializeField]
    private float longRange = 15f;

    private Animator animator;
    private TC_Player player;
    private TC_EnemyTargeter enemyTargeter;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<TC_Player>();
        enemyTargeter = GetComponent<TC_EnemyTargeter>();
    }

    void Update()
    {
        if (!player.CanAttack)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var enemy = enemyTargeter.EnemyTarget;
            if (enemy == null)
                return;

            var distance = Vector3.Distance(transform.position, enemy.position);

            if (distance > longRange)
                return;

            transform.LookAt(enemy);

            if (distance <= shortRange)
                MeleeAttack(enemy);
            else
                JumpAttack(enemy);
        }
    }

    private void JumpAttack(Transform enemy)
    {
        Debug.Log("Jump Attack");

        player.CanAttack = false;
        player.CanMove = false;

        animator.SetTrigger("JumpAttack");

        StartCoroutine(BeginJumpAttack());

        IEnumerator BeginJumpAttack()
        {
            var jumpDuration = 0.75f; 
            var time = 0f;

            var startPosition = transform.position;
            var endPosition = Vector3.MoveTowards(enemy.position, startPosition, 1f); // shift the landing position a little closer to where the player started

            while (time < jumpDuration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, time / jumpDuration);
                time += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition;
            player.CanAttack = true;
            player.CanMove = true;
        }
    }

    private void MeleeAttack(Transform enemy)
    {
        Debug.Log("Melee Attack");

        player.CanAttack = false;
        player.CanMove = false;
        animator.SetTrigger("MeleeAttack");

        StartCoroutine(BeginMeleeAttack());

        IEnumerator BeginMeleeAttack()
        {
            var animationDuration = 0.5f;
            var time = 0f;

            while (time < animationDuration)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("combo melee attack");
                }
                    
                time += Time.deltaTime;
                yield return null;
            }

            player.CanAttack = true;
            player.CanMove = true;
        }
    }



}
