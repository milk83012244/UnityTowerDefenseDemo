using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 開始面板
/// </summary>
public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //播放攝影機動畫再顯示選角面板
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                print("顯示選角面板");
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });

            UIManager.Instance.HidePanel<BeginPanel>();
        });

        btnSetting.onClick.AddListener(() =>
        {
            //顯示設置介面
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        btnAbout.onClick.AddListener(() =>
        {

        });

        btnQuit.onClick.AddListener(() =>
        {
            //退出遊戲
            Application.Quit();
        });
    }
}
