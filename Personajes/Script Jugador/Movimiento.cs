using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//[System.Serializable]
public class Movimiento : MonoBehaviour {

    //Enum
    public enum FormaDeMover { Directo, InteligenciaArtifical, Ninguno };
    public enum EventoDeMover { Clic,Llamado};
    //Publicas
    [Header("Configuracion de Movimiento")]
    public FormaDeMover formaDeMover;
    public EventoDeMover eventoDeMover;

    [Header("Movimiento Directo")]
    public float velocidadMovimiento = 7f;
    public float velocidadMinima = 1f;
    public float velocidadMaxima = 20;
    public float velocidadRotacion = 5f;
    public float distanciaParada = 1.5f;
    public bool tieneRotacion = true;

    [Header("Paradas")]
    public GameObject[] paradas;
    public Transform objetoDestino;

    private bool activoNav = false;

    //Variables que obtienen el componente agregado al personaje
    [Header("Requisitos Funcionalidad")]
    public ControladorNavMesh CNcontrolaNav;
    public UtilizaClic CNutilizaClic;
    public Sensores CNsensores;

    //Privadas
    private int paradaActual = 0;

    private void Awake()
    {
        if (CNcontrolaNav != null)//Verifica que tenga un objeto NavMesh
            CNcontrolaNav = GetComponent<ControladorNavMesh>();

        if (CNutilizaClic != null)//Verifica que tenga un objeto de verificacion de clic de mouse
            CNutilizaClic = GetComponent<UtilizaClic>();

        if (CNsensores != null)//Verifica que tenga un objeto de sensores
            CNsensores = GetComponent<Sensores>();

    }
    private void Update()
    {
        if (CNsensores != null)
            CNsensores.DetectaEntorno();      //Detecta todos los objetos que estan cerca y calcula las distancias

        if (eventoDeMover == EventoDeMover.Clic) { 
            if (Input.GetMouseButton(0)) {      //si presiono boton derecho de mouse se empieza a mover
                paradaActual = 0;               //inicializa la parada actual

                CNutilizaClic.VerificaPosicionClicMouse();     //captura la posicion del clic de mouse en cualquier objeto

                if (formaDeMover == FormaDeMover.Directo) // MOVIMIENTO **SIN** INTELIGENCIA ARTIFICIAL
                {
                    StopAllCoroutines();        //detiene las corrutinas para comenzar el posible movimiento
                        if (paradas.Length > 0)
                        {
                            Movete(paradas[paradaActual].transform.position);//si tiene paradas hace la ruta.
                        } 
                        else {
                            Movete(CNutilizaClic.objetivo);     //sino tiene paradas mueve hacia adonde se hizo clic
                        }
                }
                if (CNcontrolaNav != null)
                {
                    activoNav = true;
                    CNcontrolaNav.ResetearNav();
                }
            }
        
            if (CNcontrolaNav != null)
            {
                if (formaDeMover == FormaDeMover.InteligenciaArtifical && activoNav == true) // MOVIMIENTO **CON** INTELIGENCIA ARTIFICIAL
                {
                    if (CNcontrolaNav.LlegoAlDestino())//Verificac si el NavMeshAgent llego al destino
                    {
                        if (paradas.Length > 0)
                        {
                            if (paradaActual < paradas.Length)
                            { //Mueve el personaje hacia cada parada establecida
                                CNcontrolaNav.ActualizarPuntoDestinoNav(paradas[paradaActual].transform.position);
                            }
                            else if (paradas.Length > 0 && paradaActual == paradas.Length)
                            { //cuando termina las paradas va hacia el punto clickeado con el mouse
                                CNcontrolaNav.ActualizarPuntoDestinoNav(CNutilizaClic.objetivo);
                            }
                        }
                        else
                        {   //Si no tiene paradas va directamente al punto clickeado con el mouse
                            if (CNutilizaClic != null)
                                CNcontrolaNav.ActualizarPuntoDestinoNav(CNutilizaClic.objetivo);
                        }

                        paradaActual++;

                        if (paradaActual > paradas.Length && CNcontrolaNav.LlegoAlDestino())
                        { //detiene al personaje cuando hizo todas las paradas inclusive el clic de mouse
                            CNcontrolaNav.DetenerNav(); paradaActual = 0; activoNav = false;
                        }
                    }
                }
            }
        }//Fin Evento de Mover con clic de mouse

        if (eventoDeMover == EventoDeMover.Llamado) {
            if (objetoDestino != null) { 
                
                Movete(objetoDestino.position); //Es mover el objeto actual hacia otro objeto en forma automatica ya que no usa clic de mouse
                
            }
        }
    }



    //Inicia la corrutina de movimiento SIN utilizar NavMesh
    private void Movete(Vector3 proximoDestino) {
        StartCoroutine(MostrameElMovimiento(proximoDestino));
    }

    IEnumerator MostrameElMovimiento(Vector3 proximoDestino) {
        while (true)
        {
            Vector3 personaje = gameObject.transform.position;  //guarda la posicion actual del personaje
            Vector3 destino = proximoDestino;                   //guarda la posicion de la proxima parada
            Vector3 direccionRelativa = destino - personaje;    //calcula la direccion desde el personaje a la parada

            float distanciaRelativa = Vector3.Distance(personaje, destino); //calcula la distancia entre el personaje y la proxima parada
            float distanciaInicial = distanciaRelativa;         //guarda la distancia inicial

            Ray ray = new Ray(personaje, destino);              //lanza un rayo entre el personaje y la proxima parada

            float porcentaje = (distanciaRelativa * 100) / distanciaInicial;    //convierte la distancia en un porcentaje

            //ROTANDO
            if (tieneRotacion)
            {
                gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccionRelativa), velocidadRotacion * Time.deltaTime);
                //MOVIENDO
                gameObject.transform.position += gameObject.transform.forward * velocidadMovimiento * Time.deltaTime; //mueve al personaje
            }
            else {
                //MOVIENDO SIN ROTACION
               

                Debug.Log("Z:" + transform.position +",Destino Z:" + destino);
                gameObject.transform.SetPositionAndRotation(destino, Quaternion.identity);
                

            }

            //modifico la velocidad segun el porcentaje de distancia hacia el objetivo
            if (porcentaje < 120 && porcentaje > 60 && velocidadMovimiento < velocidadMaxima)
                velocidadMovimiento += 10f * Time.deltaTime;

            if (porcentaje < 60 && porcentaje > 0 && velocidadMovimiento > velocidadMinima)
                velocidadMovimiento -= 10f * Time.deltaTime;

            Debug.DrawRay(ray.origin, direccionRelativa * 1.0f, Color.green);   //dibuja el rayo en el modo de edicion
            //Debug.Log(distanciaRelativa);                       //muestra la distancia relativa en la consola

            if (distanciaRelativa < distanciaParada) {          //detiene al personaje cuando llega a la parada (con una infima diferencia de distancia)
                paradaActual += 1;                              //aumenta el contador de paradas
                break;
            }
            yield return null;                                  //la corrutina se detiene y prepara todo para el proximo fotograma
        }

        if (paradaActual <= paradas.Length)       //Una vez terminada la corrutina ejecuta estas lineas
        {     //hace tantas auto llamadas a si misma como paradas tenga por hacer el personaje
            Movete(paradas[paradaActual - 1].transform.position);           //envia el proximo destino
        }
        else {
            if (paradas.Length > 0 && paradaActual == paradas.Length + 1)
            {
                Movete(CNutilizaClic.objetivo);
            }
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (eventoDeMover == EventoDeMover.Llamado) { 
            Destroy(other.gameObject);
            StopAllCoroutines();
            eventoDeMover = EventoDeMover.Clic;
        }
    }   
}
