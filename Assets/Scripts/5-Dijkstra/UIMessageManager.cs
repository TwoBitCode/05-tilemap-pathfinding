using UnityEngine;
using TMPro; // Required for TextMeshPro
using System.Collections; // Required for IEnumerator

public class UIMessageManager : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText; // Use TMP_Text for TextMeshPro
    [SerializeField] private float messageDuration = 2f;

    private Coroutine currentMessageCoroutine;

    public void ShowMessage(string message)
    {
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }
        currentMessageCoroutine = StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        messageText.text = message;
        messageText.enabled = true;

        yield return new WaitForSeconds(messageDuration);

        messageText.text = "";
        messageText.enabled = false;
    }
}
