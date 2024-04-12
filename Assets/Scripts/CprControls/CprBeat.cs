using UnityEngine;

public class CprBeat : MonoBehaviour
{
    [SerializeField] float beatSpeed = 240; // tempo

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.position -= new Vector3(beatSpeed * Time.deltaTime, 0f, 0f);
    }

}
