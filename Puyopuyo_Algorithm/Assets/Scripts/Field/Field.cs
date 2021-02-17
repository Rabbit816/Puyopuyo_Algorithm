using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Field : MonoBehaviour
{
    private BlockView[,] fieldBlocks = new BlockView[DataManager.FIELD_SIZE_X, DataManager.FIELD_SIZE_Y];

    void Awake()
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
        float x = -1 * DataManager.FIELD_SIZE_X * 0.5f;
        float y = -1 * DataManager.FIELD_SIZE_Y * 0.5f;
        for(int i = 0; i < DataManager.FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < DataManager.FIELD_SIZE_Y; j++)
            {
                BlockView block = new GameObject("Block").AddComponent<BlockView>();
                block.transform.SetParent(parent.transform);
                block.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0);
                block.transform.localScale = Vector3.one * 0.95f;
                fieldBlocks[i, j] = block;
                if(i == DataManager.GAMEOVER_CELL_X && j == DataManager.GAMEOVER_CELL_Y)
                {
                    DataManager.GameOverCellPos = block.transform.localPosition;
                }
                y++;
                if(y == DataManager.FIELD_SIZE_Y * 0.5f)
                {
                    y = -1 * DataManager.FIELD_SIZE_Y * 0.5f;
                    x++;
                }
            }
        }
    }

    /// <summary>
    /// 盤面をデータをもとに描画する
    /// </summary>
    /// <param name="data"></param>
    public void DrawField(BLOCK_COLOR[,] data)
    {
        for(int i = 0; i < DataManager.FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < DataManager.FIELD_SIZE_Y; j++)
            {
                fieldBlocks[i, j].ColorID = data[i, j];
            }
        }

        DataManager.SetFieldData(data);
    }

    /// <summary>
    /// 盤面のカラー情報を取得
    /// </summary>
    /// <returns></returns>
    private BLOCK_COLOR[,] GetFieldColors()
    {
        BLOCK_COLOR[,] temp = new BLOCK_COLOR[DataManager.FIELD_SIZE_X, DataManager.FIELD_SIZE_Y];
        for(int i = 0; i < DataManager.FIELD_SIZE_X; i++)
        {
            for(int j = 0; j < DataManager.FIELD_SIZE_Y; j++)
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
        for (int i = 0; i < DataManager.FIELD_SIZE_X; i++)
        {
            for (int j = 0; j < DataManager.FIELD_SIZE_Y; j++)
            {
                fieldBlocks[i, j].ColorID = BLOCK_COLOR.NONE;
            }
        }

        DataManager.SetFieldData(GetFieldColors());
    }

    /// <summary>
    /// ランダムな色で盤面を描画する
    /// </summary>
    public void RandomColorField()
    {
        for (int i = 0; i < DataManager.FIELD_SIZE_X; i++)
        {
            for (int j = 0; j < DataManager.FIELD_SIZE_Y; j++)
            {
                fieldBlocks[i, j].ColorID = (BLOCK_COLOR)Random.Range(1, System.Enum.GetNames(typeof(BLOCK_COLOR)).Length);
            }
        }

        DataManager.SetFieldData(GetFieldColors());
    }
}
