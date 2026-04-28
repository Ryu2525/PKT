using UnityEngine;

public class MovimentoAmigo : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2f;
    public float distanciaMovimento = 3f;
    public bool moverNaHorizontal = true;
    public bool inverterSprite = true; // Ativa/Desativa o giro do boneco

    private Vector2 posicaoInicial;
    private SpriteRenderer spriteRenderer;
    private float posicaoAnterior;

    void Start()
    {
        posicaoInicial = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Inicializa a posição anterior para evitar um flip brusco no primeiro frame
        posicaoAnterior = moverNaHorizontal ? transform.position.x : transform.position.y;
    }

    void Update()
    {
        // Cálculo suave usando PingPong
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

        // Se a posição atual for maior que a anterior, ele está indo para Direita (ou Cima)
        if (posicaoAtual > posicaoAnterior + 0.001f) // Pequena margem de erro para estabilidade
        {
            spriteRenderer.flipX = false; 
        }
        // Se for menor, está indo para Esquerda (ou Baixo)
        else if (posicaoAtual < posicaoAnterior - 0.001f)
        {
            spriteRenderer.flipX = true;
        }

        posicaoAnterior = posicaoAtual;
    }
}