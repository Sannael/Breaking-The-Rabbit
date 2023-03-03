using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    private int dropMinHit, dropMaxHit; //Quantia minima/maxima que pode ser dropada quando o inimigo ser atingido (Não morto) pela carambola 
    private int dropMinKill, dropMaxKill; //Quantia minima/maxima que pode ser dropada quando o inimigo morrer com a carambola 
    [Tooltip("Drop padrão, se houver; Caso colocar um valor em drop padrão o aleatorio NÃO VAI FUNCIONAR!!")]
    public int defaultDrop; //Drop padrão
    public int ammoCount; //Contagem de munição que contém na caixa
    public string ammoType; //Tipo de munição
    public GameObject player;
    public Sprite[] sprite = new Sprite[6]; //Sprites de todos os tipos de munição, ordem abaixo
    private SpriteRenderer spriteRend; 
    private int ammoId; //ID do tipo de munição; mesma ordem dos sprites
    public bool hitDrop, hitKill; //Caso for pego atraves da StarFruit; hitdrop: acertou mas não matou; hitKill: Matou
    private int count; //Evita bug de pegar duas vezes a munição
    
    void Start()
    {   
        count = 1;
        spriteRend = this.gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        ammoType = player.GetComponentInChildren<GunStatus>().ammoType;

        switch (ammoType) //Troca o Sprite de acordo com o tipo de munição da arma que o player tiver no momento que for dropado
        {
            case "Revolver":
            ammoId = 0;
            dropMinHit = 2; //Drop minimo da munição de revolver por hit da StarFruit 
            dropMaxHit = 4; //Drop maximo da munição de revolver por hit da StarFruit
            dropMinKill = 5; //Drop minimo da munição de revolver por kill da StarFruit
            dropMaxKill = 7; //Drop maximo da munição de revolver por kill da StarFruit
            break;

            case "Shotgun":
            ammoId = 1;
            dropMinHit = 2;
            dropMaxHit = 3;
            dropMinKill = 3;
            dropMaxKill = 5;
            break;

            case "Assault Rifle":
            ammoId = 2;
            dropMinHit = 1;
            dropMaxHit = 3; 
            dropMinKill = 3;
            dropMaxKill = 4;      
            break;

            case "Magnum":
            ammoId = 3;
            dropMinHit = 0;
            dropMaxHit = 1;
            dropMinKill = 1;
            dropMaxKill = 2;
            break;

            case "SMG":
            ammoId = 4;
            dropMinHit = 1;
            dropMaxHit = 4;
            dropMinKill = 3;
            dropMaxKill = 6;
            break;

            case "Pistol":
            ammoId = 5;
            dropMinHit = 1;
            dropMaxHit = 5;
            dropMinKill = 4;
            dropMaxKill = 6;
            break;
        }
        ChangeSprite(ammoId);

        if(hitDrop == true)
        {
            Drophit();
        }
        if(hitKill == true)
        {
            DropKill();
        }
        
    }
    void Update()
    {   
    }

    private void Drophit()
    {
        ammoCount = Random.Range(dropMinHit, dropMaxHit +1); //Dropa um valor aleatório
        StartCoroutine(StopFalling());
    }

    private void DropKill()
    {
        ammoCount = Random.Range(dropMinKill, dropMaxKill +1); //Dropa um valor aleatório
        StartCoroutine(StopFalling());
    }

    private void ChangeSprite(int spriteId)
    {
        spriteRend.sprite = sprite[spriteId];
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player") && count >0)
        {
            count --;
            other.GetComponent<PlayerScript>().TakeAmmo(ammoType, ammoCount);
            Destroy(gameObject);
        }    
    }

    public IEnumerator StopFalling()
    {
        Vector2 pos = transform.position; //Valor random, pra n bugar samerda
        pos[0] = 0;
        pos[1] = 0;
        yield return new WaitForSeconds(0.5f); 
        this.GetComponent<Rigidbody2D>().velocity = pos; //Parar a quada da caixa de munição
        this.GetComponent<Rigidbody2D>().gravityScale = 0; //Tirar a gravidade da moeda
        
        float range = Random.Range(-0.05f, 0.05f); //Daqui pra baixo é só um esquema pra moeda se mexer um poquinho pra colidir uma com a outra
        Vector3 lastPos = transform.position;
        lastPos[0] = lastPos[0] + range;
        transform.position = Vector2.MoveTowards(transform.position, lastPos, 1);
    }
}
