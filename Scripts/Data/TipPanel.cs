using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提示面板
/// </summary>
public class TipPanel : BasePanel
{
    public Text txtInfo;

    public Button btnSure;

    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipPanel>();
        });
    }

    /// <summary>
    /// 改變提示文字方法給外部使用
    /// </summary>
    /// <param name="info"></param>
    public void ChangeInfo(string info)
    {
        txtInfo.text = info;
    }
}
