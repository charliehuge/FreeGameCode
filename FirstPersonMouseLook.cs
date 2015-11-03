using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Mouse look with input smoothing for a first-person camera rig
/// To set up your camera rig, parent a camera to a "body" GameObject
/// 
/// Adapted from:
/// http://forum.unity3d.com/threads/a-free-simple-smooth-mouselook.73117/
/// http://wiki.unity3d.com/index.php/SmoothMouseLook
/// </summary>
public class FirstPersonMouseLook : MonoBehaviour 
{
    // the key that toggles mouse look
    [SerializeField] private KeyCode _enableKey = KeyCode.Escape;

    // should the mouse cursor disappear when mouse look is enbled?
    [SerializeField] private bool _lockCursor = true;

    // speed and constraint settings
    [SerializeField] private float _speedX = 180f;
    [SerializeField] private float _smoothingX = 3f;
    [SerializeField] private float _minX = -180f;
    [SerializeField] private float _maxX = 180f;
    [SerializeField] private float _speedY = 180f;
    [SerializeField] private float _smoothingY = 3f;
    [SerializeField] private float _minY = -45f;
    [SerializeField] private float _maxY = 45f;

    private bool _enabled;
    private Transform _bodyTransform;
    private Transform _cameraTransform;
    private Quaternion _originalBodyRotation;
    private Quaternion _originalCameraRotation;
    private float _smoothInputX;
    private float _smoothInputY;
    private float _currentX;
    private float _currentY;

    // to be set in game prefs
    public bool InvertY { get; set; }

    private void Start()
    {
        // cache the body and camera transforms
        _bodyTransform = GetComponent<Transform>();

        var camera = GetComponentInChildren<Camera>();

        if (camera == null)
        {
            Debug.LogException(
                new MissingComponentException(
                "Camera is missing from the player rig. FirstPersonMouseLook will not work"));
        }

        _cameraTransform = camera.GetComponent<Transform>();

        // cache the original rotation. 
        // We will use this to calculate rotation with the constraints set above.
        _originalBodyRotation = _bodyTransform.localRotation;
        _originalCameraRotation = _cameraTransform.localRotation;

        _enabled = true;
        Screen.lockCursor = _lockCursor;

        _currentX = 0f;
        _currentY = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_enableKey))
        {
            _enabled = !_enabled;

            Screen.lockCursor = _enabled && _lockCursor;
        }

        if (!_enabled)
        {
            return;
        }

        // Set yaw
        // smooth the x-axis input before setting yaw
        _smoothInputX = Mathf.Lerp(_smoothInputX, Input.GetAxisRaw("Mouse X"), 1f / _smoothingX);

        _currentX += _smoothInputX * Time.deltaTime * _speedX;

        // wrap yaw
        if (_currentX > 180f)
        {
            _currentX -= 360f;
        }
        else if (_currentX < -180f)
        {
            _currentX += 360f;
        }

        _currentX = Mathf.Clamp(_currentX, _minX, _maxX);
        _bodyTransform.localRotation = _originalBodyRotation * Quaternion.AngleAxis(_currentX, Vector3.up);

        // Set tilt
        // smooth the y-axis input before setting tilt
        var inputY = Input.GetAxisRaw("Mouse Y") * (InvertY ? -1f : 1f);
        _smoothInputY = Mathf.Lerp(_smoothInputY, inputY, 1f / _smoothingY);

        _currentY += _smoothInputY * Time.deltaTime * _speedY;
        _currentY = Mathf.Clamp(_currentY, _minY, _maxY);
        _cameraTransform.localRotation = _originalCameraRotation * Quaternion.AngleAxis(_currentY, Vector3.left);
    }
}
