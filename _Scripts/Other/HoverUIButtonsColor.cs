using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HoverUIButtonsColor : MonoBehaviour
{

    private Image imageColor;
    private Vector2 startSclae;
    private RectTransform buttonsLocation;
    public Color[] colorChoice;
    float grabScale;

    private void Awake()
    {
        imageColor = GetComponent<Image>();
        buttonsLocation = GetComponent<RectTransform>();
        startSclae = buttonsLocation.localScale;
        GetLocal();
    }

    private void GetLocal()
    {
        float temp = buttonsLocation.localScale.x;
        grabScale = (((temp * 2) * .65f));
        Debug.Log(grabScale);
    } 

    public void OnHover()
    {
        imageColor.color = colorChoice[1];
        buttonsLocation.DOScale(grabScale, .5f).SetEase(Ease.OutBounce);
    }
    public void AfterHover()
    {
        imageColor.color = colorChoice[0];
        buttonsLocation.DOScale(startSclae, .1f).SetEase(Ease.InBack);
    }
}
