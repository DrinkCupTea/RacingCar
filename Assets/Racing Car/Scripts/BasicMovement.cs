using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private float m_speed = 30f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * m_speed * Time.deltaTime);
    }

    public void setSpeed(float speed)
    {
        if (speed < 0)
        {
            speed = 0;
        }
        m_speed = speed;
    }
}
