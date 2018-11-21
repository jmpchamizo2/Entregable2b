using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

    public void StartGame() {
        GameConfig.CargarEscena(true);

    }

    public void ContinueGame() {
        GameConfig.CargarEscena(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
