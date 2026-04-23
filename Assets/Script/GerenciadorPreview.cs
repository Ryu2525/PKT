using UnityEngine;
using UnityEngine.UI;

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

        // NOVA LÓGICA: Cabelo(9) -> Pele(3) -> Uniforme(1)
        // Removido o "+ 1" para que a primeira combinação seja o índice 0
        int contaCabelo = nCabelo * 9;
        int contaPele = nPele * 3;
        int indiceFinal = contaCabelo + contaPele + nUniforme;

        // Esse log vai mostrar exatamente o que está acontecendo no Console
        Debug.Log($"SOMA: (Cabelo:{nCabelo} * 9) + (Pele:{nPele} * 3) + Uniforme:{nUniforme} = ÍNDICE: {indiceFinal}");
        
        // Agora usamos o número para o nome do arquivo (ex: Personagem1, Personagem2...)
        // Como seus arquivos começam no 1, aqui mantemos o +1 apenas para o NOME da busca
        string nomeProcurado = "Personagem" + (indiceFinal + 1);
        Debug.Log("Buscando Sprite com nome: " + nomeProcurado);

        if (todasAsCombinacoes.Length == 0) return;

        bool encontrou = false;
        foreach (Sprite s in todasAsCombinacoes)
        {
            if (s != null && s.name.StartsWith(nomeProcurado))
            {
                displayPersonagem.sprite = s;
                
                Color c = displayPersonagem.color;
                c.a = 1f; 
                displayPersonagem.color = c;
                encontrou = true;
                break;
            }
        }

        if (!encontrou)
        {
            Debug.LogWarning($"Não foi possível encontrar o arquivo: {nomeProcurado}");
        }
    }
}