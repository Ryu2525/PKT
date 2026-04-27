using UnityEngine;
using System.Collections;

public class LogicaBola : MonoBehaviour
{
    private Vector2 posicaoInicial;
    private Rigidbody2D rb;
    private GerenciadorPartida gerenciador; // Alterado de GerenciadorPlacar para o novo
    private bool pausada = false;

    [Header("Configuração de Cenas")]
    public GameObject cenaPasse; 
    public GameObject cenaChute; 
    
    [Header("Posições Iniciais da Bola")]
    public Vector2 posInicialPasse; 
    public Vector2 posInicialChute; 

    private bool estaNaCenaChute = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Busca o novo gerenciador que você vai criar
        gerenciador = FindFirstObjectByType<GerenciadorPartida>();
        
        posicaoInicial = posInicialPasse;
        transform.position = posicaoInicial;
    }

    void Update()
    {
        if (transform.position.x < -12f || transform.position.x > 12f || transform.position.y > 5.6f)
        {
            // Se a bola sair do campo, conta como erro e reseta
            if (gerenciador != null) gerenciador.ErrouChute();
            ResetarBola();
        }
    }

    private void OnCollisionEnter2D(Collision2D colisor)
    {
        if (pausada) return;

        // 1. GOL -> CHAMA PAINEL VITORIA
        if (colisor.gameObject.CompareTag("Gol"))
        {
            if (gerenciador != null) gerenciador.MarcouGol();
            return; 
        }

        // 2. TRAVE
        if (colisor.gameObject.CompareTag("Trave"))
        {
            StartCoroutine(EfeitoTrave());
            return;
        }

        // 3. OUTRAS COLISÕES
        if (colisor.gameObject.CompareTag("Amigo") && !estaNaCenaChute)
        {
            MudarParaCenaChute();
        }
        else if (colisor.gameObject.CompareTag("Inimigo") || colisor.gameObject.CompareTag("Goleiro"))
        {
            // Bateu no inimigo? Perde uma tentativa e reseta
            if (gerenciador != null) gerenciador.ErrouChute();
            ResetarBola();
        }
    }

    IEnumerator EfeitoTrave()
    {
        pausada = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        yield return new WaitForSeconds(1f);
        pausada = false;
        ResetarBola();
    }

    void MudarParaCenaChute()
    {
        estaNaCenaChute = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        
        if(cenaPasse) cenaPasse.SetActive(false); 
        if(cenaChute) cenaChute.SetActive(true);  
        
        posicaoInicial = posInicialChute; 
        transform.position = posicaoInicial;
    }

    public void ResetarBola()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = posicaoInicial;

        SistemaDeChute scriptAtivo = FindFirstObjectByType<SistemaDeChute>();
        if(scriptAtivo != null) 
        {
            scriptAtivo.estadoAtual = SistemaDeChute.EstadoChute.Parado;
        }
    }
}