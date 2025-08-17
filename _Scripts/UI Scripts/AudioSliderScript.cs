using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderScript : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    private Slider currentSlider;

    private void Awake()
    {
        currentSlider = GetComponent<Slider>();
        currentSlider.value = 1;
    }
    public void AudioSlider(bool sfxTru)
    {
        float sliderValue = 0;
        if (sfxTru)
        {
            sliderValue = currentSlider.value;
            audioMixer.SetFloat("SFX_Fader", Mathf.Log10(sliderValue) * 20);
        }
        else
        {
            sliderValue = currentSlider.value;
            audioMixer.SetFloat("Music_Fader", Mathf.Log10(sliderValue) * 20);
        }
    }
}
