using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConfig : MonoBehaviour {
    private const string XPOS = "xPos";
    private const string YPOS = "yPos";
    private const string ZPOS = "zPos";
    private const string SCENE = "scene";
    public static bool llamadoDesdeStartGame = false;



    public static void StorePosicionEscena(PosicionEscena posEscena) {
        PlayerPrefs.SetFloat(XPOS, posEscena.getPosicion().x );
        PlayerPrefs.SetFloat(YPOS, posEscena.getPosicion().y);
        PlayerPrefs.SetFloat(ZPOS, posEscena.getPosicion().z);
        PlayerPrefs.SetInt(SCENE, posEscena.getEscena());
        PlayerPrefs.Save();
    }


    public static Vector3 GetPosicion() {
        Vector3 position = Vector3.zero;
        if (!llamadoDesdeStartGame && PlayerPrefs.HasKey(XPOS)) {
            float x = PlayerPrefs.GetFloat(XPOS);
            float y = PlayerPrefs.GetFloat(YPOS);
            float z = PlayerPrefs.GetFloat(ZPOS);
            position = new Vector3(x, y, z);
            
        }
        return position;
    }

    //Ponemos un bool que se encargará de controlar si el personaje debe empezar desde 0
    //ya que el método ha sido llamado desde el StartGame en Menu o debe continuar desde 
    //donde estaba grabado que es el resto de los casos
    public static void CargarEscena(bool _llamadoDesdeStartGame)
    {
        llamadoDesdeStartGame = _llamadoDesdeStartGame;
        int escena = 1;
        if (!llamadoDesdeStartGame)
        {
            escena = PlayerPrefs.GetInt(SCENE);
        }
        SceneManager.LoadScene(escena);
    }




    public static void StorePuntuacion(int escena, int puntuacion) {

        string nivelString = ConvertirNivelAString(escena);
        if (PlayerPrefs.HasKey(nivelString) && PlayerPrefs.GetInt(nivelString) < puntuacion) {
            PlayerPrefs.SetInt(nivelString, puntuacion);
            PlayerPrefs.Save();
        } else if (!PlayerPrefs.HasKey(nivelString)) {
            PlayerPrefs.SetInt(nivelString, puntuacion);
            PlayerPrefs.Save();
        }


    }


    public static int GetPuntuacion(int nivel) {
        int puntuacion = 0;
        string nivelString = ConvertirNivelAString(nivel);
        if (PlayerPrefs.HasKey(nivelString)) {
            puntuacion = PlayerPrefs.GetInt(nivelString);
        }
        return puntuacion;
    }


    private static string ConvertirNivelAString(int nivel) {
        string nivelString = "";
        switch (nivel) {
            case 1:
                nivelString = "Nivel1";
                break;
            case 2:
                nivelString = "Nivel2";
                break;
        }
        return nivelString;
    }
}





