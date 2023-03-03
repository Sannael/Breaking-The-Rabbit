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
    [Tooltip("Tipo de munição da arma, seguindo o padrão: Pistol, Revolver, Shotgun, SMG, Assault Rifle, Magnum")]
    public string ammoType; //Tipo de munição 
    private PlayerScript ps;
    private GameObject starFruit; //Carambola arremessavel
    private GameObject player;
    [Tooltip("A cadencia da arma é automatica?")]
    public bool automatic; //Cadencia automatica?
    
    void Start()
    {
        playerAmmo = 0;
        gunAnimator = this.GetComponent<Animator>(); //Pega o Animator do objeto
        reloading = false;
        player = GameObject.Find("Player");
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        ps = player.GetComponent<PlayerScript>();
        starFruit = ps.starFruit;
        //SetAmmo();
    }
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

            if(Input.GetMouseButtonDown(1) && canShoot == true && ammo > 0 && automatic == false) //Se clicar com o direito enquanto recarrega e tem bala; o player atira e para de recarregar (Só funciona cpom arma de reload manual)
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
            
            if(Input.GetMouseButton(1) && canShoot == true && ammo > 0 && automatic == true) //Tiro de arma automatica
            {
                if(reloading == true)
                {
                    reloading = false;
                }
                gunAnimator.SetBool("AutoShooting", true);
            }

            if(Input.GetMouseButtonUp(1) && automatic == true) //Parar de atirar com arma que é automatica
            {
                gunAnimator.SetBool("AutoShooting", false);
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

            if(Input.GetKeyDown(KeyCode.Space) && ps.starFruitCount >0)
            {
                GameObject starFruitTrhow = Instantiate(starFruit, barrelTip.position, barrelTip.rotation);
                ps.starFruitCount --;
            } 
            SetAmmo();    
        }
    }

    private void SetAmmo() //Setar a Quantidade de munição de cada arma
    {
        switch(ammoType) //isso armazena a munição e toda vez que houver alteração ele armazena de novo
        {
            case "Pistol":
            if(ps.pistolAmmo >0)
            {
                playerAmmo += ps.pistolAmmo;
                ps.pistolAmmo = 0;
            }
            break;

            case "Revolver":
            if(ps.revolverAmmo >0)
            {
                playerAmmo += ps.revolverAmmo;
                ps.revolverAmmo = 0;
            }
            break;

            case "Shotgun":
            if(ps.shotgunAmmo >0)
            {
                playerAmmo += ps.shotgunAmmo;
                ps.shotgunAmmo = 0;
            }
            break;

            case "Assault Rifle":
            if(ps.assaultRifleAmmo >0)
            {
                playerAmmo += ps.assaultRifleAmmo;
                ps.assaultRifleAmmo = 0;
            }
            break;

            case "SMG":
            if(ps.smgAmmo >0)
            {
                playerAmmo += ps.smgAmmo;
                ps.smgAmmo = 0;   
            }
            break;

            case "Magnum":
            if(ps.magnumAmmo > 0)
            {
                playerAmmo += ps.magnumAmmo;
                ps.magnumAmmo = 0;
            }
            break;
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

