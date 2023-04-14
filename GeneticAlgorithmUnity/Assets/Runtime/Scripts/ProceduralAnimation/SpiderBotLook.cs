using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBotLook : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private void Update()
    {
        Vector3 direction = _target.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
    }
}
