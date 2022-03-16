using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownPanel : BasePanel
{
    public float countdownTime;
    public Text txtCount;

    public bool countdownDone;
    public override void Init()
    {
        StartCoroutine(CountdownToStart());
        Time.timeScale = 0;
    }

    IEnumerator CountdownToStart()
    {
        countdownDone = false;
        while (countdownTime > 0)
        {
            txtCount.text = countdownTime.ToString();

            yield return new WaitForSecondsRealtime(1f);
            countdownTime--;
        }
        txtCount.text = "Start!";
        Time.timeScale = 1;
        countdownDone = true;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public bool CountdownOver()
    {
        if (countdownDone)
        {
            return true;
        }
        return false;
    }
}
