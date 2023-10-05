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
        UpdateText();
    }

    public void OnNext()
    {
        current = (current+1)%total;
        UpdateText();
    }
    
    // Je suppose que la fonction est utilisé que avec un chiffre valide à chaque fois, sinon elle fait rien
    public void SetDifficulty(int diff)
    {
    	if (current < total && 0 <= current){
    		current = diff;
    		UpdateText();
    	}
    	
    }
    
    public int GetDifficulty()
    {
    	return current;
    }
    
    private void UpdateText()
    {
    	difficulty.text = battleDatas[current].difficultyName;
        GameManager.instance.battleData = battleDatas[current];
    }
}
