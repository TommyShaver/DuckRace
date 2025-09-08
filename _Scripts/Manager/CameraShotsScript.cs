using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraShotsScript : MonoBehaviour
{

    public static CameraShotsScript instance { get; set; }
    public GameObject fixedCamera;
    public GameObject duckCamera1;
    public GameObject duckCamera2;
    public GameObject startingCamera1;
    public GameObject startingCamera2;

    private bool stopCameraAnim;

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

    public void ChangeCamera()
    {
        stopCameraAnim = false;
        StartCoroutine(CameraIdleAnim());
    }

    public void TurnOffCamera()
    {
        stopCameraAnim = true;
        CleanUp();
        StopAllCoroutines();
    }

    private void CleanUp()
    {
        fixedCamera.SetActive(true);
        duckCamera1.SetActive(false);
        duckCamera2.SetActive(false);
        startingCamera1.SetActive(false);
        startingCamera2.SetActive(false);
    }

    public void DoubleCheck()
    {
        fixedCamera.SetActive(false);
    }

    private IEnumerator CameraIdleAnim()
    {
        yield return new WaitForSeconds(120);
        int cameraNumber = 0;
        while (!stopCameraAnim)
        {
            yield return new WaitForSeconds(7);
            CleanUp();
            cameraNumber++;
            if (cameraNumber > 4)
            {
                cameraNumber = 0;
            }
            switch (cameraNumber)
            {
                case 0:
                    fixedCamera.SetActive(true);
                    break;
                case 1:
                    duckCamera1.SetActive(true);
                    break;
                case 2:
                    duckCamera2.SetActive(true);
                    break;
                case 3:
                    startingCamera1.SetActive(true);
                    break;
                case 4:
                    startingCamera2.SetActive(true);
                    break;
            }
        }
    }
}
