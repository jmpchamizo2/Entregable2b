using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") {
            Vector3 posicion = other.gameObject.transform.position;
            int escena = SceneManager.GetActiveScene().buildIndex;
            GameConfig.StorePosicionEscena(new PosicionEscena(posicion, escena));
            Destroy(this.gameObject);
        }
    }
}
