using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipModelBase : MonoBehaviour
{
    public Transform leftCannon, rightCannon;
    [SerializeField] Transform tShip;
    public Transform uiPos;
    [SerializeField] ParticleSystem spawnPtcl;


    public void CannonInit(ShipMover _shipMover, int _damage)
    {
        for (int i = 0; i < leftCannon.childCount; i++)
        {
            var leftCannonLauncher = leftCannon.GetChild(i).GetComponent<CannonLauncher>();
            var rightCannonLauncher = rightCannon.GetChild(i).GetComponent<CannonLauncher>();

            if (leftCannonLauncher != null)
            {
                leftCannonLauncher.CannonInit(_shipMover, _damage);
            }
            if (rightCannonLauncher != null)
            {
                rightCannonLauncher.CannonInit(_shipMover, _damage);
            }
        }
    }

    public void MAXLevelCannonDMGUP()
    {
        for (int i = 0; i < leftCannon.childCount; i++)
        {
            var leftCannonLauncher = leftCannon.GetChild(i).GetComponent<CannonLauncher>();
            var rightCannonLauncher = rightCannon.GetChild(i).GetComponent<CannonLauncher>();

            if (leftCannonLauncher != null)
            {
                leftCannonLauncher.CannonMAXLevelDamageUP();
            }
            if (rightCannonLauncher != null)
            {
                rightCannonLauncher.CannonMAXLevelDamageUP();
            }
        }
    }

    public void SailInit( ShipMover _shipMover)
    {
        for (int i = 0; i < tShip.childCount; i++)
        {
            var sail = tShip.GetChild(i).GetComponent<SailFollowRotation>();
            if (sail != null)
            {
                sail.SailInit(_shipMover);
            }
        }
    }

    public void SailChange()
    {
        for (int i = 0; i < tShip.childCount; i++)
        {
            var sail = tShip.GetChild(i).GetComponent<SailFollowRotation>();
            if (sail != null)
            {
                sail.ChangeFlag();
            }
        }
    }
    public void SpawnPtclPlay() => spawnPtcl.Play();
}
