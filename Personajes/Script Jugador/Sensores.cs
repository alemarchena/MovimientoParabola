using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensores : MonoBehaviour {

    [Header("Sensor Obstaculos")]
    public float visionAdelante = 4f;
    public float disObAdelante;
    public string nombreObsAdelante;
    public Transform sensorAdelante;

    public float visionAtras = 4f;
    public float disObAtras;
    public string nombreObsAtras;
    public Transform sensorAtras;

    public float visionDerecha = 4f;
    public float disObDerecha;
    public string nombreObsDerecha;
    public Transform sensorDerecha;

    public float visionIzquierda = 4f;
    public float disObIzquierda;
    public string nombreObsIzquierda;
    public Transform sensorIzquierda;

    [HideInInspector] public float distanciaAdelante;
    [HideInInspector] public float distanciaAtras;
    [HideInInspector] public float distanciaDerecha;
    [HideInInspector] public float distanciaIzquierda;
    [HideInInspector] public Ray rayoAdelante;
    [HideInInspector] public Ray rayoAtras;
    [HideInInspector] public Ray rayoDerecha;
    [HideInInspector] public Ray rayoIzquierda;

    public void DetectaEntorno()
    {

        Vector3 PosicionJugador = gameObject.transform.position;

        //Rayo al Adelante
        Vector3 sensorAde = sensorAdelante.transform.position; //transform.forward * visionAdelante;
        Vector3 direccionAde = sensorAde - PosicionJugador;
        distanciaAdelante = Vector3.Distance(PosicionJugador, sensorAde);
        rayoAdelante = new Ray(PosicionJugador, sensorAde);
        Debug.DrawRay(PosicionJugador, direccionAde * 1.0f, Color.black);
        
        //Rayo al Atras
        Vector3 sensorAtr = sensorAtras.transform.position; //-transform.forward * visionAtras;
        Vector3 direccionAtr = sensorAtr - PosicionJugador;
        distanciaAtras = Vector3.Distance(PosicionJugador, sensorAtr);
        rayoAtras = new Ray(PosicionJugador, sensorAtr);
        Debug.DrawRay(PosicionJugador, direccionAtr * 1.0f, Color.blue);

        //Rayo a la Derecha
        Vector3 sensorDer = sensorDerecha.transform.position; //transform.right * visionDerecha;
        Vector3 direccionDer = sensorDer - PosicionJugador;
        distanciaDerecha = Vector3.Distance(PosicionJugador, sensorDer);
        rayoDerecha = new Ray(PosicionJugador, sensorDer);
        Debug.DrawRay(PosicionJugador, direccionDer * 1.0f, Color.green);

        //Rayo a la Izquierda
        Vector3 sensorIzq = sensorIzquierda.transform.position; //transform.right * -visionIzquierda;
        Vector3 direccionIzq = sensorIzq - PosicionJugador;
        distanciaIzquierda = Vector3.Distance(PosicionJugador, sensorIzq);
        rayoIzquierda = new Ray(PosicionJugador, sensorIzq);
        Debug.DrawRay(PosicionJugador, direccionIzq * 1.0f, Color.gray);

        //Sensor de choque de Obstaculos Adelante
        RaycastHit[] hitAdelante;
        hitAdelante = Physics.RaycastAll(rayoAdelante, visionAdelante);
        disObAdelante = 0;
        nombreObsAdelante = string.Empty;

        if (hitAdelante.Length > 0)
        {

            foreach (RaycastHit f in hitAdelante)
            {
                GameObject obstaculoHitAdelante = f.transform.gameObject;
                Vector3 obstaculoAde = obstaculoHitAdelante.transform.position;
                Vector3 direccionObstaculoAdelante = obstaculoAde - PosicionJugador;
                disObAdelante = Vector3.Distance(PosicionJugador, obstaculoAde);
                nombreObsAdelante = f.transform.gameObject.name;
                Debug.DrawRay(rayoAdelante.origin, direccionObstaculoAdelante * 1.0f, Color.red);
                break;
            }
        }


        //Sensor de choque de Obstaculos Atras
        RaycastHit[] hitAtras;
        hitAtras = Physics.RaycastAll(rayoAtras, visionAtras);
        disObAtras = 0;
        nombreObsAtras = string.Empty;

        if (hitAtras.Length > 0)
        {

            foreach (RaycastHit a in hitAtras)
            {
                GameObject obstaculoHitAtras = a.transform.gameObject;
                Vector3 obstaculoAtr = obstaculoHitAtras.transform.position;
                Vector3 direccionObstaculoAtras = obstaculoAtr - PosicionJugador;
                disObAtras = Vector3.Distance(PosicionJugador, obstaculoAtr);
                nombreObsAtras = a.transform.gameObject.name;
                Debug.DrawRay(rayoAtras.origin, direccionObstaculoAtras * 1.0f, Color.red);
                break;
            }
        }

        //Sensor de choque de Obstaculos Derecha
        RaycastHit[] hitDerecha;
        hitDerecha = Physics.RaycastAll(rayoDerecha, visionDerecha);
        disObDerecha = 0;
        nombreObsDerecha = string.Empty;

        if (hitDerecha.Length > 0)
        {

            foreach (RaycastHit d in hitDerecha)
            {
                GameObject obstaculoHitDerecha = d.transform.gameObject;
                Vector3 obstaculoDer = obstaculoHitDerecha.transform.position;
                Vector3 direccionObstaculoDerecha = obstaculoDer - PosicionJugador;
                disObDerecha = Vector3.Distance(PosicionJugador, obstaculoDer);
                nombreObsDerecha = d.transform.gameObject.name;
                Debug.DrawRay(rayoDerecha.origin, direccionObstaculoDerecha * 1.0f, Color.red);
                break;
            }
        }

        //Sensor de choque de Obstaculos Izquierda
        RaycastHit[] hitIzquierda;
        hitIzquierda = Physics.RaycastAll(rayoIzquierda, visionIzquierda);
        disObIzquierda = 0;
        nombreObsIzquierda = string.Empty;

        if (hitIzquierda.Length > 0)
        {
            foreach (RaycastHit i in hitIzquierda)
            {
                GameObject obstaculoHitIzquierda = i.transform.gameObject;
                Vector3 obstaculoIzq = obstaculoHitIzquierda.transform.position;
                Vector3 direccionObstaculoIzquierda = obstaculoIzq - PosicionJugador;
                disObIzquierda = Vector3.Distance(PosicionJugador, obstaculoIzq);
                nombreObsIzquierda = i.transform.gameObject.name;
                Debug.DrawRay(rayoIzquierda.origin, direccionObstaculoIzquierda * 1.0f, Color.red);
                break;
            }
        }
    }
}
