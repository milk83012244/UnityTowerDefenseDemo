using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家類
/// </summary>
public class PlayerObject : MonoBehaviour
{
    private Animator animator;

    //玩家屬性初始化
    private int atk;
    public int money;
    private float roundSpeed = 50;
    //打擊特效
    public string effStr;

    public Transform gunPoint;


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    /// <summary>
    /// 初始化玩家基礎屬性
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="money"></param>
    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;

        UpdateMoney();
    }

    // Update is called once per frame
    void Update()
    {
        //移動變化
        //動作本身有位移所以只要改變動畫觸發的數值
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        //旋轉
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * roundSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //設置蹲下動畫權重
            animator.SetLayerWeight(1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Roll");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Fire");
        }
    }
    //不同攻擊動作的處理
    /// <summary>
    /// 刀武器攻擊動作傷害檢測
    /// </summary>
    public void KnifeEvent()
    {
        //進行傷害檢測 (範圍內檢測多個目標所以用陣列)
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 1, 1 << LayerMask.NameToLayer("Monster"));
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Knife");
        
        //遍歷碰撞到的物體
        for (int i = 0; i < colliders.Length; i++)
        {
            //得到碰撞對象上的怪物腳本讓其受傷
            MonsterObject monster = colliders[i].gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                monster.Wound(atk);
                //只會碰到一個對象
                break;
            }
        }
    }
    /// <summary>
    /// 射擊武器射線檢測
    /// </summary>
    public void ShootEvent()
    {
        //進行射線檢測 前提要有開火點
        RaycastHit[] hits = Physics.RaycastAll(new Ray(gunPoint.position, this.transform.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Gun");

        for (int i = 0; i < hits.Length; i++)
        {
            //得到碰撞對象上的怪物腳本讓其受傷
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                //打擊特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                //在擊中目標上的法線面生成特效
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1);

                monster.Wound(atk);
                //只會碰到一個對象
                break;
            }
        }
    }
    //金錢變化的邏輯
    public void UpdateMoney()
    {
        //調用遊戲面板的更新金錢方法
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }
    /// <summary>
    /// 提供給外部加錢方法
    /// </summary>
    /// <param name="money"></param>
    public void AddMoney(int money)
    {
        this.money += money;
        UpdateMoney();
    }
}
