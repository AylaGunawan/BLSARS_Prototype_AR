using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerCrouch : MonoBehaviour
{
    // https://www.youtube.com/watch?v=tECethq4JQ0&list=PLdPQ93duD7PCkWpTo4ElrWlyx6S9bF18V&index=4
    // https://www.youtube.com/watch?v=b7qdx9UtMt4&list=PLdPQ93duD7PCkWpTo4ElrWlyx6S9bF18V&index=6

    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float crouchSpeed = 10f;
    [SerializeField] float crouchMoveSpeedMultiplier = .5f;

    Player player;
    PlayerInput playerInput;
    InputAction crouchAction;
    Vector3 standCamHeight; // initial offset of camera when standing

    float standHeight;
    float currentHeight;

    bool IsCrouching => standHeight - currentHeight > .1f;

    void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        crouchAction = playerInput.actions["crouch"];
    }

    void Start()
    {
        standCamHeight = player.cam.transform.localPosition;
        standHeight = currentHeight = player.Height;
    }

    void Update()
    {

    }

    void OnEnable() => player.OnBeforeMove += OnBeforeMove;
    void OnDisable() => player.OnBeforeMove -= OnBeforeMove;

    private void OnBeforeMove()
    {
        // check if player is about to crouch
        bool isTryingToCrouch = crouchAction.ReadValue<float>() > 0;
        float targetHeight = isTryingToCrouch ? crouchHeight : standHeight; // toggle for which height to use

        // smooth out crouch movement
        float crouchDelta = crouchSpeed * Time.deltaTime;
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, crouchDelta);

        // get camera height based on player height when crouching
        Vector3 halfHeightDiff = new Vector3(0, (standHeight - currentHeight) / 2, 0);
        Vector3 crouchCamHeight = standCamHeight - halfHeightDiff;

        // set player and camera heights
        player.cam.transform.localPosition = crouchCamHeight;
        player.Height = targetHeight;

        if (IsCrouching)
        {
            player.moveSpeedMultiplier *= crouchMoveSpeedMultiplier;
        }
    }
}
