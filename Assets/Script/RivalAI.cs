using UnityEngine;

public class RivalAI : MonoBehaviour
{
    public float speed = 3f; // velocidade do jogador rival
    private Transform bola;

    void Start()
    {
        // procura o objeto com a tag "Bola"
        bola = GameObject.FindGameObjectWithTag("Bola").transform;
    }

    void Update()
    {
        if (bola != null)
        {
            // move o jogador rival em direção à bola
            transform.position = Vector3.MoveTowards(
                transform.position,
                bola.position,
                speed * Time.deltaTime
            );
        }
    }
}
