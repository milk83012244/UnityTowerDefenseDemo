using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 面板基類
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    //控制面板透明度組件
    private CanvasGroup canvasGroup;

    private float alphaSpeed = 10;

    public bool isShow = false;
    //隱藏完之後執行的委託
    private UnityAction hideCallBack = null;

    protected virtual void Awake()
    {
        //獲取面板上的組件
        canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }
    /// <summary>
    /// 註冊控件事件方法 所有子面板都需要註冊一些控件事件 用抽象類必須實現
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// 顯示自己
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }
    /// <summary>
    /// 隱藏自己
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;

        hideCallBack = callBack;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //顯示時淡入
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        //隱藏時淡出
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
    }
}
