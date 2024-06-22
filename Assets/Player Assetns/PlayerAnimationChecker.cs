using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationChecker : MonoBehaviour
{
    public bool isPlayerAttackAnimationFinished;
    public bool hasPlayerAnimationAttackedYet;

    public SecondMovement player;

    private void Start()
    {
        hasPlayerAnimationAttackedYet = false;
    }

    public void Started()
    {
        isPlayerAttackAnimationFinished = false;
    }
    public void Finished()
    {
        isPlayerAttackAnimationFinished = true;
    }
    public void HasAttacked()
    {
        player.CheckForDamage();
    }
}
