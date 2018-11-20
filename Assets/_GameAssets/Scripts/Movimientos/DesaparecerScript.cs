using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesaparecerScript : MonoBehaviour {


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Invoke("Desaparecer", 1);
        }
    }

    private void Desaparecer()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        Invoke("Aparecer", 3);
    }

    private void Aparecer()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

}
