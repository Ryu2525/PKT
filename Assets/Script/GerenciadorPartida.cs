using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorPartida : MonoBehaviour
{
    public GameObject painelVitoria;
    public GameObject painelDerrota;
    public int tentativas = 3;

    public void MarcouGol()
    {
        painelVitoria.SetActive(true);
    }

    public void ErrouChute()
    {
        tentativas--;
        if (tentativas <= 0)
        {
            painelDerrota.SetActive(true);
        }
    }

    public void IrParaHub()
    {
        SceneManager.LoadScene("Hub");
    }
}