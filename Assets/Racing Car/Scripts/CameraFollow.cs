using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetHeight = 2.0f;
    [SerializeField] private float rotationDamping = 2.0f;
    [SerializeField] private float heightDamping = 3.0f;
    [SerializeField] private float distance = 3.0f;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;
        float toRotationAngle = target.eulerAngles.y;
        float toHeight = target.position.y + offsetHeight;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, toRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, toHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        transform.LookAt(target);
    }
}
