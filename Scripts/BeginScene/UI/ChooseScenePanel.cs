using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 選擇場景面板
/// </summary>
public class ChooseScenePanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnStart;
    public Button btnBack;

    public Text txtInfo;

    public Image imgScene;

    private int nowIndex;
    private SceneInfo nowSceneInfo;
    public override void Init()
    {
        btnLeft.onClick.AddListener(() =>
        {
            //向左選擇場景 選到最左邊後從List最後一個開始
            --nowIndex;
            if (nowIndex < 0)
            {
                nowIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;
            }
            ChangeScene();
        });
        btnRight.onClick.AddListener(() =>
        {
            //向右選擇場景 選到最左邊後從List最後一個開始
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.sceneInfoList.Count)
            {
                nowIndex = 0;
            }
            ChangeScene();
        });
        btnStart.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //切換場景
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            //關卡初始化
            ao.completed += (obj) => 
            {
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);
            };        
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        ChangeScene();
    }
    /// <summary>
    /// 切換介面顯示的場景訊息
    /// </summary>
    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];
        //更新圖片和文字訊息
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);
        txtInfo.text = "名稱:" + nowSceneInfo.name + "\n" + "描述:" + nowSceneInfo.tips;//\n空行
    }
}
