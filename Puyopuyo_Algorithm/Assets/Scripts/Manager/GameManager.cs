using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Algorithm;

public class GameManager : MonoBehaviour
{
    private Field field = null;
    private Fall fall = null;

    private bool doOnce = false;
    private bool coroutineFlag = false;
    private bool isGameOver = false;

    [SerializeField] private GameObject gameOverObject = null;

    // Start is called before the first frame update
    void Start()
    {
        GameInitialize();
        gameOverObject.SetActive(false);
        fall.StartFall();
    }

    // Update is called once per frame
    void Update()
    {
        GetEndKey();

        DoAction();

        if(Input.GetKeyDown(KeyCode.Space) && isGameOver)
        {
            isGameOver = false;
            gameOverObject.SetActive(false);
            field.ClearField();
        }
    }

    private void GameInitialize()
    {
        GameObject obj = GameObject.Find("Field");
        field = obj.GetComponent<Field>();
        fall = obj.GetComponent<Fall>();
    }

    /// <summary>
    /// ゲーム終了キーの取得
    /// </summary>
    private void GetEndKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }
    }

    /// <summary>
    /// ブロックを消すコルーチン処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteCoroutine()
    {
        coroutineFlag = true;

        // 配置後のブロックを描画する
        field.DrawField(DataManager.GetFieldData());

        float time = 0;
        while(time < 0.25f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // ブロックを下詰めにして描画
        BLOCK_COLOR[,] before = GameAlgorithm.SortBlock(DataManager.GetFieldData());
        field.DrawField(before);

        bool flag = true;
        while (flag)
        {
            // 削除後のデータを取得
            BLOCK_COLOR[,] after = GameAlgorithm.DeleteBlock(before);

            // ブロックが削除されたかをチェック
            flag = GameAlgorithm.CheckDelete(before, after);

            if (flag)
            {
                time = 0;
                while (time < 0.25f)
                {
                    time += Time.deltaTime;
                    yield return null;
                }

                // 削除後のブロックを描画
                field.DrawField(after);

                time = 0;
                while (time < 0.25f)
                {
                    time += Time.deltaTime;
                    yield return null;
                }

                // ブロックを下詰めにして描画
                before = GameAlgorithm.SortBlock(after);
                field.DrawField(before);
            }

            yield return null;
        }

        coroutineFlag = false;
    }

    private void DoAction()
    {
        if (!fall.IsFall && !doOnce)
        {
            doOnce = true;
            StartCoroutine(DeleteCoroutine());
        }

        if(doOnce && !coroutineFlag)
        {
            if (!isGameOver)
            {
                if (DataManager.GameOverCheck())
                {
                    // ゲームオーバー処理
                    gameOverObject.SetActive(true);
                    isGameOver = true;
                }
                else
                {
                    fall.StartFall();
                }
            }
            doOnce = false;
        }
    }
}
