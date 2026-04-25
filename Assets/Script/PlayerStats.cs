using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // Dados de Carreira
    private int dinheiroAtual = 50;
    private int nivelAtual = 1;
    private int vitoriasNoCiclo = 0;
    private int partidasJogadas = 0;
    private string timeAtual = "União Vila Nova FC";

    // NOVO: Controle de Rebaixamento
    private int ciclosFracassados = 0; 

    // Valores das barras
    private float nivelFelicidade = 0.5f;
    private float nivelFama = 0.5f;
    private float nivelMoralTreinador = 0.5f;
    private float nivelMoralApostador = 0.5f;

    void Start()
    {
        AtualizarTudo();
    }

    public void FinalizarPartida(bool venceu)
    {
        partidasJogadas++;
        
        if (venceu) vitoriasNoCiclo++;

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
            ciclosFracassados = 0; // Resetamos as falhas pois ele teve sucesso
        }
        else
        {
            ciclosFracassados++;
            Debug.Log("Falha acumulada: " + ciclosFracassados);

            if (ciclosFracassados >= 2)
            {
                ProcessarFracassoCritico();
            }
        }

        // Reseta para a próxima sequência de 3 jogos
        partidasJogadas = 0;
        vitoriasNoCiclo = 0;
    }

    void SubirDeNivel()
    {
        if (nivelAtual < 3)
        {
            nivelAtual++;
            AtualizarDadosDoTime();
            Debug.Log("Subiu de Nível! Agora no Nível: " + nivelAtual);
        }
    }

    void ProcessarFracassoCritico()
    {
        if (nivelAtual == 1)
        {
            Debug.LogError("GAME OVER: Você foi demitido do time amador!");
            // Aqui você pode carregar uma cena de Game Over ou resetar o jogo
            // SceneManager.LoadScene("GameOver"); 
        }
        else
        {
            nivelAtual--;
            ciclosFracassados = 0; // Reseta para dar chance no nível inferior
            AtualizarDadosDoTime();
            Debug.Log("REBAIXADO! Você voltou para o Nível: " + nivelAtual);
        }
    }

    void AtualizarDadosDoTime()
    {
        // Centralizei a troca de nomes para facilitar
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
        if (textoPartidasContador != null) textoPartidasContador.text = partidasJogadas + "/3";

        if (Felicidade != null) Felicidade.fillAmount = nivelFelicidade;
        if (Fama != null) Fama.fillAmount = nivelFama;
        if (MoralTreinador != null) MoralTreinador.fillAmount = nivelMoralTreinador;
        if (MoralApostador != null) MoralApostador.fillAmount = nivelMoralApostador;
    }
}