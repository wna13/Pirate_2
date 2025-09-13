using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultRewardAnimation : MonoBehaviour
{
    public int rewardMultiply = 2;
    [SerializeField] ResultUImanager resultUImanager;
    public void RewardMultiplyChange ( int _value )
    {
        rewardMultiply = _value;
        resultUImanager.RewardValueChange(rewardMultiply);
    }

}
