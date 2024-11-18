using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Player))]
public class PlayerInteract : MonoBehaviour
{
    // https://www.youtube.com/watch?v=_yf5vzZ2sYE

    [SerializeField] TMP_Text interactText;
    [SerializeField] Image interactBar;
    [SerializeField] float interactDistance = 5f;
    [SerializeField] float interactTime = .5f;

    Player player;
    PlayerInput playerInput;
    InputAction interactAction;

    //masks
    LayerMask interactMasks;
    LayerMask dangerMasks;
    Interactable lastInteractable = null;

    float interactTimer = 0f;
    bool isInteracting = false;

    void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["interact"];
    }

    void Start()
    {
        interactMasks = LayerMask.GetMask("Danger", "Danger_Holdable", "Response", "Send_Help", "Airway", "Breathing", "CPR", "Defibrillation");
        dangerMasks = LayerMask.GetMask("Danger", "Danger_Holdable");
    }

    void Update()
    {
        interactText.text = string.Empty;

        isInteracting = interactAction.ReadValue<float>() > 0;


        // set up raycast
        Ray ray = new Ray(player.cam.transform.position, player.cam.transform.forward);
        RaycastHit hit;

        // handle raycast
        if (Physics.Raycast(ray, out hit, interactDistance, interactMasks))
        {
            if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
            {
                interactText.text = interactable.interactMessage;

                // handle outline
                interactable.outline.OutlineWidth = 5f;
                lastInteractable = interactable;
                // handle interact bar
                if (isInteracting)
                    interactTimer += Time.deltaTime;
                else
                    interactTimer = 0;


                interactBar.fillAmount = interactTimer / interactTime;

                //___________________________________________________________DANGER___________________________________________________________?

                if (interactTimer >= interactTime) // when interact is done
                {
                    interactable.BaseInteract();
                    isInteracting = false;
                }

            }
        }
        else
        {
            isInteracting = false;
            interactBar.fillAmount = 0;

            // handle outline removal
            if (lastInteractable != null)
            {
                lastInteractable.outline.OutlineWidth = 0f;
                lastInteractable = null;
            }
        }

        
    }


    private void OnBeforeMove()
    {
        //// check if player is trying to interact
        //bool 


        //// set up raycast to make sure we can only interact with interactables
        //Ray ray = new Ray(player.cam.transform.position, player.cam.transform.forward);
        //RaycastHit hit;
        //// handle raycast

        ////Because I've moved the interact code here, the phone will temporarily not work

        ////___________________________________________________________DANGER___________________________________________________________

        ////if we hit something considered a danger this will be true.
        //bool interactableHit = Physics.Raycast(ray, out hit, interactDistance, dangerMasks) ? true : false;
        //if (interactableHit)
        //{
        //    //if the object has the interactable component
        //    if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
        //    {
                
        //    }
        //}
        //else
        //{
        //    interactBar.fillAmount = 0;
        //}




    }
}
