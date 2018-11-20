using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    int nivelMaximo = SceneManager.sceneCount;
    [SerializeField] Text[] puntuacionNivelesTxt = new Text[SceneManager.sceneCount];



    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ContinueGame() {

    }

    public void Stats() {
        for(int i = 1; i <= nivelMaximo; i++) {
            puntuacionNivelesTxt[i - 1].text = GameConfig.GetPuntuacion(i).ToString();
        }

    }

    public void QuitGame() {
        Application.Quit();
    }

}
