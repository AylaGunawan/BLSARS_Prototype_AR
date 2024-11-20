using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    public List<GameObject> nearbyDangers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Danger") || other.gameObject.layer == LayerMask.NameToLayer("Danger_Holdable"))
        {
            nearbyDangers.Add(other.gameObject);
            Debug.Log("Uh oh theres " + nearbyDangers.Count + " dangers near me!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Danger") || other.gameObject.layer == LayerMask.NameToLayer("Danger_Holdable"))
        {
            nearbyDangers.Remove(other.gameObject);
            Debug.Log("Yippee! Now theres only " + nearbyDangers.Count + " dangers near me!");
            if (nearbyDangers.Count == 0)
            {
                Debug.Log("I'm feeling very safe right now. No dangers nearby!");
            }
        }
    }
}
