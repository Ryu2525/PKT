using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    void Start() 
    { 
        // Quando o Hub carregar, ele pega os dados globais atualizados
        AtualizarTudo(); 
    }

    public void JogarProximaPartida()
    {
        // Pega o nível atual lá dos Dados Globais
        string dificuldade = (DadosDoJogador.nivelAtual == 1) ? "Facil" : (DadosDoJogador.nivelAtual == 2) ? "Medio" : "Dificil";
        
        int rodada = DadosDoJogador.partidasJogadas + 1;
        
        SceneManager.LoadScene("Jogo_" + dificuldade + "_" + rodada);
    }

    void AtualizarTudo()
    {
        // Lê tudo diretamente de DadosDoJogador
        if (textoDinheiro != null) textoDinheiro.text = "R$ " + DadosDoJogador.dinheiroAtual;
        if (textoNomeTime != null) textoNomeTime.text = DadosDoJogador.timeAtual;
        if (textoNivel != null) textoNivel.text = "NÍVEL: " + DadosDoJogador.nivelAtual;
        if (textoVitorias != null) textoVitorias.text = "Vitorias: " + DadosDoJogador.vitoriasNoCiclo;
        
        if (textoPartidasContador != null) 
            textoPartidasContador.text = (DadosDoJogador.partidasJogadas + 1) + "/3";

        if (Felicidade != null) Felicidade.fillAmount = DadosDoJogador.nivelFelicidade;
        if (Fama != null) Fama.fillAmount = DadosDoJogador.nivelFama;
        if (MoralTreinador != null) MoralTreinador.fillAmount = DadosDoJogador.nivelMoralTreinador;
        if (MoralApostador != null) MoralApostador.fillAmount = DadosDoJogador.nivelMoralApostador;
    }
}