using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondMovement : MonoBehaviour
{
    public CharacterController cc;
    public float attackComboTimer = 15f;
    public float Combo = 0;
    public PlayerAnimationChecker checker;

    private bool isCloseRangeAttack = false;
    //private bool isFarRangeAttack = false;

    private int xx = 0;

    public int LocalAttackTriad = 0;

    public bool isAttackingFromFar;
    public bool isAttackingFromClose;

    public Camera cam;

    public Animator animate;
    //public Animation animation;

    public VERYQuickEnemScript enemyScript;

    public LayerMask mask;

    private bool animTrueFalse;

    //THIS IS A TEST VARIABLE
    private bool testAttackTime = true;
    public float TimeLeft;
    public bool TimerOn = true;

    public float maxAttackRange = 7f;

    private float rotationSpeed = 20f;

    public float speed = 12f;

    public Transform came;

    public Transform enemy;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool isAttacking;
    public bool canAttack;

    public bool isAttacked;

    private float speed2test = 25f;

    private RaycastHit hit;

    private float AttackRange = 2.1f;

    public ClosestEnemyDetector detector;

    void Update()
    {
        RunAnimation();
        //TimerScript();
        //AttackComboTracker();
        AttackChecker();
        Attack();
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

        if (!isAttacking)
        {
            animate.SetBool("IsKicking", false);
            animate.SetBool("IsPunching", false);
        }
    }


    private void RunAnimation()
    {
        bool pressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (!isAttacking)
        {
            animate.SetBool("IsRunning", pressed);
        }
    }

    private void AttackChecker()
    {
        if (Input.GetMouseButtonDown(0) && isAttacking == false)
        {
            isAttacking = true;

            if (Combo > 1)
            {
                if (!WithinRange())
                {
                    isAttackingFromFar = true;
                    isAttackingFromClose = false;
                }

                if (WithinRange())
                {
                    isAttackingFromFar = false;
                    isAttackingFromClose = true;
                }
            }
        }

            animate.SetInteger("CloseAttackRange", LocalAttackTriad);
    }

    private bool WithinRange()
    {
        return Vector3.Distance(enemy.position, transform.position) < AttackRange;
    }

    public void CheckForDamage()
    {
        
    }

    private void Attack()
    {
        //enemy = detector.closestEnemy;

        if (isAttacking)
        {
            //if (isCloseRangeAttack)
            //{
            //animTrueFalse = true;
            if (Combo > 1)
            {
                if (isAttackingFromClose)
                {
                    AttackFromClose();
                }

                if (!isAttackingFromClose)
                {
                    if (Vector3.Distance(enemy.position, transform.position) <= (maxAttackRange + (Combo * 2)))
                    {
                        JumpAttack();
                    }
                }
            }

            if (Combo == 0)
            {
                AttackFromClose();
            }
        } 

        if (!isAttacking)
        {
            enemy = detector.closestEnemy;
        }
    }

    private void AttackFromClose()
    {
        animate.SetBool("IsPunching", true);
        //animate.SetBool("IsRunning", false);
        if (xx == 0)
        {
            StartCoroutine(resetAttackWaitTime());
            xx = 1;
        }
    }

    private void JumpAttack()
    {
        //enemy = detector.closestEnemy;

        if (enemy != null)
        {
            if (Vector3.Distance(enemy.position, transform.position) >= 1f)
            {
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
            else
            {
                StartCoroutine(waitALittle());
            }
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
        //AnimatorStateInfo stateInfo = animate.GetCurrentAnimatorStateInfo(0);
        //float animationLength = stateInfo.length;
        //yield return new WaitForSeconds(animationLength);

        yield return new WaitForSeconds(0.6f);
        animate.SetBool("IsPunching", false);
        animate.SetBool("IsKicking", false);

        //yield return new WaitForSeconds(0.83f);
        animTrueFalse = false;
        isAttacking = false;
        testAttackTime = true;
        xx = 0;
    }

    private IEnumerator waitALittle()
    {
        //AnimatorStateInfo stateInfo = animate.GetCurrentAnimatorStateInfo(0);
        //float animationLength = stateInfo.length;
        //yield return new WaitForSeconds(animationLength);

        yield return new WaitForSeconds(0.2f);
        animate.SetBool("IsPunching", false);
        animate.SetBool("IsKicking", false);

        //yield return new WaitForSeconds(0.83f);
        animTrueFalse = false;
        isAttacking = false;
        testAttackTime = true;
        xx = 0;
    }
}
