using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    // https://www.youtube.com/watch?v=HHzQMYxtmU4&list=PLdPQ93duD7PCkWpTo4ElrWlyx6S9bF18V&index=4

    public Camera cam;

    public float Height
    {
        get => controller.height;
        set => controller.height = value;
    }

    public event Action OnBeforeMove;

    [SerializeField] float lookSpeed = 3f; // mouse sensitivity
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float mass = 1f; // "fallSpeed"
    [SerializeField] float acceleration = 20f;

    //GameManager gameManager;
    //StageManager stageManager;

    CharacterController controller;
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;
    Vector2 look;
    Vector3 velocity;

    PlayerInteract playerInteract;

    internal float moveSpeedMultiplier;
     
    void Awake()
    {
        //gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["move"];
        lookAction = playerInput.actions["look"];

        playerInteract = GetComponent<PlayerInteract>();
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
        UpdateMode();
    }

    void UpdateMode()
    {
        if (GameManager.Instance.cprCompressMode)
        {

        }
        else
        {

        }
    }

    void UpdateGravity()
    {
        // get gravity input y
        var gravity = Physics.gravity * mass * Time.deltaTime;

        // change velocity y by gravity if on the ground
        velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;
    }

    private Vector3 GetMoveVector()
    {
        // get move input xy from input actions
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // make a vector to represent movement
        Vector3 moveVector = new Vector3();
        moveVector += transform.forward * moveInput.y;
        moveVector += transform.right * moveInput.x;
        moveVector = Vector3.ClampMagnitude(moveVector, 1f); // limit diagonal speed
        moveVector *= moveSpeed * moveSpeedMultiplier;
        return moveVector;
    }

    private void UpdateMove()
    {
        moveSpeedMultiplier = 1f;
        OnBeforeMove?.Invoke(); // check if any move events from other scripts happen first

        Vector3 moveVector = GetMoveVector();

        // smooth out velocity to 0 based on th vector
        float factor = acceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, moveVector.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, moveVector.z, factor);

        // apply velocity to controller xy
        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateLook()
    {
        // get mouse input xy from input actions
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        // add mouse input xy to look xy
        look.x += lookInput.x * lookSpeed;
        look.y += lookInput.y * lookSpeed;

        // limit y rotation
        look.y = Mathf.Clamp(look.y, -89f, 89f);

        // set x rotation
        transform.localRotation = Quaternion.Euler(0, look.x, 0);

        // set y rotation (cam only)
        cam.transform.localRotation = Quaternion.Euler(-look.y, 0, 0);

    }
}
