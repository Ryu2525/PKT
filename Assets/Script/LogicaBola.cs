using UnityEngine;

public class LogicaBola : MonoBehaviour
{
    private Vector2 posicaoInicial;
    private Rigidbody2D rb;
    private SistemaDeChute scriptChute; // Referência ao seu script de controle

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicaoInicial = transform.position;
        scriptChute = FindFirstObjectByType<SistemaDeChute>();
    }

    private void OnCollisionEnter2D(Collision2D colisor)
    {
        // Se bater no adversário (Inimigo)
        if (colisor.gameObject.CompareTag("Inimigo"))
        {
            Debug.Log("Acertou o inimigo! Voltando...");
            ResetarBola();
        }
        // Se bater no amigo (Camisa Amarela)
        else if (colisor.gameObject.CompareTag("Amigo"))
        {
            Debug.Log("Passe perfeito!");
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            
            // Aqui você pode chamar a função para trocar de cena/etapa
            // Exemplo: scriptChute.IrParaChuteAoGol();
        }
    }

    public void ResetarBola()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = posicaoInicial;
        
        // Opcional: Voltar o estado do chute para "Parado" para o jogador tentar de novo
        if(scriptChute != null) scriptChute.estadoAtual = SistemaDeChute.EstadoChute.Parado;
    }
}