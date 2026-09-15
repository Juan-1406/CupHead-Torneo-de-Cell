using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    // Variables que podrás modificar desde el Inspector en Unity
    public float speed = 8f;
    
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        // Conectamos el script con el Rigidbody2D del jugador
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Detectamos el movimiento en los ejes X (izquierda/derecha) e Y (arriba/abajo)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalizamos el vector para que no se mueva más rápido al ir en diagonal
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Aplicamos la velocidad al personaje
        rb.linearVelocity = movement * speed;
    }
}