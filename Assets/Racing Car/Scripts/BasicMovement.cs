using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private Car car;
    private Transform carTransform;

    private float m_moveSpeed = 30.0f;
    private float m_rotateSpeed = 30.0f;

    void Start()
    {
        car = FindObjectOfType<Car>();
        if (car != null)
        {
            carTransform = car.gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * m_moveSpeed * Time.deltaTime);
        if (car != null)
        {
            UpdateRotation();
        }
    }

    public void SetMoveSpeed(float speed)
    {
        if (speed < 0)
        {
            speed = 0;
        }
        m_moveSpeed = speed;
    }

    public void SetRotateSpeed(float speed)
    {
        if (speed < 0)
        {
            speed = 0;
        }
        m_rotateSpeed = speed;
    }

    private void UpdateRotation()
    {
        Vector3 direction = -Vector3.forward;
        float carRotation = carTransform.localEulerAngles.y;
        if (carRotation > car.rotationAngle * 2)
        {
            carRotation -= 360.0f;
        }
        // Debug.Log("Car rotation: " + carRotation);
        transform.Rotate(carRotation / car.rotationAngle * m_rotateSpeed * Time.deltaTime * direction);
    }
}
