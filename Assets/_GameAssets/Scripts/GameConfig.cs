using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour {
    private const string XPOS = "xPos";
    private const string YPOS = "yPos";
    private const string ZPOS = "zPos";
    private const string SCENE = "scene";



    public static void StorePosicionEscena(PosicionEscena posEscena) {
        PlayerPrefs.SetFloat(XPOS, posEscena.getPosicion().x );
        PlayerPrefs.SetFloat(YPOS, posEscena.getPosicion().y);
        PlayerPrefs.SetFloat(ZPOS, posEscena.getPosicion().z);
        PlayerPrefs.SetInt(SCENE, posEscena.getEscena());
        PlayerPrefs.Save();
    }

    public static PosicionEscena GetPosicionEscena() {
        Vector3 position;
        int escena;
        if (PlayerPrefs.HasKey(XPOS) && PlayerPrefs.HasKey(YPOS)) {
            float x = PlayerPrefs.GetFloat(XPOS);
            float y = PlayerPrefs.GetFloat(YPOS);
            float z = PlayerPrefs.GetFloat(ZPOS);
            escena = PlayerPrefs.GetInt(SCENE);
            position = new Vector3(x, y, z);
        } else {
            position = Vector3.zero;
            escena = 0;
        }
        return new PosicionEscena(position, escena);
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





