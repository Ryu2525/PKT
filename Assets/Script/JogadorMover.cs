using UnityEngine;

public class JogadorMover : MonoBehaviour
{
    public float speed = 4f; // velocidade do jogador
    public Vector3 destino;  // ponto final no campo

    void Update()
    {
        // move o jogador até o destino
        transform.position = Vector3.MoveTowards(
            transform.position,
            destino,
            speed * Time.deltaTime
        );

        // opcional: parar quando chegar no destino
        if (Vector3.Distance(transform.position, destino) < 0.1f)
        {
            // pode adicionar animação de parada ou outra lógica
            Debug.Log("Jogador chegou ao destino!");
        }
    }
}
