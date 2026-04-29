using UnityEngine;
using UnityEngine.SceneManagement;

public static class DadosDoJogador
{
    // =========================
    // DADOS DO PERSONAGEM
    // =========================

    public static Sprite PersonagemSelecionado;
    public static string NomeJogador = "Jogador 1";


    // =========================
    // DADOS DA CARREIRA
    // =========================


    public static bool CicloFinalizado = false;

    public static bool CicloVencido = false;
    public static int DinheiroAtual = 50;
    public static int NivelAtual = 1;
    public static int VitoriasNoCiclo = 0;
    public static int PartidasJogadas = 0;
    public static string TimeAtual = "União Vila Nova FC";
    public static int CiclosFracassados = 0;


    // =========================
    // BARRAS DO JOGADOR
    // =========================

    public static float Felicidade = 0.5f;
    public static float Fama = 0.5f;
    public static float MoralTreinador = 0.5f;
    public static float MoralApostador = 0.5f;


    // =========================
    // OBJETIVO DA PRÉ-PARTIDA
    // =========================

    public static string ObjetivoEscolhido = "";
    public static string TipoObjetivo = "";
    public static string AcaoObjetivo = "";

    public static int RecompensaObjetivo = 0;
    public static int QuantidadeNecessaria = 0;

    public static float ImpactoPositivoObjetivo = 0f;
    public static float PenalidadeObjetivo = 0f;
    public static float PenalidadeTreinadorExtra = 0f;  

    // =========================
    // RESULTADO DA ÚLTIMA PARTIDA
    // =========================

    public static bool UltimoObjetivoCumprido = false;
    public static bool UltimaPartidaVencida = false;

    public static string UltimaDificuldade = "";
    public static int UltimoDinheiroGanho = 0;
    public static string UltimoBonusRisco = "Não incluído";

    public static float FelicidadeAntes = 0f;
    public static float FelicidadeDepois = 0f;

    public static float FamaAntes = 0f;
    public static float FamaDepois = 0f;

    public static float MoralTreinadorAntes = 0f;
    public static float MoralTreinadorDepois = 0f;

    public static float MoralApostadorAntes = 0f;
    public static float MoralApostadorDepois = 0f;


    // =========================
    // REGISTRO DE RESULTADO
    // =========================

    public static void RegistrarResultado(bool venceu)
    {
        CicloFinalizado = false;
        CicloVencido = false;

        if (venceu)
        {
            VitoriasNoCiclo++;

            Fama = Mathf.Clamp01(Fama + 0.1f);
            Felicidade = Mathf.Clamp01(Felicidade + 0.1f);
        }
        else
        {
            Felicidade = Mathf.Clamp01(Felicidade - 0.1f);
            MoralTreinador = Mathf.Clamp01(MoralTreinador - 0.1f);
        }

        PartidasJogadas++;

        if (PartidasJogadas >= 3)
        {
            ConcluirCicloDePartidas();
        }

        AtualizarDadosDoTime();
        LimitarBarras();
    }

    private static void ConcluirCicloDePartidas()
    {
        CicloFinalizado = true;

        if (VitoriasNoCiclo >= 2)
        {
            CicloVencido = true;

            if (NivelAtual < 3)
            {
                SubirDeNivel();
            }
            else
            {
                Debug.Log("Jogador concluiu o último nível.");
            }
        }
        else
        {
            CicloVencido = false;
            Debug.Log("Jogador perdeu o ciclo.");
        }

        PartidasJogadas = 0;
        VitoriasNoCiclo = 0;

        AtualizarDadosDoTime();
    }

    private static void SubirDeNivel()
    {
        if (NivelAtual < 3)
        {
            NivelAtual++;
            AtualizarDadosDoTime();
        }
    }

    private static void ProcessarFracassoCritico()
    {
        if (NivelAtual == 1)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            NivelAtual--;
            CiclosFracassados = 0;
            AtualizarDadosDoTime();
        }
    }

    public static void AtualizarDadosDoTime()
    {
        if (NivelAtual == 1)
        {
            TimeAtual = "União Vila Nova FC";
        }
        else if (NivelAtual == 2)
        {
            TimeAtual = "Atlético Vale Verde";
        }
        else if (NivelAtual == 3)
        {
            TimeAtual = "Esporte Clube Aurora Paulista";
        }
    }

    public static void LimitarBarras()
    {
        Felicidade = Mathf.Clamp01(Felicidade);
        Fama = Mathf.Clamp01(Fama);
        MoralTreinador = Mathf.Clamp01(MoralTreinador);
        MoralApostador = Mathf.Clamp01(MoralApostador);
    }
}