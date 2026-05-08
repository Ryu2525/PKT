using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EventoAleatorioManager : MonoBehaviour
{
    [Header("Cards das Opções")]
    public Image cardOpcaoA;
    public Image cardOpcaoB;

    [Header("Status Superior")]
    public Image barraFelicidade;
    public Image barraFama;
    public Image barraMoralTreinador;
    public Image barraMoralApostador;
    public TextMeshProUGUI textoDinheiroTotal;

    [Header("Texto do Evento")]
    public TextMeshProUGUI textoTituloEvento;
    public TextMeshProUGUI textoDescricaoEvento;

    [Header("Opção A")]
    public TextMeshProUGUI tituloOpcaoA;
    public TextMeshProUGUI impactoA1;
    public TextMeshProUGUI impactoA2;
    public TextMeshProUGUI impactoA3;
    public TextMeshProUGUI impactoA4;

    [Header("Opção B")]
    public TextMeshProUGUI tituloOpcaoB;
    public TextMeshProUGUI impactoB1;
    public TextMeshProUGUI impactoB2;
    public TextMeshProUGUI impactoB3;
    public TextMeshProUGUI impactoB4;

    [Header("Botão Confirmar")]
    public Button botaoConfirmar;

    private EventoMoral eventoAtual;
    private int opcaoSelecionada = 0;
    // 0 = nenhuma
    // 1 = opção A
    // 2 = opção B

    void Start()
    {
        AtualizarStatusSuperior();
        SortearEvento();

        if (botaoConfirmar != null)
        {
            botaoConfirmar.interactable = false;
        }
    }

    void AtualizarStatusSuperior()
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

        if (textoDinheiroTotal != null)
        {
            textoDinheiroTotal.text = "R$ " + DadosDoJogador.DinheiroAtual;
        }
    }

    void SortearEvento()
    {
        List<EventoMoral> eventos = CriarEventosPossiveis();

        int indiceAleatorio = Random.Range(0, eventos.Count);
        eventoAtual = eventos[indiceAleatorio];

        PreencherTelaComEvento();

        Debug.Log("Evento sorteado: " + eventoAtual.nomeInterno);
    }

    void DestacarOpcao(int opcao)
    {
        // Remove cor dos dois cards
        if (cardOpcaoA != null)
        {
            cardOpcaoA.color = new Color(1f, 1f, 1f, 0f);
        }

        if (cardOpcaoB != null)
        {
            cardOpcaoB.color = new Color(1f, 1f, 1f, 0f);
        }

        // Destaca a opção escolhida
        if (opcao == 1 && cardOpcaoA != null)
        {
            cardOpcaoA.color = new Color(0.6f, 1f, 0.6f, 0.5f);
        }
        else if (opcao == 2 && cardOpcaoB != null)
        {
            cardOpcaoB.color = new Color(0.6f, 1f, 0.6f, 0.5f);
        }
    }

    List<EventoMoral> CriarEventosPossiveis()
    {
        List<EventoMoral> eventos = new List<EventoMoral>();

        eventos.Add(CriarEventoFamilia());
        eventos.Add(CriarEventoEmpresario());
        eventos.Add(CriarEventoDivida());
        eventos.Add(CriarEventoPresente());
        eventos.Add(CriarEventoLesao());
        eventos.Add(CriarEventoTreinador());
        eventos.Add(CriarEventoImprensa());
        eventos.Add(CriarEventoAmigo());
        eventos.Add(CriarEventoPatrocinio());
        eventos.Add(CriarEventoMoradia());

        return eventos;
    }

    EventoMoral CriarEventoFamilia()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Família em dificuldade";

        evento.descricao =
            "Sua família está passando por dificuldades financeiras. Um conhecido dos apostadores oferece dinheiro rápido em troca de uma aproximação perigosa. Você prefere recusar o acordo ou aceitar a proposta?";

        evento.tituloOpcaoA = "RECUSAR O ACORDO";
        evento.tituloOpcaoB = "ACEITAR O ACORDO";

        evento.felicidadeA = Random.Range(8, 16);
        evento.dinheiroA = -Random.Range(15, 31);
        evento.famaA = Random.Range(3, 9);
        evento.riscoA = -Random.Range(5, 13);

        evento.dinheiroB = Random.Range(35, 61);
        evento.moralApostadorB = Random.Range(10, 21);
        evento.famaB = -Random.Range(3, 11);
        evento.riscoB = Random.Range(10, 21);

        return evento;
    }

    EventoMoral CriarEventoEmpresario()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Empresário suspeito";

        evento.descricao =
            "Um empresário suspeito promete divulgar seu nome para clubes maiores, mas pede que você aceite se aproximar de pessoas ligadas às apostas. A decisão pode acelerar sua carreira, mas também aumentar seu risco.";

        evento.tituloOpcaoA = "RECUSAR PROPOSTA";
        evento.tituloOpcaoB = "ACEITAR PROPOSTA";

        evento.felicidadeA = Random.Range(3, 9);
        evento.dinheiroA = -Random.Range(5, 16);
        evento.famaA = Random.Range(2, 7);
        evento.riscoA = -Random.Range(5, 11);

        evento.dinheiroB = Random.Range(20, 46);
        evento.moralApostadorB = Random.Range(8, 16);
        evento.famaB = Random.Range(5, 13);
        evento.riscoB = Random.Range(10, 18);

        return evento;
    }

    EventoMoral CriarEventoDivida()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Dívida da família";

        evento.descricao =
            "Uma dívida antiga da sua família voltou a aparecer. Você pode pagar parte dela com suas economias ou aceitar ajuda dos apostadores para resolver tudo rapidamente.";

        evento.tituloOpcaoA = "PAGAR COM ECONOMIAS";
        evento.tituloOpcaoB = "ACEITAR AJUDA";

        evento.felicidadeA = Random.Range(8, 14);
        evento.dinheiroA = -Random.Range(25, 46);
        evento.famaA = Random.Range(1, 6);
        evento.riscoA = -Random.Range(4, 10);

        evento.dinheiroB = Random.Range(30, 56);
        evento.moralApostadorB = Random.Range(12, 21);
        evento.famaB = -Random.Range(4, 10);
        evento.riscoB = Random.Range(12, 22);

        return evento;
    }

    EventoMoral CriarEventoPresente()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Presente dos apostadores";

        evento.descricao =
            "Os apostadores enviam um presente caro para tentar ganhar sua confiança. Você pode devolver o presente ou aceitar e se aproximar ainda mais deles.";

        evento.tituloOpcaoA = "DEVOLVER PRESENTE";
        evento.tituloOpcaoB = "ACEITAR PRESENTE";

        evento.felicidadeA = Random.Range(2, 8);
        evento.dinheiroA = -Random.Range(0, 11);
        evento.famaA = Random.Range(4, 10);
        evento.riscoA = -Random.Range(6, 13);

        evento.dinheiroB = Random.Range(25, 51);
        evento.moralApostadorB = Random.Range(10, 18);
        evento.famaB = -Random.Range(3, 9);
        evento.riscoB = Random.Range(10, 18);

        return evento;
    }

    EventoMoral CriarEventoLesao()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Lesão antes da partida";

        evento.descricao =
            "Você sente dores antes da próxima partida. O treinador recomenda cuidado, mas os apostadores sugerem jogar mesmo assim em troca de dinheiro extra.";

        evento.tituloOpcaoA = "SEGUIR TREINADOR";
        evento.tituloOpcaoB = "FORÇAR O JOGO";

        evento.felicidadeA = Random.Range(4, 10);
        evento.dinheiroA = -Random.Range(5, 16);
        evento.famaA = Random.Range(2, 8);
        evento.riscoA = -Random.Range(4, 10);

        evento.dinheiroB = Random.Range(25, 46);
        evento.moralApostadorB = Random.Range(8, 16);
        evento.famaB = -Random.Range(2, 8);
        evento.riscoB = Random.Range(8, 16);

        return evento;
    }

    EventoMoral CriarEventoTreinador()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Conversa com treinador";

        evento.descricao =
            "O treinador percebe rumores sobre sua aproximação com apostadores e chama você para conversar. Você pode ser transparente ou tentar esconder a situação.";

        evento.tituloOpcaoA = "SER TRANSPARENTE";
        evento.tituloOpcaoB = "ESCONDER SITUAÇÃO";

        evento.felicidadeA = Random.Range(3, 9);
        evento.dinheiroA = -Random.Range(0, 11);
        evento.famaA = Random.Range(2, 7);
        evento.riscoA = -Random.Range(6, 13);

        evento.dinheiroB = Random.Range(15, 36);
        evento.moralApostadorB = Random.Range(6, 14);
        evento.famaB = -Random.Range(4, 11);
        evento.riscoB = Random.Range(8, 18);

        return evento;
    }

    EventoMoral CriarEventoImprensa()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Pergunta da imprensa";

        evento.descricao =
            "Um jornalista faz perguntas sobre sua evolução repentina e possíveis ligações fora de campo. Você pode responder com calma ou aceitar ajuda dos apostadores para controlar a narrativa.";

        evento.tituloOpcaoA = "RESPONDER COM CALMA";
        evento.tituloOpcaoB = "USAR INFLUÊNCIA";

        evento.felicidadeA = Random.Range(2, 8);
        evento.dinheiroA = -Random.Range(0, 8);
        evento.famaA = Random.Range(5, 12);
        evento.riscoA = -Random.Range(3, 9);

        evento.dinheiroB = Random.Range(20, 41);
        evento.moralApostadorB = Random.Range(8, 16);
        evento.famaB = Random.Range(2, 8);
        evento.riscoB = Random.Range(10, 20);

        return evento;
    }

    EventoMoral CriarEventoAmigo()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Amigo pede ajuda";

        evento.descricao =
            "Um amigo próximo pede dinheiro para resolver um problema urgente. Os apostadores oferecem pagar essa ajuda se você aceitar ficar devendo um favor.";

        evento.tituloOpcaoA = "AJUDAR COM SEU DINHEIRO";
        evento.tituloOpcaoB = "ACEITAR FAVOR";

        evento.felicidadeA = Random.Range(10, 18);
        evento.dinheiroA = -Random.Range(20, 41);
        evento.famaA = Random.Range(2, 8);
        evento.riscoA = -Random.Range(4, 10);

        evento.dinheiroB = Random.Range(25, 51);
        evento.moralApostadorB = Random.Range(10, 20);
        evento.famaB = -Random.Range(2, 9);
        evento.riscoB = Random.Range(12, 22);

        return evento;
    }

    EventoMoral CriarEventoPatrocinio()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Patrocínio duvidoso";

        evento.descricao =
            "Uma marca pequena oferece patrocínio, mas há boatos de que ela é usada para lavar dinheiro de apostadores. Você pode rejeitar ou aceitar o contrato.";

        evento.tituloOpcaoA = "REJEITAR CONTRATO";
        evento.tituloOpcaoB = "ACEITAR CONTRATO";

        evento.felicidadeA = Random.Range(2, 9);
        evento.dinheiroA = -Random.Range(0, 16);
        evento.famaA = Random.Range(3, 9);
        evento.riscoA = -Random.Range(5, 12);

        evento.dinheiroB = Random.Range(40, 76);
        evento.moralApostadorB = Random.Range(8, 18);
        evento.famaB = Random.Range(3, 10);
        evento.riscoB = Random.Range(12, 24);

        return evento;
    }

    EventoMoral CriarEventoMoradia()
    {
        EventoMoral evento = new EventoMoral();

        evento.nomeInterno = "Problema de moradia";

        evento.descricao =
            "Sua família recebe uma cobrança inesperada de aluguel. Você pode ajudar imediatamente ou aceitar dinheiro dos apostadores para resolver o problema sem afetar suas economias.";

        evento.tituloOpcaoA = "AJUDAR FAMÍLIA";
        evento.tituloOpcaoB = "PEGAR DINHEIRO";

        evento.felicidadeA = Random.Range(12, 20);
        evento.dinheiroA = -Random.Range(30, 51);
        evento.famaA = Random.Range(1, 6);
        evento.riscoA = -Random.Range(3, 9);

        evento.dinheiroB = Random.Range(35, 66);
        evento.moralApostadorB = Random.Range(10, 21);
        evento.famaB = -Random.Range(3, 10);
        evento.riscoB = Random.Range(10, 22);

        return evento;
    }

    void PreencherTelaComEvento()
    {
        if (textoTituloEvento != null)
        {
            textoTituloEvento.text = "";
        }

        if (textoDescricaoEvento != null)
        {
            textoDescricaoEvento.text = eventoAtual.descricao;
        }

        if (tituloOpcaoA != null)
        {
            tituloOpcaoA.text = eventoAtual.tituloOpcaoA;
        }

        if (tituloOpcaoB != null)
        {
            tituloOpcaoB.text = eventoAtual.tituloOpcaoB;
        }

        if (impactoA1 != null)
        {
            impactoA1.text = FormatarPorcentagem(eventoAtual.felicidadeA, "Felicidade");
        }

        if (impactoA2 != null)
        {
            impactoA2.text = FormatarMoedas(eventoAtual.dinheiroA);
        }

        if (impactoA3 != null)
        {
            impactoA3.text = FormatarPorcentagem(eventoAtual.famaA, "Fama");
        }

        if (impactoA4 != null)
        {
            impactoA4.text = FormatarPorcentagem(eventoAtual.riscoA, "Risco");
        }

        if (impactoB1 != null)
        {
            impactoB1.text = FormatarMoedas(eventoAtual.dinheiroB);
        }

        if (impactoB2 != null)
        {
            impactoB2.text = FormatarPorcentagem(eventoAtual.moralApostadorB, "Moral Apost.");
        }

        if (impactoB3 != null)
        {
            impactoB3.text = FormatarPorcentagem(eventoAtual.famaB, "Fama");
        }

        if (impactoB4 != null)
        {
            impactoB4.text = FormatarPorcentagem(eventoAtual.riscoB, "Risco");
        }
    }

    string FormatarPorcentagem(int valor, string nome)
    {
        if (valor > 0)
        {
            return "+" + valor + "% " + nome;
        }

        if (valor < 0)
        {
            return valor + "% " + nome;
        }

        return "0% " + nome;
    }

    string FormatarMoedas(int valor)
    {
        if (valor > 0)
        {
            return "+" + valor + " Moedas";
        }

        if (valor < 0)
        {
            return valor + " Moedas";
        }

        return "0 Moedas";
    }

    public void SelecionarOpcaoA()
    {
        opcaoSelecionada = 1;

        DestacarOpcao(1);

        if (botaoConfirmar != null)
        {
            botaoConfirmar.interactable = true;
        }

        Debug.Log("Opção A selecionada: " + eventoAtual.tituloOpcaoA);
    }

    public void SelecionarOpcaoB()
    {
        opcaoSelecionada = 2;

        DestacarOpcao(2);

        if (botaoConfirmar != null)
        {
            botaoConfirmar.interactable = true;
        }

        Debug.Log("Opção B selecionada: " + eventoAtual.tituloOpcaoB);
    }

    public void ConfirmarEscolha()
    {
        if (opcaoSelecionada == 0)
        {
            Debug.Log("Nenhuma opção selecionada.");
            return;
        }

        if (opcaoSelecionada == 1)
        {
            AplicarOpcaoA();
        }
        else if (opcaoSelecionada == 2)
        {
            AplicarOpcaoB();
        }

        DadosDoJogador.LimitarBarras();
        AtualizarStatusSuperior();

        Debug.Log("=== EVENTO ALEATÓRIO CONFIRMADO ===");
        Debug.Log("Evento: " + eventoAtual.nomeInterno);
        Debug.Log("Opção selecionada: " + opcaoSelecionada);
        Debug.Log("Dinheiro atual: " + DadosDoJogador.DinheiroAtual);
        Debug.Log("Felicidade: " + DadosDoJogador.Felicidade);
        Debug.Log("Fama: " + DadosDoJogador.Fama);
        Debug.Log("Moral Treinador: " + DadosDoJogador.MoralTreinador);
        Debug.Log("Moral Apostador: " + DadosDoJogador.MoralApostador);
        Debug.Log("Risco Interrogatório: " + DadosDoJogador.RiscoInterrogatorio);

        SceneManager.LoadScene("PrePartida");
    }

    void AplicarOpcaoA()
    {
        DadosDoJogador.Felicidade += eventoAtual.felicidadeA / 100f;
        DadosDoJogador.DinheiroAtual += eventoAtual.dinheiroA;
        DadosDoJogador.Fama += eventoAtual.famaA / 100f;
        DadosDoJogador.RiscoInterrogatorio += eventoAtual.riscoA / 100f;

        Debug.Log("Aplicou Opção A: " + eventoAtual.tituloOpcaoA);
    }

    void AplicarOpcaoB()
    {
        DadosDoJogador.DinheiroAtual += eventoAtual.dinheiroB;
        DadosDoJogador.MoralApostador += eventoAtual.moralApostadorB / 100f;
        DadosDoJogador.Fama += eventoAtual.famaB / 100f;
        DadosDoJogador.RiscoInterrogatorio += eventoAtual.riscoB / 100f;

        Debug.Log("Aplicou Opção B: " + eventoAtual.tituloOpcaoB);
    }

    public void FecharEvento()
    {
        SceneManager.LoadScene("PrePartida");
    }
}

[System.Serializable]
public class EventoMoral
{
    public string nomeInterno;
    public string descricao;

    public string tituloOpcaoA;
    public string tituloOpcaoB;

    public int felicidadeA;
    public int dinheiroA;
    public int famaA;
    public int riscoA;

    public int dinheiroB;
    public int moralApostadorB;
    public int famaB;
    public int riscoB;
}