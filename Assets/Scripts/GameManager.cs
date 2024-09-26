using UnityEngine;

public enum GameMode
{
    MAIN, CPR, EVAL
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public GameMode gameMode;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //cprMode = false;
        gameMode = GameMode.CPR;
        
    }

    void Update()
    {
        
    }
}
