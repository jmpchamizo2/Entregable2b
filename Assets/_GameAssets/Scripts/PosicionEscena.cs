using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionEscena {

    Vector3 posicion;
    int escena;

    public PosicionEscena() {

    }

    public PosicionEscena(Vector3 posicion, int escena) {
        this.posicion = posicion;
        this.escena = escena;
    }

    public Vector3 getPosicion() {
        return this.posicion;
    }

    public void setPosicion(Vector3 posicion) {
        this.posicion = posicion;
    }

    public int getEscena() {
        return this.escena;
    }

    public void setEscena(int escena) {
        this.escena = escena;
    }
}
