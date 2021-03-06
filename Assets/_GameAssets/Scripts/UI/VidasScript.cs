﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidasScript : MonoBehaviour
{
    [SerializeField] GameObject prefabAvatarVida;
    [SerializeField] GameObject player;
    Vector3 correcionSiguienteAvatarVida = new Vector3(0, 0, 0.497f);
    GameObject[] avataresVida;
    Vector3 posicionInicialAvatarVida = new Vector3(84.02F, 513.77f, 14.06f);

    private void Start()
    {
        avataresVida = new GameObject[player.GetComponent<Player>().getVidasMaximas()];
        for (int i = 0; i < avataresVida.Length; i++)
        {
            avataresVida[i] = Instantiate(prefabAvatarVida, this.transform);
        }
        RestarVida();
    }



    public void RestarVida()
    {

        for (int i = player.GetComponent<Player>().getVidas(); i < avataresVida.Length; i++)
        {

            Renderer[] hijosDelAvatar = avataresVida[i].GetComponentsInChildren<Renderer>();
            foreach (Renderer hijo in hijosDelAvatar)
            {
                if (hijo.name.Equals("vanguard_Mesh"))
                {
                    hijo.material.SetColor("_Color", new Color32(0, 0, 0, 128));
                }
            }
        }

    }

    public void SumarVida()
    {
        for (int i = player.GetComponent<Player>().getVidas() - 1; i >= 0; i--)
        {
            Renderer[] hijosDelAvatar = avataresVida[i].GetComponentsInChildren<Renderer>();
            foreach (Renderer hijo in hijosDelAvatar)
            {
                if (hijo.name.Equals("vanguard_Mesh"))
                {
                    hijo.material.SetColor("_Color", new Color32(255, 255, 255, 255));
                }
            }
        }
    }
}
