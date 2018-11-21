using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaScript : MonoBehaviour
{
    ParticleSystem[] motores;
    [SerializeField] int velocidad = 10;
    float aceleracion = 0;
    bool ignicion = false;


    void Start()
    {
        motores = this.GetComponentsInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (ignicion)
        {
            aceleracion += Time.deltaTime;
            this.transform.Translate(Vector3.up * velocidad * aceleracion* Time.deltaTime);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            foreach (ParticleSystem motor in motores)
            {
                motor.Play();
            }
            ignicion = true;
        }
        
        Invoke("SubirNivel", 5);

    }


    private void SubirNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
