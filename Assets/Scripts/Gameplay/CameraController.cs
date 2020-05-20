using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    // modify these
    public float zoomTransitionTimeInSeconds = .25f;
    public float zoomForwardBackwardChange = 2f;
    public float zoomUpDownChange = 2f;

    public float rotateTransitionTimeInSeconds = .25f;
    public float rotateDegrees = 45f;

    public float tiltTransitionTimeInSeconds = .25f;
    public float tiltDegrees = 15f;

    public float panSensitivity = 1f;

    public KeyCode HotkeyZoomIn = KeyCode.KeypadPlus;
    public KeyCode HotkeyZoomOut = KeyCode.KeypadMinus;
    public KeyCode HotkeyRotateCameraRight = KeyCode.Period;
    public KeyCode HotkeyRotateCameraLeft = KeyCode.Comma;
    public KeyCode HotkeyTiltCameraUp = KeyCode.PageUp;
    public KeyCode HotkeyTiltCameraDown = KeyCode.PageDown;
    public int HotkeyPanCameraMouseButton = 2; // 0=LMB; 1=RMB; 2=MMB

    // ======do not modify these=====
    private bool zoomedOut = false;
    private Vector3 newZoomCameraLocation;

    private Quaternion currentCameraRotationLocation;
    private Quaternion newCameraRotationLocation;

    private bool tiltedUp = false;
    private Quaternion currentCameraTiltLocation;
    private Quaternion newCameraTiltLocation;

    private Vector3 mousePanStartLocation;
    // ==============================
    private void Start()
    {
        ZoomOut();
        TiltDown();
    }
    void Update()
    {

        if ((Input.GetAxis("Mouse ScrollWheel") > 0) || (Input.GetKeyDown(HotkeyZoomIn)) && (zoomedOut == true))
        {
            ZoomIn();
        }
        if ((Input.GetAxis("Mouse ScrollWheel") < 0) || (Input.GetKeyDown(HotkeyZoomOut)) && (zoomedOut == false))
        {
            ZoomOut();
        }
        if (Input.GetKeyDown(HotkeyRotateCameraLeft))
        {
            RotateLeft();
        }
        if (Input.GetKeyDown(HotkeyRotateCameraRight))
        {
            RotateRight();
        }
        if (Input.GetKeyDown(HotkeyTiltCameraUp) && tiltedUp == false)
        {
            TiltUp();
        }
        if (Input.GetKeyDown(HotkeyTiltCameraDown) && tiltedUp == true)
        {
            TiltDown();
        }
    }
        
    void LateUpdate()
    { 
        if (Input.GetMouseButtonDown(HotkeyPanCameraMouseButton))
        {
            mousePanStartLocation = Input.mousePosition;
        }
        if (!Input.GetMouseButton(HotkeyPanCameraMouseButton)) return;
        
        var relativeMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition - mousePanStartLocation);
        var move = new Vector3(relativeMousePosition.x * panSensitivity, 0, relativeMousePosition.y * panSensitivity);
        
        transform.Translate(move, Space.World);   
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
        endingPosition.Normalize();
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
    }

    public void RotateRight()
    {
        Debug.Log("Rotating camera right");
        currentCameraRotationLocation = transform.rotation;
        newCameraRotationLocation = currentCameraRotationLocation * Quaternion.Euler(transform.rotation.x, transform.rotation.y - rotateDegrees, transform.rotation.z);

        StartCoroutine(SlerpFromTo(currentCameraRotationLocation, newCameraRotationLocation, rotateTransitionTimeInSeconds));
    }

    #endregion

    #region Tilt

    public void TiltUp()
    {
        Debug.Log("Tilting camera up");
        tiltedUp = true;
        currentCameraTiltLocation = transform.rotation;
        newCameraTiltLocation = currentCameraTiltLocation * Quaternion.Euler(transform.rotation.x + tiltDegrees, transform.rotation.y, transform.rotation.z);

        StartCoroutine(SlerpFromTo(currentCameraTiltLocation, newCameraTiltLocation, tiltTransitionTimeInSeconds));
    }

    public void TiltDown()
    {
        Debug.Log("Tilting camera down");
        tiltedUp = false;
        currentCameraTiltLocation = transform.rotation;
        newCameraTiltLocation = currentCameraTiltLocation * Quaternion.Euler(transform.rotation.x - tiltDegrees, transform.rotation.y, transform.rotation.z);

        StartCoroutine(SlerpFromTo(currentCameraTiltLocation, newCameraTiltLocation, tiltTransitionTimeInSeconds));
    }

    #endregion
}
