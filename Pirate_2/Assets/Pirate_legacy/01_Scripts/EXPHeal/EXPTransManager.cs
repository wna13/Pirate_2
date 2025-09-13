using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPTransManager : MonoBehaviour
{
    public static EXPTransManager Instance;
    public Transform EXPParent;
    void Start()
    {
        Instance= this;
    }


}
