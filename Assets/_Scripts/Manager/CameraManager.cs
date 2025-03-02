using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public bool startMovingCamera;
    [SerializeField] private float speedSet;
    [SerializeField] private Vector3 startPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        startPos = transform.position;
    }

    public void GameReset()
    {
        speedSet = 0;
        startMovingCamera = false;
        transform.position = startPos;
    }
    public void StopCamera()
    {
        speedSet = 0;
    }

    public void GrabCurrentSpeed(float speed)
    {
        speedSet = speed;
    }
    public void WaitAndGo()
    {
        StartCoroutine(WaitASecond());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (startMovingCamera)
            transform.Translate(speedSet * Time.deltaTime * Vector2.right);
    }

    private IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(2);
        startMovingCamera = true;
    }

}
