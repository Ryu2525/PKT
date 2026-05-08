using UnityEngine;
using UnityEngine.EventSystems;

public class MudarCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D cursorSelecionar; // Arraste a imagem da "mãozinha" aqui
    private Vector2 hotSpot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Muda para o ícone de selecionar quando o mouse entra no botão
        Cursor.SetCursor(cursorSelecionar, hotSpot, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Volta para o cursor padrão quando o mouse sai do botão
        Cursor.SetCursor(null, hotSpot, CursorMode.Auto);
    }
}