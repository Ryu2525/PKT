using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorPartida : MonoBehaviour
{
    public GameObject painelVitoria;
    public GameObject painelDerrota;

    public int tentativas = 3;

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

    void Start()
    {
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
    }

    public void MarcouGol()
    {
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
        if (partidaFinalizada) return;

        primeiroChute = false;

        tentativas--;

        Debug.Log("Erro de chute. Tentativas restantes: " + tentativas);

        if (tentativas <= 0)
        {
            FinalizarPartida(false);
        }
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
        tentativas = 3;
        Debug.Log("Tentativas resetadas para: " + tentativas);
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