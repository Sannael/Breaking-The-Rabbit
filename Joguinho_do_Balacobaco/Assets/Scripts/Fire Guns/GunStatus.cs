using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Reflection;

using static GunSaveStatus;

public class GunStatus : MonoBehaviour
{ 
    public GunSaveStatus.GunType gunType; 
    //enum GunType{AssaultRifle, Magnum, Pistol, Revolver, Shotgun, SMG};
    //public GunType gunType;
    //public string gunType;
    [SerializeField]
    private InputActionReference shoot, reload, starFruitAction; //Armazena os comandos de cada action que o script usa
    public Transform barrelTip; //Cano da arma, pra sair o projetil
    public Transform capsuleLocate; //Saida da capsula, pra armas que a capsula precisa sair de um lugar diferente do cano da arma
    public GameObject bullet, capsule; //Projétil e capsula da arma
    private Animator gunAnimator; //Animator da arma 
    public float gunRate; //cadencia da arma 
    public int damage;
    public int trueDamage;
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
    [Tooltip("Tipo de munição da arma, seguindo o padrão: Pistol, Revolver, Shotgun, SMG, AssaultRifle, Magnum")]
    public string ammoType; //Tipo de munição 
    private PlayerScript ps;
    private GameObject starFruit; //Carambola arremessavel
    private GameObject player;
    [Tooltip("A cadencia da arma é automatica?")]
    public bool automatic; //Cadencia automatica?
    public bool cockingGun; //pra armas que tem o engatilhar fora da animação de recarregar 
    [Tooltip("Usdo somente para armas que precisam ter uma dispersão no tiro(exemplo uma shotgun), se precisar é só colocar aqui a quantidade de balas que vão fazer essa dispersão, se não precisar deixa 0")]
    public int bulletSpread; //Pra armas que o tiro precisa ter uma dispersão
    [Tooltip("Distancia minima e maxima da dispersao, se n houver deixa 0")]
    public int bulletSpreadMin, bulletSpreadMax; //Distancia minima e maxima da dispersão
    [Tooltip("Aqui coloca o prefab da arma so que dropada, meio q é quase um clone da arma mas sem a capacidade de atirar e com um custo outra arte e tal")]
    public GameObject thisGunChange; //Prefab da arma que fica dropada (pra poder trocar a arma de boa)
    public GameObject magazine; //Pente de arma; Se houver
    public Transform magLocate;
    public Vector2 magForce;
    public Item item;
    private bool canThrowStarFruit = true;
   [HideInInspector]
    public GameObject gun;
    void Start()
    {
        if(System.Enum.TryParse(ammoType, out GunType a))
        {
            gunType = a;
        }
        gun = item.thisPrefabDrop.GetComponent<ChangeGun>().gun;
        CoreInventory._instance.inventory.GetItem(item, 0, true, false, 1);
        gunAnimator = this.GetComponent<Animator>(); //Pega o Animator do objeto
        reloading = false;
        player = GameObject.Find("Player");
        ps = player.GetComponent<PlayerScript>();
        starFruit = ps.starFruit;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        reloading = false;
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

            if(shoot.action.IsPressed() && canShoot == true && ammo > 0 && automatic == false) //Se clicar com o direito enquanto recarrega e tem bala; o player atira e para de recarregar (Só funciona cpom arma de reload manual)
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
            
            if(shoot.action.IsPressed() && canShoot == true && ammo > 0 && automatic == true) //Tiro de arma automatica
            {
                if(reloading == true)
                {
                    reloading = false;
                    Animations("Idle");
                }
                gunAnimator.SetBool("AutoShooting", true);
            }

            if(shoot.action.IsPressed() == false && automatic == true || automatic == true && ammo <= 0) //Parar de atirar com arma que é automatica
            {
                gunAnimator.SetBool("AutoShooting", false);
            }

            if(ammo == 0 && reloading == false && playerAmmo >0) //Se tiver sem munição na arma, n tiver recarregando e player ainda tiver bala guardada
            {
                reloading = true;
                if(gunManualReload == true) //Arma de Reload Manual (Revolver, Shotgun)
                {
                    Animations("FirstReload");
                }
                else //Arma de reload Automatico (Troca pente todo)
                {
                    Animations("AutoReload");
                }
                
            }
            if(reload.action.IsPressed() && ammo < totalAmmo && playerAmmo >0 && reloading == false) //Usar R pra reload
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

            if(starFruitAction.action.IsPressed() && ps.starFruitCount >0 && canThrowStarFruit == true)
            {
                canThrowStarFruit = false;
                GameObject starFruitTrhow = Instantiate(starFruit, barrelTip.position, barrelTip.rotation);
                ps.starFruitCount --;
            } 
            if(starFruitAction.action.IsPressed() == false && canThrowStarFruit == false)
            {
                canThrowStarFruit = true;
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

            case "AssaultRifle":
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

    public void SaveGun()
    {
        GunSaveStatus gunsave = Resources.LoadAll("", typeof(GunSaveStatus)).Cast<GunSaveStatus>().First();
        FieldInfo[] scriptVars = typeof(GunStatus).GetFields(BindingFlags.Public | BindingFlags.Instance);
        gunsave.FillList();
        foreach(FieldInfo variable in scriptVars)
        {
            object varValue = variable.GetValue(this);
            string name = variable.Name;
            gunsave.SaveList(name, varValue);
        }
    }

    public void TakeGun(GunSaveStatus gunSave)
    {
        //GunSaveStatus gunsave = Resources.LoadAll("", typeof(GunSaveStatus)).Cast<GunSaveStatus>().First();
        gunSave.FillList();
        foreach(var save in gunSave.save)
        {
            TakeGunInfo(save.Key, save.Value);
        }
    }
    public void TakeGunInfo(string statusName, object value)
    {
        switch (statusName)
        {
            case "gunRate":
            gunRate = System.Convert.ToSingle(value);
            break;

            case "damage":
            damage = System.Convert.ToInt32(value);
            break;

            case "trueDamage":
            trueDamage = System.Convert.ToInt32(value);
            break;

            case "ammo":
            ammo = System.Convert.ToInt32(value);
            break;

            case "totalAmmo":
            totalAmmo = System.Convert.ToInt32(value);
            break;

            case "playerAmmo": 
            playerAmmo = System.Convert.ToInt32(value);
            break;            

            case "bulletSpread":
            bulletSpread = System.Convert.ToInt32(value);
            break;

            case "bulletSpreadMin":
            bulletSpreadMin = System.Convert.ToInt32(value);
            break;

            case "bulletSpreadMax":
            bulletSpreadMax = System.Convert.ToInt32(value);
            break;
        }
    }

    public void idle() //Evitar bug de animação de Reload infinito
    {
        reloading = false;
    }
    public void Animations(string animation) //animações da arma, recebe o trigger da animação como parametro 
    {
        if(gunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shooting")) //N bugar animação de quando ta atirando e vai reccarregar(ele cancelava uma e só fazia a outra)
        {
            if(animation == "FirstReload" || animation == "AutoReload")
            StartCoroutine(WaitAnimation(animation));
        }
        else
        {
            gunAnimator.Rebind(); //Reseta todos parametros do animator
            gunAnimator.SetTrigger(animation); //Chama a animação
        }
    }

    public IEnumerator WaitAnimation(string animation)
    {
        float animationTime = gunAnimator.GetCurrentAnimatorClipInfo(0).Length; //Pega o tamanho da animação que ta rodando
        yield return new WaitForSeconds(animationTime); //Espera a animação acabar e chama a outra animação
        Animations(animation);
    }

    public void Shoot()
    {
        if(bulletSpread >0)
        {
            for(int i = bulletSpread; i >0; i --)
            {
                GameObject firedBullet = Instantiate(bullet, barrelTip.position, barrelTip.rotation); //Cria cum clone da bala no cano da arma
                firedBullet.transform.Rotate(0, 0, Random.Range(bulletSpreadMin, bulletSpreadMax)); //Cria o efeito de dispersão
                firedBullet.GetComponent<DamageScript>().damage = damage;
                firedBullet.GetComponent<DamageScript>().trueDamage = trueDamage;
            }
        }
        else
        {
            GameObject firedBullet = Instantiate(bullet, barrelTip.position, barrelTip.rotation); //Cria cum clone da bala no cano da arma
            firedBullet.GetComponent<DamageScript>().damage = damage;
            firedBullet.GetComponent<DamageScript>().trueDamage = trueDamage;
        }
        
        canShoot = false;
        gunRateCurrent = gunRate; //reseta o cooldown de tiro (cadencia)
        ammo --; //Perde um de munição
    }
    public void Capsule()
    {
        if(capsuleLocate != null) //Verifica se tem um lugar diferente pra capsula sair 
        {
            try
            {
                GameObject firedCapsule = Instantiate(capsule, capsuleLocate.position, capsuleLocate.rotation); //"Cria" um clone da capsula no cano da arma
            firedCapsule.GetComponent<Rigidbody2D>().velocity = capsuleLocate.up * capsuleSpeed; //Calculo da velocidade da capsula
            }
            catch{}
        }
        else
        {
            try
            {
            GameObject firedCapsule = Instantiate(capsule, barrelTip.position, barrelTip.rotation); //"Cria" um clone da bala no cano da arma
            firedCapsule.GetComponent<Rigidbody2D>().velocity = barrelTip.up * capsuleSpeed; //Calculo da velocidade da capsula
            }
            catch{}
        }
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
        if(cockingGun == true)
        {
            if(ammo == totalAmmo || playerAmmo == 0) //pra armas que precisam engatilhar em uma animação separada 
            {
                Animations("CockingGun");
            }
        }
        reloading = false;
    }

    public void AutoReload() //Reload de armas com reload automatico
    {
        for(int i = playerAmmo; i >0; i --) //Pega os valores que o player tem de munição total
        {
            if(i <= (totalAmmo - ammo)) //Verifica se cabe na arma
            {
                ammo += i;
                playerAmmo -= i;
                break;
            }
        }
        reloading = false;   
    }

    public void DropMag() //Efeito de joagr o pente fora, pra armas que troca o pente
    {
        Vector2 magForceF = magForce;
        GameObject mag = Instantiate(magazine, magLocate.position, Quaternion.identity);
        if(gameObject.GetComponentInParent<BarrelGunScript>().direita == false)
        {
            mag.transform.Rotate(0, 180, 0);
            magForceF = - (magForce);
        }
        mag.GetComponent<Rigidbody2D>().velocity = magForceF;
    }
}

