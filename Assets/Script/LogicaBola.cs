using UnityEngine;
using System.Collections;

public class LogicaBola : MonoBehaviour
{
    private Vector2 posicaoInicialAtual;
    private Rigidbody2D rb;
    private GerenciadorPartida gerenciador;
    private bool pausada = false;

    [Header("Configuração de Etapas")]
    public GameObject[] etapasDaPartida;
    private int etapaAtualIndice = 0;

    [Header("Posições Iniciais da Bola")]
    public Vector2[] posicoesIniciaisPorEtapa;

    [Header("Limites do Campo (Segurança)")]
    [Tooltip("Distância máxima que a bola pode ir antes de resetar automaticamente")]
    public float limiteX = 15f;
    public float limiteY = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Busca o gerenciador na cena (Unity 6 usa FindFirstObjectByType)
        gerenciador = FindFirstObjectByType<GerenciadorPartida>();
        
        ConfigurarEtapa(0);
    }

    void Update()
    {
        // VERIFICAÇÃO DE LIMITE: Se a bola sair da área visível, reseta.
        if (Mathf.Abs(transform.position.x) > limiteX || Mathf.Abs(transform.position.y) > limiteY)
        {
            if (gerenciador != null) gerenciador.ErrouChute();
            ResetarBolaPosicao();
        }
    }

    private void OnCollisionEnter2D(Collision2D colisor)
    {
        if (pausada) return;

        // 1. BATEU NA TRAVE
        if (colisor.gameObject.CompareTag("Trave"))
        {
            // ---> ADICIONE ESTA LINHA AQUI <---
            if (gerenciador != null) gerenciador.ErrouChute();
            StartCoroutine(EfeitoTrave());
            return;
        }

        // 2. MARCOU GOL
        if (colisor.gameObject.CompareTag("Gol"))
        {
            if (gerenciador != null) gerenciador.MarcouGol();

            // Se ainda houver etapas (ex: gol da virada), avança.
            if (etapaAtualIndice + 1 < etapasDaPartida.Length)
            {
                StartCoroutine(EsperarParaProximaEtapa());
            }
            else
            {
                // Se era o último gol, finaliza a partida.
                if (gerenciador != null) gerenciador.FinalizarJogoManualmente();
            }
            return;
        }

        // 3. BATEU NO AMIGO (PASSE)
        if (colisor.gameObject.CompareTag("Amigo"))
        {
            AvancarEtapa();
        }

        // 4. BATEU NO INIMIGO OU GOLEIRO (ERRO)
        else if (colisor.gameObject.CompareTag("Inimigo") || colisor.gameObject.CompareTag("Goleiro"))
        {
            if (gerenciador != null) gerenciador.ErrouChute();
            ResetarBolaPosicao();
        }
    }

    // --- MÉTODOS DE CONTROLE ---

    void ConfigurarEtapa(int indice)
    {
        if (etapasDaPartida.Length == 0) return;
        etapaAtualIndice = indice;

        // Ativa apenas a 'fatia' do campo correta
        for (int i = 0; i < etapasDaPartida.Length; i++)
        {
            etapasDaPartida[i].SetActive(i == etapaAtualIndice);
        }

        // Atualiza onde a bola deve nascer nesta etapa
        if (indice < posicoesIniciaisPorEtapa.Length)
            posicaoInicialAtual = posicoesIniciaisPorEtapa[indice];

        // ---> AQUI ESTÁ A SOLUÇÃO DAS TENTATIVAS <---
        // Quando muda de etapa (ex: de passe para chute), ele reseta para 3 vidas!
        if (gerenciador != null)
        {
            gerenciador.ResetarTentativas();
        }

        ResetarBolaPosicao();
    }

    void AvancarEtapa()
    {
        ConfigurarEtapa(etapaAtualIndice + 1);
    }

    public void ResetarBolaPosicao()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = posicaoInicialAtual;
        
        // Avisa o sistema de clique/chute que ele pode atirar de novo
        SistemaDeChute chute = FindFirstObjectByType<SistemaDeChute>();
        if (chute != null) chute.estadoAtual = SistemaDeChute.EstadoChute.Parado;

        LogicaGoleiro goleiro = FindFirstObjectByType<LogicaGoleiro>();
        if (goleiro != null) goleiro.ResetarGoleiro();
    }

    // --- CORROTINAS (TEMPO) ---

    IEnumerator EfeitoTrave()
    {
        pausada = true; 
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        yield return new WaitForSeconds(0.8f); 
        pausada = false;
        ResetarBolaPosicao();
    }

    IEnumerator EsperarParaProximaEtapa()
    {
        pausada = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        yield return new WaitForSeconds(1.2f); // Tempo da comemoração
        pausada = false;
        AvancarEtapa();
    }
}