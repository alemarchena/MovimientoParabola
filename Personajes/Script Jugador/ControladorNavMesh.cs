
using UnityEngine;
using UnityEngine.AI;

//BRINDA LOS METODOS PARA CONTROLAR EL MOVIMIENTO DE UN OBJETO CON NAVMESH
public class ControladorNavMesh : MonoBehaviour
{

    [HideInInspector]
    public Transform perseguirObjetivo;
    public NavMeshAgent agente;


    public void ActualizarPuntoDestinoNav(Vector3 puntoDestino)
    {
        agente.destination = puntoDestino;//Determina el destino del NavMesh
        agente.isStopped = false;
    }

    public void ActualizarPuntoDestinoNav()
    {
        ActualizarPuntoDestinoNav(perseguirObjetivo.position); //Actualiza la ruta del NavMesh
    }

    public void DetenerNav()
    {
        agente.isStopped = true; //Detiene la navegaci�n del NavMesh 
    }

    public bool LlegoAlDestino()
    {
        return agente.remainingDistance <= agente.stoppingDistance && !agente.pathPending; //Verifica si se lleg� al objetivo
    }

    public void ResetearNav()
    {
        agente.ResetPath(); 
    }
}