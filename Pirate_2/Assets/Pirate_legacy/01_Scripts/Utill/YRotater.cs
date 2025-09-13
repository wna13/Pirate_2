using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YRotater : MonoBehaviour
{
 [SerializeField] private float rotationSpeed = 100f; // 회전 속도

    void Update()
    {
        // X축을 기준으로 매 프레임마다 회전시킴
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
