using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    Player player;
    PlayerInput input;
    InputAction pickUpAction;

    public GameObject holdPoint;
    [SerializeField] GameObject currentObjectHeld;

    public float pickUpRange;
    public float pickUpForce;
    bool pickedUp; //if it isnt marked with a key word, assume private

    LayerMask holdMasks;
    RaycastHit hit;

    float pickUpTimer;
    public float pickUpCooldown;

    //:O i wake
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
        holdMasks = LayerMask.GetMask("Holdable");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pickUpTimer);
        if (pickedUp)
        {
            //
            Vector3 positionVector = (holdPoint.transform.position - currentObjectHeld.transform.position);

            //making variables
            var acceleration = positionVector * pickUpForce;
            var objectMovementDirection = hit.rigidbody.velocity.normalized; //e.g (0.5, 0, 1)
            var targetDirection = positionVector.normalized; // e.g (0.4, 1, -1)


            //if the object velocity direction is SIMILAR to the target direction, slow down the speed
            if(objectMovementDirection.x > 0 && targetDirection.x > 0 || objectMovementDirection.x < 0 && targetDirection.x < 0 || 
                objectMovementDirection.y > 0 && targetDirection.y > 0 || objectMovementDirection.y < 0 && targetDirection.y < 0 ||
                objectMovementDirection.z > 0 && targetDirection.z > 0 || objectMovementDirection.z < 0 && targetDirection.z < 0)
            {
                //Okay so currently, With this system, the object feels very smooth to move around and you can throw it,
                //but If you strafe or move without moving the mouse, the object will be very jittery. This will work for a Demo,
                //but will need to be adjusted for a __3D half build__
                hit.rigidbody.velocity *= 0.99f;                
            }

            if (positionVector.magnitude != 0)
            {
                hit.rigidbody.velocity = hit.rigidbody.velocity + acceleration;
            }
            //currentObjectHeld.transform.position = holdPoint.transform.position;
        }
        if (pickUpTimer > 0)
        {
            pickUpTimer = pickUpTimer - Time.deltaTime;
        }



    }

    //this is code from cece's code. I'm assuming what it does is removes it
    //from a list if its disabled and adds it to the beforemove list if its enabled

    void OnEnable() => player.OnBeforeMove += OnBeforeMove;
    void OnDisable() => player.OnBeforeMove -= OnBeforeMove;


    private void OnBeforeMove()
    {
        // set up raycast
        Ray ray = new Ray(player.cam.transform.position, player.cam.transform.forward);
      
        if (pickUpTimer <= 0)
        {
            if (pickUpAction.IsPressed()) //if the player is pressing the button to hold:
            {
                if (pickedUp) //if we already have object picked up
                {

                    hit.rigidbody.useGravity = true;
                    currentObjectHeld = null; //stop picking it up
                    pickedUp = false;
                }
                if (Physics.Raycast(ray, out hit, pickUpRange, holdMasks)) //and we hit something in range in the correct layer
                {
                    if (!pickedUp) //if we dont currently have an object picked up
                    {
                        currentObjectHeld = hit.collider.gameObject; //make the hit object, the held object
                        hit.rigidbody.useGravity = false; //stops gravity for the picked up object
                        pickedUp = true;
                    }
                    pickUpTimer = pickUpCooldown;
                }
            }
        } 
    }
}
