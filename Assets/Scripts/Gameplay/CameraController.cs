using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // modify these
    [Header("Hotkeys")]
    public KeyCode HotkeyZoomIn = KeyCode.KeypadPlus;
    public KeyCode HotkeyZoomOut = KeyCode.KeypadMinus;
    public KeyCode HotkeyRotateCameraRight = KeyCode.Period;
    public KeyCode HotkeyRotateCameraLeft = KeyCode.Comma;
    public KeyCode HotkeyTiltCameraUp = KeyCode.PageUp;
    public KeyCode HotkeyTiltCameraDown = KeyCode.PageDown;
    public int HotkeyPanCameraMouseButton = 2; // 0=LMB; 1=RMB; 2=MMB

    [Header("Zoom")]
    public float zoomTransitionTimeInSeconds = .25f;
    public float zoomForwardBackwardChange = .1f;
    public float zoomUpDownChange = .1f;

    [Header("Rotation")]
    public float rotateTransitionTimeInSeconds = .25f;
    public float rotateDegrees = 45f;

    [Header("Tilt")]
    public float tiltTransitionTimeInSeconds = .25f;
    public float tiltDegrees = 15f;

    [Header("Pan")]
    public float panSensitivity = 1f;
    public float panToUnitTimeInSeconds = .25f;

    // ======do not modify these=====
    private bool zoomedOut = false;
    private Vector3 newZoomCameraLocation;

    private Quaternion currentCameraRotationLocation;
    private Quaternion newCameraRotationLocation;

    private bool tiltedUp = false;
    private Quaternion currentCameraTiltLocation;
    private Quaternion newCameraTiltLocation;

    private Vector3 mousePanStartLocation;
    private Vector3 originalCameraLocation;
    private Quaternion originalCameraRotation;
    private Camera mainCamera;
    // ==============================
    private void Start()
    {
        originalCameraLocation = transform.position;
        originalCameraRotation = transform.rotation;
        mainCamera = Camera.main;
    }
    void Update()
    {

        if (zoomedOut == true && (Input.GetAxis("Mouse ScrollWheel") > 0) || (Input.GetKeyDown(HotkeyZoomIn)))
        {
            ZoomIn();
        }
        if (zoomedOut == false && (Input.GetAxis("Mouse ScrollWheel") < 0) || (Input.GetKeyDown(HotkeyZoomOut)))
        {
            ZoomOut();
        }
        if (Input.GetKeyUp(HotkeyRotateCameraLeft))
        {
            RotateLeft();
        }
        if (Input.GetKeyUp(HotkeyRotateCameraRight))
        {
            RotateRight();
        }
        if (Input.GetKeyUp(HotkeyTiltCameraUp) && tiltedUp == false)
        {
            TiltUp();
        }
        if (Input.GetKeyUp(HotkeyTiltCameraDown) && tiltedUp == true)
        {
            TiltDown();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            ResetCameraPosition();
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
        
        transform.Translate(move, Space.Self);   
    }

    /// <summary>
    /// Move camera in a straight line
    /// </summary>
    /// <param name="startingPosition"></param>
    /// <param name="endingPosition"></param>
    /// <param name="durationOfMove"></param>
    /// <returns></returns>
    IEnumerator LerpTo(Vector3 endingPosition, float durationOfMove)
    {
        Debug.Log("Lerping camera");
        for (float t = 0f; t < durationOfMove; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(gameObject.transform.position, endingPosition, t / durationOfMove);
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
    IEnumerator SLerpTo(Quaternion endingPosition, float durationOfMove)
    {
        Debug.Log("Slerping camera");
        for (float t = 0f; t < durationOfMove; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, endingPosition, t / durationOfMove);
            yield return 0;
        }
        transform.rotation = endingPosition;
    }

    public void PanCameraToLocation(Vector3 location)
    { 
        StartCoroutine(LerpTo(location, panToUnitTimeInSeconds));
    }

    public void ResetCameraPosition()
    {
        StartCoroutine(LerpTo(originalCameraLocation, zoomTransitionTimeInSeconds));
        StartCoroutine(SLerpTo(originalCameraRotation, rotateTransitionTimeInSeconds));
    }

    #region Zoom

    public Vector3 ZoomLocation()
    {
        var zoomVariable = zoomedOut ? -1 : 1;
        var forward = transform.forward * zoomForwardBackwardChange * zoomVariable;
        var down = -transform.up * zoomUpDownChange * zoomVariable;
        return transform.position + forward + down;
    }

    public void ZoomOut()
    {
        zoomedOut = true;
        StartCoroutine(LerpTo(ZoomLocation(), zoomTransitionTimeInSeconds));
    }
    
    public void ZoomIn()
    {
        zoomedOut = false;
        StartCoroutine(LerpTo(ZoomLocation(), zoomTransitionTimeInSeconds));        
    }

    #endregion

    #region Rotate

    public Vector3 GetRotationQuat(bool turnRight)
    {
        Vector3 newLocation = transform.rotation.eulerAngles;
        var rotateLeftOrRight = turnRight ? -1 : 1;
        var yTwist = (transform.rotation.y + (rotateDegrees)) * rotateLeftOrRight;
        newLocation.y += yTwist;
        return newLocation;
    }

    public void RotateLeft()
    {        
        Debug.Log("Rotating camera left");
        //currentCameraRotationLocation = transform.rotation;
        //newCameraRotationLocation = currentCameraRotationLocation * Quaternion.Euler(transform.rotation.x, transform.rotation.y + rotateDegrees, transform.rotation.z);

        StartCoroutine(SLerpTo(Quaternion.Euler(GetRotationQuat(false)), rotateTransitionTimeInSeconds));
    }

    public void RotateRight()
    {
        Debug.Log("Rotating camera right");
        //currentCameraRotationLocation = transform.rotation;
        //newCameraRotationLocation = currentCameraRotationLocation * Quaternion.Euler(transform.rotation.x, transform.rotation.y - rotateDegrees, transform.rotation.z);

        StartCoroutine(SLerpTo(Quaternion.Euler(GetRotationQuat(true)), rotateTransitionTimeInSeconds));
    }

    #endregion

    #region Tilt

    public void TiltUp()
    {
        Debug.Log("Tilting camera up");
        tiltedUp = true;
        currentCameraTiltLocation = transform.rotation;
        newCameraTiltLocation = currentCameraTiltLocation * Quaternion.Euler(transform.rotation.x + tiltDegrees, transform.rotation.y, transform.rotation.z);

        StartCoroutine(SLerpTo(newCameraTiltLocation, tiltTransitionTimeInSeconds));
    }

    public void TiltDown()
    {
        Debug.Log("Tilting camera down");
        tiltedUp = false;
        currentCameraTiltLocation = transform.rotation;
        newCameraTiltLocation = currentCameraTiltLocation * Quaternion.Euler(transform.rotation.x - tiltDegrees, transform.rotation.y, transform.rotation.z);

        StartCoroutine(SLerpTo(newCameraTiltLocation, tiltTransitionTimeInSeconds));
    }

    #endregion
}
