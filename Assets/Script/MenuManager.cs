using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class MenuManager : MonoBehaviour
{
    // Nome da cena de criação como aparece nos seus Assets
    [SerializeField] private string nomeDaCenaCriacao = "Criação de Personagem";

    public void IniciarJogo()
    {
        Debug.Log("Carregando cena de criação...");
        SceneManager.LoadScene(nomeDaCenaCriacao);
    }

    public void VoltarParaHome()
    {
        SceneManager.LoadScene("Home");
    }

    public void IrParaHub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}