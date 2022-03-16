using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景音樂控制 Singleton
/// </summary>
public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;

    public static BKMusic Instance
    {
        get
        {
            return instance;
        }
    }

    private AudioSource bkSource;
    private void Awake()
    {
        instance = this;

        bkSource = this.GetComponent<AudioSource>();

        //通過數據設置音樂大小和開關
        MusicData data = GameDataMgr.Instance.musicData;//得到音樂數據
        SetIsOpen(data.musicOpen);
        ChangeValue(data.musicValue);
    }
    /// <summary>
    /// 背景音樂開關方法
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }
    /// <summary>
    /// 改變音量大小方法
    /// </summary>
    /// <param name="v"></param>
    public void ChangeValue(float v)
    {
        bkSource.volume = v;
    }
}
