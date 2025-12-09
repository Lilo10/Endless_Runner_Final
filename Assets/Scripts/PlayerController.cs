using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneDistance = 2f;  // -10, 0, +10
    public float laneChangeSpeed = 12f;

    private int currentLane = 0; 
    private Vector3 targetPosition;

    [Header("Jump Settings")]
    public float jumpForce = 8f;
    public float gravity = -20f;
    private float verticalVelocity = 0f;

    private CharacterController controller;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetPosition = transform.position;
    }



    void Update()
    {
        // -------------------------------------
        // 1. READ INPUT FOR LANE MOVEMENT
        // -------------------------------------
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }


        // -------------------------------------
        // 2. LANE SWITCHING MOVEMENT
        // -------------------------------------
        Vector3 horizontalTarget = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, horizontalTarget, laneChangeSpeed * Time.deltaTime);



        // -------------------------------------
        // 3. JUMP LOGIC (CharacterController)
        // -------------------------------------
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                verticalVelocity = jumpForce;
            }
        }

        verticalVelocity += gravity * Time.deltaTime;


        // -------------------------------------
        // 4. FINAL MOVEMENT VECTOR
        // -------------------------------------
        Vector3 move = Vector3.zero;

        move += transform.forward * forwardSpeed;   // Forward
        move.y = verticalVelocity;                  // Vertical (jump + gravity)

        controller.Move(move * Time.deltaTime);
    }



    // -----------------------------------------
    // CHANGE LANE FUNCTION
    // -----------------------------------------
    void ChangeLane(int direction)
    {
        currentLane += direction;
        currentLane = Mathf.Clamp(currentLane, -1, 1);

        float targetX = currentLane * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
