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
    
    [Header("Tamanho da Bola")]
    [Tooltip("Escala da bola (X,Y)")]
    public Vector2 tamanhoBola = new Vector2(1f, 1f);

    [Header("Limites do Campo (Segurança)")]
    [Tooltip("Distância máxima que a bola pode ir antes de resetar automaticamente")]
    public float limiteX = 15f;
    public float limiteY = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Unity 6 usa FindFirstObjectByType
        gerenciador = FindFirstObjectByType<GerenciadorPartida>();

        ConfigurarEtapa(0);
    }

    void Update()
    {
        // Se a bola sair da área visível, considera chute para fora.
        if (Mathf.Abs(transform.position.x) > limiteX || Mathf.Abs(transform.position.y) > limiteY)
        {
            if (gerenciador != null)
            {
                gerenciador.RegistrarChuteParaFora();
            }

            ResetarBolaPosicao();
        }
    }

    private void OnCollisionEnter2D(Collision2D colisor)
    {
        if (pausada) return;

        // 1. BATEU NA TRAVE
        if (colisor.gameObject.CompareTag("Trave"))
        {
            if (gerenciador != null)
            {
                gerenciador.ErrouChute();
            }

            StartCoroutine(EfeitoTrave());
            return;
        }

        // 2. MARCOU GOL
        if (colisor.gameObject.CompareTag("Gol"))
        {
            if (gerenciador != null)
            {
                gerenciador.MarcouGol();
            }

            // Se ainda houver etapas, avança para a próxima.
            if (etapaAtualIndice + 1 < etapasDaPartida.Length)
            {
                StartCoroutine(EsperarParaProximaEtapa());
            }
            else
            {
                // Se era o último gol/última etapa, finaliza a partida.
                if (gerenciador != null)
                {
                    gerenciador.FinalizarJogoManualmente();
                }
            }

            return;
        }

        // 3. BATEU NO AMIGO
        if (colisor.gameObject.CompareTag("Amigo"))
        {
            if (gerenciador != null)
            {
                gerenciador.RegistrarAssistencia();
            }

            AvancarEtapa();
            return;
        }

        // 4. BATEU NO INIMIGO
        if (colisor.gameObject.CompareTag("Inimigo"))
        {
            if (gerenciador != null)
            {
                gerenciador.RegistrarBatidaInimigo();
            }

            ResetarBolaPosicao();
            return;
        }

        // 5. BATEU NO GOLEIRO
        if (colisor.gameObject.CompareTag("Goleiro"))
        {
            if (gerenciador != null)
            {
                gerenciador.RegistrarBatidaGoleiro();
            }

            ResetarBolaPosicao();
            return;
        }
    }

    // --- MÉTODOS DE CONTROLE ---

    void ConfigurarEtapa(int indice)
    {
        if (etapasDaPartida.Length == 0) return;

        etapaAtualIndice = indice;

        // Ativa apenas a etapa correta da partida.
        for (int i = 0; i < etapasDaPartida.Length; i++)
        {
            etapasDaPartida[i].SetActive(i == etapaAtualIndice);
        }

        // Define onde a bola nasce nessa etapa.
        if (indice < posicoesIniciaisPorEtapa.Length)
        {
            posicaoInicialAtual = posicoesIniciaisPorEtapa[indice];
        }

        // Define onde a bola nasce nessa etapa.
        if (indice < posicoesIniciaisPorEtapa.Length)
        {
            posicaoInicialAtual = posicoesIniciaisPorEtapa[indice];
        }

        // Ajusta o tamanho da bola
        transform.localScale = new Vector3(tamanhoBola.x, tamanhoBola.y, 1f);

        // Quando muda de etapa, reseta as tentativas.
        if (gerenciador != null)
        {
            gerenciador.ResetarTentativas();
        }

        ResetarBolaPosicao();
    }

    void AvancarEtapa()
    {
        if (etapaAtualIndice + 1 < etapasDaPartida.Length)
        {
            ConfigurarEtapa(etapaAtualIndice + 1);
        }
    }

    public void ResetarBolaPosicao()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        transform.position = posicaoInicialAtual;

        // Libera o sistema de chute para novo chute.
        SistemaDeChute chute = FindFirstObjectByType<SistemaDeChute>();

        if (chute != null)
        {
            chute.estadoAtual = SistemaDeChute.EstadoChute.Parado;
        }

        LogicaGoleiro goleiro = FindFirstObjectByType<LogicaGoleiro>();

        if (goleiro != null)
        {
            goleiro.ResetarGoleiro();
        }

        // Reseta todos os jogadores (amigos e inimigos)
        LogicaJogador[] jogadores = FindObjectsByType<LogicaJogador>(FindObjectsSortMode.None);
        foreach (LogicaJogador j in jogadores)
        {
            j.ResetarPosicao();
        }
    }

    // --- CORROTINAS ---

    IEnumerator EfeitoTrave()
    {
        pausada = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        yield return new WaitForSeconds(0.8f);

        pausada = false;
        ResetarBolaPosicao();
    }

    IEnumerator EsperarParaProximaEtapa()
    {
        pausada = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        yield return new WaitForSeconds(1.2f);

        pausada = false;
        AvancarEtapa();
    }
}