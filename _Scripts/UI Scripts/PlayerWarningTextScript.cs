using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerWarningTextScript : MonoBehaviour
{
    public static PlayerWarningTextScript instance { get; private set; }
    private TextMeshProUGUI warningText;
    private Coroutine stopTextCoroutine;
    [SerializeField] float textSpeed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        warningText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CleanUpWarningMessage();
    }

    public void CleanUpWarningMessage()
    {
        warningText.text = string.Empty;
    }

    public void SwitchPlayerWarningText(string message)
    {
        if (stopTextCoroutine != null)
        {
            StopCoroutine(stopTextCoroutine);
            stopTextCoroutine = null;
        }
        CleanUpWarningMessage();
        stopTextCoroutine = StartCoroutine(TypeLine(message));
    }

    private IEnumerator TypeLine(string linesOfSpeech)
    {
        foreach (char c in linesOfSpeech.ToCharArray())
        {
            warningText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
