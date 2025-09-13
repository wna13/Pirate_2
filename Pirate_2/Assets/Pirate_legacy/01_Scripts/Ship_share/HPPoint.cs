using UnityEngine;

public class HPPoint : MonoBehaviour
{
    public int fullHP;
    public int currentHP;
    ShipMover myShip;

    public void HPInitSetting( ShipMover _ship)
    {
        myShip = _ship;
        GetFullHP();

        currentHP = fullHP;
    }
    void GetFullHP()
    {
        fullHP = ShipModelData.Instance.shipData[myShip.shipModelIndex].defaultHP * (myShip.level + 1);

        if ( myShip.isPlayer == false ) fullHP += BalanceFactorManager.Instance.HP();
        else
        {
            float balance = 1f;
            int playCount = GameDataManager.Instance.playCount;
            if ( playCount < 3 ) balance = 0.6f;
            if ( playCount >= 3 && playCount < 6  ) balance = 0.66f;
            if ( playCount >= 6 && playCount < 10  ) balance = 0.72f;
            if ( playCount >= 10 && playCount < 15  ) balance = 0.88f;

            float percentageValue = fullHP * balance;
            int newHP = Mathf.RoundToInt(percentageValue);

            fullHP = newHP;
        }
    }
    public void HPRefreshByLevelUP()
    {
        int _minusHP = fullHP - currentHP;

        GetFullHP();
        currentHP = fullHP - _minusHP;
        myShip.shipUI.HPBarValueChange(fullHP, currentHP);
    }
    public void HPMaxUP()
    {
        int _minusHP = fullHP - currentHP;

        float percentageValue = fullHP * 1/10;
        int hpAdded = Mathf.RoundToInt(percentageValue);

        fullHP += hpAdded;

        currentHP = fullHP - _minusHP;
        myShip.shipUI.HPBarValueChange(fullHP, currentHP);
    }
    public bool GetDamage(int _damage)
    {
        currentHP -= _damage;

        if (currentHP <= 0)  // 죽음.
        {
            // HPBar 게이지 마이너스 되지 않도록 보정
            currentHP = 0;

            // 죽음 처리
            return false;
        }
        else
        {
            return true;
        }
        // 게이지바 UI 업데이트
    }

    public void GetHealObj()
    {
        int _heal = Mathf.CeilToInt(fullHP / 5f);
        currentHP += _heal;
        if ( currentHP >= fullHP ) currentHP = fullHP;
        myShip.shipUI.HPBarValueChange(fullHP, currentHP);

        // heal effect
        GameObject _go = null;
        Vector3 pos = myShip.shipUI.transform.position;


        if (ObjectPoolManager.Instance.healPtcl.TryGetNextObject(pos, Quaternion.identity, out _go))
        {
            Transform parent = _go.transform.parent;
            _go.transform.SetParent(myShip.shipUI.transform);
            _go.transform.eulerAngles = Vector3.zero;
            _go.transform.SetParent(parent);
            _go.GetComponent<ParticleSystem>().Play();
        }
    }
}
