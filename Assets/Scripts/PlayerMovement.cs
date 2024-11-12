using UnityEngine;

// CODE AND TUTORIAL FROM: https://www.youtube.com/watch?v=f473C43s8nE&ab_channel=Dave%2FGameDevelopment

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]

    public float jumpForce;
    public float airMultiplier;
    public float jumpCoolDown;
    bool canJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Detection")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;


    public float speed;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }

    private void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && grounded && canJump){
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // using raycasts to detect if the player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + .2f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded){
            rb.linearDamping = groundDrag;
        } else {
            rb.linearDamping = 0;
        }
    }

    private void FixedUpdate(){
        MovePlayer();
    }

    private void MovePlayer(){
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if (grounded){
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        } else if (!grounded){ 
            rb.AddForce(moveDirection.normalized * speed * airMultiplier * 10f, ForceMode.Force);
        }
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity
        if (flatVel.magnitude > speed){
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump(){

        // reset y velo
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump(){
        canJump = true;
    }
    
}
