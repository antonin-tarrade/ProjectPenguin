using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Objet/Component
    public GameObject GameManager;
    private Game_Manager gameManager;

    public GameObject player;
    private Player playerSystem;

    // Menu
    public GameObject mainMenu;
    public GameObject rulesMenu;
    public GameObject creditMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject overMenu;

    // Textes
    public GameObject textShards;
    public GameObject UIHealth;

    // Variables
    public GameObject menuActif;

    public bool isPaused;

    private void Awake() {
        Initialisation();

        //Initialisation Component
        playerSystem = player.GetComponent<Player>();
        gameManager = GameManager.GetComponent<Game_Manager>();
    }

    public void Initialisation() {

        Time.timeScale = 0f;
        
        menuActif = mainMenu;
        mainMenu.SetActive(true);
        rulesMenu.SetActive(false);
        creditMenu.SetActive(false);
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        overMenu.SetActive(false);
    }

    private void Update() {

        // Menu Over
        if (gameManager.isOver){

            if(Input.GetKeyDown(KeyCode.R)){
                gameManager.Play();
            }

            Switch(overMenu);
        }

        // Menu Pause
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                gameManager.Pause();
            }
            else {
                gameManager.Unpause();
            }
        }
        if (isPaused) return;

        // Menu Game
        // Score
        textShards.GetComponent<Text>().text = playerSystem.iceShards.ToString();
        // Vie (texte)
        UIHealth.GetComponent<Text>().text = playerSystem.health.ToString();
    }

    public void Switch(GameObject to) {
        menuActif.SetActive(false);
        to.SetActive(true);

        menuActif = to;
    }
}
