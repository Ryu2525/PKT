using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InterrogatorioManager : MonoBehaviour
{
    [Header("Status Superior")]
    public Image barraFelicidade;
    public Image barraFama;
    public Image barraMoralTreinador;
    public Image barraMoralApostador;
    public TextMeshProUGUI textoDinheiroTotal;

    [Header("Dificuldade")]
    public Image barraDificuldade;
    public TextMeshProUGUI textoDificuldadeEsquema;

    [Header("Contadores")]
    public TextMeshProUGUI textoAcertos;
    public TextMeshProUGUI textoErros;

    [Header("Cartas")]
    public Sprite versoCarta;
    public Sprite[] iconesPares; // Bola, Chuteira, Apito, Troféu, Dado
    public Image[] imagensCartas; // Carta1 até Carta10
    public Button[] botoesCartas; // Carta1 até Carta10

    [Header("Botão Continuar")]
    public Button botaoContinuar;

    private List<int> valoresCartas = new List<int>();
    private bool[] cartasEncontradas;

    private int primeiraCarta = -1;
    private int segundaCarta = -1;

    private int acertos = 0;
    private int erros = 0;
    private int errosMaximos = 4;

    private bool bloqueado = false;
    private bool jogoFinalizado = false;
    private bool sucesso = false;

    void Start()
    {
        AtualizarStatusSuperior();
        ConfigurarDificuldade();
        PrepararCartas();

        if (botaoContinuar != null)
        {
            botaoContinuar.interactable = false;
        }
    }

    void AtualizarStatusSuperior()
    {
        DadosDoJogador.LimitarBarras();

        if (barraFelicidade != null)
            barraFelicidade.fillAmount = DadosDoJogador.Felicidade;

        if (barraFama != null)
            barraFama.fillAmount = DadosDoJogador.Fama;

        if (barraMoralTreinador != null)
            barraMoralTreinador.fillAmount = DadosDoJogador.MoralTreinador;

        if (barraMoralApostador != null)
            barraMoralApostador.fillAmount = DadosDoJogador.MoralApostador;

        if (textoDinheiroTotal != null)
            textoDinheiroTotal.text = "R$ " + DadosDoJogador.DinheiroAtual;
    }

    void ConfigurarDificuldade()
    {
        float moralApostador = DadosDoJogador.MoralApostador;

        if (moralApostador < 0.4f)
        {
            errosMaximos = 6;

            if (textoDificuldadeEsquema != null)
                textoDificuldadeEsquema.text = "BAIXA";

            if (barraDificuldade != null)
                barraDificuldade.fillAmount = 0.25f;
        }
        else if (moralApostador < 0.7f)
        {
            errosMaximos = 4;

            if (textoDificuldadeEsquema != null)
                textoDificuldadeEsquema.text = "MÉDIA";

            if (barraDificuldade != null)
                barraDificuldade.fillAmount = 0.55f;
        }
        else
        {
            errosMaximos = 2;

            if (textoDificuldadeEsquema != null)
                textoDificuldadeEsquema.text = "ALTA";

            if (barraDificuldade != null)
                barraDificuldade.fillAmount = 0.85f;
        }

        AtualizarContadores();
    }

    void PrepararCartas()
    {
        if (iconesPares.Length < 5)
        {
            Debug.LogError("Você precisa colocar 5 sprites no campo Icones Pares.");
            return;
        }

        if (imagensCartas.Length < 10 || botoesCartas.Length < 10)
        {
            Debug.LogError("Você precisa colocar 10 imagens e 10 botões de cartas no Inspector.");
            return;
        }

        valoresCartas.Clear();

        // Cria 5 pares: 0,0,1,1,2,2,3,3,4,4
        for (int i = 0; i < 5; i++)
        {
            valoresCartas.Add(i);
            valoresCartas.Add(i);
        }

        EmbaralharCartas();

        cartasEncontradas = new bool[10];

        for (int i = 0; i < imagensCartas.Length; i++)
        {
            imagensCartas[i].sprite = versoCarta;
            botoesCartas[i].interactable = true;
            cartasEncontradas[i] = false;
        }

        acertos = 0;
        erros = 0;
        primeiraCarta = -1;
        segundaCarta = -1;
        bloqueado = false;
        jogoFinalizado = false;
        sucesso = false;

        AtualizarContadores();
    }

    void EmbaralharCartas()
    {
        for (int i = 0; i < valoresCartas.Count; i++)
        {
            int indiceAleatorio = Random.Range(i, valoresCartas.Count);

            int temporario = valoresCartas[i];
            valoresCartas[i] = valoresCartas[indiceAleatorio];
            valoresCartas[indiceAleatorio] = temporario;
        }
    }

    public void ClicarCarta(int indice)
    {
        if (jogoFinalizado) return;
        if (bloqueado) return;
        if (indice < 0 || indice >= valoresCartas.Count) return;
        if (cartasEncontradas[indice]) return;
        if (indice == primeiraCarta) return;

        RevelarCarta(indice);

        if (primeiraCarta == -1)
        {
            primeiraCarta = indice;
            return;
        }

        segundaCarta = indice;
        StartCoroutine(VerificarPar());
    }

    void RevelarCarta(int indice)
    {
        int valorDaCarta = valoresCartas[indice];
        imagensCartas[indice].sprite = iconesPares[valorDaCarta];
    }

    void EsconderCarta(int indice)
    {
        imagensCartas[indice].sprite = versoCarta;
    }

    IEnumerator VerificarPar()
    {
        bloqueado = true;

        yield return new WaitForSeconds(0.7f);

        int valorPrimeira = valoresCartas[primeiraCarta];
        int valorSegunda = valoresCartas[segundaCarta];

        if (valorPrimeira == valorSegunda)
        {
            acertos++;

            cartasEncontradas[primeiraCarta] = true;
            cartasEncontradas[segundaCarta] = true;

            botoesCartas[primeiraCarta].interactable = false;
            botoesCartas[segundaCarta].interactable = false;

            Debug.Log("Par encontrado. Acertos: " + acertos);

            if (acertos >= 5)
            {
                FinalizarInterrogatorio(true);
            }
        }
        else
        {
            erros++;

            EsconderCarta(primeiraCarta);
            EsconderCarta(segundaCarta);

            Debug.Log("Erro no par. Erros: " + erros);

            if (erros >= errosMaximos)
            {
                FinalizarInterrogatorio(false);
            }
        }

        primeiraCarta = -1;
        segundaCarta = -1;

        AtualizarContadores();

        bloqueado = false;
    }

    void AtualizarContadores()
    {
        if (textoAcertos != null)
            textoAcertos.text = acertos + "/5";

        if (textoErros != null)
            textoErros.text = erros + "/" + errosMaximos;
    }

    void FinalizarInterrogatorio(bool venceu)
    {
        jogoFinalizado = true;
        sucesso = venceu;

        for (int i = 0; i < botoesCartas.Length; i++)
        {
            botoesCartas[i].interactable = false;
        }

        if (botaoContinuar != null)
        {
            botaoContinuar.interactable = true;
        }

        if (venceu)
        {
            Debug.Log("Interrogatório vencido. Carreira preservada.");

            DadosDoJogador.RiscoInterrogatorio -= 0.20f;
            DadosDoJogador.Fama += 0.03f;
        }
        else
        {
            Debug.Log("Interrogatório perdido. Fim da carreira.");

            DadosDoJogador.Felicidade -= 0.20f;
            DadosDoJogador.MoralTreinador -= 0.20f;
        }

        DadosDoJogador.LimitarBarras();
        AtualizarStatusSuperior();
    }

    public void Continuar()
    {
        if (!jogoFinalizado)
        {
            Debug.Log("O interrogatório ainda não terminou.");
            return;
        }

        if (sucesso)
        {
            SceneManager.LoadScene("PrePartida");
        }
        else
        {
            SceneManager.LoadScene("FimPreso");
        }
    }

    // Funções para configurar no OnClick das cartas
    public void ClicarCarta1() { ClicarCarta(0); }
    public void ClicarCarta2() { ClicarCarta(1); }
    public void ClicarCarta3() { ClicarCarta(2); }
    public void ClicarCarta4() { ClicarCarta(3); }
    public void ClicarCarta5() { ClicarCarta(4); }
    public void ClicarCarta6() { ClicarCarta(5); }
    public void ClicarCarta7() { ClicarCarta(6); }
    public void ClicarCarta8() { ClicarCarta(7); }
    public void ClicarCarta9() { ClicarCarta(8); }
    public void ClicarCarta10() { ClicarCarta(9); }
}