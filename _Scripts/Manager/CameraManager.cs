using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public bool startMovingCamera;
    [SerializeField] private float speedSet;
    [SerializeField] private Vector3 startPos;
    private Camera mainCamera;
    private bool stopCamera;

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
        mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        startPos = transform.position;
        mainCamera.fieldOfView = 70;
    }

    public void GameReset()
    {
        speedSet = 0;
        startMovingCamera = false;
        transform.position = startPos;
        mainCamera.fieldOfView = 70;
    }
    public void StopCamera()
    {
        speedSet = 0;
        mainCamera.fieldOfView = 70;
        stopCamera = true;
    }

    public void GrabCurrentSpeed(float speed)
    {
        speedSet = speed;
    }
    public void WaitAndGo()
    {
        startMovingCamera = true;
        stopCamera = false;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (startMovingCamera)
            transform.Translate(speedSet * Time.deltaTime * Vector2.right);

        if (transform.position.x >= 245 && !stopCamera)
        {
            EndOfGamePOV();
        }
    }

    private void EndOfGamePOV()
    {
        if (mainCamera.fieldOfView >= 60)
        {
            mainCamera.fieldOfView -= .05f;
        }
    }

    public void SetCameraForRace()
    {
        transform.position = new Vector3(0, -5, -10);
        mainCamera.fieldOfView = 70;
    }
}
