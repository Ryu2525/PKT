using UnityEngine;

public class MovimentoInimigo : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2.5f;
    public float distanciaMovimento = 4f;
    public bool moverNaHorizontal = true;
    public bool inverterSprite = true; // Se deve girar o boneco ao mudar de direção

    private Vector2 posicaoInicial;
    private SpriteRenderer spriteRenderer;
    private float posicaoAnterior;

    void Start()
    {
        posicaoInicial = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        posicaoAnterior = moverNaHorizontal ? transform.position.x : transform.position.y;
    }

    void Update()
    {
        // Calcula o movimento de vai-e-vem
        float movimento = Mathf.PingPong(Time.time * velocidade, distanciaMovimento);
        float offset = movimento - (distanciaMovimento / 2f);

        if (moverNaHorizontal)
        {
            transform.position = new Vector2(posicaoInicial.x + offset, posicaoInicial.y);
            ControlarFlip(transform.position.x);
        }
        else
        {
            transform.position = new Vector2(posicaoInicial.x, posicaoInicial.y + offset);
            ControlarFlip(transform.position.y);
        }
    }

    void ControlarFlip(float posicaoAtual)
    {
        if (!inverterSprite || spriteRenderer == null) return;

        // Se a posição atual for maior que a anterior, ele está indo para Direita/Cima
        if (posicaoAtual > posicaoAnterior)
        {
            spriteRenderer.flipX = false; 
        }
        // Se for menor, está indo para Esquerda/Baixo
        else if (posicaoAtual < posicaoAnterior)
        {
            spriteRenderer.flipX = true;
        }

        posicaoAnterior = posicaoAtual;
    }
}