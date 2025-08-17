using UnityEngine;

public class ConfettiScirpt : MonoBehaviour
{
    private ParticleSystem confettiParticleSystem;

    private void Awake()
    {
        confettiParticleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        GameManager.OnDucksGo += PlayAnimation;
    }
    private void Oisable()
    {
        GameManager.OnDucksGo -= PlayAnimation;
    }

    private void PlayAnimation()
    {
        confettiParticleSystem.Play();
    }
}
