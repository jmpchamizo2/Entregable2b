using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergiaScript : MonoBehaviour {

	[SerializeField] int cantidadEnergia = 50;
    private void OnTriggerEnter(Collider other) {
        if (other.name.Equals("Player")) {
            other.GetComponent<Player>().IncrementarEnergiaArmas(cantidadEnergia);
            Destroy(this.gameObject);
        }
    }
}
