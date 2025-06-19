using System.Collections;
using UnityEngine;

public class DucksQuack : MonoBehaviour
{
    public AudioClip[] ducksQuake;
    private AudioSource atmoDuckPlayer;
    private bool canQuack;

    private void OnEnable()
    {
        GameManager.OnDucksGo += StartQuack;
        GameManager.OnResetPlayers += StopQuack;
        GameManager.OnClearPlayers += StopQuack;
    }

    private void OnDisable()
    {
        GameManager.OnDucksGo -= StartQuack;
        GameManager.OnResetPlayers -= StopQuack;
        GameManager.OnClearPlayers -= StopQuack;
    }
    private void Awake()
    {
        atmoDuckPlayer = GetComponent<AudioSource>();
    }

    private void StartQuack()
    {
        canQuack = true;
        StartCoroutine(DuckQuackLoop());
    }

    private void StopQuack()
    {
        canQuack = false;
    }

    private IEnumerator DuckQuackLoop()
    {
        while (canQuack)
        {
            int numberRandom = Random.Range(0, 2);
            float secondsRandom = Random.Range(.5f, 3f);
            float pitchRandom = Random.Range(1f, 1.5f);

            yield return new WaitForSeconds(numberRandom);
            atmoDuckPlayer.clip = ducksQuake[numberRandom];
            atmoDuckPlayer.pitch = pitchRandom;
            atmoDuckPlayer.Play();
        }

        atmoDuckPlayer.Stop();
    }
    

}
