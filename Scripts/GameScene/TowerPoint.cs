using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 造塔點
/// </summary>
public class TowerPoint : MonoBehaviour
{
    //生成點的塔物件
    private GameObject towerObj = null;
    //生成出來的塔的數據
    public TowerInfo nowTowerInfo = null;
    //可以建造的塔的id(配置表裡面的id)
    public List<int> chooseIDs;

    /// <summary>
    /// 建造塔
    /// </summary>
    /// <param name="id"></param>
    public void CreateTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        //檢測錢夠不夠
        if (info.money>GameLevelMgr.Instance.player.money)
        {
            return;
        }
        //扣錢
        GameLevelMgr.Instance.player.AddMoney(-info.money);
        //生成塔
        //先判斷是否有塔
        if (towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        //實例化塔對象
        towerObj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
        //初始化塔
        towerObj.GetComponent<TowerObject>().InitInfo(info);
        //記錄當前塔的數據
        nowTowerInfo = info;

        //依照是否可以升級更新遊戲介面上的內容
        if (nowTowerInfo.nextLevel != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //判斷有塔 且能不能再升級
        if (nowTowerInfo != null && nowTowerInfo.nextLevel == 0)
        {
            return;
        }
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }
    private void OnTriggerExit(Collider other)
    {
        //傳空取消顯示
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
