using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSprinting;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Debug.Log($"Input: H={Input.GetAxis("Horizontal")}, V={Input.GetAxis("Vertical")}");
        HandleMovement();
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Прыжок только когда на земле
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                Debug.Log($"Jump! Velocity: {velocity.y}");
            }
        }
        else
        {
            Debug.Log($"No");
            velocity.y += gravity * Time.deltaTime;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, 0, z).normalized;

        move = transform.TransformDirection(move);

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }
}
