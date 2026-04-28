using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GerenciadorPartida : MonoBehaviour
{
    [Header("Configurações de UI")]
    public GameObject painelVitoria;
    public GameObject painelDerrota;
    public TextMeshProUGUI textoTentativas;

    [Header("Regras de Jogo")]
    [Tooltip("Quantidade de tentativas que o jogador terá ao iniciar cada cena")]
    public int tentativasPorCena = 3;
    
    // Variável que armazena o valor atual de tentativas
    [HideInInspector] public int tentativas;

    private GerenciadorPlacar placar;
    private bool processandoErro = false; // Trava para evitar perder múltiplas vidas de uma vez

    void Awake()
    {
        // 1. GARANTIA DE RESET POR CENA:
        // O Awake roda antes do Start. Sempre que a cena carregar,
        // ele define as tentativas com o valor máximo configurado.
        tentativas = tentativasPorCena;
    }

    void Start()
    {
        placar = FindFirstObjectByType<GerenciadorPlacar>();
        
        // Esconde os painéis ao iniciar
        if (painelVitoria != null) painelVitoria.SetActive(false);
        if (painelDerrota != null) painelDerrota.SetActive(false);
        
        AtualizarUI();
    }

    public void MarcouGol()
    {
        // Aumenta o placar no GerenciadorPlacar
        if (placar != null) placar.MarcarGol(true);
    }

    public void ErrouChute()
    {
        // 2. PROTEÇÃO CONTRA PERDA DUPLA:
        // Se a bola bater em dois inimigos ou sair do campo e bater em algo ao mesmo tempo,
        // essa trava impede que o código execute duas vezes seguidas.
        if (processandoErro) return;

        tentativas--;
        AtualizarUI();
        
        if (tentativas <= 0)
        {
            FinalizarJogo();
        }
        else
        {
            // Inicia a trava temporária
            StartCoroutine(TravarErrosTemporariamente());
        }
    }

    // Corrotina que impede o registro de novos erros por um curto período (0.5 segundos)
    IEnumerator TravarErrosTemporariamente()
    {
        processandoErro = true;
        yield return new WaitForSeconds(0.5f);
        processandoErro = false;
    }

    // Chamado pela LogicaBola quando o jogador passa de etapa (ex: Passe para o Chute)
    public void ResetarTentativas()
    {
        tentativas = tentativasPorCena;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (textoTentativas != null)
        {
            textoTentativas.text = "Tentativas:" + tentativas;
        }
    }

    public void FinalizarJogoManualmente()
    {
        FinalizarJogo(); 
    }

    void FinalizarJogo()
    {
        // Verifica no placar se o jogador marcou mais gols que o adversário
        if (placar != null && placar.JogadorEstaGanhando())
        {
            if (painelVitoria != null) painelVitoria.SetActive(true);
            DadosDoJogador.RegistrarResultado(true); 
        }
        else
        {
            if (painelDerrota != null) painelDerrota.SetActive(true);
            DadosDoJogador.RegistrarResultado(false); 
        }
    }

    public void IrParaHub()
    {
        SceneManager.LoadScene("Hub");
    }
}