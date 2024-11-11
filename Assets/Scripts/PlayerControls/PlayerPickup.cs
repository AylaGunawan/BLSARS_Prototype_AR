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
        Debug.Log(pickUpTimer);
        if (pickedUp)
        {
            Vector3 direction = (holdPoint.transform.position - currentObjectHeld.transform.position);


            //var factor = 20f * Time.deltaTime;
            //hit.rigidbody.velocity = new Vector3(Mathf.Lerp(hit.rigidbody.velocity.x, (hit.rigidbody.velocity + direction * pickUpForce).x, factor), Mathf.Lerp(hit.rigidbody.velocity.y, (hit.rigidbody.velocity + direction * pickUpForce).y, factor), Mathf.Lerp(hit.rigidbody.velocity.z, (hit.rigidbody.velocity + direction * pickUpForce).z, factor));
            //Mathf.Lerp()

            //var uhh = direction.magnitude / 10;
            //;

            
            //TODO: I need to find a better algortithm to get picked up objects to slow down as they get to the center of the pick up point
            if (direction.magnitude != 0)
            {
                hit.rigidbody.velocity = hit.rigidbody.velocity + (direction * pickUpForce * Mathf.Sqrt(direction.magnitude));
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

                        hit.rigidbody.useGravity = true;
                        currentObjectHeld = null; //stop picking it up
                        pickedUp = false;

                    }
                    else //if we dont currently have an object picked up
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
