using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Block
{
    /// <summary>
    /// ブロックの色リスト
    /// </summary>
    public enum BLOCK_COLOR
    {
        NONE = 0,
        RED = 1,
        BLUE = 2,
        GREEN = 3,
        YELLOW = 4
    }

    public class BlockView : MonoBehaviour
    {

        public BLOCK_COLOR ColorID = BLOCK_COLOR.NONE;
        private SpriteRenderer spriteRenderer = null;

        // Start is called before the first frame update
        void Start()
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
}

