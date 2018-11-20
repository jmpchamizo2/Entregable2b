using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPlasmaScript : MonoBehaviour {
    [SerializeField] int tiempoActivaciónTrampa = 5;

    private void Start()
    {
        InvokeRepeating("Reproducir", tiempoActivaciónTrampa, tiempoActivaciónTrampa);
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.name.Equals("Player")){
            print("Te pille");
        }
    }

    private void Reproducir()
    {
        this.GetComponent<ParticleSystem>().Play();
    }

}
