using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Outline outline;
    public string interactMessage;

    protected GameManager gameManager;
    protected StageManager stageManager;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();

        outline = GetComponent<Outline>();
    }

    void Start()
    {
        outline.OutlineWidth = 0f;
    }

    void Update()
    {
        
    }

    public void BaseInteract() // called by PlayerInteract
    {
        Interact();
    }

    protected virtual void Interact() // overriden by child class
    {

    }
}
