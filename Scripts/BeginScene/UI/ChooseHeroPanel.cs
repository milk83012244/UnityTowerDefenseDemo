using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 選擇英雄面板
/// </summary>
public class ChooseHeroPanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnUnlock;
    public Button btnStart;
    public Button btnBack;

    public Text txtUnLock;
    public Text txtMoney;
    public Text txtName;

    private Transform heroPos;

    //記錄當前選擇的角色物件
    private GameObject heroObj;
    //記錄當前選擇的角色數據
    private RoleInfo nowRoleData;
    //當前使用數據索引
    private int nowIndex;
    public override void Init()
    {
        //找到放置角色的位置
        heroPos = GameObject.Find("HeroPos").transform;

        //更新左上角玩家擁有的錢
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();

        btnLeft.onClick.AddListener(() => 
        {
            //向左選擇角色 選到最左邊後從List最後一個開始
            --nowIndex;
            if (nowIndex < 0)
            {
                nowIndex = GameDataMgr.Instance.roleInfosList.Count - 1;
            }
            //模型更新
            ChangeHero();
        });

        btnRight.onClick.AddListener(() =>
        {
            //向右選擇角色 選到最右邊後從List第一個開始
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.roleInfosList.Count)
            {
                nowIndex = 0;
            }
            //模型更新
            ChangeHero();
        });

        btnUnlock.onClick.AddListener(() =>
        {   
            //獲得玩家數據
            PlayerData data = GameDataMgr.Instance.playerData;
            //判斷錢夠不夠
            if (data.haveMoney >= nowRoleData.lockMoney)
            {
                //購買邏輯 持有的錢減去花費的錢
                data.haveMoney -= nowRoleData.lockMoney;
                txtMoney.text = data.haveMoney.ToString();
                //記錄購買的id
                data.buyHero.Add(nowRoleData.id);
                GameDataMgr.Instance.SavePlayerData();
                //更新解鎖按鈕
                UpdateLockButton();

                //提示面板顯示購買成功
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("購買成功");
            }
            else
            {
                //提示面板顯示錢不夠
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金錢不足");
            }
        });

        btnStart.onClick.AddListener(() =>
        {
            //記錄當前選擇角色
            GameDataMgr.Instance.nowSelRole = nowRoleData;
            //隱藏自己顯示選擇場景選擇介面
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            //攝影機轉回去顯示開始介面
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });

        ChangeHero();
    }
    /// <summary>
    /// 切換模型方法
    /// </summary>
    private void ChangeHero()
    {
        //檢測是否已生成模型 有就刪除
        if (heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
        //根據索引值取出一條角色數據
        nowRoleData = GameDataMgr.Instance.roleInfosList[nowIndex];
        //生成角色模型
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);
        //在選角介面不需要角色控制 刪除
        Destroy(heroObj.GetComponent<PlayerObject>());
        //更新角色名稱
        txtName.text = nowRoleData.tips;
        //根據解鎖相關數據來決定是否解鎖按鈕
        UpdateLockButton();
    }
    /// <summary>
    /// 更新解鎖按鈕顯示情況
    /// </summary>
    private void UpdateLockButton()
    {
        //如果該角色需要解鎖但還沒解鎖的話就顯示解鎖按鈕並隱藏開始按鈕
        if (nowRoleData.lockMoney > 0 && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id))
        {
            //更新解鎖按鈕顯示並更新上面的錢
            btnUnlock.gameObject.SetActive(true);
            txtUnLock.text = "$" + nowRoleData.lockMoney;
            //隱藏開始按鈕
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            //解鎖 顯示開始按鈕
            btnUnlock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }
    //隱藏面板自己時刪除模型
    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        if (heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
