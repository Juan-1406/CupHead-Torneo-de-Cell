using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int saludMaxima = 3;
    private int saludActual;
    
    public Slider healthSlider;
    private Animator anim;

    void Start()
    {
        saludActual = saludMaxima;
        anim = GetComponent<Animator>();
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = saludMaxima;
            healthSlider.value = saludActual;
        }
    }

    public void RecibirDano(int cantidad)
    {
        saludActual -= cantidad;
        
        if (healthSlider != null) 
        {
            healthSlider.value = saludActual;
        }

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        // Desactiva las colisiones
        GetComponent<Collider2D>().enabled = false;
        
        // Apaga el script del dash y movimiento para que Mateo quede quieto
        if (GetComponent<TopDownMovement>() != null)
        {
            GetComponent<TopDownMovement>().enabled = false; 
        }

        // Activa la animación si existe
        if (anim != null)
        {
            anim.SetTrigger("Derrota");
        }

        // Le avisa al GameManager (que tiene el Singleton) que perdimos
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PerderVida();
        }
    }
}