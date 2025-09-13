using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataManagerTable : MonoBehaviour
{
    public static UIDataManagerTable Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public Color colorHPFull, colorHPMid, colorHPLow, hitUIDimStartColor;


}
