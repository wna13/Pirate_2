using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagParentChenger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParentChange();
    }

    void ParentChange()
    {
        if (transform.parent == null || transform.parent.parent == null)
        {
            Debug.LogWarning("No Parent of Parent");
            return;
        }

        // 현재 전역 위치와 회전을 저장합니다.
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        // 부모를 부모의 부모로 변경합니다.
        transform.SetParent(transform.parent.parent);

        // 전역 위치와 회전을 원래대로 설정합니다.
        transform.position = originalPosition;
        transform.rotation = originalRotation;

    }
}
