using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    [SerializeField] Text puntuacionNivel1Txt;
    [SerializeField] Text puntuacionNivel2Txt;
    [SerializeField] GameObject panelMenu;
    [SerializeField] GameObject panelStats;



    public void StartGame()
    {
        GameConfig.CargarEscena(true);

    }

    public void ContinueGame()
    {
        GameConfig.CargarEscena(false);
    }

    public void Stats() {
        panelMenu.SetActive(false);
        panelStats.SetActive(true);
        puntuacionNivel1Txt.text = GameConfig.GetPuntuacion(1).ToString();
        puntuacionNivel2Txt.text = GameConfig.GetPuntuacion(2).ToString();

    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Back()
    {
        panelStats.SetActive(false);
        panelMenu.SetActive(true);
    }

}
