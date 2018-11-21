using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilCanyonScript : MonoBehaviour {
    [SerializeField] int danyo = 5;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            collision.gameObject.GetComponent<Player>().RecibirDanyo(danyo);
        }
        Destroy(this.gameObject);
    }
}
