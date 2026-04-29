using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ResultadoPartidaManager : MonoBehaviour
{
    [Header("Avatar do Jogador")]
    public Image imagemAvatarJogador;

    [Header("Resumo da Partida")]
    public TextMeshProUGUI textoStatusObjetivo;
    public TextMeshProUGUI textoObjetivo;
    public TextMeshProUGUI textoDificuldade;

    [Header("Dinheiro")]
    public TextMeshProUGUI textoDinheiroGanho;
    public TextMeshProUGUI textoBonusRisco;
    public TextMeshProUGUI textoDinheiroTotal;

    [Header("Felicidade")]
    public TextMeshProUGUI textoFelicidadeAntes;
    public TextMeshProUGUI textoFelicidadeDepois;

    [Header("Fama")]
    public TextMeshProUGUI textoFamaAntes;
    public TextMeshProUGUI textoFamaDepois;

    [Header("Moral Treinador")]
    public TextMeshProUGUI textoMoralTreinadorAntes;
    public TextMeshProUGUI textoMoralTreinadorDepois;

    [Header("Moral Apostadores")]
    public TextMeshProUGUI textoMoralApostadorAntes;
    public TextMeshProUGUI textoMoralApostadorDepois;

    [Header("Barras Superiores")]
    public Image barraFelicidade;
    public Image barraFama;
    public Image barraMoralTreinador;
    public Image barraMoralApostador;

    void Start()
    {
        AtualizarTelaResultado();
    }

    void AtualizarTelaResultado()
    {
        MostrarAvatar();
        MostrarResumo();
        MostrarDinheiro();
        MostrarStatus();
        AtualizarBarrasSuperiores();
    }

    void MostrarAvatar()
    {
        if (imagemAvatarJogador != null && DadosDoJogador.PersonagemSelecionado != null)
        {
            imagemAvatarJogador.sprite = DadosDoJogador.PersonagemSelecionado;
            imagemAvatarJogador.enabled = true;
        }
    }

    void MostrarResumo()
    {
        if (textoStatusObjetivo != null)
            {
                textoStatusObjetivo.text = DadosDoJogador.UltimoObjetivoCumprido ? "SIM" : "NÃO";
            }

        if (textoObjetivo != null)
        {
            textoObjetivo.text = DadosDoJogador.ObjetivoEscolhido;
        }

        if (textoDificuldade != null)
        {
            textoDificuldade.text = ObterNomeDificuldade();
        }
    }

    void MostrarDinheiro()
    {
        if (textoDinheiroGanho != null)
        {
            if (DadosDoJogador.UltimoDinheiroGanho > 0)
            {
                textoDinheiroGanho.text = "+ " + DadosDoJogador.UltimoDinheiroGanho + " MOEDAS";
            }
            else
            {
                textoDinheiroGanho.text = "+ 0 MOEDAS";
            }
        }

        if (textoBonusRisco != null)
        {
            textoBonusRisco.text = DadosDoJogador.UltimoBonusRisco;
        }

        if (textoDinheiroTotal != null)
        {
            textoDinheiroTotal.text = "R$ " + DadosDoJogador.DinheiroAtual;
        }
    }

    void MostrarStatus()
    {
        if (textoFelicidadeAntes != null)
        {
            textoFelicidadeAntes.text = ConverterBarraParaTexto(DadosDoJogador.FelicidadeAntes);
        }

        if (textoFelicidadeDepois != null)
        {
            textoFelicidadeDepois.text = ConverterBarraParaTexto(DadosDoJogador.FelicidadeDepois);
        }

        if (textoFamaAntes != null)
        {
            textoFamaAntes.text = ConverterBarraParaTexto(DadosDoJogador.FamaAntes);
        }

        if (textoFamaDepois != null)
        {
            textoFamaDepois.text = ConverterBarraParaTexto(DadosDoJogador.FamaDepois);
        }

        if (textoMoralTreinadorAntes != null)
        {
            textoMoralTreinadorAntes.text = ConverterBarraParaTexto(DadosDoJogador.MoralTreinadorAntes);
        }

        if (textoMoralTreinadorDepois != null)
        {
            textoMoralTreinadorDepois.text = ConverterBarraParaTexto(DadosDoJogador.MoralTreinadorDepois);
        }

        if (textoMoralApostadorAntes != null)
        {
            textoMoralApostadorAntes.text = ConverterBarraParaTexto(DadosDoJogador.MoralApostadorAntes);
        }

        if (textoMoralApostadorDepois != null)
        {
            textoMoralApostadorDepois.text = ConverterBarraParaTexto(DadosDoJogador.MoralApostadorDepois);
        }
    }

    void AtualizarBarrasSuperiores()
    {
        DadosDoJogador.LimitarBarras();

        if (barraFelicidade != null)
        {
            barraFelicidade.fillAmount = DadosDoJogador.Felicidade;
        }

        if (barraFama != null)
        {
            barraFama.fillAmount = DadosDoJogador.Fama;
        }

        if (barraMoralTreinador != null)
        {
            barraMoralTreinador.fillAmount = DadosDoJogador.MoralTreinador;
        }

        if (barraMoralApostador != null)
        {
            barraMoralApostador.fillAmount = DadosDoJogador.MoralApostador;
        }
    }

    string ConverterBarraParaTexto(float valor)
    {
        int valorConvertido = Mathf.RoundToInt(valor * 100f);
        return valorConvertido + "/100";
    }

    string ObterNomeDificuldade()
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

    public void VoltarParaHub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void ProximaPartida()
    {
        if (DadosDoJogador.CicloFinalizado)
        {
            if (DadosDoJogador.CicloVencido)
            {
                Debug.Log("Ciclo vencido. Voltando para o Hub.");
                SceneManager.LoadScene("Hub");
            }
            else
            {
                Debug.Log("Ciclo perdido. Voltando para a Home.");
                SceneManager.LoadScene("Home");
            }

            return;
        }

        SceneManager.LoadScene("PrePartida");
    }

    public void IrParaLoja()
    {
        SceneManager.LoadScene("Loja");
    }
}