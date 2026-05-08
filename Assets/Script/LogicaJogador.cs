using UnityEngine;

public class LogicaJogador : MonoBehaviour
{
    [Header("Posição Inicial do Jogador")]
    public Vector2 posicaoInicial = new Vector2(0f, 0f);

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ao iniciar, coloca o jogador na posição inicial definida
        transform.position = posicaoInicial;
    }

    public void ResetarPosicao()
    {
        transform.position = posicaoInicial;

        // Se tiver Rigidbody2D, zera a velocidade
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
