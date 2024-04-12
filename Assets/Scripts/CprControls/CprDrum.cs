using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CprDrum : MonoBehaviour
{
    [SerializeField] GameObject beatPrefab;
    [SerializeField] float beatFrequency = 1;

    [SerializeField] Color activeColor = Color.red;
    [SerializeField] Color inactiveColor = Color.white;

    PlayerInput playerInput;
    InputAction compressAction;

    Image sprite;


    public float beatTimer = 0f;
    bool isCompressing = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        compressAction = playerInput.actions["compress"];

        sprite = GetComponent<Image>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        isCompressing = compressAction.ReadValue<float>() > 0;

        if (isCompressing)
        {
            SetActive();
        }
        else
        {
            SetInactive();
        }

        beatTimer += Time.deltaTime;

        if (beatTimer >= beatFrequency)
        {
            GameObject beatObject = Instantiate(beatPrefab, new Vector3(transform.position.x + 500f, transform.position.y, transform.position.z), Quaternion.identity);
            beatObject.transform.parent = transform.parent;
            beatTimer = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "CprBeat")
        {
            if (isCompressing)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void SetActive()
    {
        sprite.color = activeColor;
    }

    public void SetInactive()
    {
        sprite.color = inactiveColor;
    }

}
