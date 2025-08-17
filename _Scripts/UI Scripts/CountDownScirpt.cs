using UnityEngine;
using DG.Tweening;
using TMPro;

public class CountDownScirpt : MonoBehaviour
{
    public static CountDownScirpt instance { get; private set; }

    private TextMeshProUGUI countDownText;
    private Transform localPos;
    private Tween colorTween;
    private float cycDuration = 3.25f;
    private Vector2 goalScale = new (1.5f, 1.5f);
    private Vector2 startScale = new (1,1);

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

        countDownText = GetComponent<TextMeshProUGUI>();
        localPos = GetComponent<Transform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDownText.text = string.Empty;
        countDownText.alpha = 0;
    }

    public void ChangeCountDownText(string input)
    {
        if (input == "3")
        {
            ColorSpinStart();
        }
        countDownText.text = input;
        countDownText.alpha = 1;
        localPos.DOScale(goalScale, 1).SetEase(Ease.InOutBack);

        countDownText.DOFade(0, .5f).SetDelay(1).OnComplete(() =>
        {
            localPos.DOScale(startScale, 0).SetEase(Ease.InOutBack);
            countDownText.text = string.Empty;
            if (input == "GO!")
            {
                colorTween.Kill();
            }
        });
    }

    private void ColorSpinStart()
    {
         colorTween = DOTween.To(() => 0f, h =>
        {
            Color color = Color.HSVToRGB(h, 1f, 1f);
            countDownText.color = color;
        }, 1f, cycDuration).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
