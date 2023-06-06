using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMelee : MonoBehaviour
{
    [Tooltip("Aqui coloca o gameobject dessa arma sem ser a dropada")]
    public GameObject melee; //Gameobject da arma
    private GameObject playerMelee; //Puxa qual a arma que o player ta usando agora
    private int drops; //Variavel pra barrar duplicação de troca de arma (acaba duplicandop as armas)
    private float time = 0.8f;  //Tempo de quando a arma ta no chão pra poder trocar (um delayzinho pra n bugar a poha toda :v)
    public bool canChange; //Checa se o player poder trocar (basicamente checa se o player ta dentro do campo de troca)
    public GameObject uiChange; //Gameobejct de troca de arma
    private int ui = 1; //Mais uma variavel que evita bugar 
    public bool droped; //Checa se a arma vemd e bau, loja ou ta sendo dropada no chão

    void Start()
    {
        time = 0.8f;
        drops =-1;

        if(droped == true) //se for dropada chama a parabola de drop no chão
        {
            Vector3 pos = this.transform.position; 
            Vector3 playerPos = GameObject.Find("Player").GetComponent<Transform>().position;
            if(playerPos[0] < pos[0]) //Checa a direção que o player ta em realçao a arma
            {
                StartCoroutine(MoveMelee(5, 2));
            }
            else
            {
                StartCoroutine(MoveMelee(-5, 2));
            }
        }
    }
    void Update()
    { 
        try
        {
            playerMelee = GameObject.Find("Player").GetComponent<PlayerScript>().melee.GetComponentInChildren<MeleeController>().weapon.GetComponent<MeleeScript>().thisMeleeChange; //Tenta pegar a case do player (se o player usa a sabronora case disaparece, ent buga essa merda)
        }
        catch{}
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        if(time <0)
        {
            drops =1;
        }
        if(canChange == true && ui>0 && drops ==1) //Se tiver dentro do campo de trocar de arma
        {
            ui --;
            GameObject uiGunChange = Instantiate(uiChange, transform.position, Quaternion.identity); //Aparece a UI de troca de arma
            uiGunChange.transform.parent = transform;
        }
        if(canChange == false && ui <1) 
        {
            ui ++;
            Destroy(GameObject.Find("UI ChangeGuns(Clone)")); //DEsabilita a UI de troca de arma
        }
    }

    public void changeMelee() //Evento de troca de arma
    {
        drops --;
        if(drops ==0) //Evita duplicar
        {
            string[] meleeName = playerMelee.ToString().Split(" Drop");//Separa o nome pelo drop, vai sempre pegar o nome normal da arma
            try
            {
                Destroy(GameObject.Find(meleeName[0])); //Destroi a arma que ta com o player
            }
            catch{}
            try
            {
                Destroy(GameObject.Find(meleeName[0] + "(Clone)")); //Destroi a arma que ta com o player
            }
            catch{}
    
            GameObject dropMelee = Instantiate(playerMelee, transform.position, Quaternion.identity); //Cria um Gameobject da arma que o player tinha na mão antes da troca
            dropMelee.GetComponent<ChangeMelee>().droped = true; //Marca na arma dropada que ela foi dropada kkk (pra fazer a parabola de drop)

            GameObject.Find("Melee").GetComponent<ChangeMeleeInCase>().ChangeMelee(melee); //Setar o melee como "pai da arma que criou", sistema de hierarquia e blablabla
            Destroy(this.gameObject);
        }
    }

    private IEnumerator MoveMelee(int distanceRight, int distanceUp) //Parabola de dropar a arma (se pa mexo qnd tiver bau e pa)
    {
        if(distanceRight <0) //Checa se precisa espelhar a arte
        {
            transform.Rotate(0, 180, 0);
            distanceRight = - (distanceRight);
        }
        Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>(); //pega o rigidbody da arma que vai ser dropada 
        Vector2 parable = rb.velocity; //valor random, Unity para de chiar 
        parable[0] = transform.right[0] * distanceRight; //calculo horizontal da parabola
        parable[1] = transform.up[1] * distanceUp; //Calculo vertical da parabola
        rb.velocity = parable; //faz a aprabola
        yield return new WaitForSeconds(0.1f);
        parable[1] = 0; //Para de "empurar" pra cima, pra ficar bonitinho
        rb.velocity = parable;
        rb.gravityScale = 1; //Colocando a gravidade pra fazer o meu trabalho
        yield return new WaitForSeconds(0.2f);
        rb.velocity = rb.velocity *0; //Para de "Empurar" pra frente
        rb.gravityScale =0; //Cabo o trampo da gravidade
    }

}
