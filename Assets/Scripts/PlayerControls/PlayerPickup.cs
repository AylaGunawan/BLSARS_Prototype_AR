using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerPickup : MonoBehaviour
{
    
    Player player;
    PlayerInput input;
    InputAction pickUpAction;

    public Transform holdPoint;
    [SerializeField] GameObject currentObjectHeld;


    [SerializeField] RaycastHit objectHit;
    private Vector3 worldRotation;


    //private Quaternion localObjectRot;
    private Vector3 vectorToContact;

    public float pickUpRange;
    public float pickUpForce;
    bool pickedUp; //if it isnt marked with a key word, assume private

    LayerMask holdMasks;
    RaycastHit hit;

    float pickUpTimer;
    public float pickUpCooldown;


    

    /// <summary>
    /// Initializes variables
    /// </summary>
    void Awake()
    {
        player = GetComponent<Player>();
        input = GetComponent<PlayerInput>();
        pickUpAction = input.actions["hold"];
    }


    // Start is called before the first frame update
    void Start()
    {
        pickedUp = false;
        holdMasks = LayerMask.GetMask("Holdable", "Danger_Holdable", "Victim");
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {
            //system for picking up, held by the center of the object

            //variables
            Vector3 directionToHoldPoint = (holdPoint.transform.position - currentObjectHeld.transform.position);
            var acceleration = directionToHoldPoint * pickUpForce;
            var objectVelocityDirection = objectHit.rigidbody.velocity.normalized; //e.g (0.5, 0, 1)
            var targetDirection = directionToHoldPoint.normalized; // e.g (0.4, 1, -1)

            //if the object velocity direction is SIMILAR to the target direction, slow down the speed
            if (objectVelocityDirection.x > 0 && targetDirection.x > 0 || objectVelocityDirection.x < 0 && targetDirection.x < 0 ||
                objectVelocityDirection.y > 0 && targetDirection.y > 0 || objectVelocityDirection.y < 0 && targetDirection.y < 0 ||
                objectVelocityDirection.z > 0 && targetDirection.z > 0 || objectVelocityDirection.z < 0 && targetDirection.z < 0)

                //Okay so currently, With this system, the object feels very smooth to move around and you can throw it,
                //but If you strafe or move without moving the mouse, the object will be very jittery. This will work for a Demo,
                //but will need to be adjusted for a 3D half build
                objectHit.rigidbody.velocity *= 0.98f;

            if (directionToHoldPoint.magnitude != 0)
                objectHit.rigidbody.velocity += acceleration;
        }
        if (pickUpTimer > 0)
            pickUpTimer = pickUpTimer - Time.deltaTime;
    }

    //this is code from cece's code. I'm assuming what it does is removes it
    //from a list if its disabled and adds it to the beforemove list if its enabled

    void OnEnable() => player.OnBeforeMove += OnBeforeMove;
    void OnDisable() => player.OnBeforeMove -= OnBeforeMove;


    private void OnBeforeMove()
    {
        if (pickUpTimer <= 0)
        {
            if (pickUpAction.IsPressed()) //if the player is pressing the button to hold:
            {
                // set up raycast
                Ray ray = new Ray(player.cam.transform.position, player.cam.transform.forward);
                if (Physics.Raycast(ray, out hit, pickUpRange, holdMasks)) //and we hit something in range in the correct layer
                {
                    
                    if (pickedUp) //if we already have object picked up
                    {
                        Debug.Log("ruh");
                        DropObject();
                    }
                        
                    else //if we dont currently have an object picked up
                    {
                        Debug.Log("roh");
                        HoldObject();
                    }
                    pickUpTimer = pickUpCooldown;
                }
                else
                {
                    Debug.Log("raggy");
                    DropObject();
                }
            }
        } 
    }

    void HoldObject()
    {

        objectHit = hit;
        
        objectHit.collider.gameObject.transform.parent = holdPoint;

        currentObjectHeld = objectHit.collider.gameObject; //make the hit object, the held object
        pickedUp = true;
    }

    void DropObject()
    {
        currentObjectHeld.transform.parent = null;
        currentObjectHeld = null; //stop picking it up
        pickedUp = false;
    }

}


