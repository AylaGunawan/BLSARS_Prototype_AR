using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CprManager : MonoBehaviour
{
    [SerializeField] GameObject gameManagerObject;
    [SerializeField] GameObject drumObject;
    [SerializeField] GameObject beatPrefab;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] float beatFrequency = 1f; // 1 beat per second
    [SerializeField] AudioSource music;

    GameManager gameManager;
    CprDrum drum;

    PlayerInput playerInput;
    InputAction compressAction;

    // WIP make compress and breath mode in here, not game manager

    float beatTimer = 0f;
    float accuracy = 1.0f; // 100%
    float trueTotal = 0f; // distance total to calculate accuracy
    int beatsHit = 0;
    int beatsMissed = 0;
    bool isPlaying = false;

    public static CprManager Instance { get; private set; }

    public int beatsPassed = 0;
    public int beatCounter = 0;
    public bool isCompressing = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // alt: Instance = this only

        gameManager = gameManagerObject.GetComponent<GameManager>();
        drum = drumObject.GetComponent<CprDrum>();

        playerInput = GetComponent<PlayerInput>();
        compressAction = playerInput.actions["compress"];
        scoreText.text = "acc=" + accuracy*100 + "%/hit=" + beatsHit + "/miss=" + beatsMissed;
    }

    void Start()
    {

    }

    void Update()
    {
        if (gameManager.cprCompressMode == true)
        {
            if (beatCounter < 30) // 30 compressions
            {

                drumObject.SetActive(true);
                isCompressing = compressAction.ReadValue<float>() > 0;

                if (isCompressing)
                {
                    drum.SetActive();
                }
                else
                {
                    drum.SetInactive();
                }

                beatTimer += Time.deltaTime;

                if (beatTimer >= beatFrequency)
                {
                    GameObject beatObject = Instantiate(beatPrefab, new Vector3(transform.position.x + 200f, transform.position.y, transform.position.z), Quaternion.identity);
                    beatObject.transform.parent = drumObject.transform.parent;
                    beatTimer = 0f;
                }

            }
            else // 2 breaths WIP
            {
                beatCounter = 0;
                DisableDrumBeats();
            }
        }
        else
        {
            DisableDrumBeats();
        }
        
    }

    public void BeatHit(float beatDistance)
    {
        beatsHit += 1;
        accuracy = calculateAccuracy(beatDistance);
        scoreText.text = "acc=" + accuracy*100 + "%/hit=" + beatsHit + "/miss=" + beatsMissed;
    }

    public void BeatMissed()
    {
        beatsMissed += 1;
        accuracy = calculateAccuracy(100);
        scoreText.text = "acc=" + accuracy*100 + "%/hit=" + beatsHit + "/miss=" + beatsMissed;
    }

    float calculateAccuracy(float beatDistance)
    {
        trueTotal += beatDistance;

        float beatAccuracy;
        if (beatsPassed == 0)
        {
            beatAccuracy = 100 - trueTotal;
        }
        else
        { 
            beatAccuracy = (100 * beatsPassed) - trueTotal;
        }

        return (Mathf.Ceil(beatAccuracy / beatsPassed) / 100);
    }

    void DisableDrumBeats()
    {
        drumObject.SetActive(false);
        beatTimer = 0f;
        GameObject[] beats = GameObject.FindGameObjectsWithTag("CprBeat");
        foreach (GameObject beat in beats)
        {
            Destroy(beat);
        }
    }
}
