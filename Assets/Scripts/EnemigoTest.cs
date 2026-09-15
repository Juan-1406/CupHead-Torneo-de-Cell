using UnityEngine;

public class EnemigoTest : MonoBehaviour
{
    public GameObject balaEnemigaPrefab;
    public float fuerzaDisparo = 5f;
    private Transform jugador;

    void Start()
    {
        // Busca automáticamente al objeto que tenga el tag "Player"
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Repite la función Disparar cada 2 segundos
        InvokeRepeating("Disparar", 1f, 2f); 
    }

    void Disparar()
    {
        if (jugador != null)
        {
            GameObject bala = Instantiate(balaEnemigaPrefab, transform.position, Quaternion.identity);
            Vector2 direccion = (jugador.position - transform.position).normalized;
            bala.GetComponent<Rigidbody2D>().linearVelocity = direccion * fuerzaDisparo;
            Destroy(bala, 3f);
        }
    }
}
