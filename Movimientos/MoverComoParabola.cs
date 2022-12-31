using UnityEngine;

public class MoverComoParabola : MonoBehaviour {

    /// <summary>
    /// Velocidad inicial en X
    /// </summary>
    [SerializeField]
    float velIniX;

    /// <summary>
    /// Velocidad Inicial en Y
    /// </summary>
    [SerializeField]
    [Tooltip("Velocidad inicial en Y")]
    float velIniY;

    /// <summary>
    /// Angulo de la parabola
    /// </summary>
    [SerializeField]
    [Tooltip("Angulo de la Parabola")]
    float angulo;

    /// <summary>
    /// Gravedad
    /// </summary>
    [SerializeField]
    [Tooltip("Gravedad")]
    float gravedad;

    /// <summary>
    /// La posicion en X esta definida : Velocidad x Tiempo ya que la velocidad es constante en X
    /// </summary>
    [SerializeField]
    float tiempo;

    public bool seMueve = false;

    private Vector3 vec;
    private float yMax;
    private float xMax;
    private float tiempoVuelo;


    private void Start()
    {
        //altura maxima en Y
        yMax = (((velIniY * Mathf.Sin(angulo)) * (velIniY * Mathf.Sin(angulo))) / 2 * gravedad) + transform.position.y;
        
        Debug.Log("Altura Max en Y:" + yMax + " mtr/seg");

        //tiempo de vuelo del objeto expresado en seg
        tiempoVuelo = ((2 * velIniX) * Mathf.Sin(angulo)) / gravedad;
        Debug.Log("Tiempo de vuelo:" + tiempoVuelo + " mtr/seg");

        //alvance max en x del objeto expresado en mtr
        xMax = ((velIniX * velIniX) * Mathf.Sin( 2 * angulo) )/ gravedad + transform.position.x;
        Debug.Log("Distancia Max en X" + xMax + " mtr/seg");

    }
    private void Update()
    {
        Mueve();
	}

    private void Mueve()
    {
        if (seMueve) { 
            velIniY = velIniY - gravedad;
            float px = transform.position.x + (velIniX * Mathf.Cos(angulo) * tiempo);
            float py = transform.position.y + (-1/2 * gravedad * (tiempo * tiempo)) + velIniY * Mathf.Sin(angulo) * tiempo;

            vec = new Vector3(px, py, 0);
            transform.SetPositionAndRotation(vec, Quaternion.identity);

            if (py >= yMax)
            {
                Debug.Log("Altura:" + py + ", velocidad:" + velIniY);
            }
            if (py <= 0) {
                seMueve = false;
            }
        }
    }
}
