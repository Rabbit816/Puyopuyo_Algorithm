using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
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

    public class DataManager : MonoBehaviour
    {
        public const int FIELD_SIZE_X = 6;
        public const int FIELD_SIZE_Y = 13;
        public const int GAMEOVER_CELL_X = 2;
        public const int GAMEOVER_CELL_Y = 11;

        private static readonly BLOCK_COLOR[,] fieldData = new BLOCK_COLOR[FIELD_SIZE_X, FIELD_SIZE_Y];
        public static Vector3 GameOverCellPos = Vector3.zero;

        /// <summary>
        /// フィールドのブロック配置を登録する
        /// </summary>
        /// <param name="data"></param>
        public static void SetFieldData(BLOCK_COLOR[,] data)
        {
            Array.Copy(data, 0, fieldData, 0, fieldData.Length);
        }

        /// <summary>
        /// フィールドのブロック配置を取得する
        /// </summary>
        /// <returns></returns>
        public static BLOCK_COLOR[,] GetFieldData()
        {
            BLOCK_COLOR[,] temp = new BLOCK_COLOR[FIELD_SIZE_X, FIELD_SIZE_Y];
            Array.Copy(fieldData, 0, temp, 0, temp.Length);
            return temp;
        }

        /// <summary>
        /// カラー情報を登録する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void SetColorID(int x, int y, BLOCK_COLOR color)
        {
            fieldData[x, y] = color;
        }

        /// <summary>
        /// カラー情報を取得する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static BLOCK_COLOR GetColorID(int x, int y)
        {
            return fieldData[x, y];
        }

        /// <summary>
        /// ゲームオーバーかチェックする
        /// </summary>
        /// <returns></returns>
        public static bool GameOverCheck()
        {
            return fieldData[GAMEOVER_CELL_X, GAMEOVER_CELL_Y] != BLOCK_COLOR.NONE;
        }
    }
}

