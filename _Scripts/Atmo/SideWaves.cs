using UnityEngine;

public class SideWaves : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animationPath;

    private void Awake()
    {
        animationPath.GetComponent<Animator>();
    }
    private void Start()
    {
        animationPath.speed = Random.Range(0.2f, 0.5f);
    }
}
