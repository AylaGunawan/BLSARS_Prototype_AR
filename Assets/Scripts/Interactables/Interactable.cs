using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    public Outline outline;
    public string interactMessage;

    void Awake()
    {
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
