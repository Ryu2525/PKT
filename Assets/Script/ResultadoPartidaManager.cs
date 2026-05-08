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
        DadosDoJogador.LimitarBarras();

        // 1. Checagem de DERROTA CRÍTICA (Barras zeradas)
        if (DadosDoJogador.Felicidade <= 0f || DadosDoJogador.MoralTreinador <= 0f)
        {
            SceneManager.LoadScene("FimDeCarreira");
            return;
        }

        // 2. Checagem de FIM DE CICLO (3 Partidas jogadas)
        if (DadosDoJogador.CicloFinalizado)
        {
            if (DadosDoJogador.CicloVencido)
            {
                // VERIFICAÇÃO DE VITÓRIA DEFINITIVA (Sua nova lógica aqui)
                // Se ele está no nível 3 E as barras estão cheias
                if (DadosDoJogador.VitoriaDefinitiva)
                {
                    SceneManager.LoadScene("FimVitoria");
                    return;
                }

                // Se venceu mas não é a vitória definitiva, volta pro Hub
                SceneManager.LoadScene("Hub");
            }
            else
            {
                // Se perdeu o ciclo, decide se cai de nível ou se é Fim de Jogo (Nível 1)
                if (DadosDoJogador.NivelAtual == 1)
                {
                    SceneManager.LoadScene("FimFase1");
                }
                else
                {
                    // Rebaixamento (Nível 2 cai para 1, Nível 3 cai para 2 ou fica no 3 conforme sua regra)
                    DadosDoJogador.ExecutarFracassoOuRebaixamento(); 
                }
            }
            return;
        }

        // 3. FLUXO NORMAL (Interrogatório ou Eventos entre partidas 1 e 2)
        if (DeveAcontecerInterrogatorio())
        {
            SceneManager.LoadScene("Interrogatorio");
            return;
        }

        int chanceEvento = Random.Range(1, 101);
        if (chanceEvento <= 35)
        {
            SceneManager.LoadScene("EventoAleatorio");
        }
        else
        {
            SceneManager.LoadScene("PrePartida");
        }
    }
    bool DeveAcontecerInterrogatorio()
    {
        int chanceInterrogatorio = 5;

        if (DadosDoJogador.Fama >= 0.9f)
        {
            chanceInterrogatorio += 30;
        }
        else if (DadosDoJogador.Fama >= 0.7f)
        {
            chanceInterrogatorio += 20;
        }
        else if (DadosDoJogador.Fama >= 0.5f)
        {
            chanceInterrogatorio += 10;
        }

        chanceInterrogatorio += Mathf.RoundToInt(DadosDoJogador.RiscoInterrogatorio * 45f);

        chanceInterrogatorio = Mathf.Clamp(chanceInterrogatorio, 0, 75);

        int sorteio = Random.Range(1, 101);

        Debug.Log("Chance interrogatório: " + chanceInterrogatorio + "% | Sorteio: " + sorteio);

        return sorteio <= chanceInterrogatorio;
    }
    public void IrParaLoja()
    {
        SceneManager.LoadScene("Loja");
    }
}