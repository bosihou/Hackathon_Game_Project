using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonReadyTester : MonoBehaviour
{
    [Header("UI References for Testing")]
    // Assign in the Inspector the TMP_Text that shows the status.
    public TMP_Text statusText;
    // Assign in the Inspector the Start Game Button from your UI.
    public Button startGameButton;

    void Start()
    {
        // Set the default text to indicate the button is not ready.
        if (statusText != null)
        {
            statusText.text = "button not ready";
        }
    }

    void Update()
    {
        // Every frame, check the button's interactable state and update the text accordingly.
        if (statusText != null && startGameButton != null)
        {
            if (startGameButton.interactable)
            {
                statusText.text = "button ready";
            }
            else
            {
                statusText.text = "button not ready";
            }
        }
    }
}