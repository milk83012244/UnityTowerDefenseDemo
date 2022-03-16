using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲資料管理器 Singleton
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance
    {
        get
        {
            return instance;
        }
    }

    //記錄選擇的角色數據 之後在遊戲中創建
    public RoleInfo nowSelRole;
    //音效控制數據
    public MusicData musicData;
    //玩家數據
    public PlayerData playerData;

    //角色數據
    public List<RoleInfo> roleInfosList;
    //場景數據
    public List<SceneInfo> sceneInfoList;
    //怪物數據
    public List<MonsterInfo> monsterInfoList;
    //防禦塔數據
    public List<TowerInfo> towerInfoList;

    private GameDataMgr()
    {
        //初始化默認數據用Json讀取
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        //初始化角色數據用Json讀取
        roleInfosList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        //初始化玩家數據用Json讀取
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        //初始化場景數據用Json讀取
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        //初始化怪物數據用Json讀取
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        //初始化防禦塔數據用Json讀取
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }
    /// <summary>
    /// 儲存音效數據
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
    /// <summary>
    /// 儲存玩家數據
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }
    /// <summary>
    /// 播放音效檔案
    /// </summary>
    /// <param name="resName"></param>
    public void PlaySound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);
        a.volume = musicData.soundValue;
        a.mute = !musicData.soundOpen;

        a.Play();

        GameObject.Destroy(musicObj, 1);
    }
}
