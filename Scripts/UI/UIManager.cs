using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理器 Singleton
/// </summary>
public class UIManager
{
    private static UIManager instance = new UIManager();

    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }
    //存儲顯示面板的字典 顯示時加入 隱藏時獲取字典中的面板隱藏
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform canvasTrans;

    private UIManager()
    {
        //創建Canvas 並且過場景不移除
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);
    }
    /// <summary>
    /// 顯示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ShowPanel<T>() where T:BasePanel
    {
        //保證泛型類型和面板預設體名字一樣
        string panelName = typeof(T).Name;

        //判斷字典中是否已經顯示面板
        if (panelDic.ContainsKey(panelName))
        {
            //返回面板
            return panelDic[panelName] as T;
        }
        //根據面板名字動態創建預設體設置父對象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(canvasTrans, false);
        //執行面板顯示邏輯並保存到字典 然後顯示 然後返回
        T panel = panelObj.GetComponent<T>();
        panelDic.Add(panelName, panel);
        panel.ShowMe();

        return panel;
    }
    /// <summary>
    /// 隱藏面板
    /// </summary>
    /// <typeparam name="T">面板類型</typeparam>
    /// <param name="isFade">是否淡出</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //保證泛型類型和面板預設體名字一樣
        string panelName = typeof(T).Name;
        //判斷字典中是否已經顯示面板 
        if (panelDic.ContainsKey(panelName))
        {
            //是否淡出隱藏
            if (isFade)
            {
                //淡出完後再刪除物件和字典的值
                panelDic[panelName].HideMe(() =>
                {
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //刪除物件和字典的值
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }
        }
    }
    /// <summary>
    /// 獲得面板
    /// </summary>
    /// <typeparam name="T">面板類型</typeparam>
    /// <returns></returns>
    public T GetPanel<T>() where T : BasePanel
    {
        //保證泛型類型和面板預設體名字一樣
        string panelName = typeof(T).Name;
        //判斷字典中有沒有此面板
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }
}
