using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStatus : MonoBehaviour
{
    public Transform barrelTip; //Cano da arma, pra sair o projetil
    public GameObject bullet, capsule; //Projétil e capsula da arma
    private Animator gunAnimator; //Animator da arma 
    public float gunRate; //cadencia da arma 
    public float gunRateCurrent; //cadencia da arma atual
    public float bulletSpeed, capsuleSpeed; //velocidade do projétil / capsula 
    [Tooltip("Munição atual no pente/tambor/cano da arma")]
    public int ammo; //Munição atual no pente
    [Tooltip("Munição máxima do pente/tambor/cano da arma")]
    public int totalAmmo; //munição total do pente/tambor da arma 
    [Tooltip("Munição atual do Player, não conta o que esta no pente")]
    public int playerAmmo; //munição do player/ muncição total do player (sem contar do cano/pente/tambor)
    public bool canShoot; //Checa se pode atirar
    public bool reloading = false; //Checa se ta recarregando, evita bugs
    [Tooltip("Marcar como verdadeiro somente armas que recaregam de maneira manual como: Revolveres e Espingardas")] //Dicazinha sobre a variavel abixo pro inspetro
    public bool gunManualReload; //Armas que carregam de maneira manual
    private GameController gameController;
    private bool isPaused;
    public string ammoType; //Tipo de munição usada na arma
    
    void Start()
    {
        gunAnimator = this.GetComponent<Animator>(); //Pega o Animator do objeto
        reloading = false;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        isPaused = gameController.isPaused;
        if(isPaused == false)
        {
            if(gunRateCurrent >0) //Verifica a cadencia de desparo (Cooldown)
            {
                canShoot = false; 
                gunRateCurrent -= Time.deltaTime; //Vai retirando o Cooldown de uma forma fluido
            }
            else if(ammo >0) //Se tiver munição na arma pode atirar
            {
                canShoot = true;
            }

            if(Input.GetMouseButtonDown(1) && canShoot == true && ammo > 0) //Se clicar com o direito enquanto recarrega e tem bala; o player atira e para de recarregar (Só funciona cpom arma de reload manual)
            {
                
                if(reloading == true) //Checa se ta recarregando
                {
                    reloading = false; //Cancela o reloading (Somente em armas de reload manual)
                    Animations("Shoot"); //Chama alguma animação, passando nome do "Trigger" da animação como parametro
                }
                else
                {
                    Animations("Shoot"); //Chama alguma animação, passando nome do "Trigger" da animação como parametro
                }
                
            }
            if(ammo == 0 && reloading == false && playerAmmo >0) //Se tiver sem munição na arma, n tiver recarregando e player ainda tiver bala guardada
            {
                if(gunManualReload == true) //Arma de Reload Manual (Revolver, Shotgun)
                {
                    Animations("FirstReload");
                }
                else //Arma de reload Automatico (Troca pente todo)
                {
                    Animations("AutoReload");
                }
                reloading = true;
            }
            if(Input.GetKeyDown(KeyCode.R) && ammo < totalAmmo && playerAmmo >0 && reloading == false) //Usar R pra reload
            {
                reloading = true;
                if(gunManualReload == true)
                {
                    Animations("FirstReload"); //Arma de Reload Manual (Revolver, Shotgun)
                }
                else
                {
                    Animations("AutoReload"); //Arma de reload Automatico (Troca pente todo)
                }
            }        
        }
    }

    public void idle() //Evitar bug de animação de Reload infinito
    {
        reloading = false;
    }
    public void Animations(string animation) //animações da arma, recebe o trigger da animação como parametro 
    {
        gunAnimator.Rebind(); //Reseta todos parametros do animator
        gunAnimator.SetTrigger(animation); //Chama a animação
    }

    public void Shoot()
    {
        GameObject firedCapsule = Instantiate(capsule, barrelTip.position, barrelTip.rotation); //"Cria" um clone da bala no cano da arma
        firedCapsule.GetComponent<Rigidbody2D>().velocity = barrelTip.up * capsuleSpeed; //Calculo da velocidade do disparo disparo

        GameObject firedBullet = Instantiate(bullet, barrelTip.position, barrelTip.rotation); //Cria cum clone da capsula no cano da arma
        canShoot = false;
        gunRateCurrent = gunRate; //reseta o cooldown de tiro (cadencia)
        ammo --; //Perde um de munição
    }

    public void ManualReload() //Armas que tem reload manual
    {
        ammo ++;
        playerAmmo --;
    }
    public void StayReload() //Repetir o reload pra armas de reload manual 
    {
        if(ammo < totalAmmo && playerAmmo >0)
        {
            Animations("ManualReload"); //Colocar bala por bala até atingir munição maxima/ cancelar a animação
        }
    }

    public void AutoReload() //Reload de armas com reload automatico
    {
                    //Ainda vou mexer nisso só ignora
    }
}
