using UnityEngine;
public enum GameState { NullState, Intro, MainMenu, Game }
public delegate void OnStateChangeHandler();
public class GameManager
{
    public GameState gameState { get; private set; }
    public event OnStateChangeHandler OnStateChange;

    public bool On { get; set; }

    public int TopSpriteLayer = 100;
    public float BarrierTopY { get; set; }
    public float BarrierBottomY { get; set; }
    public float BeltSpriteBoundY { get; set; }
    public float DistanceFromCamera { get; set; }

    public Vector2 leftBinPosition { get; set; }
    public Vector2 rightBinPosition { get; set; }

    public float edgeOfScreen { get; set; }

    public float fallIntoBinSpeed { get; set; }
    public float throwSpeed { get; set; }
    public float slapSpeed { get; set; }
    public float beltSpeed { get; set; }

    public Transform CursorTransform { get; set; }

    public DialogueManager dialogueManager { get; set; }

    public SoundManager soundManager { get; set; }

    protected GameManager()
    {
        On = false;

        beltSpeed = Time.deltaTime;
        fallIntoBinSpeed = Time.deltaTime * 2.0f;
        throwSpeed = Time.deltaTime * 5.0f;
        slapSpeed = Time.deltaTime * 5.0f;

        BeltSpriteBoundY = 0;

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        DistanceFromCamera = Mathf.Abs(camera.transform.position.z);
        
        leftBinPosition = GameObject.FindGameObjectWithTag("LeftBin").transform.position;
        rightBinPosition = GameObject.FindGameObjectWithTag("RightBin").transform.position;
        CursorTransform = GameObject.FindGameObjectWithTag("Cursor").transform;

        edgeOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x + 5;

        dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    }

    private static GameManager _instance = null;

    // Singleton pattern implementation
    public static GameManager Instance
    {
        get
        {
            if (GameManager._instance == null)
            {
                GameManager._instance = new GameManager();
            }
            return GameManager._instance;
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        OnStateChange?.Invoke();
    }

    public int GetTopSpriteLater()
    {
        TopSpriteLayer += 1;
        if (TopSpriteLayer > 200)
        {
            TopSpriteLayer = 100;
        }
        return TopSpriteLayer;
    }
}