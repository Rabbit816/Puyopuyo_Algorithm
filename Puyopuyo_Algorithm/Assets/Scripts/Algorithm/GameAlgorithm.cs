using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Block;

namespace Algorithm
{
    public class GameAlgorithm : MonoBehaviour
    {
        private static readonly List<string> deleteBlockList = new List<string>();

        /// <summary>
        /// 4つ以上同じ色のブロックが繋がっているかを探索し該当ブロックを削除する
        /// </summary>
        /// <param name="colorData">盤面のカラー情報</param>
        /// <returns></returns>
        public static BLOCK_COLOR[,] DeleteBlock(BLOCK_COLOR[,] colorData)
        {
            deleteBlockList.Clear();
            BLOCK_COLOR[,] check = new BLOCK_COLOR[Field.FIELD_SIZE_X, Field.FIELD_SIZE_Y];
            for(int i = 0; i < Field.FIELD_SIZE_X; i++)
            {
                for (int j = 0; j < Field.FIELD_SIZE_Y; j++)
                {
                    check[i, j] = colorData[i, j];
                }
            }


            for(int i = 0; i < Field.FIELD_SIZE_X; i++)
            {
                for(int j = 0; j < Field.FIELD_SIZE_Y; j++)
                {
                    if(check[i, j] != BLOCK_COLOR.NONE)
                    {
                        List<string> temp = new List<string>();
                        Count(i, j, check, temp);
                        if(temp.Count >= 4)
                        {
                            foreach (var item in temp)
                            {
                                deleteBlockList.Add(item);
                            }
                        }
                    }
                }
            }

            if(deleteBlockList.Count > 0)
            {
                return Vanish(colorData);
            }

            return colorData;
        }

        /// <summary>
        /// ブロックの連結数をカウント
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="checkList"></param>
        /// <param name="data"></param>
        private static void Count(int x, int y, BLOCK_COLOR[,] checkList, List<string> data)
        {
            BLOCK_COLOR color = checkList[x, y];
            checkList[x, y] = BLOCK_COLOR.NONE;
            string str = x.ToString() + "," + y.ToString();
            data.Add(str);

            if (x + 1 < Field.FIELD_SIZE_X && color == checkList[x + 1, y])
            {
                Count(x + 1, y, checkList, data);
            }
            if (y + 1 < Field.FIELD_SIZE_Y && color == checkList[x, y + 1])
            {
                Count(x, y + 1, checkList, data);
            }
            if (x - 1 >= 0 && color == checkList[x - 1, y])
            {
                Count(x - 1, y, checkList, data);
            }
            if (y - 1 >= 0 && color == checkList[x, y - 1])
            {
                Count(x, y - 1, checkList, data);
            }
        }

        /// <summary>
        /// ブロックを消す
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static BLOCK_COLOR[,] Vanish(BLOCK_COLOR[,] data)
        {
            BLOCK_COLOR[,] temp = new BLOCK_COLOR[Field.FIELD_SIZE_X, Field.FIELD_SIZE_Y];

            for(int i = 0; i < Field.FIELD_SIZE_X; i++)
            {
                for(int j = 0; j < Field.FIELD_SIZE_Y; j++)
                {
                    bool flag = false;
                    foreach(var item in deleteBlockList)
                    {
                        string[] str = item.Split(',');
                        int[] numData = new int[str.Length];
                        for (int k = 0; k < numData.Length; k++)
                        {
                            numData[k] = int.Parse(str[k]);
                        }

                        if (i == numData[0] && j == numData[numData.Length - 1])
                        {
                            flag = true;
                            break;
                        }
                    }

                    temp[i, j] = flag ? BLOCK_COLOR.NONE : data[i, j];
                }
            }

            return temp;
        }

        /// <summary>
        /// ブロックを下詰めにする
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BLOCK_COLOR[,] SortBlock(BLOCK_COLOR[,] data)
        {
            for(int i = 0; i < Field.FIELD_SIZE_X; i++)
            {
                BLOCK_COLOR[] vertical = new BLOCK_COLOR[Field.FIELD_SIZE_Y];

                for (int j = 0; j < Field.FIELD_SIZE_Y; j++)
                {
                    vertical[j] = data[i, j];
                }

                BLOCK_COLOR[] temp = vertical.Where(color => color != BLOCK_COLOR.NONE).ToArray();

                for(int j = 0; j < Field.FIELD_SIZE_Y; j++)
                {
                    data[i, j] = j < temp.Length ? temp[j] : BLOCK_COLOR.NONE;
                }
            }
            
            return data;
        }
    }
}

