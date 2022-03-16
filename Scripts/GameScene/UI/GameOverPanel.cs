using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Text txtWin;
    public Text txtInfo;
    public Text txtMoney;

    public Button btnSure;
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();


            GameLevelMgr.Instance.ClearInfo();
            //切換場景
            SceneManager.LoadScene("BeginScene");
        });
    }
    /// <summary>
    /// 改變介面文字和玩家數據
    /// </summary>
    /// <param name="money"></param>
    /// <param name="isWin"></param>
    public void InitInfo(int money, bool isWin)
    {
        txtWin.text = isWin ? "通關" : "失敗";
        txtInfo.text = isWin ? "獲得勝利獎勵" : "獲得失敗獎勵";

        txtMoney.text = "$" + money;
        //改變玩家數據
        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }
    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }
}
