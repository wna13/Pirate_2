using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SailFollowRotation : MonoBehaviour
{
    private List<Animator> animators = new List<Animator>();
    private ShipMover shipMover;
    private bool shipMoving;

    public void SailInit(ShipMover _shipMover)
    {
        if (_shipMover == null)
        {
            Debug.LogError("Ship Transform is not assigned.");
            enabled = false;
            return;
        }

        shipMover = _shipMover;
        shipMoving = shipMover.isMoving;

        int _flagIndex = shipMover.flagIndex;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Renderer flagRenderer = this.gameObject.transform.GetChild(i).GetComponent<Renderer>();
            if (flagRenderer == null) 
            {
                Renderer secondFlagRenderer = this.gameObject.transform.GetChild(i).GetChild(0).GetComponent<Renderer>();
                if (secondFlagRenderer == null) 
                {
                    Debug.LogError("No flag Renderer");
                }
                else
                {
                    flagRenderer = secondFlagRenderer;
                }

                continue;
            }

            flagRenderer.material.mainTexture = textureFromSprite(ShipFlagData.Instance.flagData[_flagIndex].flagImage);

            Animator animator = flagRenderer.GetComponent<Animator>();
            if (animator != null)
            {
                animators.Add(animator);
            }
        }
    }

    public void ChangeFlag()
    {
        Debug.Log("ChangeFlag()");

        int _flagIndex = 0;
        if ( shipMover != null ) _flagIndex = shipMover.flagIndex;
        else _flagIndex = ShipFlagData.Instance.playerEquippedFlagIndex;
        
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Renderer flagRenderer = this.gameObject.transform.GetChild(i).GetComponent<Renderer>();
            if (flagRenderer == null) 
            {
                Renderer secondFlagRenderer = this.gameObject.transform.GetChild(i).GetChild(0).GetComponent<Renderer>();
                if (secondFlagRenderer == null) 
                {
                    Debug.LogError("No flag Renderer");
                }
                else
                {
                    flagRenderer = secondFlagRenderer;
                }
                continue;
            }
            // Debug.Log("flag index = " + _flagIndex);
            // flagRenderer.material.mainTexture = textureFromSprite(ShipFlagData.Instance.flagData[_flagIndex].flagImage);
            if ( flagRenderer == null ) Debug.Log("Null");
            Texture2D tex = textureFromSprite(ShipFlagData.Instance.flagData[_flagIndex].flagImage);
            Debug.Log("Texture assigned: " + tex);
            flagRenderer.material.mainTexture = tex;
        }
    }

    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    private void Update()
    {
        if (shipMover == null) return;

        float tiltZ = shipMover.tilt.localEulerAngles.z;
        if (tiltZ > 180f) tiltZ -= 360f;

        float targetZRotation = tiltZ * 0.7f;

        transform.localEulerAngles = new Vector3(0f, 0f, targetZRotation);

        if (shipMover.isMoving != shipMoving)
        {
            shipMoving = shipMover.isMoving;
            for (int i = 0; i < animators.Count; i++)
            {
                animators[i].SetBool("isMove", shipMoving);
            }
        }
    }
}
