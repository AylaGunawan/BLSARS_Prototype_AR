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
    public Transform artificialCenterPoint;
    public Transform objectFacing;
    [SerializeField] GameObject currentObjectHeld;

    [SerializeField] RaycastHit objectHit;



    [SerializeField] Vector3 localObjectPos;
    [SerializeField] Vector3 localObjectRot;

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
        holdMasks = LayerMask.GetMask("Holdable", "Danger_Holdable");
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {

            
            //now that object can be picked up by a contact point and get a new artificial center point, now we need it to swing like a pendulum

            artificialCenterPoint.position = holdPoint.position;

            //currentObjectHeld.transform.localPosition = localObjectPos;



            //this will need to be adjusted, but it will look at its center point
            

            var initialLocalObjectDirection = localObjectPos.normalized;
            var currentLocalObjectDirection = currentObjectHeld.transform.localPosition.normalized;

            var initialLocalObjectDistance = localObjectPos.magnitude;

            var initialLocalObjectRot = localObjectRot;

            //Debug.Log(Mathf.Abs(currentObjectHeld.transform.position.x));
            

            var angleFromForwardToContact = new Vector3(Mathf.DeltaAngle(currentObjectHeld.transform.forward.x, -initialLocalObjectDirection.x),
            Mathf.DeltaAngle(currentObjectHeld.transform.forward.y, -initialLocalObjectDirection.y),
            Mathf.DeltaAngle(currentObjectHeld.transform.forward.z, -initialLocalObjectDirection.z));

            //currentObjectHeld.transform.LookAt(artificialCenterPoint, currentObjectHeld.transform.up);

            var diff = (artificialCenterPoint.position - currentObjectHeld.transform.position).normalized - angleFromForwardToContact.normalized;
            //+

            currentObjectHeld.transform.forward = (artificialCenterPoint.position - currentObjectHeld.transform.position).normalized - diff;
            Debug.Log("a" + currentObjectHeld.transform.forward);
            //currentObjectHeld.transform.forward += ;

            //if (Mathf.Abs(currentObjectHeld.transform.position.x) > Mathf.Abs(artificialCenterPoint.position.x) + 0.01f ||
            //    Mathf.Abs(currentObjectHeld.transform.position.z) > Mathf.Abs(artificialCenterPoint.position.z) + 0.01f)
            //{
            //    objectHit.rigidbody.constraints = RigidbodyConstraints.None;
            //    //Debug.Log(currentObjectHeld.transform.rotation);
            //}
            //else
            //{
            //    objectHit.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
            //    objectHit.rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            //    //currentObjectHeld.transform.rotation.SetLookRotation(Vector3.up);//0, currentObjectHeld.transform.rotation.eulerAngles.y, 0);
            //}


            //if the object is directly under the the center point, then stop trying to look at it, because it causes it to glitch out, instead look straight up and keep y rotational



            //currentObjectHeld.transform.eulerAngles += localObjectPos;
            //this is the rotation of "forward" this should face towards the centerpoint
            //var localObjectRotation = localObjectRot;

            //currentObjectHeld.transform.rotation *= localObjectRotation;

            //if the object is not a localObjectDistance distance away from the artificialCenterPoint
            //then we want to set the local position to the localObjectDistance * localObjectDirection

            if (currentObjectHeld.transform.localPosition.magnitude != initialLocalObjectDistance)
            {
                currentObjectHeld.transform.localPosition = currentObjectHeld.transform.localPosition.normalized * initialLocalObjectDistance;
            }

            if (objectHit.rigidbody.velocity.magnitude > 3)
            {
                objectHit.rigidbody.velocity = objectHit.rigidbody.velocity.normalized * 3f;
            }


            //the part im currently confused about is how to mkae this system for physicsy. as of right now, the box will be picked up at a location (pog)
            //and it will swing down till it reaches the bottom of the pendulum arc(kinda pog) whils maintaining a rotaion that changes with the direction to the center(pog).
            //but moving the camera up, down, left, or right will not change the pendulum swing of the object, it will remain the down position because of gravity

            //for the time being, that feature is not needed, i will work on it later because it is very confusing imo




            //old system for picking up, held by the center of the object

            //Vector3 direction = (holdPoint.transform.position - currentObjectHeld.transform.position);

            ////variables
            //var acceleration = direction * pickUpForce;
            //var objectVelocityDirection = hit.rigidbody.velocity.normalized; //e.g (0.5, 0, 1)
            //var targetDirection = direction.normalized; // e.g (0.4, 1, -1)

            ////if the object velocity direction is SIMILAR to the target direction, slow down the speed
            //if (objectVelocityDirection.x > 0 && targetDirection.x > 0 || objectVelocityDirection.x < 0 && targetDirection.x < 0 ||
            //    objectVelocityDirection.y > 0 && targetDirection.y > 0 || objectVelocityDirection.y < 0 && targetDirection.y < 0 ||
            //    objectVelocityDirection.z > 0 && targetDirection.z > 0 || objectVelocityDirection.z < 0 && targetDirection.z < 0)

            //    //Okay so currently, With this system, the object feels very smooth to move around and you can throw it,
            //    //but If you strafe or move without moving the mouse, the object will be very jittery. This will work for a Demo,
            //    //but will need to be adjusted for a 3D half build
            //    hit.rigidbody.velocity *= 0.98f;

            //if (direction.magnitude != 0)
            //    hit.rigidbody.velocity = hit.rigidbody.velocity + acceleration;
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
                //Debug.Log("huh");
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
        artificialCenterPoint.position = objectHit.point;
        //objectHit.collider.gameObject.transform.parent = player.transform;
        objectHit.collider.gameObject.transform.parent = artificialCenterPoint;
        artificialCenterPoint.position = holdPoint.position;


        //hit.rigidbody.useGravity = false;
        

        currentObjectHeld = objectHit.collider.gameObject; //make the hit object, the held object
        localObjectPos = currentObjectHeld.transform.localPosition;

        



        localObjectRot = currentObjectHeld.transform.eulerAngles;
        vectorToContact = (artificialCenterPoint.transform.position - currentObjectHeld.transform.position);

        pickedUp = true;
    }

    void DropObject()
    {
        //hit.rigidbody.useGravity = true;
        currentObjectHeld.transform.parent = null;
        currentObjectHeld = null; //stop picking it up
        pickedUp = false;
    }

}


