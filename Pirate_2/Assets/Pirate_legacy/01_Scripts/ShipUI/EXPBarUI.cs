using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using PirateConfig.Data;
using System.Linq;
public class EXPBarUI : MonoBehaviour
{
    ShipMover myShip;
    [SerializeField] TextMeshProUGUI tLevel;
    [SerializeField] Image progressBar;
    [SerializeField] int expTotal;

    public void LevelInit( ShipMover _ship )
    {
        myShip = _ship;
        expTotal = 0;
        progressBar.fillAmount = 0f;

        tLevel.text = "Lv."+ (myShip.level + 1);
    }

    private void Update() {
        if ( Input.GetKeyDown(KeyCode.E) )
        {
            if ( myShip.isPlayer )
            EXPChange(1);
        }
    }

    public void EXPChange(int exp)
    {
        expTotal += exp;
        if ( GameDataManager.Instance.isTutorialMode )
        {
            int tutoFullExp = 4;
            float _ratio = (float) expTotal / (float) tutoFullExp;
            _ratio = Mathf.Clamp(_ratio, 0f, 1f);

            progressBar.DOKill();
            if ( expTotal >= tutoFullExp )
            {
                LevelUP();
                return;
            }
            else
            {
                progressBar.DOFillAmount(_ratio, 0.2f);
                return;
            }
        }
        else
        {
            if ( GameFlowManager.Instance != null ) GameFlowManager.Instance.EXPGotThisGameValue(exp);
            float _ratio = (float) expTotal / (float) ShipData.DefaultStat.NeedExp[myShip.level];
            _ratio = Mathf.Clamp(_ratio, 0f, 1f);

            progressBar.DOKill();

            if ( expTotal >=  ShipData.DefaultStat.NeedExp[myShip.level] )
            {
                LevelUP();
                return;
            }
            else
            {
                progressBar.DOFillAmount(_ratio, 0.2f);
                return;
            }
        }
    }

    [SerializeField] GameObject maxInfo;
    void LevelUP()
    {
        if ( ShipModelData.Instance.shipData[myShip.shipModelIndex].model.Length  <=  myShip.level)
        {
            //Max 처리
            tLevel.text = "Lv.MAX";
            maxInfo.SetActive(true);
            myShip.SHipLevelUPMAX();
        }
        else
        {
            myShip.ShipLevelUP();
            tLevel.text = "Lv."+(myShip.level + 1);
            maxInfo.SetActive(false);
        }
        expTotal = 0;
        progressBar.fillAmount = 0f;
    }

}
