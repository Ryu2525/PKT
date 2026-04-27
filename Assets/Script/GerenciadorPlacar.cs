using UnityEngine;
using UnityEngine.UI;
using TMPro; // <--- ADICIONE ESTA LINHA

public class GerenciadorPlacar : MonoBehaviour
{
    public int golsTimeA = 0;
    public int golsTimeB = 0;
    public TextMeshProUGUI textoPlacar;

    void Start()
    {
        AtualizarPlacarUI();
    }

    public void MarcarGol(bool foiTimeA)
    {
        if (foiTimeA) golsTimeA++;
        else golsTimeB++;
        
        AtualizarPlacarUI();
    }

    void AtualizarPlacarUI()
    {
        if (textoPlacar != null)
            textoPlacar.text = golsTimeA + " x " + golsTimeB;
    }
}