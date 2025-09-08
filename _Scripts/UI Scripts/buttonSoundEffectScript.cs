using UnityEngine;

public class buttonSoundEffectScript : MonoBehaviour
{
    public AudioClip UI_Hover_clip;
    public AudioClip Select_Clip;
    public AudioClip UI_Hover_Button_Clip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void HoverOverButton()
    {
        audioSource.clip = UI_Hover_Button_Clip;
        audioSource.volume = 1f;
        audioSource.pitch = 1.5f;
        audioSource.Play();
    }

    public void HoverOverInputFeild()
    {
        audioSource.clip = UI_Hover_clip;
        audioSource.volume = 1f;
        audioSource.pitch = .7f;
        audioSource.Play();
    }

    public void SelectButton()
    {
        audioSource.clip = Select_Clip;
        audioSource.pitch = 1;
        audioSource.Play();
    }
}
