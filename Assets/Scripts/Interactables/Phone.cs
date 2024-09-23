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

        GameManager.Instance.cprCompressMode = true;

        gameObject.SetActive(false);
    }
}
