using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SeletorCustomizacao : MonoBehaviour
{
    public enum TipoCategoria { Cabelo, Pele, Uniforme }

    [Header("Configurações do Grupo")]
    public TipoCategoria categoria; 
    public List<Image> iconesDestaCategoria; 
    public GerenciadorPreview previewManager; 

    [Header("Estilo de Seleção")]
    public Color corSelecionado = Color.green;
    public Color corPadrao = Color.white;
    public float opacidadeInativo = 0.4f;

    public void SelecionarComIndice(int indice)
    {
        for (int i = 0; i < iconesDestaCategoria.Count; i++)
        {
            Image img = iconesDestaCategoria[i];
            Color c = (i == indice) ? corSelecionado : corPadrao;
            c.a = (i == indice) ? 0.26f : opacidadeInativo;
            img.color = c;
        }

        if (previewManager != null)
        {
            // REMOVA O (+ 1) DAQUI:
            if (categoria == TipoCategoria.Cabelo) previewManager.SetCabelo(indice);
            else if (categoria == TipoCategoria.Pele) previewManager.SetPele(indice);
            else if (categoria == TipoCategoria.Uniforme) previewManager.SetUniforme(indice);
        }
    }
}