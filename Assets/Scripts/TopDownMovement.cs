using UnityEngine;
using UnityEngine.UI; // Necesario para controlar la barra de vida

public class TopDownMovement : MonoBehaviour
{
    public float speed = 8f;
    
    // --- DISPARO ---
    public GameObject balaPrefab; 
    public float fuerzaDisparo = 15f;

    // --- SISTEMA DE VIDA ---
    public int vidaMaxima = 3;
    private int vidaActual;
    public Slider barraDeVida; // Referencia a nuestra UI

    // --- PARRY Y ESCUDO ---
    public GameObject escudoVisual; // Referencia al círculo opaco
    private bool estaHaciendoParry = false;
    private float tiempoParry = 0.2f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        // Configuramos la vida al máximo
        vidaActual = vidaMaxima; 
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaActual;
        }

        // Aseguramos que el escudo empiece apagado
        if (escudoVisual != null) escudoVisual.SetActive(false);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (Input.GetMouseButtonDown(0)) DispararHaciaMouse();
        if (Input.GetMouseButtonDown(1)) HacerParry();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    void DispararHaciaMouse()
    {
        Vector3 posicionMouse = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = (posicionMouse - transform.position).normalized;

        GameObject nuevaBala = Instantiate(balaPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rbBala = nuevaBala.GetComponent<Rigidbody2D>();
        rbBala.linearVelocity = direccion * fuerzaDisparo;
        Destroy(nuevaBala, 2f);
    }

    void HacerParry()
    {
        if (!estaHaciendoParry)
        {
            estaHaciendoParry = true;
            if (escudoVisual != null) escudoVisual.SetActive(true); // Encendemos el escudo
            Invoke("TerminarParry", tiempoParry);
        }
    }

    void TerminarParry()
    {
        estaHaciendoParry = false;
        if (escudoVisual != null) escudoVisual.SetActive(false); // Apagamos el escudo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BalaEnemiga"))
        {
            if (estaHaciendoParry)
            {
                Debug.Log("¡PARRY EXITOSO! Bala rebotada.");
                
                // Magia pura: Invertimos la velocidad de la bala y la aceleramos al doble
                Rigidbody2D rbBalaEnemiga = collision.GetComponent<Rigidbody2D>();
                rbBalaEnemiga.linearVelocity = -rbBalaEnemiga.linearVelocity * 2f; 
                
                // Le quitamos la etiqueta para que no nos vuelva a pegar si rebota de nuevo
                collision.gameObject.tag = "Untagged"; 
            }
            else
            {
                vidaActual--; 
                if (barraDeVida != null) barraDeVida.value = vidaActual; // Actualizamos la UI
                
                Destroy(collision.gameObject);

                if (vidaActual <= 0)
                {
                    Debug.Log("¡GAME OVER!");
                    gameObject.SetActive(false); 
                }
            }
        }
    }
}