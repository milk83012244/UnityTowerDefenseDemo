using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 攝影機相關動畫
/// </summary>
public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    //記錄動畫播放完後要做的事情的委託函數
    private UnityAction overAction;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    //左轉
    public void TurnLeft(UnityAction action)
    {
        animator.SetTrigger("Left");
        //委託函數中傳入括號傳入的函數
        overAction = action;
    }
    //右轉
    public void TurnRight(UnityAction action)
    {
        animator.SetTrigger("Right");
        overAction = action;
    }

    //動畫播放完時調用的方法
    public void PlayerOver()
    {
        overAction?.Invoke();
        overAction = null;
    }
}
