using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生怪點
/// </summary>
public class MonsterPoint : MonoBehaviour
{
    //最大波數
    public int maxWave;
    //每波怪物數量
    public int monsterNumOneWave;
    //記錄當前怪物剩餘數量
    private int nowNum;
    //怪物ID組 可以隨機創建不同怪物
    public List<int> monsterIDs;
    //記錄當前波創建ID
    private int nowID;
    //怪物生成間隔時間
    public float createOffectTime;
    //每波間隔時間
    public float delayTime;
    //首波間隔時間
    public float firstDelayTime;
    // Start is called before the first frame update
    void Start()
    {
        //第一波帶延遲
        Invoke("CreateWave", firstDelayTime);
        //記錄出怪點
        GameLevelMgr.Instance.AddMonsterPoint(this);
        //更新最大波數
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);
    }
    /// <summary>
    /// 創建一波怪物
    /// </summary>
    private void CreateWave()
    {
        //當前波隨機生成怪物的ID
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];
        //當前波怪物有多少
        nowNum = monsterNumOneWave;
        CreateMonster();
        //減少當前波數
        --maxWave;
        GameLevelMgr.Instance.ChangeNowWaveNum(1);
    }
    //創建怪物
    private void CreateMonster()
    {
        //獲得怪物數據
        MonsterInfo info = GameDataMgr.Instance.monsterInfoList[nowID - 1];
        //創建怪物
        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
        //預設體添加怪物組件腳本初始化
        MonsterObject monsterObj = obj.AddComponent<MonsterObject>();
        monsterObj.InitInfo(info);
        //告訴管理器增加怪物數量
        //GameLevelMgr.Instance.ChangeMonsterNum(1);
        GameLevelMgr.Instance.AddMonster(monsterObj);

        //創建完減少要生成的怪物數量
        --nowNum;
        //檢測此波怪物是否生成完畢
        if (nowNum == 0)
        {
            //檢測是否還有波數
            if (maxWave > 0)
            {
                //依照每波延遲時間執行下一波
                Invoke("CreateWave", delayTime);
            }
        }
        else
        {
            //依照間隔時間生成怪物
            Invoke("CreateMonster", createOffectTime);
        }
    }
    /// <summary>
    /// 出怪點是否出怪結束
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
