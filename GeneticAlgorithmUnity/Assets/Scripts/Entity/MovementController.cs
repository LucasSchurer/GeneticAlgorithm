using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            _rb.AddForce(direction * _movementSpeed * 10f, ForceMode.Force);
        }
    }

    public void RotateOld(Vector3 direction)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Quaternion smoothRotation = Quaternion.LookRotation(hit.point - transform.position);

            smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.fixedDeltaTime * 6);

            transform.rotation = Quaternion.Euler(new Vector3(0, smoothRotation.eulerAngles.y));
        }
    }

    public void Rotate(Vector3 direction)
    {
        Quaternion smoothRotation = Quaternion.LookRotation(direction);

        smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.fixedDeltaTime * 6);

        transform.rotation = Quaternion.Euler(new Vector3(0, smoothRotation.eulerAngles.y));
    }
}
