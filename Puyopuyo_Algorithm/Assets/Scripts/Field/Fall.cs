using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Fall : MonoBehaviour
{
    private GameObject parentObject = null;
    private readonly BlockView[] blockViews = new BlockView[2];
    private readonly Vector2[] blockCells = new Vector2[2];
    public bool IsFall { private set; get; } = false;

    private int rotate = 0;
    private int dropTime = 0;

    private enum MoveDirection
    {
        LEFT = 0,
        RIGHT = 1,
        DOWN = 2
    }

    private void Awake()
    {
        FallInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void FallInitialize()
    {
        parentObject = new GameObject("FALL");
        parentObject.transform.SetParent(transform);
        parentObject.transform.localScale = Vector3.one;

        for(int i = 0; i < blockViews.Length; i++)
        {
            BlockView block = new GameObject("Block").AddComponent<BlockView>();
            block.transform.SetParent(parentObject.transform);
            block.transform.localPosition = new Vector3(0, i, 0);
            block.transform.localScale = Vector3.one * 0.95f;
            blockViews[i] = block;
        }
    }

    /// <summary>
    /// ブロックの落下を開始する
    /// </summary>
    public void StartFall()
    {
        SetRandomColor();
        parentObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        parentObject.transform.localPosition = DataManager.GameOverCellPos;

        for(int i = 0; i < blockCells.Length; i++)
        {
            blockCells[i].x = DataManager.GAMEOVER_CELL_X;
            blockCells[i].y = DataManager.GAMEOVER_CELL_Y + i;
        }
        
        parentObject.SetActive(true);

        IsFall = true;
    }

    /// <summary>
    /// 落下ブロックの色をランダムで決定する
    /// </summary>
    private void SetRandomColor()
    {
        for(int i = 0; i < blockViews.Length; i++)
        {
            blockViews[i].ColorID = (BLOCK_COLOR)Random.Range(1, System.Enum.GetNames(typeof(BLOCK_COLOR)).Length);
        }
    }

    /// <summary>
    /// 移動判定
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool VaildMove(MoveDirection direction)
    {
        for (int i = 0; i < blockCells.Length; i++)
        {
            int x = Mathf.FloorToInt(blockCells[i].x) + (direction == MoveDirection.LEFT ? -1 : direction == MoveDirection.RIGHT ? 1 : 0);
            int y = Mathf.FloorToInt(blockCells[i].y) + (direction == MoveDirection.DOWN ? -1 : 0);

            if (x < 0 || y < 0 || DataManager.FIELD_SIZE_X - 1 < x || DataManager.FIELD_SIZE_Y - 1 < y)
            {
                return false;
            }

            if (DataManager.GetColorID(x, y) != BLOCK_COLOR.NONE)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// ブロックの配置を登録
    /// </summary>
    private void AddData()
    {
        for (int i = 0; i < blockViews.Length; i++)
        {
            int x = Mathf.FloorToInt(blockCells[i].x);
            int y = Mathf.FloorToInt(blockCells[i].y);

            DataManager.SetColorID(x, y, blockViews[i].ColorID);
        }
    }

    /// <summary>
    /// 入力を取得する
    /// </summary>
    private void GetInput()
    {
        if (!IsFall) { return; }

        int limit = Input.GetKey(KeyCode.DownArrow) ? 5 : 100;
        dropTime++;

        if(limit <= dropTime)
        {
            if (VaildMove(MoveDirection.DOWN))
            {
                MoveBlock(MoveDirection.DOWN);
            }
            else
            {
                AddData();
                parentObject.SetActive(false);
                IsFall = false;
            }
            dropTime = 0;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (VaildMove(MoveDirection.RIGHT))
            {
                MoveBlock(MoveDirection.RIGHT);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (VaildMove(MoveDirection.LEFT))
            {
                MoveBlock(MoveDirection.LEFT);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // 左(90°回転)
            rotate = 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // 右(-90°回転)
            rotate = -1;
        }

        if(rotate != 0)
        {
            

            rotate = 0;
        }
    }

    /// <summary>
    /// ブロックの移動処理
    /// </summary>
    /// <param name="direction"></param>
    private void MoveBlock(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.DOWN:
                parentObject.transform.localPosition += Vector3.down;
                for(int i = 0; i < blockCells.Length; i++)
                {
                    blockCells[i].y -= 1;
                }
                break;
            case MoveDirection.LEFT:
                parentObject.transform.localPosition += Vector3.left;
                for (int i = 0; i < blockCells.Length; i++)
                {
                    blockCells[i].x -= 1;
                }
                break;
            case MoveDirection.RIGHT:
                parentObject.transform.localPosition += Vector3.right;
                for (int i = 0; i < blockCells.Length; i++)
                {
                    blockCells[i].x += 1;
                }
                break;
        }
    }
}
