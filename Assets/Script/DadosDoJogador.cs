using UnityEngine;
using UnityEngine.SceneManagement;

public static class DadosDoJogador
{
// ADICIONE ESTA LINHA PARA CORRIGIR OS ERROS:
    public static Sprite PersonagemSelecionado; 

    // As outras variáveis que já tínhamos:
    public static int dinheiroAtual = 50;
    public static int nivelAtual = 1;
    public static int vitoriasNoCiclo = 0;
    public static int partidasJogadas = 0; 
    public static string timeAtual = "União Vila Nova FC";
    public static int ciclosFracassados = 0; 

    public static float nivelFelicidade = 0.5f;
    public static float nivelFama = 0.5f;
    public static float nivelMoralTreinador = 0.5f;
    public static float nivelMoralApostador = 0.5f;

    // Lógica movida para cá para poder ser chamada direto do jogo
    public static void RegistrarResultado(bool venceu)
    {
        if (venceu) 
        {
            vitoriasNoCiclo++;
            // Exemplo: Aumenta fama e felicidade na vitória
            nivelFama = Mathf.Clamp(nivelFama + 0.1f, 0f, 1f);
            nivelFelicidade = Mathf.Clamp(nivelFelicidade + 0.1f, 0f, 1f);
        }
        else
        {
            // Exemplo: Diminui felicidade e moral no erro
            nivelFelicidade = Mathf.Clamp(nivelFelicidade - 0.1f, 0f, 1f);
            nivelMoralTreinador = Mathf.Clamp(nivelMoralTreinador - 0.1f, 0f, 1f);
        }
        
        partidasJogadas++;

        // Se terminou a 3ª partida
        if (partidasJogadas >= 3)
        {
            ConcluirCicloDePartidas();
        }
    }

    private static void ConcluirCicloDePartidas()
    {
        if (vitoriasNoCiclo >= 2)
        {
            SubirDeNivel();
            ciclosFracassados = 0;
        }
        else
        {
            ciclosFracassados++;
            if (ciclosFracassados >= 2) ProcessarFracassoCritico();
        }

        // Reseta o ciclo para as próximas 3 partidas
        partidasJogadas = 0;
        vitoriasNoCiclo = 0;
    }

    private static void SubirDeNivel()
    {
        if (nivelAtual < 3)
        {
            nivelAtual++;
            AtualizarDadosDoTime();
        }
    }

    private static void ProcessarFracassoCritico()
    {
        if (nivelAtual == 1) 
        {
            SceneManager.LoadScene("GameOver"); 
        }
        else
        {
            nivelAtual--;
            ciclosFracassados = 0;
            AtualizarDadosDoTime();
        }
    }

    private static void AtualizarDadosDoTime()
    {
        if (nivelAtual == 1) timeAtual = "União Vila Nova FC";
        else if (nivelAtual == 2) timeAtual = "Atlético Vale Verde";
        else if (nivelAtual == 3) timeAtual = "Esporte Clube Aurora Paulista";
    }
}