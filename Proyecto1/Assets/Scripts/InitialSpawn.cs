using UnityEngine;

public class InitialSpawn : MonoBehaviour
{
    private void Start()
    {
        // Buscar al jugador en la escena mediante la etiqueta oficial
        GameObject player = GameObject.FindWithTag("Player");

        // Mover al jugador a la posición de este punto al cargar la escena
        if (player != null)
        {
            player.transform.position = transform.position;
        }
    }
}