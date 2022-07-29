using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the camera movement.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _height;
    [SerializeField]
    private float _interpolationSpeed;

    private void FixedUpdate()
    {
        Follow();
    }

    /// <summary>
    /// Interpolate the camera position to the target position.
    /// </summary>
    private void Follow()
    {
        if (_target != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, _target.position, _interpolationSpeed);
            newPosition.y = _height;
            transform.position = newPosition;
        }
    }
}
