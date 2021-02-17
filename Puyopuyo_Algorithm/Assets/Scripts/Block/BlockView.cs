using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;


public class BlockView : MonoBehaviour
{

    public BLOCK_COLOR ColorID = BLOCK_COLOR.NONE;
    private SpriteRenderer spriteRenderer = null;

    void Awake()
    {
        BlockInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        SetColor();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void BlockInitialize()
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Block");
    }

    /// <summary>
    /// ブロックの色を割り当てる
    /// </summary>
    private void SetColor()
    {
        switch (ColorID)
        {
            case BLOCK_COLOR.RED:
                spriteRenderer.color = Color.red;
                break;
            case BLOCK_COLOR.BLUE:
                spriteRenderer.color = Color.blue;
                break;
            case BLOCK_COLOR.GREEN:
                spriteRenderer.color = Color.green;
                break;
            case BLOCK_COLOR.YELLOW:
                spriteRenderer.color = Color.yellow;
                break;
            case BLOCK_COLOR.NONE:
                spriteRenderer.color = Color.clear;
                break;
        }
    }
}

