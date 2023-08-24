using Attacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySettings : MonoBehaviour
{
    private BattleData[] battleDatas;
    private int total;
    private int current;

    [SerializeField]
    private Text difficulty;


    void Start()
    {
        battleDatas = Resources.LoadAll<BattleData>("GameData/GameMode");

        total = battleDatas.Length;
        current = 0;
        difficulty.text = battleDatas[current].difficultyName;
        GameManager.instance.battleData = battleDatas[current];
    }

    public void OnNext()
    {
        current = (current+1)%total;
        difficulty.text = battleDatas[current].difficultyName;
        GameManager.instance.battleData = battleDatas[current];
    }
}
