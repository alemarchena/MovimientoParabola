using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilizaClic : MonoBehaviour {

    [HideInInspector] public Vector3 objetivo;
    [HideInInspector] public Ray RayoObjetivo;

    public void VerificaPosicionClicMouse()
    {

        RayoObjetivo = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit resultado;

        Physics.Raycast(RayoObjetivo, out resultado, 100f);
        objetivo = resultado.point;

    }
}
