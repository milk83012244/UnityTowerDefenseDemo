using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //攝影機看向的對象
    public Transform target;
    //攝影機對目標對象的偏移位置
    public Vector3 offsetPos;
    //看相目標位置的Y偏移值
    public float bodyHeight;

    public float moveSpeed;
    public float rotationSpeed;

    //攝影機和目標的偏移位置
    private Vector3 targetPos;
    //攝影機看向目標的四元數值
    private Quaternion targetRotation;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
        //根據目標對象來計算攝影機當前位置和角度
        //位移計算
        targetPos = target.position + target.forward * offsetPos.z;//向後偏移Z座標
        targetPos += Vector3.up * offsetPos.y;//向上偏移Y座標(因為Y軸不會被旋轉偏移所以使用Vector3)
        targetPos += target.right * offsetPos.x;
        //差值運算 讓攝影機不停向目標靠攏
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);

        //旋轉計算 
        //攝影機看向目標的四元數值 Quaternion.LookRotation(向量)
        targetRotation = Quaternion.LookRotation(target.position + Vector3.up * bodyHeight - this.transform.position);
        //差值運算 讓攝影機不停向目標角度靠
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 設置攝影機看向的目標對象
    /// </summary>
    /// <param name="player"></param>
    public void SetTarget(Transform player)
    {
        target = player;
    }
}
