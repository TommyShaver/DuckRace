using UnityEngine;

public class EndCheeringScript : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    [SerializeField]private bool hasHappned;

    private void OnEnable()
    {
        GameManager.OnResetPlayers += CleanUp;
        GameManager.OnClearPlayers += CleanUp;
    }

    private void OnDisable()
    {
        GameManager.OnResetPlayers -= CleanUp;
        GameManager.OnClearPlayers -= CleanUp;
    }

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasHappned)
        {
            hasHappned = true;
            SoundManager.instance.PlaySFXFromSoundEffect(SoundManager.SFX_Clip.End_Cheering_SFX);
        }
    }

    private void CleanUp()
    {
        hasHappned = false;
    }
}
