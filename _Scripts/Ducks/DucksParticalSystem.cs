using UnityEngine;

public class DucksParticalSystem : MonoBehaviour
{
    private ParticleSystem particleSystemDuck;
    private Renderer partSystemRender;

    private void Awake()
    {
        particleSystemDuck = GetComponent<ParticleSystem>();
        partSystemRender = GetComponent<ParticleSystem>().GetComponent<Renderer>();
    }

    private void Start()
    {
        EffectStop();
    }

    public void SpawnParticleSystem(int i)
    {
        int layer = 1 - i;
        partSystemRender.sortingOrder = layer;
    }

    public void EffectStart()
    {
        particleSystemDuck.Play();
    }
    public void EffectStop()
    {
        particleSystemDuck.Stop();
    }
}
