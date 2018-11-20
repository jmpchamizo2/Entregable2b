using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntuacionEscena {

    int puntuacion;
    int escena;

    public PuntuacionEscena() {

    }

    public PuntuacionEscena(int puntuacion, int escena) {
        this.puntuacion = puntuacion;
        this.escena = escena;
    }

    public int getPuntuacion() {
        return this.puntuacion;
    }

    public void setPuntuacion(int puntuacion) {
        this.puntuacion = puntuacion;
    }

    public int getEscena() {
        return this.escena;
    }

    public void setEscena(int escena) {
        this.escena = escena;
    }
}
}
