using UnityEngine;
using TMPro;
using System.Collections;

public class WinnerTextScript : MonoBehaviour
{
    public Color[] colors;
    public int startingColor;

    private TextMeshProUGUI textFont;

    private void Awake()
    {
        textFont = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        WinnerScaler.OnWinnerCall += WinnerAnimation;
        WinnerScaler.OnWinnerCleanUp += CleanUp;
        GameManager.OnResetPlayers += CleanUp;
        GameManager.OnClearPlayers += CleanUp;
    }

    private void OnDisable()
    {
        WinnerScaler.OnWinnerCall -= WinnerAnimation;
        WinnerScaler.OnWinnerCleanUp -= CleanUp;
        GameManager.OnResetPlayers -= CleanUp;
        GameManager.OnResetPlayers -= CleanUp;
    }

    public void WinnerAnimation()
    {
        StartCoroutine(ChangeColor());
    }

    public void CleanUp()
    {
        StopAllCoroutines();
    }

    private IEnumerator ChangeColor()
    {
        int updateNumber = 0;
        updateNumber = startingColor;
        while (true)
        {
            updateNumber++;
            if (updateNumber >= colors.Length)
            {
                updateNumber = 0;
            }
            textFont.color = colors[updateNumber];
            yield return new WaitForSeconds(.05f);
        }
    }
}
