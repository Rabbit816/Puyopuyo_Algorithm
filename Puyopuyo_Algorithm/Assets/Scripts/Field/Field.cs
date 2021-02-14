using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Block;

public class Field : MonoBehaviour
{
    public const int FIELD_SIZE_X = 6;
    public const int FIELD_SIZE_Y = 13;
    private BlockView[,] fieldBlocks = new BlockView[FIELD_SIZE_X, FIELD_SIZE_Y];

    // Start is called before the first frame update
    void Start()
    {
        FieldInitialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void FieldInitialize()
    {
        // フィールドに描画用のブロックを配置
        GameObject parent = new GameObject("FIELD");
        parent.transform.SetParent(transform);
        parent.transform.localScale = Vector3.one;
        float x = -1 * FIELD_SIZE_X * 0.5f;
        float y = -1 * FIELD_SIZE_Y * 0.5f;
        for(int i = 0; i < FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < FIELD_SIZE_Y; j++)
            {
                BlockView block = new GameObject("Block").AddComponent<BlockView>();
                block.transform.SetParent(parent.transform);
                block.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0);
                block.transform.localScale = Vector3.one * 0.95f;
                fieldBlocks[i, j] = block;
                y++;
                if(y == FIELD_SIZE_Y * 0.5f)
                {
                    y = -1 * FIELD_SIZE_Y * 0.5f;
                    x++;
                }
            }
        }
    }

    /// <summary>
    /// 盤面をデータをもとに描画する
    /// </summary>
    /// <param name="colors">ブロックのカラー情報</param>
    public void DrawField(BLOCK_COLOR[,] colors)
    {
        for(int i = 0; i < FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < FIELD_SIZE_Y; j++)
            {
                fieldBlocks[i, j].ColorID = colors[i, j];
            }
        }
    }

    /// <summary>
    /// 盤面のカラー情報を取得
    /// </summary>
    /// <returns></returns>
    public BLOCK_COLOR[,] GetFieldColors()
    {
        BLOCK_COLOR[,] temp = new BLOCK_COLOR[FIELD_SIZE_X, FIELD_SIZE_Y];
        for(int i = 0; i < FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < FIELD_SIZE_Y; j++)
            {
                temp[i, j] = fieldBlocks[i, j].ColorID;
            }
        }
        return temp;
    }

    /// <summary>
    /// 盤面を初期化する
    /// </summary>
    public void ClearField()
    {
        for (int i = 0; i < FIELD_SIZE_X; i++)
        {
            for (int j = 0; j < FIELD_SIZE_Y; j++)
            {
                fieldBlocks[i, j].ColorID = BLOCK_COLOR.NONE;
            }
        }
    }

    /// <summary>
    /// ランダムな色で盤面を描画する
    /// </summary>
    public void RandomColorField()
    {
        for (int i = 0; i < FIELD_SIZE_X; i++)
        {
            for (int j = 0; j < FIELD_SIZE_Y; j++)
            {
                fieldBlocks[i, j].ColorID = (BLOCK_COLOR)Random.Range(1, System.Enum.GetNames(typeof(BLOCK_COLOR)).Length);
            }
        }
    }
}
