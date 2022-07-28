using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the camera movement.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    private Transform _target;
    [SerializeField]
    private float _interpolationSpeed;
    [SerializeField]
    private float pixelToUnits = 40f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Follow();
    }
    public void SetTarget(Transform target)
    {
        if (target != null)
        {
            _target = target;
        }
    }

    /// <summary>
    /// Interpolate the camera position to the target position.
    /// </summary>
    private void Follow()
    {
        if (_target != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, _target.position, _interpolationSpeed);
            newPosition.z = -10;
            transform.position = newPosition;
        }
    }
    public float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = unityUnits * pixelToUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
        return roundedUnityUnits;
    }
}
