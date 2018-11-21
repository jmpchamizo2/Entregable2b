using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum estadoPlayer { parado, andando, corriendo, volando, nadando, sinEstado, muerto }

public class Player : MonoBehaviour
{
    float zPos;
    float ySpeedActual;
    estadoPlayer estado;
    Rigidbody rb;
    bool corriendo = false;
    Animator playerAnimator;
    bool mostrarVida = false;
    bool mostrarCombustible = false;
    bool mostrarEnergia = false;
    bool inmune = false;

    [Header ("Características")]
    [SerializeField] int velocidad;
    [SerializeField] int fuerzaSalto = 10;
    [SerializeField] GameObject posicionPies;
    [Header("Objetos")]
    [SerializeField] int cantidadCombustibleDeVuelo = 100;
    int cantidadDeCombustibleMaxima = 200;
    bool ignicion = false;
    float tiempoActivarTrigger = 1f;
    [Header("Salud")]
    [SerializeField] int salud = 100;
    private int saludMaxima = 100;
    [SerializeField] int vidas = 1;
    private int vidasMaximas = 3;
    [Header("FX")]
    [SerializeField] ParticleSystem explosionIgnicion;
    [SerializeField] AudioSource sonidoIgnicion;
    [SerializeField] GameObject laser;
    [Header("Armas")]
    [SerializeField] Transform disparoVolando;
    [SerializeField] Transform disparoTierra;
    [SerializeField] int fuerzaDisparo = 5000;
    [SerializeField] int energiaArmas = 100;
    private int energiaMaximaArmas = 100;
    private int puntuacion = 0;
    [Header("UI")]
    [SerializeField] Text puntuacionTxt;
    [SerializeField] Image frontalBarraVida;
    [SerializeField] Image frontalBarraCombustible;
    [SerializeField] Image frontalBarraEnergia;
    [SerializeField] CanvasGroup barraVida;
    [SerializeField] CanvasGroup barraCombustible;
    [SerializeField] CanvasGroup barraEnergia;
    [SerializeField] GameObject gameOverMenu;




    private void Start()
    {

        estado = estadoPlayer.parado;
        rb = this.GetComponent<Rigidbody>();
        playerAnimator = this.GetComponent<Animator>();
        //Recupera la posición del último checkpoint
        Vector3 posicion = GameConfig.GetPosicion();
        if (posicion != Vector3.zero) {
          this.transform.position = posicion;
        }
        gameOverMenu.SetActive(false);
    }

    private void Update()
    {
        if(estado != estadoPlayer.muerto) {
            CorrerEstaApretado();
            IgnicionEstaApretado();
            Disparar();
        }
        Animar();
        MostrarBarraDeVida();
        MostrarBarraDeEnergia();
        MostrarBarraDeCombustible();
        SalirAMenu();

    }


    private void FixedUpdate()
    {
        if (estado != estadoPlayer.muerto) {
            zPos = Input.GetAxis("Horizontal");
            ySpeedActual = rb.velocity.y;
            VolarSaltarCaminar();
            CambiarDireccion();
        }
        
    }


    private void CorrerEstaApretado()
    {
        if(estado != estadoPlayer.volando || estado != estadoPlayer.nadando)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                velocidad *= 2;
                corriendo = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                velocidad /= 2;
                corriendo = false;
            }
        }
        
    }


    private void IgnicionEstaApretado()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (estado == estadoPlayer.nadando)
            {
                if (cantidadCombustibleDeVuelo > 0)
                {
                    CambioEstadoEnAgua(estadoPlayer.volando);
                    Invoke("ActivarTrigger", tiempoActivarTrigger);
                    ignicion = true;
                }

            }
            else
            {
                CambioEstado(estadoPlayer.volando);
                Invoke("ActivarTrigger", tiempoActivarTrigger);
                ignicion = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ignicion = false;
        }
        
    }



    private void CambiarDireccion()
    {
        if (zPos > 0.01f) {
            this.transform.localScale = new Vector3(1, 1, 1);
            disparoTierra.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            disparoVolando.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            CorriendoOAndando();

        }
        if (zPos < -0.01f) {
            this.transform.localScale = new Vector3(1, 1, -1);
            disparoTierra.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            disparoVolando.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            CorriendoOAndando();
        }
        if (zPos < 0.01f && zPos > -0.01f) {
            if (this.estado != estadoPlayer.volando && EstaEnSuelo()) {
                CambioEstado(estadoPlayer.parado);
            }
        }
       
    }

    private void CorriendoOAndando() {
        estadoPlayer estadoNuevo = estadoPlayer.sinEstado;
        if (this.estado != estadoPlayer.volando && EstaEnSuelo()) {
            estadoNuevo = (corriendo) ? estadoPlayer.corriendo : estadoPlayer.andando;
        }

        CambioEstado(estadoNuevo);
    }

    private void Animar()
    {
        playerAnimator.SetInteger("estadoPlayer", (int)estado);
    }


    private void VolarSaltarCaminar()
    {
        float velocidadY = ySpeedActual;
         
        if (estado == estadoPlayer.volando && EstaEnSuelo())
        {
            velocidadY = fuerzaSalto;
        }
        else if (ignicion && cantidadCombustibleDeVuelo > 0)
        {
            velocidadY = fuerzaSalto;
            explosionIgnicion.Play();
            sonidoIgnicion.Play();  
            cantidadCombustibleDeVuelo -= 1;
            frontalBarraCombustible.fillAmount = (float)cantidadCombustibleDeVuelo / cantidadDeCombustibleMaxima;
            MostrarCombustible();
        }
        if (Mathf.Abs(zPos) > 0.01f || ignicion) {
            rb.velocity = new Vector3(0, velocidadY, zPos * velocidad);
        }



    }


    private bool EstaEnSuelo()
    {
        bool enSuelo = false;
        float radio = 0.1f;
        Collider[] col = Physics.OverlapSphere(posicionPies.transform.position, radio);
        foreach (Collider c in col)
        {
            if (c != null && c.gameObject.tag.Equals("Suelo"))
            {
                    enSuelo = true;
            }
        }
        return enSuelo;

    }


    private void ActivarTrigger()
    {
        posicionPies.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {    
        if (other.tag.Equals("Suelo"))
        {
            posicionPies.GetComponent<BoxCollider>().enabled = false;
            CambioEstado(estadoPlayer.parado);
        }   
    }

    private void Disparar()
    {
        Transform posicionDisparo = null;
        int gastoEnergiaLaser = 2;

        if (Input.GetKeyDown(KeyCode.S) && energiaArmas  > 0)
        {
            GameObject proyectil;
            posicionDisparo = (estado != estadoPlayer.volando) ? disparoVolando : disparoTierra;
            proyectil = Instantiate(laser, posicionDisparo.transform);
            proyectil.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * fuerzaDisparo);
            energiaArmas -= gastoEnergiaLaser;
            energiaArmas = Mathf.Max(energiaArmas, 0);
            frontalBarraEnergia.fillAmount = (float)energiaArmas / energiaMaximaArmas;
            MostrarEnergia();
            
        }
    }


    public void CambioEstadoEnAgua(estadoPlayer estado)
    {

        if(estado == estadoPlayer.nadando)
        {
            this.estado = estado;
            velocidad /= 2;
            fuerzaSalto /= 2;
        }
        if (this.estado == estadoPlayer.nadando)
        {
            if (estado == estadoPlayer.volando)
            {
                velocidad *= 2;
                fuerzaSalto *= 2;
                this.estado = estado;
                Invoke("ActivarTrigger", tiempoActivarTrigger);
            }
        }
       

        
    }

    private void CambioEstado(estadoPlayer estado)
    {
        if (this.estado != estadoPlayer.nadando && estado != estadoPlayer.nadando && estado != estadoPlayer.sinEstado)
        { 
            this.estado = estado;
        }
        
    }

    public void Inmunizar(float tiempo)
    {
        inmune = true; ;
        Invoke("QuitarInmunidad", tiempo);
    }

    private void QuitarInmunidad()
    {
        inmune = false;
    }

    public void RecibirDanyo(int danyo)
    {
        if(!inmune) {
            salud -= danyo;
            if (salud < 0) {
                salud += saludMaxima;
                ModificarVida(false);
            }
            frontalBarraVida.fillAmount = (float)salud / saludMaxima;
            MostrarVida();
        }
        
    }

    public void Sanar(int sanacion)
    {
        salud += sanacion;
        if(salud > saludMaxima)
        {
            salud -= saludMaxima;
            ModificarVida(true);
        }
        frontalBarraVida.fillAmount = (float)salud / saludMaxima;
        MostrarVida();
    }

    

    public void ModificarVida(bool aumentar)
    {
        VidasScript vs = FindObjectOfType<VidasScript>();
        if (aumentar)
        {
            Mathf.Min(++vidas, vidasMaximas);
            vs.SumarVida();
        }
        else
        {
            Mathf.Max(--vidas, 0);
            vs.RestarVida();
            if (vidas == 0)
            {
                Morir();
            }
        }



    }

    public void IncrementarCombustible(int cantidadCombustible)
    {
        cantidadCombustibleDeVuelo += cantidadCombustible;
        cantidadCombustibleDeVuelo = Mathf.Min(cantidadCombustible, cantidadDeCombustibleMaxima);
        frontalBarraCombustible.fillAmount = (float)cantidadCombustibleDeVuelo / cantidadDeCombustibleMaxima;
        MostrarCombustible();

    }

    public void IncrementarEnergiaArmas(int cantidadEnergia) {
        energiaArmas += cantidadEnergia;
        energiaArmas = Mathf.Min(energiaArmas, energiaMaximaArmas);
        frontalBarraEnergia.fillAmount = (float)energiaArmas / energiaMaximaArmas;
        MostrarEnergia();
    }

    public void incrementarPuntuacion()
    {
        puntuacion++;
        puntuacionTxt.text = puntuacion.ToString();
        GameConfig.StorePuntuacion(SceneManager.GetActiveScene().buildIndex, puntuacion);
    }

    public int getPuntuacion()
    {
        return puntuacion;
    }

    public int getVidasMaximas()
    {
        return vidasMaximas;
    }

    public int getVidas()
    {
        return vidas;
    }


    private void MostrarVida() {
        int tiempoMostrandoBarraVida = 4;
        CambiarMostrarVida();
        Invoke("CambiarMostrarVida", tiempoMostrandoBarraVida);
    }

    private void OcultarBarraDeVida()
    {
        float velocidadTransparente = 0.01f;
        if (barraVida.alpha > 0)
        {
            barraVida.alpha -= velocidadTransparente;
        }
        
    }

    private void MostrarBarraDeVida()
    {
        if (mostrarVida) {
            barraVida.alpha = 1;
        } else {
            OcultarBarraDeVida();
        }
    }

    private void CambiarMostrarVida()
    {
        mostrarVida = !mostrarVida;
    }


    private void MostrarCombustible() {
        int tiempoMostrandoBarraCombustible = 4;
        CambiarMostrarCombustible();
        Invoke("CambiarMostrarCombustible", tiempoMostrandoBarraCombustible);
    }


    private void OcultarBarraDeCombustible() {
        float velocidadTransparente = 0.01f;
        if (barraCombustible.alpha > 0) {
            barraCombustible.alpha -= velocidadTransparente;
        }

    }

    private void MostrarBarraDeCombustible() { 
        if (mostrarCombustible) {
            barraCombustible.alpha = 1;
        } else {
            OcultarBarraDeCombustible();
        }
    }

    private void CambiarMostrarCombustible() {
        mostrarCombustible = !mostrarCombustible;
    }


    private void MostrarEnergia() {
        int tiempoMostrandoBarraEnergia = 4;
        CambiarMostrarEnergia();
        Invoke("CambiarMostrarEnergia", tiempoMostrandoBarraEnergia);
    }

    private void OcultarBarraDeEnergia() {
        float velocidadTransparente = 0.01f;
        if (barraEnergia.alpha > 0) {
            barraEnergia.alpha -= velocidadTransparente;
        }

    }

    private void MostrarBarraDeEnergia() {
        
        if (mostrarEnergia) {
            barraEnergia.alpha = 1;
        } else {
            OcultarBarraDeEnergia();
        }

    }

    private void CambiarMostrarEnergia() {
        mostrarEnergia = !mostrarEnergia;
    }

    public void Morir() {
        GameConfig.StorePuntuacion(SceneManager.GetActiveScene().buildIndex, puntuacion);
        gameOverMenu.SetActive(true);
        estado = estadoPlayer.muerto;
    }

    public void setPosicion(Vector3 posicion)
    {
        this.transform.position = posicion;
    }

    private void SalirAMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
