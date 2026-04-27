using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Adicionado para carregar cenas

public class PlayerStats : MonoBehaviour
{
    [Header("Barras de UI")]
    public Image Felicidade;
    public Image Fama;
    public Image MoralTreinador;
    public Image MoralApostador;

    [Header("Economia e Textos")]
    public TextMeshProUGUI textoDinheiro;
    public TextMeshProUGUI textoNomeTime;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoVitorias;
    public TextMeshProUGUI textoPartidasContador;

    private int dinheiroAtual = 50;
    private int nivelAtual = 1;
    private int vitoriasNoCiclo = 0;
    private int partidasJogadas = 0; 
    private string timeAtual = "União Vila Nova FC";
    private int ciclosFracassados = 0; 

    private float nivelFelicidade = 0.5f;
    private float nivelFama = 0.5f;
    private float nivelMoralTreinador = 0.5f;
    private float nivelMoralApostador = 0.5f;

    void Start() { AtualizarTudo(); }

    // Chamado pelo botão "CONFIRMAR" no HUB
    public void JogarProximaPartida()
    {
        string dificuldade = (nivelAtual == 1) ? "Facil" : (nivelAtual == 2) ? "Medio" : "Dificil";
        int rodada = partidasJogadas + 1;
        
        // Certifique-se de que suas cenas no Build Settings tenham esses nomes exatos
        SceneManager.LoadScene("Jogo_" + dificuldade + "_" + rodada);
    }

    public void FinalizarPartida(bool venceu)
    {
        if (venceu) vitoriasNoCiclo++;
        
        partidasJogadas++;

        // Se terminou a 3ª partida (índice 2 que virou 3)
        if (partidasJogadas >= 3)
        {
            ConcluirCicloDePartidas();
        }

        AtualizarTudo();
    }

    void ConcluirCicloDePartidas()
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

        partidasJogadas = 0;
        vitoriasNoCiclo = 0;
    }

    void SubirDeNivel()
    {
        if (nivelAtual < 3)
        {
            nivelAtual++;
            AtualizarDadosDoTime();
        }
    }

    void ProcessarFracassoCritico()
    {
        if (nivelAtual == 1) SceneManager.LoadScene("GameOver"); 
        else
        {
            nivelAtual--;
            ciclosFracassados = 0;
            AtualizarDadosDoTime();
        }
    }

    void AtualizarDadosDoTime()
    {
        if (nivelAtual == 1) timeAtual = "União Vila Nova FC";
        else if (nivelAtual == 2) timeAtual = "Atlético Vale Verde";
        else if (nivelAtual == 3) timeAtual = "Esporte Clube Aurora Paulista";
    }

    void AtualizarTudo()
    {
        if (textoDinheiro != null) textoDinheiro.text = "R$ " + dinheiroAtual;
        if (textoNomeTime != null) textoNomeTime.text = timeAtual;
        if (textoNivel != null) textoNivel.text = "NÍVEL: " + nivelAtual;
        if (textoVitorias != null) textoVitorias.text = "Vitorias: " + vitoriasNoCiclo;
        
        // EXIBIÇÃO: Mostra 1/3, 2/3 ou 3/3 para o jogador
        if (textoPartidasContador != null) 
            textoPartidasContador.text = (partidasJogadas + 1) + "/3";

        if (Felicidade != null) Felicidade.fillAmount = nivelFelicidade;
        if (Fama != null) Fama.fillAmount = nivelFama;
        if (MoralTreinador != null) MoralTreinador.fillAmount = nivelMoralTreinador;
        if (MoralApostador != null) MoralApostador.fillAmount = nivelMoralApostador;
    }
}
