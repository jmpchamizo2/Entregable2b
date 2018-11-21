using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InmunidadScript : MonoBehaviour {
    [SerializeField] int tiempoInmune = 10;
    private void OnTriggerEnter(Collider other) {
        if (other.name.Equals("Player")) {
            other.GetComponent<Player>().Inmunizar(tiempoInmune);
            Destroy(this.gameObject);
        }
    }
}
