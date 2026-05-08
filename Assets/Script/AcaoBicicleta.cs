using UnityEngine;
using System.Collections;

public class AcaoBicicleta : MonoBehaviour
{
    [Header("Sprites de Ação")]
    [Tooltip("Arraste o sprite dele em pé (o padrão)")]
    public Sprite spriteParado;
    [Tooltip("Arraste o sprite dele fazendo a bicicleta")]
    public Sprite spriteBicicleta;

    [Header("Tempo da Animação")]
    [Tooltip("Quanto tempo ele fica na pose de bicicleta antes de voltar a ficar em pé")]
    public float tempoDePose = 0.8f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // Se inscreve para ouvir o evento global de chute
        SistemaDeChute.AoConfirmarChute += IniciarBicicleta;
    }

    void OnDisable()
    {
        // Se desinscreve para evitar erros de memória
        SistemaDeChute.AoConfirmarChute -= IniciarBicicleta;
    }

    void IniciarBicicleta()
    {
        // Troca para o sprite da bicicleta IMEDIATAMENTE
        if (spriteRenderer != null && spriteBicicleta != null)
        {
            spriteRenderer.sprite = spriteBicicleta;
            
            // Inicia o cronômetro para ele voltar a ficar em pé
            StartCoroutine(RetornarParaPoseParado());
            Debug.Log("Jogador deu uma bicicleta!");
        }
    }

    IEnumerator RetornarParaPoseParado()
    {
        yield return new WaitForSeconds(tempoDePose);
        
        // Volta para o sprite padrão
        if (spriteRenderer != null && spriteParado != null)
        {
            spriteRenderer.sprite = spriteParado;
        }
    }

    // Opcional: Função pública caso a bola resete ANTES do tempo acabar
    public void ForcarResetarPose()
    {
        StopAllCoroutines(); // Para o cronômetro atual
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spriteParado;
        }
    }
}