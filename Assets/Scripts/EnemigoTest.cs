using UnityEngine;

public class EnemigoTest : MonoBehaviour
{
    [Header("Salud")]
    public int saludMaxima = 3; // Cuántos tiros aguanta el enemigo
    private int saludActual;

    [Header("Movimiento")]
    public float velocidadMovimiento = 2.5f;
    
    [Header("Disparo")]
    public GameObject balaEnemigaPrefab;
    public float fuerzaDisparo = 5f;
    
    private Transform jugador;

    void Start()
    {
        saludActual = saludMaxima; // Inicia con la vida al máximo

        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
        
        InvokeRepeating("Disparar", 1f, 2f); 
    }

    void Update()
    {
        if (jugador != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidadMovimiento * Time.deltaTime);
        }
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

    // Unity llama a esta función automáticamente cuando otro objeto con "Collider" lo toca
    private void OnTriggerEnter2D(Collider2D collision)
{
    // Esto imprimirá en la consola cualquier cosa que lo toque
    Debug.Log("¡Auch! Me tocó un objeto llamado: " + collision.gameObject.name + " con el tag: " + collision.gameObject.tag);
    
    if (collision.gameObject.CompareTag("BalaJugador"))
    {
        RecibirDano(1);
        Destroy(collision.gameObject);
    }
}

    public void RecibirDano(int cantidad)
    {
        saludActual -= cantidad;
        
        if (saludActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        // Destruye al enemigo de la pantalla
        Destroy(gameObject); 
    }
}