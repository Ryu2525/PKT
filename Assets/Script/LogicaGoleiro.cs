using UnityEngine;
using System.Collections;

public class LogicaGoleiro : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 posicaoInicial;

    [Header("Sprites de Defesa")]
    public Sprite spriteParado;
    public Sprite spritePuloEsquerda;
    public Sprite spritePuloDireita;

    [Header("Configurações de Movimento")]
    public float limiteX = 4.60f; // Conforme sua solicitação
    public float velocidadePulo = 8f;
    public float tempoDeDefesa = 1.2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        posicaoInicial = transform.position;
        // Garante que comece com o sprite parado
        if (spriteParado != null) spriteRenderer.sprite = spriteParado;
    }

    public void DecidirAcaoGoleiro()
    {
        // 0 = Meio, 1 = Esquerda, 2 = Direita
        int sorteio = Random.Range(0, 3);

        switch (sorteio)
        {
            case 0:
                Debug.Log("Goleiro ficou no meio.");
                // Não muda o sprite nem a posição
                break;
            case 1:
                StartCoroutine(ExecutarPulo(-limiteX, spritePuloEsquerda));
                break;
            case 2:
                StartCoroutine(ExecutarPulo(limiteX, spritePuloDireita));
                break;
        }
    }

    IEnumerator ExecutarPulo(float destinoX, Sprite spritePulo)
    {
        // 1. Troca a imagem
        if (spritePulo != null) spriteRenderer.sprite = spritePulo;

        // 2. Move para o lado (limitado a 4.60 ou -4.60)
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
        spriteRenderer.sprite = spriteParado;
        transform.position = posicaoInicial;
    }
}