using UnityEngine;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    [Header("Configurações do Hub")]
    public Image displayNoHub; // Arraste aqui o quadrado cinza onde ficará o personagem

    void Start()
    {
        // Ao iniciar a cena do Hub, ele busca o que foi salvo
        if (DadosDoJogador.PersonagemSelecionado != null)
        {
            displayNoHub.sprite = DadosDoJogador.PersonagemSelecionado;
            
            // Garante que a imagem apareça (Alpha em 100%)
            Color c = displayNoHub.color;
            c.a = 1f;
            displayNoHub.color = c;
        }
        else
        {
            Debug.LogError("Nenhum personagem foi salvo na cena anterior!");
        }
    }

    public void VoltarParaCriacao()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Criação de Personagem");
    }
}