using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家數據
/// </summary>
public class PlayerData
{
    public int haveMoney = 300;
    //當前已解鎖角色
    public List<int> buyHero = new List<int>();
}
