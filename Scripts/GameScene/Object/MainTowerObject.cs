using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主堡物件類 Singleton
/// </summary>
public class MainTowerObject : MonoBehaviour
{
    private int hp;
    private int maxHp;

    private bool isDead;


    //讓別人快速得到自己位置(單例模式)

    private static MainTowerObject instance;

    public static MainTowerObject Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }


    //更新血量
    public void UpdateHp(int hp,int maxHP)
    {
        this.hp = hp;
        this.maxHp = maxHP;

        //更新介面上血量顯示
        UIManager.Instance.GetPanel<GamePanel>().UpdataTowerHp(hp, maxHp);
    }

    //自己受到傷害
    public void Wound(int dmg)
    {
        if (isDead)
        {
            return;
        }
        hp -= dmg;
        if (hp<=0)
        {
            hp = 0;
            isDead = true;
            //遊戲結束
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            //失敗獎勵
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);
        }
        UpdateHp(hp, maxHp);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
