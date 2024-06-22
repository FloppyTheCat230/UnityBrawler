using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthQuick : MonoBehaviour
{
    public PlayerAnimationChecker checker;
    public SecondMovement pm;
    public Transform player;
    public Animator animator;

    private int xx = 0;

    private int health = 100;
    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= 2.5f)
        {
            //Debug.Log("player nearby");
            //if (checker.hasPlayerAnimationAttackedYet)
            //{
            if (pm.isAttacking == true)
            {
                if (xx == 0)
                {
                    animator.SetBool("IsBeingHit", true);
                    Debug.Log("enemyhit");
                    TakeDamage();
                    //xx = 1;
                }
            }
            else
            {
                xx = 0;
            } 
            //}
        }

        if (pm.LocalAttackTriad > 3)
        {
            pm.LocalAttackTriad = 0;
        }

    }

    private void TakeDamage()
    {
        //take damage animation
        Debug.Log("enemyhit");
        health -= 25;
        pm.Combo += 1;
        pm.LocalAttackTriad += 1;
        StartCoroutine(waitForHitCooldow());
        xx = 1;
    }

    private IEnumerator waitForHitCooldow()
    {
        yield return new WaitForSeconds(0.6f);
        animator.SetBool("IsBeingHit", false);
    }
}
