using UnityEngine;
using UnityEngine.SceneManagement;

public class SistemaNavegacao : MonoBehaviour
{
    // ==========================================================
    // FUNÇÃO PARA VOLTAR AO MENU RESETANDO TUDO
    // ==========================================================
    public void VoltarAoMenuEResetar()
    {
        // 1. Chamada interna para resetar os dados estáticos
        ExecutarResetDeDados();

        // 2. Carrega a cena do Menu Principal
        // Certifique-se de que o nome da cena é exatamente "Home" ou "Menu"
        SceneManager.LoadScene("Home");
    }

    // ==========================================================
    // LÓGICA DE RESET (BASEADA NO SEU DADOSDOJOGADOR)
    // ==========================================================
    private void ExecutarResetDeDados()
    {
        // Reseta progresso e identificação
        DadosDoJogador.NivelAtual = 1;
        DadosDoJogador.TimeAtual = "União Vila Nova FC";
        DadosDoJogador.DinheiroAtual = 50;
        DadosDoJogador.CiclosFracassados = 0;
        DadosDoJogador.PartidasJogadas = 0;
        DadosDoJogador.VitoriasNoCiclo = 0;
        
        // Reseta status de ciclo
        DadosDoJogador.CicloFinalizado = false;
        DadosDoJogador.CicloVencido = false;

        // Reseta as barras de status (Valores iniciais do seu script)
        DadosDoJogador.Felicidade = 0.5f;
        DadosDoJogador.Fama = 0.25f;
        DadosDoJogador.MoralTreinador = 0.4f;
        DadosDoJogador.MoralApostador = 0.2f;
        DadosDoJogador.RiscoInterrogatorio = 0f;

        // Limpa registros da última partida
        DadosDoJogador.UltimoObjetivoCumprido = false;
        DadosDoJogador.UltimaPartidaVencida = false;
        DadosDoJogador.UltimoDinheiroGanho = 0;
        
        Debug.Log("Dados do jogador limpos e prontos para um novo início.");
    }

    // ==========================================================
    // FUNÇÃO PARA BOTÃO "NOVO JOGO" (DO MENU PARA O HUB)
    // ==========================================================
    public void IniciarNovoJogo()
    {
        ExecutarResetDeDados();
        SceneManager.LoadScene("Hub");
    }
}