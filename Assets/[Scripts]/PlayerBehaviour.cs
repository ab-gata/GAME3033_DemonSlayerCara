using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    // Movement and action attributes
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float sprintSpeed = 10;
    [SerializeField] private float aimSensitivity = 10;

    // State bools
    private bool isSprinting = false;
    private bool isAiming = false;
    private bool isShooting = false;
    private bool isReloading = false;

    // Components
    private Rigidbody rigidbody;
    private Animator animator;
    [SerializeField] GameObject cameraTarget;

    // Input system & movement references
    private Vector2 moveInput = Vector2.zero;
    private Vector3 moveDirection = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    // Animation hashes
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isRunningHash = Animator.StringToHash("IsSprinting");




    void Start()
    {
        // Get references to components
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        // Camera Rotation
        cameraTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity * Time.deltaTime, Vector3.up);
        cameraTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity * Time.deltaTime, Vector3.left);
        var angles = cameraTarget.transform.localEulerAngles;
        angles.z = 0.0f;
        var angle = cameraTarget.transform.localEulerAngles.x;
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 70)
        {
            angles.x = 70;
        }
        cameraTarget.transform.localEulerAngles = angles;

        //transform.rotation = Quaternion.Euler(0, cameraTarget.transform.rotation.eulerAngles.y, 0);
        //cameraTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        // Player movement
        if (!(moveInput.magnitude > 0)) moveDirection = Vector3.zero;

        moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);
        transform.position += movementDirection;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // Update animation
        animator.SetFloat(movementXHash, moveInput.x);
        animator.SetFloat(movementYHash, moveInput.y);
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;

        // Update animation
        animator.SetBool(isRunningHash, isSprinting);
    }

    public void OnAim(InputValue value)
    {
        isAiming = value.isPressed;
    }
}
