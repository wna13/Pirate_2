using System.Collections;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public static PlayerMover Instance;
    public ShipMover shipMover;
    private void Start() 
    {
        Instance = this;
        if ( shipMover == null ) shipMover = this.gameObject.GetComponent<ShipMover>();
        shipMover.isPlayer = true;
    }
}
