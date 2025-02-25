using UnityEngine;
using UnityEngine.SceneManagement;

// A simple singleton to manage global game data and scene transitions.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Lives")]
    public int hearts = 5;

    [Header("Respawn Settings")]
    public Transform restartPoint;  // Where player reappears after losing a life

    [Header("Player Prefab")]
    public GameObject playerPrefab;  // Assign via Inspector (the *only* Player prefab)

    // GPT-generated attributes are stored here after "Generate" is clicked
    public PlayerStats savedStats;

    private void Awake()
    {
        // Basic singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // This object persists across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the Unity sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we just loaded a "level" (e.g., Level1, Level2, Level3), spawn the Player
        // (Adjust logic if your scene naming is different.)
        if (scene.name.StartsWith("Level"))
        {
            SpawnPlayer();
            ScreenHeartManager.Instance.UpdateHeartsUI();
        }
    }

    // Spawns a new Player in the current scene, applying the GPT-generated stats
    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("No playerPrefab assigned in GameManager!");
            return;
        }

        // Instantiate player at the 'restartPoint' (or zero if missing).
        Vector3 spawnPos = (restartPoint != null) ? restartPoint.position : Vector3.zero;
        GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        // Apply GPT attributes if we have them
        PlayerController pc = playerObj.GetComponent<PlayerController>();
        if (pc != null && savedStats != null)
        {
            pc.InitializeStats(savedStats);
        }
        else
        {
            Debug.LogWarning("PlayerController or savedStats is null. Using default values.");
        }
    }

    // Called whenever the player loses a heart (e.g., from a hazard)
    public void LoseHeart()
    {
        hearts--;
        Debug.Log("Player lost a heart! Remaining hearts: " + hearts);
        ScreenHeartManager.Instance.UpdateHeartsUI();
        if (hearts <= 0)
        {
            // If hearts are depleted, go to the GameOverScene
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
