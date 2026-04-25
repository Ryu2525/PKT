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
    public GameObject setaDirecao; // Arraste o PIVOT aqui
    public float velocidadeSeta = 100f;
    private float anguloSeta = 0f;
    private int direcaoSeta = 1;

    [Header("Referência da Bola")]
    public Rigidbody2D rbBola;
    public float multiplicadorForca = 20f;

    void Start()
    {
        if(barraForca) barraForca.gameObject.SetActive(false);
        if(setaDirecao) setaDirecao.SetActive(false);
    }

    // Esta função será chamada pelo Player Input Component 
    // Certifique-se de que a Action se chama "Space" ou "Jump" (ou o nome que você deu)
    public void OnJump(InputValue value)
    {
        // No New Input System, verificamos se a tecla foi pressionada
        if (value.isPressed)
        {
            AvancarEstado();
        }
    }

    // Caso você não queira configurar o Player Input por eventos, 
    // pode usar esta linha dentro do Update para detectar o espaço:
    void Update()
    {
        // Alternativa via código direto (New Input System):
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AvancarEstado();
        }

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
        if (estadoAtual == EstadoChute.Parado)
        {
            estadoAtual = EstadoChute.EscolhendoForca;
            barraForca.gameObject.SetActive(true);
            valorForca = 0;
        }
        else if (estadoAtual == EstadoChute.EscolhendoForca)
        {
            estadoAtual = EstadoChute.EscolhendoDirecao;
            setaDirecao.SetActive(true);
        }
        else if (estadoAtual == EstadoChute.EscolhendoDirecao)
        {
            ExecutarChute();
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
        if (anguloSeta >= 90f) direcaoSeta = -1;
        if (anguloSeta <= -90f) direcaoSeta = 1;
        setaDirecao.transform.localRotation = Quaternion.Euler(0, 0, anguloSeta);
    }

    void ExecutarChute()
    {
        // Pega a direção "frente" da seta (eixo Y local do Pivot)
        Vector2 direcaoFinal = setaDirecao.transform.up;
        rbBola.AddForce(direcaoFinal * (valorForca * multiplicadorForca), ForceMode2D.Impulse);

        barraForca.gameObject.SetActive(false);
        setaDirecao.SetActive(false);
        estadoAtual = EstadoChute.Parado;
    }
}