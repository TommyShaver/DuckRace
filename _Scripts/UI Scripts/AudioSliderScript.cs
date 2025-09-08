using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderScript : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    private Slider currentSlider;
    public bool isSFX;

    private void Awake()
    {
        currentSlider = GetComponent<Slider>();
        currentSlider.value = 1;
    }
    private void Start()
    {
        if (isSFX)
        {
            currentSlider.value = SaveDataManager.instance.sfxVolumeToSave;
        }
        else
        {
            currentSlider.value = SaveDataManager.instance.musicVolumeToSave;
        }
    }

    public void AudioSlider(bool sfxTru)
    {
        float sliderValue = 0;
        if (sfxTru)
        {
            sliderValue = currentSlider.value;
            audioMixer.SetFloat("SFX_Fader", Mathf.Log10(sliderValue) * 20);
            SaveDataManager.instance.sfxVolumeToSave = currentSlider.value;
            isSFX = true;
        }
        else
        {
            sliderValue = currentSlider.value;
            audioMixer.SetFloat("Music_Fader", Mathf.Log10(sliderValue) * 20);
            SaveDataManager.instance.musicVolumeToSave = currentSlider.value;
            isSFX = false;
        }
    }
}
