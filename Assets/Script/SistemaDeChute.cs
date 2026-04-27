using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SistemaDeChute : MonoBehaviour
{
    public enum EstadoChute { Parado, EscolhendoForca, EscolhendoDirecao }
    
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
    public int tentativasRestantes = 3;
    public TextMeshProUGUI textoTentativas;
    public Animator meuAnimator;

    void Start()
    {
        // Inicializa a UI
        if (barraForca != null) barraForca.gameObject.SetActive(false);
        if (setaDirecao != null) setaDirecao.SetActive(false);
        
        AtualizarUITentativas();
    }

    void Update()
    {
        // Detecta o clique ou espaço
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
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
                if (tentativasRestantes > 0)
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
        tentativasRestantes--;
        AtualizarUITentativas();

        // 1. Aciona a animação do jogador
        if (meuAnimator != null) 
        {
            meuAnimator.SetTrigger("DispararChute");
        }

        // 2. Avisa o goleiro para agir (se ele existir na cena atual)
        LogicaGoleiro goleiro = FindFirstObjectByType<LogicaGoleiro>();
        if (goleiro != null)
        {
            goleiro.DecidirAcaoGoleiro();
        }

        // 3. Aplica a física na bola
        if (rbBola != null)
        {
            rbBola.linearVelocity = Vector2.zero; 
            rbBola.angularVelocity = 0f;

            // Calcula a direção baseada na seta (Vector2.up é o "frente" da seta)
            Vector2 direcaoFinal = setaDirecao.transform.up; 
            rbBola.AddForce(direcaoFinal * (valorForca * multiplicadorForca), ForceMode2D.Impulse);
        }

        // 4. Limpa a UI e reseta estado
        if (barraForca != null) barraForca.gameObject.SetActive(false);
        if (setaDirecao != null) setaDirecao.SetActive(false);
        estadoAtual = EstadoChute.Parado;
    }

    public void AtualizarUITentativas()
    {
        if (textoTentativas != null)
        {
            textoTentativas.text = "Tentativas:" + tentativasRestantes;
        }
    }

    // Função útil para quando você muda de cena e quer dar mais chances ao jogador
    public void ResetarTentativas(int quantidade)
    {
        tentativasRestantes = quantidade;
        AtualizarUITentativas();
    }
}