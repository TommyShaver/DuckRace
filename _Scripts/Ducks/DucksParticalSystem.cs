using UnityEngine;

public class DucksParticalSystem : MonoBehaviour
{
    public ParticleSystem landInWater;

    private ParticleSystem particleSystemDuck;
    private Renderer partSystemRender;
    private int currentLayer = 50;

    private void Awake()
    {
        particleSystemDuck = GetComponent<ParticleSystem>();
        partSystemRender = particleSystemDuck.GetComponent<Renderer>();
    }

    private void Start()
    {
        EffectStop();
    }

    public void SpawnParticleSystem(int i)
    {
        if (partSystemRender != null)
            partSystemRender.sortingOrder = currentLayer - i;
    }

    public void EffectStart()
    {
        particleSystemDuck?.Play();
    }

    public void EffectStop()
    {
        particleSystemDuck?.Stop();
    }

    public void DuckChangedLayer(int i)
    {
        if (partSystemRender != null)
            partSystemRender.sortingOrder += i;
    }

    public void DuckLandInWater()
    {
        landInWater?.Play();
    }
}
