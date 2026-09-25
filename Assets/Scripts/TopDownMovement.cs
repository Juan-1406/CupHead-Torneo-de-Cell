using UnityEngine;
using System.Collections; // Súper necesario para las corrutinas (el tiempo del dash)

public class TopDownMovement : MonoBehaviour
{
    [Header("Movimiento Base")]
    public float speed = 8f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Camera mainCamera;

    [Header("Mecánica de Dash")]
    public float velocidadDash = 25f; // Fuerza del impulso
    public float tiempoDash = 0.15f;  // Duración del impulso (i-frames)
    public float cooldownDash = 1f;   // Tiempo de espera para volver a usarlo
    public bool haciendoDash = false; // Público para saber si somos invulnerables
    private bool puedeHacerDash = true;
    private Vector2 direccionDash;    // Guarda hacia dónde apuntamos al hacer dash

    [Header("Disparo")]
    public GameObject balaPrefab; 
    public float fuerzaDisparo = 15f;

    [Header("Parry y Escudo")]
    public GameObject escudoVisual;
    private bool estaHaciendoParry = false;
    private float tiempoParry = 0.2f;

    // Referencia al sistema de vida
    private PlayerHealth sistemaSalud;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        sistemaSalud = GetComponent<PlayerHealth>();

        if (escudoVisual != null) escudoVisual.SetActive(false);
    }

    void Update()
    {
        // 1. Si estamos a mitad de un dash, bloqueamos disparos, parry y cambio de dirección
        if (haciendoDash) return;

        // 2. Capturamos hacia dónde nos queremos mover
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        // 3. Detectar si presionamos ESPACIO para el Dash
        if (Input.GetKeyDown(KeyCode.Space) && puedeHacerDash && movement != Vector2.zero)
        {
            StartCoroutine(EjecutarDash());
        }

        // 4. Controles de Disparo y Parry
        if (Input.GetMouseButtonDown(0)) DispararHaciaMouse();
        if (Input.GetMouseButtonDown(1)) HacerParry();
    }

    void FixedUpdate()
    {
        // Aplicamos físicas: si hacemos dash vamos a toda velocidad, sino, velocidad normal
        if (haciendoDash)
        {
            rb.linearVelocity = direccionDash * velocidadDash;
        }
        else
        {
            rb.linearVelocity = movement * speed;
        }
    }

    // La Corrutina que controla los tiempos exactos del Dash
    private IEnumerator EjecutarDash()
    {
        puedeHacerDash = false;
        haciendoDash = true;
        
        // Guardamos la dirección para no poder doblar en medio del impulso
        direccionDash = movement;

        // Esperamos que termine el tiempo de impulso (y de invulnerabilidad)
        yield return new WaitForSeconds(tiempoDash);
        
        haciendoDash = false;

        // Esperamos a que se enfríe la habilidad para volver a usarla
        yield return new WaitForSeconds(cooldownDash);
        puedeHacerDash = true;
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
            if (escudoVisual != null) escudoVisual.SetActive(true);
            Invoke("TerminarParry", tiempoParry);
        }
    }

    void TerminarParry()
    {
        estaHaciendoParry = false;
        if (escudoVisual != null) escudoVisual.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (haciendoDash) return;

        if (collision.gameObject.CompareTag("BalaEnemiga"))
        {
            // ESTA LÍNEA TE DIRÁ EXACTAMENTE QUÉ TE PEGÓ EN LA CONSOLA
            Debug.Log("¡Me acaba de golpear un objeto llamado: " + collision.gameObject.name);

            if (estaHaciendoParry)
            {
                Debug.Log("¡PARRY EXITOSO! Bala rebotada.");
                Rigidbody2D rbBalaEnemiga = collision.GetComponent<Rigidbody2D>();
                rbBalaEnemiga.linearVelocity = -rbBalaEnemiga.linearVelocity * 2f; 
                collision.gameObject.tag = "BalaJugador"; 
            }
            else
            {
                if (sistemaSalud != null)
                {
                    sistemaSalud.RecibirDano(1);
                }
                Destroy(collision.gameObject);
            }
        }
    }
}