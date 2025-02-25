using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("GPT Input/Output")]
    public TMP_InputField inputField;   // User enters character description
    public TMP_Text resultText;         // Displays GPT results or errors

    [Header("Buttons")]
    public Button generateButton;       // Click to fetch stats from GPT
    public Button startGameButton;      // Click to actually begin the game (go to Level1 or a "Ready" scene)

    private GPTClient gptClient = new GPTClient();

    void Start()
    {
        generateButton.onClick.AddListener(OnGenerateButtonClicked);
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);

        // Initially, disable the "Start Game" button until stats are generated
        startGameButton.interactable = false;
        
    }

    async void OnGenerateButtonClicked()
    {
        string description = inputField.text.Trim();

        if (string.IsNullOrEmpty(description))
        {
            resultText.text = "Please enter a character description.";
            return;
        }

        resultText.text = "Generating...";

        try
        {
            // Get stats from GPT
            PlayerStats stats = await gptClient.GenerateCharacterStats(description);

            // Store these stats in GameManager
            GameManager.Instance.savedStats = stats;

            // Update UI with new parameters
            resultText.text = $"Character Stats (Actual Values):\n" +
                              $"Speed: {PlayerConstants.CalculateSpeed(stats.Speed):F2} m/s\n" +
                              $"Jump Force: {PlayerConstants.CalculateJumpForce(stats.JumpForce):F2} N\n" +
                              $"Attack Speed: {PlayerConstants.CalculateAttackSpeed(stats.AttackSpeed):F2} hits/sec";


            // Now the user can start the game
            startGameButton.interactable = true;
        }
        catch (System.Exception e)
        {
            resultText.text = $"Error: {e.Message}";
        }
    }

    void OnStartGameButtonClicked()
    {
        if (startGameButton.interactable == false)
        {
            Debug.LogWarning("Character not ready; Start Game button click ignored.");
            return;
        }
        else
        {
            // For example, load Level1 (or any "ready-to-play" scene)
            SceneManager.LoadScene("Level1");
        }
        
        
    }
}
