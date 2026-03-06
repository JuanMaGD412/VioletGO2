using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.8f;
    public float rotationSpeed = 10f;

    public Joystick joystick;
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    float idleTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        IdleRandom();
    }

    void Move()
    {
        float horizontal;
        float vertical;

        if (joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float speed = moveSpeed;
            bool running = false;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed *= sprintMultiplier;
                running = true;
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            Vector3 moveDir = rotation * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            // 🎬 Animaciones
            animator.SetBool("walking", !running);
            animator.SetBool("running", running);
            animator.SetBool("idle", false);

            idleTimer = 0f;
        }
        else
        {
            animator.SetBool("walking", false);
            animator.SetBool("running", false);
            animator.SetBool("idle", true);

            idleTimer += Time.deltaTime;
        }
    }
    void IdleRandom()
    {
        if (!animator.GetBool("idle")) return;

        if (idleTimer >= 4f)
        {
            float random = Random.value;

            if (random < 0.4f)
            {
                animator.SetTrigger("idle2");
            }

            idleTimer = 0f;
        }
    }
}