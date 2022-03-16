using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    //動畫相關
    private Animator animator;
    //位移相關巡路組件
    private NavMeshAgent agent;

    private MonsterInfo monsterInfo;

    private int hp;
    public bool isDead = false;

    //攻擊間隔時間
    private float frontTime;

    // Start is called before the first frame update
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }
    //初始化
    public void InitInfo(MonsterInfo info)
    {
        monsterInfo = info;
        //動畫狀態機加載
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        //血量
        hp = info.hp;
        //巡路組件的速度和 加速度 加速度一樣會均速移動
        agent.speed = agent.acceleration = info.moveSpeed;
        //巡路組件的旋轉速度
        agent.angularSpeed = info.roundSpeed;
    }
    //受傷
    public void Wound(int dmg)
    {
        if (isDead)
        {
            return;
        }

        hp -= dmg;
        animator.SetTrigger("Wound");
        if (hp <= 0)
        {
            //死亡
            Dead();
        }
        else
        {
            //播放音效
            GameDataMgr.Instance.PlaySound("Music/Wound");
        }
    }
    public void Dead()
    {
        isDead = true;
        //停止移動
        //agent.isStopped = true;
        agent.enabled = false;
        animator.SetBool("Dead", true);
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/dead");
        //加錢
        GameLevelMgr.Instance.player.AddMoney(10);
    }
    //死亡動畫播放後的事件
    public void DeadEvent()
    {
        //動畫播放完後移除對象 通過關卡管理類

        //怪物數量減少 刪除自己
        //GameLevelMgr.Instance.ChangeMonsterNum(-1);
        GameLevelMgr.Instance.RemoveMonster(this);
        Destroy(this.gameObject);
        //檢查是否勝利
        if (GameLevelMgr.Instance.CheckOver())
        {
            //顯示結束介面
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            //勝利獎勵
            panel.InitInfo(GameLevelMgr.Instance.player.money, true);
        }
    }
    //出生後移動
    public void BornOver()
    {
        //出生結束後朝目標點移動
        agent.SetDestination(MainTowerObject.Instance.transform.position);

        animator.SetBool("Run", true);
    }
    //攻擊
    private void Update()
    {
        //檢測是否死亡停下攻擊
        if (isDead)
        {
            return;
        }
        //根據速度決定動畫播放
        animator.SetBool("Run", agent.velocity != Vector3.zero);
        //檢測何時攻擊
        if (Vector3.Distance(this.transform.position, MainTowerObject.Instance.transform.position) < 5
            && Time.time - frontTime >= monsterInfo.atkOffset)
        {
            //記錄攻擊這次時的時間
            frontTime = Time.time;
            animator.SetTrigger("Atk");
        }
    }
    //傷害檢測
    public void AtkEvent()
    {
        Collider[] colliders =  Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1, 1 << LayerMask.NameToLayer("MainTower"));
        GameDataMgr.Instance.PlaySound("Music/Eat");
        for (int i = 0; i < colliders.Length; i++)
        {
            //讓保護區受傷
            if (MainTowerObject.Instance.gameObject==colliders[i].gameObject)
            {
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }
        }
    }
}
