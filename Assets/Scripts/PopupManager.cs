using UnityEngine;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    void Awake()
    {
        // This makes the script globally accessible
        Instance = this;
    }

    void Start()
    {
        // Hide the popup when the game starts
        if (popupPanel != null) popupPanel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (popupPanel == null || popupText == null) return;

        // Stop any current popups so they don't glitch
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message));
    }

    // --- THIS IS THE MISSING FUNCTION ---
    public void ForceShowMessage(string message)
    {
        if (popupPanel == null || popupText == null) return;

        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message));
    }
    // ------------------------------------

    IEnumerator ShowRoutine(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        popupPanel.SetActive(false);
    }
}