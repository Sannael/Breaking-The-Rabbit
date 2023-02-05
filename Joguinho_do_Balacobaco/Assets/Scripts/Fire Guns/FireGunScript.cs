using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunScript : MonoBehaviour
{
    public Transform barrelTip; //Cano da arma, pra sair o projetil
    public GameObject bullet, capsule; //Projétil e capsula da arma
    private Vector2 lookDirection; //Direção do mouse em relação a arma
    private float lookAngle; //Angulo do mouse em relação a arma
    public GameObject gun; //Gameobject da arma
    private bool direita = true; //Sentido da arma, pra espelhar a arte da arma
    public float gunRate; //cadencia da arma 
    public float gunRateCurrent; //cadencia da arma atual
    public float bulletSpeed, capsuleSpeed; //velocidade do projétil / capsula 
    public int ammo, totalAmmo; //Munição atual / munição total
    public bool canShoot; //Checa se pode atirar
    public bool reloading = false; //Checa se ta recarregando, evita bugs
    public float reloadingTime; //Tempo de reloading total da arma
    [Tooltip("Marcar como verdadeiro somente armas que recaregam de maneira manual como: Revolveres e Espingardas")] //Dicazinha sobre a variavel abixo pro inspetro
    public bool gunManualReload; //Armas que carregam de maneira manual
    void Start()
    {
        ammo = totalAmmo; //Quando inicia a munição atual é igual a total
    }

    void Update()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //Armazena a direção do mouse 
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * (Mathf.Rad2Deg); //Calculo maluco de angulo, não faço ideia dde como funciona, só aceito
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle); //Rotaciona o braço pra mirar na direção do mouse  
        
        if(transform.rotation[2] > -0.7f && transform.rotation[2] < 0.7f && direita == false) //Checa a necessidade de espelhar a arma (Mirando pra esquerda do Player)
        {
            direita = !direita; //Inverte o valor da direção
            gun.transform.Rotate(180f ,0f ,0f); //Espelha a arma
        }
        else if(transform.rotation[2] < -0.7f && direita == true || transform.rotation[2] > 0.7f && direita == true)//Checa a necessidade de espelhar a arma (Mirando pra direita do Player)
        { 
            direita = !direita; //Inverte o valor da direção
            gun.transform.Rotate(180f ,0f ,0f); //Espelha a arma
        }

        if(gunRateCurrent >0) //Verifica a cadencia de desparo (Cooldown)
        {
            canShoot = false; 
            gunRateCurrent -= Time.deltaTime; //Vai retirando o Cooldown de uma forma fluido
        }
        else if(ammo >0) 
        {
            canShoot = true;
        }

        if(Input.GetMouseButtonDown(1) && canShoot == true && ammo > 0) //Se clicar com o direito enquanto recarrega e tem bala; o player atira e para de recarregar (Só funciona cpom arma de reload manual)
        {
            Shoot(); //Chama a função de Tiro
            if(reloading == true) //Checa se ta recarregando
            {
                reloading = false; //Cancela o reloading (Somente em armas de reload manual)   
            }
        }
        
        if(ammo == 0 && reloading == false) //Sem munição e não ta carregando
        {
            StartCoroutine(Reload()); //Chama a corotina de reload (corotina é tipo uma função, mas funciona com um ou + timers)
        }

        if(Input.GetKeyDown(KeyCode.R) && ammo < totalAmmo && reloading == false) //Recarregar de maneira manual apertando "R"
        {
            reloading = true;                   
            StartCoroutine(Reload());
        }

    }

    public void Shoot()
    {
        gun.GetComponent<Animator>().SetTrigger("Shooting"); //seta gatilho pra animação da arma atirando
        GameObject firedCapsule = Instantiate(capsule, barrelTip.position, barrelTip.rotation); //"Cria" um clone da bala no cano da arma
        firedCapsule.GetComponent<Rigidbody2D>().velocity = barrelTip.up * capsuleSpeed; //Calculo da velocidade do disparo disparo

        GameObject firedBullet = Instantiate(bullet, barrelTip.position, barrelTip.rotation); //Cria cum clone da capsula no cano da arma
        canShoot = false;
        gunRateCurrent = gunRate; //reseta o cooldown de tiro (cadencia)
        ammo --; //Perde um de munição
    }

    public IEnumerator Reload() //Corotine de Reload (Espero conseguir explicar bem sapoha kk)
    {
        reloading = true;
        canShoot = false;
        
        float oneBulletReloading; //variavel que armazena o tempo de recarga de uma bala somente (Só para armas com reload manual) 
        float timeRemaining = reloadingTime; //tempo restante de reload (Só funciona em armas com reload manual)
        if(gunManualReload) //Checa se a arma é de reload manual 
        {
            oneBulletReloading = reloadingTime / totalAmmo; //Calculo pra saber o tempo de recarga de apenas uma bala por vez
            float firstReload = oneBulletReloading *2; 
            gun.GetComponent<Animator>().SetTrigger("FirstReload");
            yield return new WaitForSeconds(firstReload);
            ammo ++;
            timeRemaining = (totalAmmo - ammo) * oneBulletReloading; //Calculo pra saber quanto tempo gastaria para recarregar levando em conta a quantia de bala atual (pra poder dar reload com qlqr quantia de bala restante)
            while(reloading == true && ammo < totalAmmo) //Enquanto tiver Recarregando e não tiver com o prente/tambor/cano cheio
            {
                yield return new WaitForSeconds(0.001f);
                if(reloading == true)
                gun.GetComponent<Animator>().SetTrigger("ReloadManual");
                yield return new WaitForSeconds(oneBulletReloading); //Espera o tempo de reload
                if(reloading == true) //se ainda tiver carregando (Duplicado com o While Pra evitar bugs)
                {
                    yield return new WaitForSeconds(0.001f); //Espera um milisegundo, tbm pra evtar bugs
                    ammo ++; //Reccarega uma bala
                }
            }
            reloading = false; //Para de reccaregar
        }

        else
        {
            yield return new WaitForSeconds(reloadingTime); //Espera o tempo de reaload do pente todo (somente para armas de reload não manual)
            ammo = totalAmmo; //Recarrega todo o pente
        }
        reloading = false; //Para de recarregar
    }
}
