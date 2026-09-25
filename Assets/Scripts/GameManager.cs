using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public static int vidasExtras = 3; 
    
    [Header("Interfaz de Usuario")]
    public TextMeshProUGUI textoVidasHUD;            
    public GameObject pantallaMuerte;      
    public TextMeshProUGUI textoVidasPantallaMuerte;  

    // Variable para saber si perdimos definitivamente
    private bool juegoTerminado = false; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f; 
        juegoTerminado = false; // Al iniciar la escena, el juego no ha terminado
        
        ActualizarHUD();
        
        if (pantallaMuerte != null)
        {
            pantallaMuerte.SetActive(false); 
        }
    }

    void Update()
    {
        // TRUCO PARA PRESENTACIONES: Si hay Game Over y presionas 'R', se reinicia todo el juego
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            vidasExtras = 5; // Restauramos las vidas iniciales
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void PerderVida()
    {
        if (vidasExtras > 0)
        {
            vidasExtras--; // Solo restamos si tenemos más de 0 vidas
            MostrarPantallaMuerte();
            Invoke("ReiniciarRonda", 3f); 
        }
        else
        {
            // Si ya estábamos en 0 y morimos, es Game Over directo sin restar a -1
            vidasExtras = 0; 
            ActualizarHUD();
            EjecutarGameOver();
        }
    }

    private void MostrarPantallaMuerte()
    {
        if (pantallaMuerte != null)
        {
            pantallaMuerte.SetActive(true); 
            if (textoVidasPantallaMuerte != null)
            {
                textoVidasPantallaMuerte.text = "Vidas restantes: " + vidasExtras;
            }
        }
    }

    private void ReiniciarRonda()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EjecutarGameOver()
{
    juegoTerminado = true;
    
    if (pantallaMuerte != null)
    {
        pantallaMuerte.SetActive(true);
        if (textoVidasPantallaMuerte != null)
        {
            // El jugador solo verá el Game Over. La tecla 'R' queda como truco oculto.
            textoVidasPantallaMuerte.text = "¡GAME OVER DEFINITIVO!";
        }
    }
    
    Time.timeScale = 0f; 
    }

    private void ActualizarHUD()
    {
        if (textoVidasHUD != null)
        {
            // Usamos Mathf.Max para asegurarnos de que JAMÁS se dibuje un número menor a 0 en el texto
            textoVidasHUD.text = "x " + Mathf.Max(0, vidasExtras);
        }
    }
}