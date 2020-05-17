using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{   
    // modify these
    public float cameraTransitionTimeInSeconds = 1f;
    public float zoomForwardBackwardChange = 2f;
    public float zoomUpDownChange = 2f;
    
    // ======do not modify these=====
    private bool zoomedOut = false;
    private Vector3 newCameraLocation;
    // ==============================
    private void Start()
    {
        ZoomOut();
    }
    void Update()
    {
    
        if ((zoomedOut == true) && (Input.GetAxis("Mouse ScrollWheel") > 0))
        {
            ZoomIn();
        }
        if ((zoomedOut == false) && (Input.GetAxis("Mouse ScrollWheel") < 0) )
        {
            ZoomOut();
        }

        }
    public void ZoomOut()
    {        
        Debug.Log("Zooming camera out");
        var currentPos = transform.position;
        var newCameraForwardLocation = -Vector3.forward * zoomForwardBackwardChange;
        var newCameraDownLocation = -Vector3.down * zoomUpDownChange;
        newCameraLocation = newCameraForwardLocation + newCameraDownLocation;
        

        StartCoroutine(LerpFromTo(currentPos, newCameraLocation, cameraTransitionTimeInSeconds));
        zoomedOut = true;
    }
    
    public void ZoomIn()
    {        
        Debug.Log("Zooming camera in");
        var currentPos = transform.position;
        var newCameraForwardLocation = Vector3.forward * zoomForwardBackwardChange;
        var newCameraDownLocation = Vector3.down * zoomUpDownChange;
        newCameraLocation = newCameraForwardLocation + newCameraDownLocation;        

        StartCoroutine(LerpFromTo(currentPos, newCameraLocation, cameraTransitionTimeInSeconds));
        zoomedOut = false;
    }

    IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        transform.position = pos2;
    }
}
