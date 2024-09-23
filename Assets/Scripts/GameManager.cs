using UnityEngine;

//enum Gamemode
//{
//    MAIN, CPR, EVAL
//}

public class GameManager : MonoBehaviour
{
    //internal Gamemode gamemode;

    public static GameManager Instance { get; private set; }

    public bool cprCompressMode;

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
        cprCompressMode = false;
    }

    void Update()
    {
        
    }
}
