using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerScript : MonoBehaviour
{
    public bool isAttacking = false;
    public CharacterController characterController;
    public Transform enemy;

    public float speed = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isAttacking == false)
        {
            //Attack();
            //while (Vector3.Distance(transform.position, enemy.position) > 2f)
            //{
                MoveTowardsTarget(0.1f);
                //isAttacking = true;
                        Debug.Log("Should Be Moving");
                        //var step = speed * Time.deltaTime;
                        //transform.position = Vector3.MoveTowards(transform.position, enemy.position, step);
                //transform.Rotate(enemy.position);
                //characterController.Move(Vector3.forward * speed * Time.deltaTime);
            //} 
        }

        void MoveTowardsTarget(float speed)
        {
            transform.DOLookAt(enemy.position, 0.2f);
            transform.DOMove(transform.position, speed);
        }
        transform.DOMove(transform.position, 0.5f);

        //if (Vector3.Distance(transform.position, enemy.position) < 2.2f)
        //{
        //    StartCoroutine(resetAttack());
        //}
    }

    private IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }
   /* public void Attack()
    {
        //attack
        while (Vector3.Distance(transform.position, enemy.position) > 2f)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, enemy.position, step);
        }

        isAttacking = true;
    } */


}
