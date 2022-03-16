using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 防禦塔類
/// </summary>
public class TowerObject : MonoBehaviour
{
    //砲塔頭部旋轉用
    public Transform head;
    //開火特效位置
    public Transform gunPoint;

    private float roundSpeed = 20;

    private TowerInfo info;
    //攻擊目標
    private MonsterObject targetObj;
    //計時攻擊間隔時間
    private float nowTime;
    //記錄怪物位置
    private Vector3 monsterPos;

    private List<MonsterObject> targetObjs;

    /// <summary>
    /// 初始化砲台相關數據
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }

    // Update is called once per frame
    void Update()
    {
        //單體攻擊
        if (info.atkType == 1)
        {
            //檢測目標狀態
            if (targetObj == null || targetObj.isDead || Vector3.Distance(this.transform.position,targetObj.transform.position)>info.atkRange)
            {
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position, info.atkRange);
            }
            if (targetObj == null)
            {
                return;
            }
            //得到怪物位置 並偏移Y讓砲台對準
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;
            //砲台旋轉
            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(monsterPos - head.position), roundSpeed * Time.deltaTime);
            //兩個對象的夾角小於範圍時才能讓目標受傷 而且攻擊間隔條件要滿足
            if (Vector3.Angle(head.forward, monsterPos - head.position) < 5 && Time.time - nowTime >= info.offsetTime)
            {
                //讓目標受傷
                targetObj.Wound(info.atk);
                //播放音效
                GameDataMgr.Instance.PlaySound("Music/Tower");
                //開火特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                Destroy(effObj, 0.2f);
                //記錄開火時間
                nowTime = Time.time;
            }
        }
        //群體攻擊
        else
        {
            //尋找範圍內目標
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position, info.atkRange);
            if (targetObjs.Count > 0 && Time.time - nowTime >= info.offsetTime)
            {
                //播放特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), gunPoint.position, gunPoint.rotation);
                Destroy(effObj, 0.5f);
                //群體受傷
                for (int i = 0; i < targetObjs.Count; i++)
                {
                    targetObjs[i].Wound(info.atk);
                }
                //記錄開火時間
                nowTime = Time.time;
            }
        }
    }
}
