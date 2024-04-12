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
    LayerMask interactMasks;
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
        interactMasks = LayerMask.GetMask("Danger", "Response", "Send_Help", "Airway", "Breathing", "CPR", "Defibrillation");
    }

    void Update()
    {
        interactText.text = string.Empty;

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

                // handle interact
                isInteracting = interactAction.ReadValue<float>() > 0;

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

            // handle outline removal
            if (lastInteractable != null)
            {
                lastInteractable.outline.OutlineWidth = 0f;
                lastInteractable = null;
            }
        }

        // handle interact bar
        if (isInteracting)
        {
            interactTimer += Time.deltaTime;
        }
        else
        {
            interactTimer = 0;
        }
        interactBar.fillAmount = interactTimer / interactTime;
    }
}
