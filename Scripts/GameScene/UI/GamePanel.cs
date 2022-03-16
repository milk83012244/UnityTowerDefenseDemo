using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 遊戲主面板
/// </summary>
public class GamePanel : BasePanel
{
    public Image imgHP;
    public Text txtHP;
    public Text txtWave;
    public Text txtMoney;

    public Button btnQuit;

    public Transform botTran;//塔控件的父對象 主要用於控制顯隱

    public float hpW = 500;//HP圖片初始寬度可以控制
    //管理造塔控件
    public List<TowerBtn> towerBtns = new List<TowerBtn>();

    //當前進入和選中的造塔點
    private TowerPoint nowSelTowerPoint;
    //標示是否檢測造塔輸入
    private bool checkInput;
    public override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GamePanel>();
            SceneManager.LoadScene("BeginScene");
            //其他
        });

        botTran.gameObject.SetActive(false);
        //鎖定滑鼠
        Cursor.lockState = CursorLockMode.Confined;
    }
    /// <summary>
    /// 更新安全區域血量函數
    /// </summary>
    /// <param name="hp">當前血量</param>
    /// <param name="maxHP">最大血量</param>
    public void UpdataTowerHp(int hp,int maxHP)
    {
        txtHP.text = hp + "/" + maxHP;
        //更新血條的長度 Vector2(血量/最大血量*血條長度,血條寬度)
        (imgHP.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHP * hpW, 46);
    }
    /// <summary>
    /// 更新波數
    /// </summary>
    /// <param name="nowNum">當前波數</param>
    /// <param name="maxNum">最大波數</param>
    public void UpdateWaveNum(int nowNum,int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }
    /// <summary>
    /// 更新金幣數量
    /// </summary>
    /// <param name="money">獲得的金幣</param>
    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }
    /// <summary>
    /// 更新當前選中造塔點界面的變化
    /// </summary>
    public void UpdateSelTower(TowerPoint point)
    {
        //根據造塔點訊息決定介面上顯示內容
        nowSelTowerPoint = point;

        if (nowSelTowerPoint == null)
        {
            checkInput = false;
            //隱藏造塔按鈕
            botTran.gameObject.SetActive(false);
        }
        else
        {
            checkInput = true;
            //顯示造塔按鈕
            botTran.gameObject.SetActive(true);

            //沒有造過塔
            if (nowSelTowerPoint.nowTowerInfo == null)
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPoint.chooseIDs[i], "數字鍵" + (i + 1));
                }
            }
            else
            {
                //升級相關
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);
                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.nextLevel, "空格鍵");
            }
        }
    }
    //造塔點鍵盤輸入用
    protected override void Update()
    {
        base.Update();
        
        if (!checkInput)
        {
            return;
        }
        //檢測有沒有造過塔
        if (nowSelTowerPoint.nowTowerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[2]);
            }
        }
        //有就顯示升級塔
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.nextLevel);
            }
        }
    }

}

