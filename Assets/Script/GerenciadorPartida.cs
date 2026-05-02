using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;


public class GerenciadorPartida : MonoBehaviour
{
    public GameObject painelVitoria;
    public GameObject painelDerrota;
    public TextMeshProUGUI textoTentativas;

    public int tentativasPorCena = 3;

    private int golsMarcados = 0;
    private int assistencias = 0;
    private int chutesNoGol = 0;
    private int chutesParaFora = 0;
    private int errosPasse = 0;
    private int batidasGoleiro = 0;
    private int batidasInimigo = 0;

    private bool primeiroChute = true;
    private bool golNaPrimeiraTentativa = false;

    private bool partidaFinalizada = false;
    private bool venceuPartida = false;

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
        
        Debug.Log("=== OBJETIVO RECEBIDO NA PARTIDA ===");
        Debug.Log("Objetivo recebido: " + DadosDoJogador.ObjetivoEscolhido);
        Debug.Log("Tipo: " + DadosDoJogador.TipoObjetivo);
        Debug.Log("Ação esperada: " + DadosDoJogador.AcaoObjetivo);
        Debug.Log("Quantidade necessária: " + DadosDoJogador.QuantidadeNecessaria);
        Debug.Log("Recompensa: " + DadosDoJogador.RecompensaObjetivo);

        if (painelVitoria != null)
        {
            painelVitoria.SetActive(false);
        }

        if (painelDerrota != null)
        {
            painelDerrota.SetActive(false);
        }

        AtualizarUI();
    }

    public void MarcouGol()
    {
        // Aumenta o placar no GerenciadorPlacar
        if (placar != null) placar.MarcarGol(true);

        if (partidaFinalizada) return;

        golsMarcados++;
        chutesNoGol++;
        venceuPartida = true;

        if (primeiroChute)
        {
            golNaPrimeiraTentativa = true;
        }

        primeiroChute = false;

        Debug.Log("Gol marcado. Total de gols: " + golsMarcados);

        // Importante:
        // Não finaliza a partida aqui.
        // A LogicaBola decide se ainda existe próxima etapa ou se acabou a partida.
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
            FinalizarJogoManualmente();
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

    public void RegistrarChuteParaFora()
    {
        if (partidaFinalizada) return;

        chutesParaFora++;
        Debug.Log("Chute para fora. Total: " + chutesParaFora);

        ErrouChute();
    }

    public void RegistrarBatidaGoleiro()
    {
        if (partidaFinalizada) return;

        batidasGoleiro++;
        chutesNoGol++;

        Debug.Log("Bateu no goleiro. Total: " + batidasGoleiro);

        ErrouChute();
    }

    public void RegistrarBatidaInimigo()
    {
        if (partidaFinalizada) return;

        batidasInimigo++;
        errosPasse++;

        Debug.Log("Bateu no inimigo. Total: " + batidasInimigo);

        ErrouChute();
    }

    public void RegistrarAssistencia()
    {
        if (partidaFinalizada) return;

        assistencias++;

        Debug.Log("Assistência registrada. Total: " + assistencias);
    }

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
        if (partidaFinalizada) return;

        FinalizarPartida(venceuPartida);
    }

    void FinalizarPartida(bool venceu)
    {
        if (partidaFinalizada) return;

        partidaFinalizada = true;
        venceuPartida = venceu;

        // Salva valores antes das alterações
        DadosDoJogador.FelicidadeAntes = DadosDoJogador.Felicidade;
        DadosDoJogador.FamaAntes = DadosDoJogador.Fama;
        DadosDoJogador.MoralTreinadorAntes = DadosDoJogador.MoralTreinador;
        DadosDoJogador.MoralApostadorAntes = DadosDoJogador.MoralApostador;

        DadosDoJogador.UltimaDificuldade = ObterDificuldadeAtual();

        bool objetivoCumprido = VerificarObjetivoCumprido();

        // Salva informações principais do resultado
        DadosDoJogador.UltimoObjetivoCumprido = objetivoCumprido;
        DadosDoJogador.UltimaPartidaVencida = venceuPartida;

        if (objetivoCumprido)
        {
            DadosDoJogador.UltimoDinheiroGanho = DadosDoJogador.RecompensaObjetivo;
        }
        else
        {
            DadosDoJogador.UltimoDinheiroGanho = 0;
        }

        if (DadosDoJogador.TipoObjetivo == "Apostador" && objetivoCumprido)
        {
            DadosDoJogador.UltimoBonusRisco = "Incluído";
        }
        else
        {
            DadosDoJogador.UltimoBonusRisco = "Não incluído";
        }

        AplicarResultadoDoObjetivo(objetivoCumprido);

        DadosDoJogador.RegistrarResultado(venceuPartida);

        DadosDoJogador.LimitarBarras();

        // Salva valores depois das alterações
        DadosDoJogador.FelicidadeDepois = DadosDoJogador.Felicidade;
        DadosDoJogador.FamaDepois = DadosDoJogador.Fama;
        DadosDoJogador.MoralTreinadorDepois = DadosDoJogador.MoralTreinador;
        DadosDoJogador.MoralApostadorDepois = DadosDoJogador.MoralApostador;

        Debug.Log("=== RESULTADO DA PARTIDA ===");
        Debug.Log("Venceu partida: " + venceuPartida);
        Debug.Log("Objetivo cumprido: " + objetivoCumprido);
        Debug.Log("Dinheiro ganho: " + DadosDoJogador.UltimoDinheiroGanho);
        Debug.Log("Dinheiro atual: " + DadosDoJogador.DinheiroAtual);
        Debug.Log("Felicidade: " + DadosDoJogador.FelicidadeAntes + " -> " + DadosDoJogador.FelicidadeDepois);
        Debug.Log("Fama: " + DadosDoJogador.FamaAntes + " -> " + DadosDoJogador.FamaDepois);
        Debug.Log("Moral treinador: " + DadosDoJogador.MoralTreinadorAntes + " -> " + DadosDoJogador.MoralTreinadorDepois);
        Debug.Log("Moral apostador: " + DadosDoJogador.MoralApostadorAntes + " -> " + DadosDoJogador.MoralApostadorDepois);

        SceneManager.LoadScene("ResultadoPartida");
    }

    bool VerificarObjetivoCumprido()
    {
        string acao = DadosDoJogador.AcaoObjetivo;
        int necessario = DadosDoJogador.QuantidadeNecessaria;

        if (acao == "GOL")
        {
            return golsMarcados >= necessario;
        }

        if (acao == "ASSISTENCIA")
        {
            return assistencias >= necessario;
        }

        if (acao == "CHUTE_NO_GOL")
        {
            return chutesNoGol >= necessario;
        }

        if (acao == "NAO_CHUTAR_FORA")
        {
            return chutesParaFora == 0;
        }

        if (acao == "VITORIA")
        {
            return venceuPartida;
        }

        if (acao == "GOL_PRIMEIRA_TENTATIVA")
        {
            return golNaPrimeiraTentativa;
        }

        if (acao == "CHUTE_FORA")
        {
            return chutesParaFora >= necessario;
        }

        if (acao == "ERRO_PASSE")
        {
            return errosPasse >= necessario;
        }

        if (acao == "NAO_MARCAR_GOL")
        {
            return golsMarcados == 0;
        }

        if (acao == "DERROTA")
        {
            return !venceuPartida;
        }

        if (acao == "BATER_GOLEIRO")
        {
            return batidasGoleiro >= necessario;
        }

        if (acao == "BATER_INIMIGO")
        {
            return batidasInimigo >= necessario;
        }

        Debug.LogWarning("Ação de objetivo não reconhecida: " + acao);
        return false;
    }

    void AplicarResultadoDoObjetivo(bool objetivoCumprido)
    {
        if (objetivoCumprido)
        {
            DadosDoJogador.DinheiroAtual += DadosDoJogador.RecompensaObjetivo;

            if (DadosDoJogador.TipoObjetivo == "Treinador")
            {
                DadosDoJogador.MoralTreinador += DadosDoJogador.ImpactoPositivoObjetivo;
                DadosDoJogador.Fama += 0.05f;
            }
            else
            {
                DadosDoJogador.MoralApostador += DadosDoJogador.ImpactoPositivoObjetivo;
                DadosDoJogador.MoralTreinador -= DadosDoJogador.PenalidadeTreinadorExtra;
            }
        }
        else
        {
            if (DadosDoJogador.TipoObjetivo == "Treinador")
            {
                DadosDoJogador.MoralTreinador -= DadosDoJogador.PenalidadeObjetivo;
            }
            else
            {
                DadosDoJogador.MoralApostador -= DadosDoJogador.PenalidadeObjetivo;
            }
        }

        DadosDoJogador.LimitarBarras();
    }

    string ObterDificuldadeAtual()
    {
        if (DadosDoJogador.NivelAtual == 1)
        {
            return "FÁCIL";
        }

        if (DadosDoJogador.NivelAtual == 2)
        {
            return "MÉDIA";
        }

        return "DIFÍCIL";
    }

    public void IrParaHub()
    {
        SceneManager.LoadScene("Hub");
    }
}