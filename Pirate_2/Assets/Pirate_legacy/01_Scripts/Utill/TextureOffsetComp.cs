using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TextureOffsetComp : MonoBehaviour
{
    public Material material;
    float _offset = 0f;

    float _speed = 1f;
    void Start()
    {
        _offset = 0f;
        _speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
            _offset += Time.deltaTime * _speed;
            material.SetTextureOffset("_MainTex", new Vector2( -_offset, 0f));
    }
}
