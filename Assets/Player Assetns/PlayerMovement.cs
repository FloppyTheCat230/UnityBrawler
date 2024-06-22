using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController cc;
    public float attackComboTimer = 15f;
    public float Combo = 0;
    public PlayerAnimationChecker checker;

    private int xx = 0;

    public bool isAttackingFromFar;
    public bool isAttackingFromClose;

    public Camera cam;

    public Animator animate;
    //public Animation animation;

    public VERYQuickEnemScript enemyScript;

    public LayerMask mask;

    //THIS IS A TEST VARIABLE
    private bool testAttackTime = true;
    public float TimeLeft;
    public bool TimerOn = true;

    public float maxAttackRange = 7f;

    private float rotationSpeed = 20f;

    public float speed = 6f;

    public Transform came;

    public Transform enemy;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool isAttacking;
    public bool canAttack;

    public bool isAttacked;

    private float speed2test = 25f;

    private RaycastHit hit;

    public ClosestEnemyDetector detector;

    void Update()
    {
        RunAnimation();
        TimerScript();
        AttackComboTracker();
        Attacking();
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            if (!isAttacking)
            {
                animate.SetBool("IsKicking", false);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + came.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * direction.magnitude;
                cc.Move(moveDir.normalized * speed * Time.deltaTime);
                //animate.SetBool("IsRunning", true);
            }
        } 
    }


    private void RunAnimation()
    {
        bool pressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (!isAttacking)
        {
            animate.SetBool("IsRunning", pressed);
        }

        Debug.Log(isAttackingFromFar);
        Debug.Log(isAttackingFromClose);
    }

    private void Attacking()
    {
        enemy = detector.closestEnemy;

        if (Input.GetMouseButtonDown(0) && isAttacking == false)
        {
           // if (Combo < 1)
           // {
            //    isAttackingFromFar = false;
            //    isAttackingFromClose = true;
            //} else
            //{
                if (Vector3.Distance(enemy.position, transform.position) > 3f)
                {
                    isAttackingFromFar = true;
                    isAttackingFromClose = false;
                }

                if (Vector3.Distance(enemy.position, transform.position) < 3f)
                {
                    isAttackingFromFar = false;
                    isAttackingFromClose = true;
                }
           // }
        }

        if (Input.GetMouseButtonDown(0) && isAttacking == false)
        {
            if (Combo == 0)
            {
                if (Vector3.Distance(enemy.position, transform.position) <= maxAttackRange) //7f is the same as maxAttackRange, but when code works a little better, rewrite with variables
                {
                    if (testAttackTime)
                    {
                        canAttack = true;
                        speed2test = 30f;

                        if (canAttack)
                        {
                            isAttacking = true;
                        }
                    }
                }
            }

            if (Combo >= 1)
            {
                if (Vector3.Distance(enemy.position, transform.position) <= (maxAttackRange + (Combo * 2)))
                {
                    if (testAttackTime)
                    {
                        canAttack = true;
                        speed2test = 30f;

                        if (canAttack)
                        {
                            isAttacking = true;
                        }
                    }
                }

                if (maxAttackRange >= 25f)
                {
                    maxAttackRange = 25f;
                }
            }
        }

        if (isAttacking)
        {
            if (isAttackingFromClose)
            {
                if (Vector3.Distance(enemy.position, transform.position) >= 2.1f)
                {
                    animate.SetBool("IsPunching", true);

                    var step = speed2test * Time.deltaTime;
                    Vector3 direction = new Vector3(enemy.position.x - transform.position.x, 0f, enemy.position.z - transform.position.z).normalized;
                    transform.position += direction * step;

                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    }

                    float distance = Vector3.Distance(transform.position, new Vector3(enemy.position.x, transform.position.y, enemy.position.z));
                    speed2test = distance * 4;
                }
                else
                {
                    if (isAttacking)
                    {
                        StartCoroutine(resetAttackWaitTime());
                        //isAttacking = false;
                        //canAttack = true;
                    }
                }
            }

            if (isAttackingFromFar)
            {
                if (Vector3.Distance(enemy.position, transform.position) >= 2f)
                {
                    //if (checker.isPlayerAttackAnimationFinished)
                    //{
                    //}

                    animate.SetBool("IsKicking", true);

                    var step = speed2test * Time.deltaTime;
                    Vector3 direction = new Vector3(enemy.position.x - transform.position.x, 0f, enemy.position.z - transform.position.z).normalized;
                    transform.position += direction * step;

                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    }

                    float distance = Vector3.Distance(transform.position, new Vector3(enemy.position.x, transform.position.y, enemy.position.z));
                    speed2test = distance * 4;
                }

                if (Vector3.Distance(enemy.position, transform.position) <= 2.5f)
                {
                    //if (!canAttack) // LATER - when animations, set that when the animation is done, THEN allow player to start attacking again
                    //{
                    //    isAttacking = false;
                    //}
                    //if (checker.isPlayerAttackAnimationFinished && xx == 0)
                    //{
                    TimeLeft = 5f;
                    Combo += 1;
                    testAttackTime = false;

                    if (isAttacking)
                    {
                        StartCoroutine(resetAttackWaitTime());
                        isAttacking = false;
                        //canAttack = true;
                    }

                    // xx = 1;
                    //}
                    // else
                    //{
                    //     xx = 0;
                    //}

                    /* if (animate.GetCurrentAnimatorStateInfo(0).IsName("FrontFlipKickAgain"))
                     {
                         animate.SetBool("IsKicking", true);
                     } else
                     {
                         animate.SetBool("IsKicking", false);
                     } */
                    /*
                    if (isAttacking)
                    {
                        StartCoroutine(resetAttackWaitTime());
                        isAttacking = false;
                        //canAttack = true;
                    } */
                }
            }
        }

        if (!isAttacking)
        {
            //if (checker.isPlayerAttackAnimationFinished)
            //{
            animate.SetBool("IsKicking", false);
            animate.SetBool("IsPunching", false);
            //}
            //animate.SetBool("IsKicking", false);
        }
    }

    private void AttackComboTracker()
    {
        if (TimeLeft == 0)
        {
            Combo = 0;
        }
    }

    public void TimerScript()
    {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                TimeLeft = 0;
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
    }

    //IMPORTANT NOTE: this resetAttackWaitTime() function will only be implemented when there needs to be a little bit of off time between player attack animations
    private IEnumerator resetAttackWaitTime()
    {
        AnimatorStateInfo stateInfo = animate.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
        testAttackTime = true;
    }
}

