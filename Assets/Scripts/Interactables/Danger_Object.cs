
// I basically just copied the code from the phone script

public class Danger_Object : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Interactable>(out Interactable interactable));
        
           
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        //base.Interact();

        //changes the outline colour to red
        this.GetComponent<Interactable>().outline.OutlineColor = UnityEngine.Color.red;
        //TODO: Add the object to a list of identified dangers (Game Manager)
        //gameObject.SetActive(false);

    }
}


