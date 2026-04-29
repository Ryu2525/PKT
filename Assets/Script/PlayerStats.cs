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
        DadosDoJogador.AtualizarDadosDoTime();
        DadosDoJogador.LimitarBarras();

        AtualizarTudo();
    }

    public void JogarProximaPartida()
    {
        // Agora o Hub leva para a tela de pré-partida.
        // A pré-partida decide se vai carregar Jogo_Facil, Jogo_Medio ou Jogo_Dificil.
        SceneManager.LoadScene("PrePartida");
    }

    public void FinalizarPartida(bool venceu)
    {
        DadosDoJogador.RegistrarResultado(venceu);
        AtualizarTudo();
    }

    void AtualizarTudo()
    {
        if (textoDinheiro != null)
        {
            textoDinheiro.text = "R$ " + DadosDoJogador.DinheiroAtual;
        }

        if (textoNomeTime != null)
        {
            textoNomeTime.text = DadosDoJogador.TimeAtual;
        }

        if (textoNivel != null)
        {
            textoNivel.text = "NÍVEL: " + DadosDoJogador.NivelAtual;
        }

        if (textoVitorias != null)
        {
            textoVitorias.text = "Vitorias: " + DadosDoJogador.VitoriasNoCiclo;
        }

        if (textoPartidasContador != null)
        {
            textoPartidasContador.text = (DadosDoJogador.PartidasJogadas + 1) + "/3";
        }

        if (Felicidade != null)
        {
            Felicidade.fillAmount = DadosDoJogador.Felicidade;
        }

        if (Fama != null)
        {
            Fama.fillAmount = DadosDoJogador.Fama;
        }

        if (MoralTreinador != null)
        {
            MoralTreinador.fillAmount = DadosDoJogador.MoralTreinador;
        }

        if (MoralApostador != null)
        {
            MoralApostador.fillAmount = DadosDoJogador.MoralApostador;
        }
    }
}