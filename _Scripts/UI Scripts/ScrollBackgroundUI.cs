using UnityEngine;
using UnityEngine.UI;

public class ScrollBackgroundUI : MonoBehaviour
{

    [SerializeField] private RawImage duckImgaes;
    [SerializeField] private float x, y;


    // Update is called once per frame
    void Update()
    {
        duckImgaes.uvRect = new Rect(duckImgaes.uvRect.position + new Vector2(x, y) * Time.deltaTime, duckImgaes.uvRect.size);
    }
}
