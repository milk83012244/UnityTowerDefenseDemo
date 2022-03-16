using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 瞄準鼠標
/// </summary>
public class FireCousor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    public Transform gunPoint;

    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        //射線檢測是否瞄準到敵人 改變鼠標
        if (Physics.Raycast(new Ray(gunPoint.position, this.transform.forward), 1000, 1 << LayerMask.NameToLayer("Monster")))
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
