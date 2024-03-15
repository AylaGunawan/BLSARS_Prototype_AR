using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    // https://www.youtube.com/watch?v=HHzQMYxtmU4&list=PLdPQ93duD7PCkWpTo4ElrWlyx6S9bF18V&index=4

    [SerializeField] Camera playerCamera;

    [SerializeField] float lookSpeed = 3f; // mouse sensitivity
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float fallSpeed = 1f; // mass

    CharacterController playerController;
    Vector2 look;
    Vector3 velocity;
     
    void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateGravity();
        UpdateMove();
        UpdateLook();
    }

    void UpdateGravity()
    {
        // get gravity input y
        var gravity = Physics.gravity * fallSpeed * Time.deltaTime;

        // change velocity y by gravity if on the ground
        velocity.y = playerController.isGrounded ? -1f : velocity.y + gravity.y;
    }

    private void UpdateMove()
    {
        // get keyboard input xy
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        // make a vector to apply as movement
        Vector3 moveInput = new Vector3();
        moveInput += transform.forward * y;
        moveInput += transform.right * x;
        moveInput = Vector3.ClampMagnitude(moveInput, 1f); // limit diag speed

        // apply movement and velocity to controller xy
        playerController.Move((moveInput * moveSpeed + velocity) * Time.deltaTime);
    }

    private void UpdateLook()
    {
        // add mouse input xy to look xy
        look.x += Input.GetAxis("Mouse X") * lookSpeed;
        look.y += Input.GetAxis("Mouse Y") * lookSpeed;

        // limit y rotation
        look.y = Mathf.Clamp(look.y, -89f, 89f);

        // set x rotation
        transform.localRotation = Quaternion.Euler(0, look.x, 0);

        // set y rotation (cam only)
        playerCamera.transform.localRotation = Quaternion.Euler(-look.y, 0, 0);

    }
}
