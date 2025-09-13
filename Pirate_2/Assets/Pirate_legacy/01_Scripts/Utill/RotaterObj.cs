using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaterObj : MonoBehaviour
{
    public float rotationSpeed = 30f;

    void Update()
    {
        // 오브젝트를 Z 축을 중심으로 회전
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
