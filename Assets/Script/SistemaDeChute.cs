using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SistemaDeChute : MonoBehaviour
{
    public enum EstadoChute { Parado, EscolhendoForca, EscolhendoDirecao }
    public EstadoChute estadoAtual = EstadoChute.Parado;

    [Header("Configuração de Força")]
    public Slider barraForca;
    public float velocidadeBarra = 2f;
    private float valorForca = 0f;
    private int direcaoForca = 1;

    [Header("Configuração de Direção")]
    public GameObject setaDirecao; 
    public float velocidadeSeta = 100f;
    public float anguloMaximo = 80f; // Evita que chute totalmente para trás
    private float anguloSeta = 0f;
    private int direcaoSeta = 1;

    [Header("Referência da Bola")]
    public Rigidbody2D rbBola;
    public float multiplicadorForca = 15f;

    void Start()
    {
        if (barraForca) barraForca.gameObject.SetActive(false);
        if (setaDirecao) setaDirecao.SetActive(false);
        
        // Garante que a bola não saia rolando sozinha no início
        if (rbBola) rbBola.simulated = true; 
    }

    void Update()
    {
        // Detecta o espaço (New Input System)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AvancarEstado();
        }

        // Processamento visual dos estados
        if (estadoAtual == EstadoChute.EscolhendoForca)
        {
            OscilarForca();
        }
        else if (estadoAtual == EstadoChute.EscolhendoDirecao)
        {
            OscilarDirecao();
        }
    }

    void AvancarEstado()
    {
        switch (estadoAtual)
        {
            case EstadoChute.Parado:
                estadoAtual = EstadoChute.EscolhendoForca;
                barraForca.gameObject.SetActive(true);
                valorForca = 0;
                break;

            case EstadoChute.EscolhendoForca:
                estadoAtual = EstadoChute.EscolhendoDirecao;
                setaDirecao.SetActive(true);
                // Resetar o ângulo para começar do centro
                anguloSeta = 0; 
                break;

            case EstadoChute.EscolhendoDirecao:
                ExecutarChute();
                break;
        }
    }

    void OscilarForca()
    {
        valorForca += Time.deltaTime * velocidadeBarra * direcaoForca;
        if (valorForca >= 1f) { valorForca = 1f; direcaoForca = -1; }
        if (valorForca <= 0f) { valorForca = 0f; direcaoForca = 1; }
        barraForca.value = valorForca;
    }

    void OscilarDirecao()
    {
        anguloSeta += Time.deltaTime * velocidadeSeta * direcaoSeta;
        
        // Limita o arco da seta para não fazer 360º
        if (anguloSeta >= anguloMaximo) direcaoSeta = -1;
        if (anguloSeta <= -anguloMaximo) direcaoSeta = 1;
        
        setaDirecao.transform.localRotation = Quaternion.Euler(0, 0, anguloSeta);
    }

    void ExecutarChute()
    {
        // 1. IMPORTANTE: Zera completamente a física antes de aplicar nova força
        rbBola.linearVelocity = Vector2.zero; 
        rbBola.angularVelocity = 0f;

        // 2. CÁLCULO DE DIREÇÃO:
        // Se a sua seta no Unity aponta para cima no desenho, usamos .up
        // Se ela aponta para a direita, usamos .right
        Vector2 direcaoFinal = setaDirecao.transform.up; 
        
        // 3. APLICAÇÃO DO IMPULSO
        // Multiplicamos pela força escolhida (valorForca vai de 0 a 1)
        rbBola.AddForce(direcaoFinal * (valorForca * multiplicadorForca), ForceMode2D.Impulse);

        // Limpeza de UI e reset de estado
        barraForca.gameObject.SetActive(false);
        setaDirecao.SetActive(false);
        estadoAtual = EstadoChute.Parado;
    }
}