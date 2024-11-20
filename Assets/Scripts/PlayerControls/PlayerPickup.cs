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
        holdMasks = LayerMask.GetMask("Holdable", "Danger_Holdable");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pickUpTimer);
        if (pickedUp)
        {
            Vector3 direction = (holdPoint.transform.position - currentObjectHeld.transform.position);

            //variables
            var acceleration = direction * pickUpForce;
            var objectVelocityDirection = hit.rigidbody.velocity.normalized; //e.g (0.5, 0, 1)
            var targetDirection = direction.normalized; // e.g (0.4, 1, -1)

            //if the object velocity direction is SIMILAR to the target direction, slow down the speed
            if (objectVelocityDirection.x > 0 && targetDirection.x > 0 || objectVelocityDirection.x < 0 && targetDirection.x < 0 ||
                objectVelocityDirection.y > 0 && targetDirection.y > 0 || objectVelocityDirection.y < 0 && targetDirection.y < 0 ||
                objectVelocityDirection.z > 0 && targetDirection.z > 0 || objectVelocityDirection.z < 0 && targetDirection.z < 0)
            {

                //tried reverse
                //if (!(objectVelocityDirection.x > 0 && targetDirection.x > 0) || !(objectVelocityDirection.x < 0 && targetDirection.x < 0) ||
                //    !(objectVelocityDirection.y > 0 && targetDirection.y > 0) || !(objectVelocityDirection.y < 0 && targetDirection.y < 0) ||
                //    !(objectVelocityDirection.z > 0 && targetDirection.z > 0) || !(objectVelocityDirection.z < 0 && targetDirection.z < 0))
                //{
                //      acceleration *= 2;



                //Okay so currently, With this system, the object feels very smooth to move around and you can throw it,
                //but If you strafe or move without moving the mouse, the object will be very jittery. This will work for a Demo,
                //but will need to be adjusted for a 3D half build
                hit.rigidbody.velocity *= 0.98f;



                //tried to use derrivative here
                //acceleration = new Vector3((0.98f / 2) * hit.rigidbody.velocity.x * hit.rigidbody.velocity.x,
                //    (0.98f / 2) * hit.rigidbody.velocity.y * hit.rigidbody.velocity.y,
                //    (0.98f / 2) * hit.rigidbody.velocity.z * hit.rigidbody.velocity.z);

                //float decel = 0.2f;

                //if (Mathf.Abs(hit.rigidbody.velocity.x) > 0 )
                //{
                //    acceleration.x -= decel;

                //}
                //if (Mathf.Abs(hit.rigidbody.velocity.y) > 0)
                //{
                //    acceleration.y -= decel;

                //}
                //if (Mathf.Abs(hit.rigidbody.velocity.z) > 0)
                //{
                //    acceleration.z -=  decel;

                //}
                //acceleration = direction * 0.01f;
                //if (direction.magnitude < 1f)
                //{
                //    acceleration *= -0.01f;
                //}



                //hit.rigidbody.velocity = new Vector3(Mathf.Lerp(hit.rigidbody.velocity.x, 0, Time.deltaTime * 5), Mathf.Lerp(hit.rigidbody.velocity.y, 0, Time.deltaTime * 5), Mathf.Lerp(hit.rigidbody.velocity.z, 0, Time.deltaTime * 5));

            }

            //slow, scaling speed that doesnt feel magnetic, more like sliding
            //hit.rigidbody.velocity = direction * 0.2f;

            if (direction.magnitude != 0)
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
                if (Physics.Raycast(ray, out hit, pickUpRange, holdMasks)) //and we hit something in range in the correct layer
                {
                    if (pickedUp) //if we already have object picked up
                    {
                        currentObjectHeld = null; //stop picking it up
                        pickedUp = false;
                    }
                    else //if we dont currently have an object picked up
                    {
                        currentObjectHeld = hit.collider.gameObject; //make the hit object, the held object
                        pickedUp = true;
                    }
                    pickUpTimer = pickUpCooldown;
                }
                else
                {
                    //im annoyed that I have to do this twice
                    currentObjectHeld = null; //stop picking it up
                    pickedUp = false;
                }
                
                
            }
        } 
    }
}
