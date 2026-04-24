using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Barras de UI")]
    public Image Felicidade;
    public Image Fama;
    public Image MoralTreinador;
    public Image MoralApostador;

    // Valores de 0.0 a 1.0 (0% a 100%)
    private float nivelFelicidade = 0.2f;
    private float nivelFama = 0.2f;

    private float nivelMoralTreinador = 0.2f;

    private float nivelMoralApostador = 0.2f;

    void Start()
    {
        AtualizarBarras();
    }

    public void AlterarFelicidade(float quantidade)
    {
        nivelFelicidade = Mathf.Clamp01(nivelFelicidade + quantidade);
        AtualizarBarras();
    }

    void AtualizarBarras()
    {
        if(Felicidade != null) Felicidade.fillAmount = nivelFelicidade;
        if(Fama != null) Fama.fillAmount = nivelFama;
        if(MoralTreinador != null) MoralTreinador.fillAmount = nivelMoralTreinador;
        if(MoralApostador != null) MoralApostador.fillAmount = nivelMoralApostador;
    }
}