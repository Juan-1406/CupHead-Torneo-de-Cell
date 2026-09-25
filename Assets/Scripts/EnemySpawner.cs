using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public float tiempoEntreSpawns = 3f; // Aparece un enemigo cada 3 segundos
    
    // Lista de lugares donde pueden aparecer los enemigos
    public Transform[] puntosDeSpawn;

    void Start()
    {
        // Comienza a generar enemigos continuamente
        InvokeRepeating("SpawnearEnemigo", 2f, tiempoEntreSpawns);
    }

    void SpawnearEnemigo()
    {
        // Verifica que haya puntos de spawn configurados
        if (puntosDeSpawn.Length > 0 && enemigoPrefab != null)
        {
            // Elige un punto al azar de la lista
            int indiceAleatorio = Random.Range(0, puntosDeSpawn.Length);
            Transform puntoElegido = puntosDeSpawn[indiceAleatorio];

            // Crea el enemigo en ese punto
            Instantiate(enemigoPrefab, puntoElegido.position, Quaternion.identity);
        }
    }
}