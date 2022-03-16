using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 組合控鍵 主要方便控制造塔相關UI更新邏輯
/// </summary>
public class TowerBtn : MonoBehaviour
{
    public Image imgPic;

    public Text txtTip;
    public Text txtMoney;

    /// <summary>
    /// 初始化造塔按鈕訊息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="inputStr"></param>
    public void InitInfo(int id,string inputStr)
    {
        //得到對應ID的塔的數據
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        imgPic.sprite = Resources.Load<Sprite>(info.imgRes);
        txtMoney.text = "$" + info.money;
        txtTip.text = inputStr;

        //檢測金錢夠不夠
        if (info.money > GameLevelMgr.Instance.player.money)
        {
            txtMoney.text = "金錢不足";
        }
    }
}
