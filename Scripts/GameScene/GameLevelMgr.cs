using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 關卡數據類 Singleton
/// </summary>
public class GameLevelMgr
{
    private static GameLevelMgr instance = new GameLevelMgr();

    public static GameLevelMgr Instance
    {
        get
        {
            return instance;
        }
    }

    public PlayerObject player;

    //所有出怪點
    private List<MonsterPoint> points = new List<MonsterPoint>();
    //剩餘波數
    private int nowWaveNum = 0;
    //總波數
    private int maxWaveNum = 0;

    private bool countdownOver = false;
    ////場景上當前怪物數量
    //private int nowMonsterNum = 0;
    //記錄當前場景上的 怪物列表
    private List<MonsterObject> monsterList = new List<MonsterObject>();
    private GameLevelMgr()
    {

    }
    //切換場景時動態創建玩家
    public void InitInfo(SceneInfo info)
    {        
        //開始前倒數計時
        UIManager.Instance.ShowPanel<CountDownPanel>();
 
        UIManager.Instance.ShowPanel<GamePanel>();
        //創建玩家 獲取當前選擇角色數據
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        //獲取出生位置
        Transform heroPos = GameObject.Find("HeroBornPos").transform;
        //生成玩家
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroPos.position, heroPos.rotation);
        //對玩家對象初始化
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk, info.money);
        
        //讓攝影機看向創建出來的玩家
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        //初始化主塔血量
        MainTowerObject.Instance.UpdateHp(info.towerHp, info.towerHp);
    }
    //通過遊戲管理器判斷是否勝利

    //記錄出怪點方法
    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }
    /// <summary>
    /// 更新最大波數
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        //更新介面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }
    /// <summary>
    /// 更新當前波數
    /// </summary>
    /// <param name="num"></param>
    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }
    /// <summary>
    /// 檢測是否勝利
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            //檢測有沒有出完怪
            if (!points[i].CheckOver())
            {
                return false;
            }
        }
        //檢測有沒有出完怪
        if (monsterList.Count > 0)
        {
            return false;
        }
        return true;
    }
    ///// <summary>
    ///// 改變當前場景上怪物數量
    ///// </summary>
    ///// <param name="num"></param>
    //public void ChangeMonsterNum(int num)
    //{
    //    nowMonsterNum += num;
    //}
    /// <summary>
    /// 記錄怪物到列表中
    /// </summary>
    /// <param name="obj"></param>
    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }
    /// <summary>
    /// 怪物死亡時將怪物從列表中移除
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }
    /// <summary>
    /// 在範圍內尋找怪物(單體攻擊)
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="Range"></param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos,int Range)
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            //找到範圍內的怪物
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= Range) 
            {
                return monsterList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 在範圍內尋找怪物(群體攻擊)
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="Range"></param>
    /// <returns></returns>
    public List<MonsterObject> FindMonsters(Vector3 pos, int Range)
    {
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= Range)
            {
                //尋找範圍內怪物存到List中
                list.Add(monsterList[i]);
            }
        }
        return list;
    }
    /// <summary>
    /// 清空當前關卡紀錄的數據
    /// </summary>
    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
        player = null;
    }
}
