using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 設定面板
/// </summary>
public class SettingPanel : BasePanel
{
    public Button btnClose;

    public Toggle togMusic;
    public Toggle togSound;

    public Slider sliderMusic;
    public Slider sliderSound;
    public Slider sliderRotationSpeed;

    public override void Init()
    {
        //根據本機儲存的設置數據初始化面板顯示內容
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn = data.musicOpen;
        togSound.isOn = data.soundOpen;
        sliderMusic.value = data.musicValue;
        sliderSound.value = data.soundValue;

        btnClose.onClick.AddListener(() =>
        {
            //關閉時儲存設定數據
            GameDataMgr.Instance.SaveMusicData();

            UIManager.Instance.HidePanel<SettingPanel>();
        });

        togMusic.onValueChanged.AddListener((v) =>
        {
            //開關背景音樂 記錄開關數據
            BKMusic.Instance.SetIsOpen(v);
            GameDataMgr.Instance.musicData.musicOpen = v;//從GameDataMgr得到MusicData改變數值
        });

        togSound.onValueChanged.AddListener((v) =>
        {
            //只記錄音效開關數據
            GameDataMgr.Instance.musicData.soundOpen = v;
        });

        sliderMusic.onValueChanged.AddListener((f) =>
        {
            //改變背景音樂大小 記錄音樂大小數據
            BKMusic.Instance.ChangeValue(f);
            GameDataMgr.Instance.musicData.musicValue = f;
        });

        sliderSound.onValueChanged.AddListener((f) =>
        {
            //只記錄音效大小數據
            GameDataMgr.Instance.musicData.soundValue = f;
        });
    }
}
