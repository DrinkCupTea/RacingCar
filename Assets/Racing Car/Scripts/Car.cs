using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform[] wheelMeshes;
    [SerializeField] private WheelCollider[] wheelColliders;

    [SerializeField] private int rotateSpeed;
    [SerializeField] public int rotationAngle;
    [SerializeField] private int wheelRotateSpeed;
    [SerializeField] private Vector3 centerOfMass;
    [SerializeField] private Transform[] grassEffects;
    [SerializeField] private Transform[] skidMarkPivots;
    [SerializeField] private float grassEffectOffset;
    [SerializeField] private GameObject skidMarkPrefab;
    [SerializeField] private float skidMarkDelay;
    [SerializeField] private float skidMarkSize;

    private WorldGenerator worldGenerator;
    private int targetRotation;
    private bool isOnGround;
    // Start is called before the first frame update
    void Start()
    {
        worldGenerator = FindObjectOfType<WorldGenerator>();
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        StartCoroutine(nameof(SkidMark));
    }

    void FixedUpdate()
    {
        UpdateEffects();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < wheelMeshes.Length; i++)
        {
            wheelColliders[i].GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheelMeshes[i].position = pos;
            wheelMeshes[i].Rotate(Time.deltaTime * wheelRotateSpeed * Vector3.right);
        }

        if (Input.GetMouseButton(0) || Input.GetAxis("Horizontal") != 0)
        {
            UpdateTargetRotation();
        }
        else if (targetRotation != 0)
        {
            targetRotation = 0;
        }
        Vector3 rotation = new(transform.localEulerAngles.x, targetRotation, transform.localEulerAngles.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotation), rotateSpeed * Time.deltaTime);
    }

    private void UpdateTargetRotation()
    {
        if (Input.GetAxis("Horizontal") == 0)
        {
            if (Input.mousePosition.x > Screen.width * 0.5f)
            {
                // Turn Right
                targetRotation = rotationAngle;
            }
            else
            {
                // Truen Left
                targetRotation = -rotationAngle;
            }
        }
        else
        {
            targetRotation = (int)(Input.GetAxis("Horizontal") * rotationAngle);
        }
    }

    private void UpdateEffects()
    {
        isOnGround = true;
        for (int i = 0; i < 2; i++)
        {
            Transform wheelMesh = wheelMeshes[i + 2];

            if (Physics.Raycast(wheelMesh.position, Vector3.down, grassEffectOffset * 1.5f))
            {
                if (!grassEffects[i].gameObject.activeSelf)
                {
                    grassEffects[i].gameObject.SetActive(true);
                }
                float effectHeitht = wheelMesh.position.y - grassEffectOffset;
                Vector3 pos = new(grassEffects[i].position.x, effectHeitht, wheelMesh.position.z);
                grassEffects[i].position = pos;
                skidMarkPivots[i].position = pos;

                isOnGround = false;
            }
            else if (grassEffects[i].gameObject.activeSelf)
            {
                grassEffects[i].gameObject.SetActive(false);
            }
        }
    }
    private IEnumerator SkidMark()
    {
        while (true)
        {
            yield return new WaitForSeconds(skidMarkDelay);
            if (isOnGround || transform.localEulerAngles.y < 10)
            {
                continue;
            }
            for (int i = 0; i < skidMarkPivots.Length; i++)
            {
                GameObject newSkidMark = Instantiate(skidMarkPrefab, skidMarkPivots[i].position, skidMarkPivots[i].rotation);
                newSkidMark.transform.parent = worldGenerator.GetWorldPiece().transform;
                newSkidMark.transform.localScale = new Vector3(1, 1, 4) * skidMarkSize;
            }
        }
    }

}
