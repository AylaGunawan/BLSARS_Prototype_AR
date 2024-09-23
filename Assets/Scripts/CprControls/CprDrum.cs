using UnityEngine;
using UnityEngine.UI;

public class CprDrum : MonoBehaviour
{
    [SerializeField] Color activeColor = Color.red;
    [SerializeField] Color inactiveColor = Color.white;

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

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "CprBeat")
        {
            if (CprManager.Instance.isCompressing)
            {
                float distance = Vector2.Distance(collision.transform.position, transform.position);

                CprManager.Instance.BeatHit(distance);

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
