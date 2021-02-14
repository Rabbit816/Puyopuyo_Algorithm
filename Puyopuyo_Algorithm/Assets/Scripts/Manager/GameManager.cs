using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Block;
using Algorithm;

public class GameManager : MonoBehaviour
{
    private Field field = null;

    // Start is called before the first frame update
    void Start()
    {
        GameInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        GetEndKey();

        if (Input.GetKeyDown(KeyCode.R))
        {
            field.RandomColorField();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            field.DrawField(GameAlgorithm.DeleteBlock(field.GetFieldColors()));
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            field.DrawField(GameAlgorithm.SortBlock(field.GetFieldColors()));
        }
    }

    private void GameInitialize()
    {
        field = FindObjectOfType<Field>();
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
}
