using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] Player mPlayerGameObjectPrefab;
    
    Player mPlayerGameObject;
    public Player mPlayer => mPlayerGameObject;
    public static GameMode MainGameMode;
    public BattleManager BattleManager { get; private set; }
    void OnDestroy()
    {
        if (MainGameMode == this)
        {
            MainGameMode = null;
        }
    }
    void Awake()
    {
        if (MainGameMode != null)
        {
            Destroy(gameObject);
        }
        MainGameMode = this;
        BattleManager = new BattleManager();

        PlayerStart playerStart = FindFirstObjectByType<PlayerStart>();

        if (!playerStart)
        {
            throw new System.Exception("Need player start for initial spawn position and rotation");
        }
        mPlayerGameObject = Instantiate(mPlayerGameObjectPrefab, playerStart.transform.position, playerStart.transform.rotation);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
