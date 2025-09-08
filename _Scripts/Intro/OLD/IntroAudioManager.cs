using UnityEngine;
using System.Collections;
using DG.Tweening;

public class IntroAudioManager : MonoBehaviour
{
    public LoadingMenuManager loadingMenuManager;

    public AudioSource player1;
    public AudioSource player2;
    public AudioSource player3;
    public AudioSource player4;

    public void MainLoopIntro()
    {
        player1.volume = .45f;
        player1.Play();
        StartCoroutine(StartMainLoop());
    }
    private void MainLoopSong()
    {
        player2.volume = .45f;
        player2.Play();
    }

    public void GameStartSFX()
    {
        player2.DOFade(0, 1f);
        player3.Play();
        player1.Stop();
        player2.Stop();
    }

    public void UI_Loaded()
    {
        player4.Play();
    }

    private IEnumerator StartMainLoop()
    {
        yield return new WaitForSeconds(1.5f);
        loadingMenuManager.ShowLogo();
        MainLoopSong();
        Debug.Log("Music Manager - StartMainLoop");
    }
}
