using UnityEngine;
using System.Collections;

public class LogicaGoleiro : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 posicaoInicial;
    private Coroutine rotinaAtual; // Para podermos cancelar o pulo se a bola resetar

    [Header("Sprites de Defesa")]
    public Sprite spriteParado;
    public Sprite spritePuloEsquerda;
    public Sprite spritePuloDireita;

    [Header("Configurações de Movimento")]
    public float limiteX = 4.60f;
    public float velocidadePulo = 8f;
    public float tempoDeDefesa = 1.2f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        posicaoInicial = transform.position;
    }

    // --- CONEXÃO COM O EVENTO DE CHUTE ---
    void OnEnable()
    {
        // Se inscreve para pular assim que o jogador confirmar o chute
        SistemaDeChute.AoConfirmarChute += DecidirAcaoGoleiro;
    }

    void OnDisable()
    {
        // Se desinscreve ao sair da cena
        SistemaDeChute.AoConfirmarChute -= DecidirAcaoGoleiro;
    }

    public void DecidirAcaoGoleiro()
    {
        // Se ele já estiver pulando, para o pulo atual para começar um novo
        if (rotinaAtual != null) StopCoroutine(rotinaAtual);

        // 0 = Meio, 1 = Esquerda, 2 = Direita
        int sorteio = Random.Range(0, 3);

        switch (sorteio)
        {
            case 0:
                Debug.Log("Goleiro ficou no meio.");
                spriteRenderer.sprite = spriteParado;
                transform.position = posicaoInicial;
                break;
            case 1:
                rotinaAtual = StartCoroutine(ExecutarPulo(-limiteX, spritePuloEsquerda));
                break;
            case 2:
                rotinaAtual = StartCoroutine(ExecutarPulo(limiteX, spritePuloDireita));
                break;
        }
    }

    IEnumerator ExecutarPulo(float destinoX, Sprite spritePulo)
    {
        // 1. Troca a imagem
        if (spritePulo != null) spriteRenderer.sprite = spritePulo;

        // 2. Move para o lado
        Vector3 destino = new Vector3(destinoX, transform.position.y, transform.position.z);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * velocidadePulo;
            transform.position = Vector3.Lerp(posicaoInicial, destino, t);
            yield return null;
        }

        // 3. Espera o tempo da defesa
        yield return new WaitForSeconds(tempoDeDefesa);

        // 4. Volta ao normal
        ResetarGoleiro();
    }

    public void ResetarGoleiro()
    {
        if (rotinaAtual != null) StopCoroutine(rotinaAtual);
        spriteRenderer.sprite = spriteParado;
        transform.position = posicaoInicial;
    }
}