using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public bool isOnOffType;
    [SerializeField] Image img;
    [SerializeField] GameObject offImg;

    public void Setting( bool _on )
    {
        if ( isOnOffType == false ) return;

        offImg.SetActive( ! _on );

        if ( _on )
        {
            img.color = Color.white;
        }
        else
        {
            img.color = new Color ( 1f, 1f, 1f, 0.3f );
        }
    }   


}
