using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoCamMove : MonoBehaviour
{
    public static TutoCamMove Instance;
    public Camera camera;
    private void OnEnable() {
        Instance = this;
    }
    void Update()
    {
        if ( PlayerMover.Instance != null)  this.transform.position = PlayerMover.Instance.transform.position;
    }
}
