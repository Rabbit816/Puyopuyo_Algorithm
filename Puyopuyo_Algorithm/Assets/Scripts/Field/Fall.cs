using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Fall : MonoBehaviour
{
    private const int BLOCK_GROUP = 2;
    private GameObject parentObject = null;
    private readonly BlockView[] blockViews = new BlockView[BLOCK_GROUP];
    private readonly Vector2[] blockCells = new Vector2[BLOCK_GROUP];
    public bool IsFall { private set; get; } = false;
    private float dropTime = 0;
    private float horizontalMoveTime = 0;
    private bool horizontalMoveFlag = false;
    private int rotateMove = 0;

    private enum MoveDirection
    {
        LEFT = 0,
        RIGHT = 1,
        DOWN = 2,
        UP = 3
    }

    private enum RotateDirection
    {
        TOP = 0,
        LEFT = 1,
        BOTTOM = 2,
        RIGHT = 3
    }

    private RotateDirection rotate = RotateDirection.TOP;

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

        rotate = RotateDirection.TOP;
        rotateMove = 0;

        horizontalMoveTime = 0;
        horizontalMoveFlag = false;
        
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
    /// ブロックの回転を実行する
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="isLeftRotate"></param>
    private void DoRotate(bool isLeftRotate = true)
    {
        bool flag = false;
        while (!flag)
        {
            if (isLeftRotate)
            {
                rotate++;
                if ((int)rotate > System.Enum.GetNames(typeof(RotateDirection)).Length - 1)
                {
                    rotate = RotateDirection.TOP;
                }
            }
            else
            {
                rotate--;
                if (rotate < 0)
                {
                    rotate = RotateDirection.RIGHT;
                }
            }
            flag = VaildRotate(rotate);

            if (flag)
            {
                RotateBlock(rotate);
                if (rotateMove != 0)
                {
                    if(rotate == RotateDirection.LEFT || rotate == RotateDirection.RIGHT)
                    {
                        MoveBlock(MoveDirection.DOWN);
                    }
                    else
                    {
                        MoveBlock(rotateMove == 1 ? MoveDirection.LEFT : MoveDirection.RIGHT);
                    }
                    rotateMove = 0;
                }
            }
            else
            {
                if(rotate == RotateDirection.LEFT && VaildRotate(RotateDirection.RIGHT))
                {
                    MoveBlock(MoveDirection.RIGHT);
                    RotateBlock(RotateDirection.LEFT);
                    rotateMove = 1;
                    flag = true;
                }
                else if(rotate == RotateDirection.RIGHT && VaildRotate(RotateDirection.LEFT))
                {
                    MoveBlock(MoveDirection.LEFT);
                    RotateBlock(RotateDirection.RIGHT);
                    rotateMove = 2;
                    flag = true;
                }
                else if(rotate == RotateDirection.BOTTOM && VaildRotate(RotateDirection.TOP))
                {
                    MoveBlock(MoveDirection.UP);
                    RotateBlock(RotateDirection.BOTTOM);
                    rotateMove = 3;
                    flag = true;
                }
            }
        }
    }

    /// <summary>
    /// 移動できるかチェック
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool VaildMove(MoveDirection direction)
    {
        for (int i = 0; i < BLOCK_GROUP; i++)
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
    /// 回転できるかチェック
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool VaildRotate(RotateDirection direction)
    {
        int x;
        int y;
        switch (direction)
        {
            case RotateDirection.TOP:
                x = Mathf.FloorToInt(blockCells[0].x);
                y = Mathf.FloorToInt(blockCells[0].y) + 1;
                break;
            case RotateDirection.LEFT:
                x = Mathf.FloorToInt(blockCells[0].x) - 1;
                y = Mathf.FloorToInt(blockCells[0].y);
                break;
            case RotateDirection.BOTTOM:
                x = Mathf.FloorToInt(blockCells[0].x);
                y = Mathf.FloorToInt(blockCells[0].y) - 1;
                break;
            case RotateDirection.RIGHT:
                x = Mathf.FloorToInt(blockCells[0].x) + 1;
                y = Mathf.FloorToInt(blockCells[0].y);
                break;
            default:
                return false;
        }

        if (x < 0 || y < 0 || DataManager.FIELD_SIZE_X - 1 < x || DataManager.FIELD_SIZE_Y - 1 < y)
        {
            return false;
        }

        if (DataManager.GetColorID(x, y) != BLOCK_COLOR.NONE)
        {
            return false;
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

        float limit = Input.GetKey(KeyCode.DownArrow) ? 0.05f : 1.0f;
        int input = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;

        if(dropTime < limit)
        {
            dropTime += Time.deltaTime;
        }

        if(dropTime >= limit)
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
        
        if(input != 0)
        {
            HorizontalMove(input);
        }
        else
        {
            horizontalMoveTime = 0;
            horizontalMoveFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            // 左(90°回転)
            DoRotate();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // 右(-90°回転)
            DoRotate(false);
        }
    }

    /// <summary>
    /// 横移動
    /// </summary>
    /// <param name="input"></param>
    private void HorizontalMove(int input)
    {
        if (horizontalMoveFlag)
        {
            if (input > 0)
            {
                if (VaildMove(MoveDirection.RIGHT))
                {
                    MoveBlock(MoveDirection.RIGHT);
                }
            }
            else
            {
                if (VaildMove(MoveDirection.LEFT))
                {
                    MoveBlock(MoveDirection.LEFT);
                }
            }
            horizontalMoveFlag = false;
        }
        else
        {
            if(horizontalMoveTime < 0.1f)
            {
                horizontalMoveTime += Time.deltaTime;
            }
            else
            {
                horizontalMoveFlag = true;
                horizontalMoveTime = 0;
            }
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
            case MoveDirection.UP:
                parentObject.transform.localPosition += Vector3.up;
                for (int i = 0; i < blockCells.Length; i++)
                {
                    blockCells[i].y += 1;
                }
                break;
        }
    }

    /// <summary>
    /// ブロックの回転処理
    /// </summary>
    /// <param name="direction"></param>
    private void RotateBlock(RotateDirection direction)
    {
        switch (direction)
        {
            case RotateDirection.TOP:
                parentObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                blockCells[blockCells.Length - 1] = new Vector2(blockCells[0].x, blockCells[0].y + 1);
                break;
            case RotateDirection.LEFT:
                parentObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                blockCells[blockCells.Length - 1] = new Vector2(blockCells[0].x - 1, blockCells[0].y);
                break;
            case RotateDirection.BOTTOM:
                parentObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                blockCells[blockCells.Length - 1] = new Vector2(blockCells[0].x, blockCells[0].y - 1);
                break;
            case RotateDirection.RIGHT:
                parentObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                blockCells[blockCells.Length - 1] = new Vector2(blockCells[0].x + 1, blockCells[0].y);
                break;
        }
    }
}
