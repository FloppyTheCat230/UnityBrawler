using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TC_PlayerController : MonoBehaviour
{
    public float Speed = 5.0f;

    private Animator animator;
    private Vector3 movement;
    private CharacterController characterController;
    private Camera cam;
    private TC_Player player;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        player = GetComponent<TC_Player>();
        cam = Camera.main;
    }

    void Update()
    {
        if (!player.CanMove)
            return;

        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction relative to camera's orientation
        var forward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        var right = cam.transform.right;
        movement = (forward * moveVertical + right * moveHorizontal).normalized;

        characterController.Move(Speed * Time.deltaTime * movement);

        // Check if the character is moving
        bool isRunning = characterController.velocity.magnitude > 0.1f;
        animator.SetBool("IsRunning", isRunning);

        // Rotate the character to face the direction of movement
        if (movement != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Speed * Time.deltaTime);
        }
    }
}
