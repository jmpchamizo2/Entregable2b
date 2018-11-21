using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {
    [SerializeField] int danyo = 2;

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.tag.Equals("Enemigo"))
        {
            collision.gameObject.GetComponent<Enemigo>().RecibirDanyo(danyo);

        }
        Destroy(this.gameObject);
    }
}
