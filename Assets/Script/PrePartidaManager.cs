using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PrePartidaManager : MonoBehaviour
{
    [Header("Textos dos Cards")]
    public TextMeshProUGUI textoTreinador1;
    public TextMeshProUGUI textoTreinador2;
    public TextMeshProUGUI textoApostador1;
    public TextMeshProUGUI textoApostador2;

    [Header("Status Superior")]
    public Image barraFelicidade;
    public Image barraFama;
    public Image barraMoralTreinador;
    public Image barraMoralApostador;
    public TextMeshProUGUI textoDinheiro;

    [Header("Botão Confirmar")]
    public Button botaoConfirmar;

    private List<ObjetivoPrePartida> objetivosNaTela = new List<ObjetivoPrePartida>();
    private ObjetivoPrePartida objetivoSelecionado;

    void Start()
    {
        DadosDoJogador.AtualizarDadosDoTime();
        DadosDoJogador.LimitarBarras();

        AtualizarStatusSuperior();

        SortearObjetivos();
        PreencherCards();

        if (botaoConfirmar != null)
        {
            botaoConfirmar.interactable = false;
        }
    }

    void AtualizarStatusSuperior()
    {
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

        if (textoDinheiro != null)
        {
            textoDinheiro.text = "R$ " + DadosDoJogador.DinheiroAtual;
        }
    }

    void SortearObjetivos()
    {
        int faseAtual = DadosDoJogador.PartidasJogadas + 1;

        List<ObjetivoPrePartida> objetivosTreinador = CriarObjetivosTreinador(faseAtual);
        List<ObjetivoPrePartida> objetivosApostadores = CriarObjetivosApostadores(faseAtual);

        List<ObjetivoPrePartida> treinadoresSorteados = SortearQuantidade(objetivosTreinador, 2);
        List<ObjetivoPrePartida> apostadoresSorteados = SortearQuantidade(objetivosApostadores, 2);

        objetivosNaTela.Clear();

        // Lado esquerdo da tela: TREINADOR
        objetivosNaTela.Add(treinadoresSorteados[0]);
        objetivosNaTela.Add(treinadoresSorteados[1]);

        // Lado direito da tela: APOSTADORES
        objetivosNaTela.Add(apostadoresSorteados[0]);
        objetivosNaTela.Add(apostadoresSorteados[1]);

        Debug.Log("Fase atual da pré-partida: " + faseAtual);
    }

    List<ObjetivoPrePartida> SortearQuantidade(List<ObjetivoPrePartida> listaOriginal, int quantidade)
    {
        List<ObjetivoPrePartida> copia = new List<ObjetivoPrePartida>(listaOriginal);
        List<ObjetivoPrePartida> sorteados = new List<ObjetivoPrePartida>();

        for (int i = 0; i < quantidade; i++)
        {
            int indiceAleatorio = Random.Range(0, copia.Count);
            sorteados.Add(copia[indiceAleatorio]);
            copia.RemoveAt(indiceAleatorio);
        }

        return sorteados;
    }

    List<ObjetivoPrePartida> CriarObjetivosTreinador(int faseAtual)
    {
        List<ObjetivoPrePartida> lista = new List<ObjetivoPrePartida>();

        if (faseAtual == 1)
        {
            // FASE 1: obrigatoriamente passa para companheiro e depois finaliza.
            lista.Add(new ObjetivoPrePartida(
                "Dar 1 assistência",
                "Treinador",
                "ASSISTENCIA",
                10,
                1,
                0.10f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Marcar 1 gol",
                "Treinador",
                "GOL",
                10,
                1,
                0.10f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Acertar 1 chute no gol",
                "Treinador",
                "CHUTE_NO_GOL",
                15,
                1,
                0.15f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Não chutar para fora",
                "Treinador",
                "NAO_CHUTAR_FORA",
                15,
                0,
                0.15f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Vencer a partida",
                "Treinador",
                "VITORIA",
                15,
                1,
                0.15f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Fazer gol na primeira tentativa",
                "Treinador",
                "GOL_PRIMEIRA_TENTATIVA",
                20,
                1,
                0.20f,
                0.10f,
                0f
            ));
        }
        else if (faseAtual == 2)
        {
            // FASE 2: apenas passe. Não tem chute e começa 1x0 para o adversário.
            // Evitamos objetivos de gol, chute, vitória e derrota automática.

            lista.Add(new ObjetivoPrePartida(
                "Dar 1 assistência",
                "Treinador",
                "ASSISTENCIA",
                10,
                1,
                0.10f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Completar 1 passe correto",
                "Treinador",
                "ASSISTENCIA",
                15,
                1,
                0.15f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Passar sem erro",
                "Treinador",
                "ASSISTENCIA",
                15,
                1,
                0.15f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Cumprir a jogada de passe",
                "Treinador",
                "ASSISTENCIA",
                20,
                1,
                0.20f,
                0.10f,
                0f
            ));
        }
        else
        {
            // FASE 3: passa uma vez, passa de novo e depois finaliza.

            lista.Add(new ObjetivoPrePartida(
                "Dar 1 assistência",
                "Treinador",
                "ASSISTENCIA",
                10,
                1,
                0.10f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Dar 2 assistências",
                "Treinador",
                "ASSISTENCIA",
                20,
                2,
                0.20f,
                0.10f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Marcar 1 gol",
                "Treinador",
                "GOL",
                10,
                1,
                0.10f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Acertar 1 chute no gol",
                "Treinador",
                "CHUTE_NO_GOL",
                15,
                1,
                0.15f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Não chutar para fora",
                "Treinador",
                "NAO_CHUTAR_FORA",
                15,
                0,
                0.15f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Vencer a partida",
                "Treinador",
                "VITORIA",
                15,
                1,
                0.15f,
                0.05f,
                0f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Fazer gol na primeira tentativa",
                "Treinador",
                "GOL_PRIMEIRA_TENTATIVA",
                20,
                1,
                0.20f,
                0.10f,
                0f
            ));
        }

        return lista;
    }

    List<ObjetivoPrePartida> CriarObjetivosApostadores(int faseAtual)
    {
        List<ObjetivoPrePartida> lista = new List<ObjetivoPrePartida>();

        if (faseAtual == 1)
        {
            // FASE 1: passe + finalização.
            // Pode errar passe, chutar fora, bater no goleiro, bater em inimigo ou não marcar gol.

            lista.Add(new ObjetivoPrePartida(
                "Errar 1 passe",
                "Apostador",
                "ERRO_PASSE",
                10,
                1,
                0.10f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Errar 2 passes",
                "Apostador",
                "ERRO_PASSE",
                15,
                2,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Chutar 1 vez para fora",
                "Apostador",
                "CHUTE_FORA",
                10,
                1,
                0.10f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Chutar 2 vezes para fora",
                "Apostador",
                "CHUTE_FORA",
                15,
                2,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Não marcar gol",
                "Apostador",
                "NAO_MARCAR_GOL",
                15,
                0,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Bater a bola no goleiro 1 vez",
                "Apostador",
                "BATER_GOLEIRO",
                10,
                1,
                0.10f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Bater a bola no goleiro 2 vezes",
                "Apostador",
                "BATER_GOLEIRO",
                15,
                2,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Bater a bola em um inimigo",
                "Apostador",
                "BATER_INIMIGO",
                15,
                1,
                0.15f,
                0.05f,
                0.05f
            ));
        }
        else if (faseAtual == 2)
        {
            // FASE 2: apenas passe.
            // Não colocamos chutar fora, goleiro, gol, vitória ou derrota.

            lista.Add(new ObjetivoPrePartida(
                "Errar 1 passe",
                "Apostador",
                "ERRO_PASSE",
                10,
                1,
                0.10f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Errar 2 passes",
                "Apostador",
                "ERRO_PASSE",
                15,
                2,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Errar 3 passes",
                "Apostador",
                "ERRO_PASSE",
                20,
                3,
                0.20f,
                0.10f,
                0.10f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Bater a bola em um inimigo",
                "Apostador",
                "BATER_INIMIGO",
                15,
                1,
                0.15f,
                0.05f,
                0.05f
            ));
        }
        else
        {
            // FASE 3: passe + passe + finalização.

            lista.Add(new ObjetivoPrePartida(
                "Errar 1 passe",
                "Apostador",
                "ERRO_PASSE",
                10,
                1,
                0.10f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Errar 2 passes",
                "Apostador",
                "ERRO_PASSE",
                15,
                2,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Errar 3 passes",
                "Apostador",
                "ERRO_PASSE",
                20,
                3,
                0.20f,
                0.10f,
                0.10f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Chutar 1 vez para fora",
                "Apostador",
                "CHUTE_FORA",
                10,
                1,
                0.10f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Chutar 2 vezes para fora",
                "Apostador",
                "CHUTE_FORA",
                15,
                2,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Não marcar gol",
                "Apostador",
                "NAO_MARCAR_GOL",
                15,
                0,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Bater a bola no goleiro 1 vez",
                "Apostador",
                "BATER_GOLEIRO",
                10,
                1,
                0.10f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Bater a bola no goleiro 2 vezes",
                "Apostador",
                "BATER_GOLEIRO",
                15,
                2,
                0.15f,
                0.05f,
                0.05f
            ));

            lista.Add(new ObjetivoPrePartida(
                "Bater a bola em um inimigo",
                "Apostador",
                "BATER_INIMIGO",
                15,
                1,
                0.15f,
                0.05f,
                0.05f
            ));
        }

        return lista;
    }

    void PreencherCards()
    {
        if (textoTreinador1 != null)
        {
            textoTreinador1.text = objetivosNaTela[0].GerarTextoDoCard();
        }

        if (textoTreinador2 != null)
        {
            textoTreinador2.text = objetivosNaTela[1].GerarTextoDoCard();
        }

        if (textoApostador1 != null)
        {
            textoApostador1.text = objetivosNaTela[2].GerarTextoDoCard();
        }

        if (textoApostador2 != null)
        {
            textoApostador2.text = objetivosNaTela[3].GerarTextoDoCard();
        }
    }

    public void SelecionarObjetivoCard1()
    {
        SelecionarObjetivo(0);
    }

    public void SelecionarObjetivoCard2()
    {
        SelecionarObjetivo(1);
    }

    public void SelecionarObjetivoCard3()
    {
        SelecionarObjetivo(2);
    }

    public void SelecionarObjetivoCard4()
    {
        SelecionarObjetivo(3);
    }

    void SelecionarObjetivo(int indice)
    {
        if (indice < 0 || indice >= objetivosNaTela.Count)
        {
            Debug.LogWarning("Índice de objetivo inválido.");
            return;
        }

        objetivoSelecionado = objetivosNaTela[indice];

        // Destacar visualmente
        DestacarCard(indice);

        if (botaoConfirmar != null)
        {
            botaoConfirmar.interactable = true;
        }

        Debug.Log("Objetivo selecionado: " + objetivoSelecionado.nome);
    }

    void DestacarCard(int indice)
    {
        string[] nomesCards =
        {
            "ObjetivoTreinador1",
            "ObjetivoTreinador2",
            "ObjetivoApostador1",
            "ObjetivoApostador2"
        };

        // Reseta todos os cards
        for (int i = 0; i < nomesCards.Length; i++)
        {
            GameObject card = GameObject.Find(nomesCards[i]);

            if (card != null)
            {
                Image img = card.GetComponent<Image>();

                if (img != null)
                {
                    img.color = new Color(1f, 1f, 1f, 0f);
                }
            }
        }

        // Destaca o selecionado
        GameObject cardSelecionado = GameObject.Find(nomesCards[indice]);

        if (cardSelecionado != null)
        {
            Image img = cardSelecionado.GetComponent<Image>();

            if (img != null)
            {
                img.color = new Color(0.6f, 1f, 0.6f, 0.5f);
            }
        }
    }


    public void ConfirmarEscolha()
    {
        if (objetivoSelecionado == null)
        {
            Debug.Log("Nenhum objetivo foi selecionado.");
            return;
        }

        DadosDoJogador.ObjetivoEscolhido = objetivoSelecionado.nome;
        DadosDoJogador.TipoObjetivo = objetivoSelecionado.tipo;
        DadosDoJogador.AcaoObjetivo = objetivoSelecionado.acao;

        DadosDoJogador.RecompensaObjetivo = objetivoSelecionado.recompensa;
        DadosDoJogador.QuantidadeNecessaria = objetivoSelecionado.quantidadeNecessaria;

        DadosDoJogador.ImpactoPositivoObjetivo = objetivoSelecionado.impactoPositivo;
        DadosDoJogador.PenalidadeObjetivo = objetivoSelecionado.penalidade;
        DadosDoJogador.PenalidadeTreinadorExtra = objetivoSelecionado.penalidadeTreinadorExtra;

        Debug.Log("Objetivo confirmado: " + DadosDoJogador.ObjetivoEscolhido);
        Debug.Log("Tipo: " + DadosDoJogador.TipoObjetivo);
        Debug.Log("Ação: " + DadosDoJogador.AcaoObjetivo);
        Debug.Log("Quantidade necessária: " + DadosDoJogador.QuantidadeNecessaria);

        CarregarPartidaCorreta();
    }

    void CarregarPartidaCorreta()
    {
        string dificuldade;

        if (DadosDoJogador.NivelAtual == 1)
        {
            dificuldade = "Facil";
        }
        else if (DadosDoJogador.NivelAtual == 2)
        {
            dificuldade = "Medio";
        }
        else
        {
            dificuldade = "Dificil";
        }

        int rodada = DadosDoJogador.PartidasJogadas + 1;
        string nomeCena = "Jogo_" + dificuldade + "_" + rodada;

        Debug.Log("Carregando cena: " + nomeCena);
        SceneManager.LoadScene(nomeCena);
    }

    public void VoltarParaHub()
    {
        SceneManager.LoadScene("Hub");
    }
}

[System.Serializable]
public class ObjetivoPrePartida
{
    public string nome;
    public string tipo;
    public string acao;

    public int recompensa;
    public int quantidadeNecessaria;

    public float impactoPositivo;
    public float penalidade;
    public float penalidadeTreinadorExtra;

    public ObjetivoPrePartida(
        string nome,
        string tipo,
        string acao,
        int recompensa,
        int quantidadeNecessaria,
        float impactoPositivo,
        float penalidade,
        float penalidadeTreinadorExtra
    )
    {
        this.nome = nome;
        this.tipo = tipo;
        this.acao = acao;
        this.recompensa = recompensa;
        this.quantidadeNecessaria = quantidadeNecessaria;
        this.impactoPositivo = impactoPositivo;
        this.penalidade = penalidade;
        this.penalidadeTreinadorExtra = penalidadeTreinadorExtra;
    }

    public string GerarTextoDoCard()
    {
        string texto = nome.ToUpper() + "\n\n";

        texto += "RECOMPENSA: " + recompensa + " MOEDAS\n";

        if (tipo == "Treinador")
        {
            texto += "CUMPRIU: TREINADOR +" + ConverterParaPorcentagem(impactoPositivo) + "%\n";
            texto += "FALHOU: TREINADOR -" + ConverterParaPorcentagem(penalidade) + "%";
        }
        else
        {
            texto += "CUMPRIU: APOST. +" + ConverterParaPorcentagem(impactoPositivo) + "%";

            if (penalidadeTreinadorExtra > 0f)
            {
                texto += " / TREIN. -" + ConverterParaPorcentagem(penalidadeTreinadorExtra) + "%";
            }

            texto += "\nFALHOU: APOST. -" + ConverterParaPorcentagem(penalidade) + "%";
        }

        return texto;
    }

    int ConverterParaPorcentagem(float valor)
    {
        return Mathf.RoundToInt(valor * 100f);
    }
}