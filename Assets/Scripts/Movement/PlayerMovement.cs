using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public InputMaster Controls;
    public static bool IsSprinting;
    public static bool IsDashing;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerCombat combat;
    [SerializeField] [Range(0f, 20f)] private float speed = 6f;
    [SerializeField] [Range(0f, 20f)] private float sprintSpeedIncrement = 6f;
    [SerializeField] [Range(0f, 60f)] private float dashSpeed = 6f;
    [SerializeField] [Range(0f, 2f)] private float dashTime = 1f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float fallAceleration = 9.8f;
    [SerializeField] private float fallTerminalVelocity = 50f;
    [SerializeField] private float fallDesacelerationVelocity = 0.8f;
    
    private float gravity;
    private float turnSmoothVelocity;
    private Vector2 normalizedDirection = Vector2.zero;

    private void Awake()
    {
        controller.detectCollisions = true;
    }

    public void OnMovementInput(Vector2 direction)
    {
        normalizedDirection = direction.normalized;
    }

    public void OnSprintButtonDown()
    {
        IsSprinting = true;
    }

    public void OnSprintButtonUp()
    {
        IsSprinting = false;
    }

    public float GetCorrectSpeed()
    {
        if (IsSprinting)
        {
            return speed + sprintSpeedIncrement;
        }

        return speed;
    }

    public void OnDashButtonPressed()
    {
        if (!IsDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;
        IsDashing = true;
        Vector3 dashDirection = new Vector3(normalizedDirection.x, 0, normalizedDirection.y);

        float targetAngle = Mathf.Atan2(normalizedDirection.x, normalizedDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);

        while (Time.time < startTime + dashTime)
        {
            controller.Move( dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
        IsDashing = false;
    }

    private void Gravity()
    {
        if (controller.isGrounded)
        {
            gravity = -0.5f;
        }
        else
        {
            gravity += -fallAceleration;
            gravity = Mathf.Clamp(gravity, -fallTerminalVelocity, 0);
        }
    }

    private void Update()
    {
        Gravity();
        if (!IsDashing && !combat.IsAttacking)
        {
            if (normalizedDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(normalizedDirection.x, normalizedDirection.y) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            float correctSpeed = GetCorrectSpeed();
            Vector3 vel;

            if (controller.isGrounded)
            {
                vel = new Vector3(normalizedDirection.x * correctSpeed, gravity, normalizedDirection.y * correctSpeed);
            }
            else
            {
                vel = new Vector3(normalizedDirection.x * correctSpeed*fallDesacelerationVelocity, gravity, normalizedDirection.y * correctSpeed * fallDesacelerationVelocity);
            }
            
            controller.Move(vel * Time.deltaTime);
        }
    }
}
