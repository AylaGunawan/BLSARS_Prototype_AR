public class Phone : Interactable
{
    void Start()
    {
        //
    }

    void Update()
    {
        //
    }

    protected override void Interact()
    {
        base.Interact();

        //stageManagerScript.interactionObjects.Add(gameObject);

        gameManager.cprMode = true;

        gameObject.SetActive(false);
    }
}
