using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GerenciadorPreview : MonoBehaviour
{
    public Image displayPersonagem; 
    public Sprite[] todasAsCombinacoes; 

    private int nCabelo = 0;
    private int nPele = 0;
    private int nUniforme = 0;

    private bool escolheuCabelo = false;
    private bool escolheuPele = false;
    private bool escolheuUniforme = false;

    void Start()
    {
        if (displayPersonagem != null)
        {
            Color c = displayPersonagem.color;
            c.a = 0f; 
            displayPersonagem.color = c;
        }
    }

    public void SetCabelo(int num) { nCabelo = num; escolheuCabelo = true; AtualizarVisual(); }
    public void SetPele(int num) { nPele = num; escolheuPele = true; AtualizarVisual(); }
    public void SetUniforme(int num) { nUniforme = num; escolheuUniforme = true; AtualizarVisual(); }

    void AtualizarVisual()
    {
        if (!escolheuCabelo || !escolheuPele || !escolheuUniforme) return; 

        int contaCabelo = nCabelo * 9;
        int contaPele = nPele * 3;
        int indiceFinal = contaCabelo + contaPele + nUniforme;

        string nomeProcurado = "Personagem" + (indiceFinal + 1);
        
        if (todasAsCombinacoes.Length == 0) return;

        foreach (Sprite s in todasAsCombinacoes)
        {
            if (s != null && s.name.StartsWith(nomeProcurado))
            {
                displayPersonagem.sprite = s;
                Color c = displayPersonagem.color;
                c.a = 1f; 
                displayPersonagem.color = c;
                break;
            }
        }
    }

    // FUNÇÃO PARA O BOTÃO CONFIRMAR
    public void ConfirmarEscolha()
    {
        if (displayPersonagem.sprite != null)
        {
            // Salva na "nuvem" estática
            DadosDoJogador.PersonagemSelecionado = displayPersonagem.sprite;
            
            // Carrega a cena do Hub (certifique-se que o nome na Build Settings seja "Hub")
            SceneManager.LoadScene("Hub");
        }
        else
        {
            Debug.LogWarning("Selecione todas as partes antes de confirmar!");
        }
    }
}