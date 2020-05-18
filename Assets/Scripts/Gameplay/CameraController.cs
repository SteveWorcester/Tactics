using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{   
    // modify these
    public float zoomTransitionTimeInSeconds = .25f;
    public float zoomForwardBackwardChange = 2f;
    public float zoomUpDownChange = 2f;

    public float rotateTransitionTimeInSeconds = .25f;
    public float rotateDegrees = 45f; 
    
    // ======do not modify these=====
    private bool zoomedOut = false;
    private Vector3 newZoomCameraLocation;

    private Quaternion currentCameraRotationLocation;
    private Quaternion newCameraRotationLocation;
    private bool currentlyRotating = false;
    // ==============================
    private void Start()
    {
        ZoomOut();
    }
    void Update()
    {

        if ((Input.GetAxis("Mouse ScrollWheel") > 0) || (Input.GetKeyDown(KeyCode.KeypadPlus)) && (zoomedOut == true))
        {
            ZoomIn();
        }
        if ((Input.GetAxis("Mouse ScrollWheel") < 0) || (Input.GetKeyDown(KeyCode.KeypadMinus)) && (zoomedOut == false))
        {
            ZoomOut();
        }
        if (Input.GetKeyDown(KeyCode.Comma) && currentlyRotating == false)
        {
            currentlyRotating = true;
            RotateLeft();
        }
        if (Input.GetKeyDown(KeyCode.Period) && currentlyRotating == false)
        {
            currentlyRotating = true;
            RotateRight();
        }
    }

    /// <summary>
    /// Move camera in a straight line
    /// </summary>
    /// <param name="startingPosition"></param>
    /// <param name="endingPosition"></param>
    /// <param name="durationOfMove"></param>
    /// <returns></returns>
    IEnumerator LerpFromTo(Vector3 startingPosition, Vector3 endingPosition, float durationOfMove)
    {
        Debug.Log("Lerping camera");
        for (float t = 0f; t < durationOfMove; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startingPosition, endingPosition, t / durationOfMove);
            yield return 0;
        }
        transform.position = endingPosition;
    }

    /// <summary>
    /// Rotate camera around a sphere
    /// </summary>
    /// <param name="startingPosition"></param>
    /// <param name="endingPosition"></param>
    /// <param name="durationOfMove"></param>
    /// <returns></returns>
    IEnumerator SlerpFromTo(Quaternion startingPosition, Quaternion endingPosition, float durationOfMove)
    {
        Debug.Log("Slerping camera");
        for (float t = 0f; t < durationOfMove; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Slerp(startingPosition, endingPosition, t / durationOfMove);
            yield return 0;
        }
        transform.rotation = endingPosition;
    }

    #region Zoom

    public void ZoomOut()
    {        
        Debug.Log("Zooming camera out");
        var currentPos = transform.position;
        var newCameraForwardLocation = -Vector3.forward * zoomForwardBackwardChange;
        var newCameraDownLocation = -Vector3.down * zoomUpDownChange;
        newZoomCameraLocation = newCameraForwardLocation + newCameraDownLocation;
        

        StartCoroutine(LerpFromTo(currentPos, newZoomCameraLocation, zoomTransitionTimeInSeconds));
        zoomedOut = true;
    }
    
    public void ZoomIn()
    {        
        Debug.Log("Zooming camera in");
        var currentPos = transform.position;
        var newCameraForwardLocation = Vector3.forward * zoomForwardBackwardChange;
        var newCameraDownLocation = Vector3.down * zoomUpDownChange;
        newZoomCameraLocation = newCameraForwardLocation + newCameraDownLocation;        

        StartCoroutine(LerpFromTo(currentPos, newZoomCameraLocation, zoomTransitionTimeInSeconds));
        zoomedOut = false;
    }

    #endregion

    #region Rotate

    public void RotateLeft()
    {        
        Debug.Log("Rotating camera left");
        currentCameraRotationLocation = transform.rotation;
        newCameraRotationLocation = currentCameraRotationLocation * Quaternion.Euler(transform.rotation.x, transform.rotation.y + rotateDegrees, transform.rotation.z);

        StartCoroutine(SlerpFromTo(currentCameraRotationLocation, newCameraRotationLocation, rotateTransitionTimeInSeconds));
        currentlyRotating = false;
    }

    public void RotateRight()
    {
        Debug.Log("Rotating camera right");
        currentCameraRotationLocation = transform.rotation;
        newCameraRotationLocation = currentCameraRotationLocation * Quaternion.Euler(transform.rotation.x, transform.rotation.y - rotateDegrees, transform.rotation.z);

        StartCoroutine(SlerpFromTo(currentCameraRotationLocation, newCameraRotationLocation, rotateTransitionTimeInSeconds));
        currentlyRotating = false;
    }

    #endregion
}
