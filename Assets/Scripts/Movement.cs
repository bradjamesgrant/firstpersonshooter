using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float gravityStrength = -25.0f;
    public float Speed;
    public float smoothMovementFactor;
    public float JumpHeight;
    private Vector3 acceleration;
    private Vector3 velocity;
    public float maxAcceleration = 10f;
    public float maxSpeed = 5.0f;

    private GameObject camera;
    private GameObject player;

    private bool isJumping = false;
    private bool isFalling = false;

    private float jumpAmount = 0.0f;

    private CharacterController controller;
    void Start()
    {
        InputManager.instance.Space += Jump;
        InputManager.instance.Movement += UpdateVelocity;
        InputManager.instance.StoppedMoving += StopMoving;
        camera = GameObject.Find("Camera");
        player = GameObject.Find("Player");
        controller = player.GetComponent<CharacterController>();

    }

void FixedUpdate()
    {
        Move();
        RunGravity();
        transform.forward = new Vector3(camera.transform.forward.x,0.0f,camera.transform.forward.z).normalized;
    }

    private void UpdateVelocity(Vector2 input, Vector2 raw)
    {

        input = Vector2.ClampMagnitude(input, 1f);
        float delta = Time.fixedDeltaTime;
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0f;
        Vector3 cameraRight = camera.transform.right;
        cameraRight.y = 0f;
        Vector3 desiredVelocity = ((cameraForward * input.y) + (cameraRight * input.x)).normalized * maxSpeed * delta;
        float maxSpeedChange = maxAcceleration * delta;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        

        

    }

    private void Move()
    {
        controller.Move(velocity);
    }

    private void StopMoving()
    {
        acceleration = Vector3.zero;
        velocity *= 0.8f;
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            isJumping = true;
            jumpAmount = -JumpHeight;
        }
    }

    private void RunGravity()
    {
            if (!isJumping)
            {
                if (!controller.isGrounded && !isFalling)
                {
                    jumpAmount = 0;
                    isFalling = true;
                }
            }
            if (isFalling && controller.isGrounded)
            {
                isFalling = false;
            }
            jumpAmount += gravityStrength * Time.deltaTime;
            controller.Move(new Vector3(0.0f,jumpAmount * Time.deltaTime,0.0f));

            if (isJumping == true && controller.isGrounded == true)
            {
                isJumping = false;
            }
        }
    
}
