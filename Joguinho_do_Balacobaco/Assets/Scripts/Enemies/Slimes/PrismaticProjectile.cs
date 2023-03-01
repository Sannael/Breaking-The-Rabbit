using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismaticProjectile : MonoBehaviour
{
    private GameObject player; //Var que vai receber o Player
    private Rigidbody2D rb; //Var que vai receber o o Rigid Body do projétil
    public float force; //Determina a força com que o projétil será lançado na direção do Player
    public float timer; //Temporizador para destruir o projétil
    private Color32[] colors = new Color32[7];
    
    void Start()
    {
        SetColor();
        DisableAllEffects();
        rb = GetComponent<Rigidbody2D>(); //Recebe o Rigid Body do projétil
        player = GameObject.Find("Player"); //Recebe o Player

        Vector3 direction = player.transform.position - transform.position; //Determina a direção em que o projétil será lançado
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force; //Determina a velocidade do projétil, baseada na direção e na força
        RandomEffect();
    }
    private void DisableAllEffects() //Desabilita todos os efeitos 
    {
        this.gameObject.GetComponent<DamageScript>().enabled = false;
        this.gameObject.GetComponent<StunScript>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player") //Checa se a collisão foi com o player
        {
            //Destroy(gameObject); //Destroy o projétil
            StartCoroutine(WaitToDestroy()); //Delay pro projetil destruir; evita que bug e n de dano no player antes de destruir
        }
    }

    private void OnBecameInvisible() //Destrói o projetil quando sair da visao da camera (Sair da cena) 
    {
        Destroy(gameObject);
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }

    private void RandomEffect() //Troca os efeitos que o slime vai inflingir
    {
        int effectId = Random.Range(0, 2); //escolhe um dos efeitos dentre a quantia de efeitos que o slime tem

        switch(effectId) //Ativa o efeito selecionado 
        {
            case 0:
                this.gameObject.GetComponent<DamageScript>().enabled = true;
                break;
            case 1:
                this.gameObject.GetComponent<StunScript>().enabled = true;
                break;
        } 
    }

    private IEnumerator Prisma()
    {
        /*int randColor = Random.Range(0, colors.Length); //Escolhe um número aleatório que sera o numero da nova cor do slime 
        Color32 col = this.gameObject.GetComponent<SpriteRenderer>().color; //Armazena a cor atual do slime
        for(float i = 0; i < 1; i += 0.2f) //Loop que se repete 10 vezes, aumentando o Tom da cor
        {
            yield return new WaitForSeconds(0.05f);
            this.gameObject.GetComponent<SpriteRenderer>().color = Color32.Lerp(col, colors[randColor], i); //Efeitinho pra mudar entre a cor atural e a nova cor de uma maneira mais "natural"
        }
        StartCoroutine(Prisma()); //Chamar o Coroutine denovo (Loop eterno)*/

        for(int color = 0; color < colors.Length; color ++)
        {
            Color32 col = this.gameObject.GetComponent<SpriteRenderer>().color; //Armazena a cor atual do slime
            for(float i = 0; i < 1; i += 0.01f) //Loop que se repete 10 vezes, aumentando o Tom da cor
            {
                yield return null;
                this.gameObject.GetComponent<SpriteRenderer>().color = Color32.Lerp(col, colors[color], i); //Efeitinho pra mudar entre a cor atural e a nova cor de uma maneira mais "natural"
            } 
        }
        StartCoroutine(Prisma()); //Chamar o Coroutine denovo (Loop eterno)
    }  
    private void SetColor() //somente armazena as cores possiveis em posições dos vetores
    {
        /*colors[0] = new Color32(138, 43, 226, 255); //Violeta
        colors[1] = new Color32(0, 0, 139, 255); //Azul
        colors[2] = new Color32(0, 128, 0, 255); //Verde
        colors[3] = new Color32(255, 255, 0, 255); //Amarelo
        colors[4] = new Color32(255, 165, 0, 255); //Laranja
        colors[5] = new Color32(255, 0, 0, 255); //Vermelho
        colors[6] = new Color32(113, 24, 52, 255); //vinho
        colors[7] = new Color32(153, 102, 204, 255); //lilás
        colors[8] = new Color32(0, 0, 128, 255); //Azul escuro
        colors[9] = new Color32(60, 0, 100, 255); //Roxão
        colors[10] = new Color32(255, 0, 0, 255); //Vermelho
        colors[11] = new Color32(40, 114, 51, 255); //Verde esmeralda
        colors[12] = new Color32(0, 127, 255, 255); //azul bebe
        colors[13] = new Color32(252, 15, 192, 255); //rosa shock
        colors[14] = new Color32(184, 134, 11, 255); //dourado*/

        colors[0] = new Color32(255, 0, 0, 255);  //Vermelho
        colors[1] = new Color32(255, 117, 24, 255); //Laranja
        colors[2] = new Color32(255, 255, 0, 255); //Amarelo
        colors[3] = new Color32(50, 205, 50, 255); //Verde
        colors[4] = new Color32(0, 255, 255, 255); //Azul claro
        colors[5] = new Color32(65, 105, 255, 255); //Royal blue 
        colors[6] = new Color32(75, 0 , 130, 255); //Indigo

        StartCoroutine(Prisma());
    }

}
