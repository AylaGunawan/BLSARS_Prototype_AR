using UnityEngine;
using UnityEngine.UI;

public class CprBeat : MonoBehaviour
{
    [SerializeField] float beatSpeed = 240f; // tempo

    Image sprite;

    void Awake()
    {
        sprite = GetComponent<Image>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.position -= new Vector3(beatSpeed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CprDrum")
        {
            CprManager.Instance.beatsPassed += 1;
            CprManager.Instance.beatCounter += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CprDrum")
        {
            if (!CprManager.Instance.isCompressing)
            {
                CprManager.Instance.BeatMissed();

                sprite.color = Color.red;
                Destroy(this.gameObject);
            }   
        }
    }
}
