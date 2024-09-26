using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

enum CprMode
{
    COMPRESS, BREATHE
}

public class CprManager : MonoBehaviour
{
    [SerializeField] GameObject gameManagerObject;
    [SerializeField] GameObject drumObject;
    [SerializeField] Image breathBar;
    [SerializeField] GameObject beatPrefab;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] float beatFrequency = 1f; // 1 beat per second
    [SerializeField] AudioSource music;
    [SerializeField] CprMode cprMode;

    GameManager gameManager;
    CprDrum drum;

    PlayerInput playerInput;
    InputAction compressAction; // replace input with movement
    InputAction breatheAction; // replace input with sound

    float beatTimer = 0f;
    float accuracy = 1.0f; // 100%
    float accuracyTotal = 0f; // beat distance total to calculate accuracy
    int beatsHit = 0;
    int beatsMissed = 0;

    float breathFill = 0f;
    float breathMax = 1.0f;
    int breathCounter = 0; // temp public
    bool isBreathing = false; // temp public

    public int beatsPassed = 0;
    public int beatCounter = 0;
    public bool isCompressing = false;

    bool isPlaying = false;

    public static CprManager Instance { get; private set; }

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
        breatheAction = playerInput.actions["breathe"];
    }

    void Start()
    {
        cprMode = CprMode.BREATHE;
        breathBar.enabled = true;
        scoreText.text = "acc=" + accuracy * 100 + "%/hit=" + beatsHit + "/miss=" + beatsMissed;
    }

    void Update()
    {
        if (gameManager.gameMode == GameMode.CPR)
        {
            // handle compression
            if (cprMode == CprMode.COMPRESS)
            {
                isCompressing = compressAction.ReadValue<float>() > 0;

                if (isCompressing)
                {
                    drum.SetPressed();
                }
                else
                {
                    drum.SetUnpressed();
                }

                beatTimer += Time.deltaTime;

                if (beatTimer >= beatFrequency)
                {
                    GameObject beatObject = Instantiate(beatPrefab, new Vector3(transform.position.x + 200f, transform.position.y, transform.position.z), Quaternion.identity);
                    beatObject.transform.parent = drumObject.transform.parent;
                    beatTimer = 0f;
                }

                if (beatCounter >= 30) // after 30 compressions
                {
                    cprMode = CprMode.BREATHE;
                    drumObject.SetActive(false);
                    beatTimer = 0f;
                    DestroyBeats();
                    beatCounter = 0;
                }
            }
            // handle breathing
            if (cprMode == CprMode.BREATHE)
            {
                isBreathing = breatheAction.ReadValue<float>() > 0;

                if (isBreathing)
                {
                    breathFill += Time.deltaTime;
                }
                else
                {
                    breathFill -= Time.deltaTime;
                }

                if (breathFill < 0)
                {
                    breathFill = 0;
                }

                if (breathFill >= breathMax) // when breath is full
                {
                    breathCounter += 1;
                    breathFill = 0f;
                    isBreathing = false;
                }

                if (breathCounter >= 2) // after 2 breaths
                {
                    cprMode = CprMode.COMPRESS;
                    drumObject.SetActive(true);
                    breathCounter = 0;
                    breathFill = 0f;
                }

                breathBar.fillAmount = breathFill / breathMax;
            }
        }
        else
        {
            drumObject.SetActive(false);
            beatTimer = 0f;
            DestroyBeats();
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
        accuracyTotal += beatDistance;

        float beatAccuracy;
        if (beatsPassed == 0)
        {
            beatAccuracy = 100 - accuracyTotal;
        }
        else
        { 
            beatAccuracy = (100 * beatsPassed) - accuracyTotal;
        }

        return (Mathf.Ceil(beatAccuracy / beatsPassed) / 100);
    }

    void DestroyBeats()
    {
        GameObject[] beats = GameObject.FindGameObjectsWithTag("CprBeat");
        foreach (GameObject beat in beats)
        {
            Destroy(beat);
        }
    }
}
