public class Danger_Object : Interactable
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
        //GameManager.Instance.gameMode = GameMode.CPR;

        gameObject.SetActive(false);
    }
}
