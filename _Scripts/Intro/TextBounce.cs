using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextBounce : MonoBehaviour
{
    private Tween danceTween;
    private TextMeshProUGUI quackDashText;
    private void Awake()
    {
        quackDashText = GetComponent<TextMeshProUGUI>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAnimStart();
    }

    private void TextAnimStart()
    {
        danceTween = quackDashText.rectTransform.DOScale(new Vector3(1.01f, 1.01f, 1.01f), .434f).SetLoops(-1, LoopType.Restart);
    }

    public void CleanUpTween()
    {
        danceTween. Kill();
    } 
}
