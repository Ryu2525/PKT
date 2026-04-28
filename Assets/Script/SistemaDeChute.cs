using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class SistemaDeChute : MonoBehaviour
{
    public TextMeshProUGUI textoTentativas; // Adicione esta linha novamente
    // Evento que avisa a Bicicleta e o Goleiro que o chute ocorreu
    public static event Action AoConfirmarChute;

    public enum EstadoChute { Parado, EscolhendoForca, EscolhendoDirecao, BolaEmMovimento }
    
    [Header("Estado Atual")]
    public EstadoChute estadoAtual = EstadoChute.Parado;

    [Header("Configuração de Força")]
    public Slider barraForca;
    public float velocidadeBarra = 2f;
    private float valorForca = 0f;
    private int direcaoForca = 1;

    [Header("Configuração de Direção")]
    public GameObject setaDirecao; 
    public float velocidadeSeta = 100f;
    public float anguloMaximo = 80f;
    private float anguloSeta = 0f;
    private int direcaoSeta = 1;

    [Header("Referência da Bola")]
    public Rigidbody2D rbBola;
    public float multiplicadorForca = 15f;

    [Header("Regras e UI")]
    // Referência ao gerenciador para não duplicar a lógica de tentativas
    private GerenciadorPartida gerenciador;
    public Animator meuAnimator;

    void Start()
    {
        gerenciador = FindFirstObjectByType<GerenciadorPartida>();

        // Inicializa a UI
        if (barraForca != null) barraForca.gameObject.SetActive(false);
        if (setaDirecao != null) setaDirecao.SetActive(false);

        // Se você ainda quiser usar o texto pelo script de chute:
        if (gerenciador != null && textoTentativas != null)
        {
            textoTentativas.text = "Tentativas:" + gerenciador.tentativas;
        }
    }

    void Update()
    {
        // Detecta o clique ou espaço
        if (Keyboard.current.spaceKey.wasPressedThisFrame || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame))
        {
            AvancarEstado();
        }

        // Lógica de oscilação baseada no estado
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
                // Só começa se ainda houver tentativas no Gerenciador
                if (gerenciador != null && gerenciador.tentativas > 0)
                {
                    estadoAtual = EstadoChute.EscolhendoForca;
                    if (barraForca != null) barraForca.gameObject.SetActive(true);
                    valorForca = 0;
                }
                break;

            case EstadoChute.EscolhendoForca:
                estadoAtual = EstadoChute.EscolhendoDirecao;
                if (setaDirecao != null) setaDirecao.SetActive(true);
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
        
        if (barraForca != null) barraForca.value = valorForca;
    }

    void OscilarDirecao()
    {
        anguloSeta += Time.deltaTime * velocidadeSeta * direcaoSeta;
        if (anguloSeta >= anguloMaximo) direcaoSeta = -1;
        if (anguloSeta <= -anguloMaximo) direcaoSeta = 1;

        if (setaDirecao != null)
        {
            setaDirecao.transform.localRotation = Quaternion.Euler(0, 0, anguloSeta);
        }
    }

    void ExecutarChute()
    {
        // 1. DISPARA O EVENTO (Isso faz a Bicicleta e o Goleiro funcionarem!)
        AoConfirmarChute?.Invoke();

        // 2. Aciona a animação do jogador (Animator)
        if (meuAnimator != null) 
        {
            meuAnimator.SetTrigger("DispararChute");
        }

        // 3. Aplica a física na bola para ela voar
        if (rbBola != null)
        {
            rbBola.linearVelocity = Vector2.zero; 
            rbBola.angularVelocity = 0f;

            // Calcula a direção baseada na seta
            Vector2 direcaoFinal = setaDirecao.transform.up; 
            rbBola.AddForce(direcaoFinal * (valorForca * multiplicadorForca), ForceMode2D.Impulse);
        }

        // 4. Limpa a UI e muda estado para não chutar duas vezes a mesma bola
        if (barraForca != null) barraForca.gameObject.SetActive(false);
        if (setaDirecao != null) setaDirecao.SetActive(false);
        
        estadoAtual = EstadoChute.BolaEmMovimento;
    }
}