using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class ControlSettings : MonoBehaviour
{
    public string[] playerControls = new string[19];
    public string[] keysBeforaSaving = new string[19];
    public Image[] keys;
    public Sprite[] allKeys; //Armazena o sprite de todas as letras (se for alterar a arte do teclado, cortar tecla por tecla e manter o padrão de nome, (nome da tecla em minuculo), Exemplo de tecla "especial": esc = "escape")
    public Sprite[] allPressedKeys; //armazena o sprite de todas as letras pressionadas (mesma ordem do vetor de cima ^, padrao de nome: nome da tecla + p, exemplo: e = "ep", esc = "escapep")
    public GameObject[] keysWarning = new GameObject[19];
    [SerializeField]
    private string[] playerActions = new string[19];
    public PlayerInput playerInput;
    public GameObject pnlControl;
    private int keyId; 
    public TMP_Text changeKeytxt;
    private bool changingColortxt;
    public GameObject pnlChangeKey, pnlChangeKeytxt;
    public TMP_InputField newKey;
    [SerializeField]
    private bool savedControl; //checa se todas alterações nos controles foram salvas
    [SerializeField]
    private bool sameKey = false;
    public GameObject pnlSureRestoring, pnlBackNoSaving;
    public Button btnSalvar;
    public InputActionReference esc, tab, leftShift, leftAlt, enter, rightButton, leftButton;

    public void Awake() 
    {
        savedControl = true;
        keyId = playerActions.Length;
        playerActions[0] = "MoveUP"; //Armazena todos os nomes de ações do game
        playerActions[1] = "MoveDown";
        playerActions[2] = "MoveLeft";
        playerActions[3] = "MoveRight";
        playerActions[4] = "MeleeAtk";
        playerActions[5] = "Shoot";
        playerActions[6] = "Reload";
        playerActions[7] = "Dash";
        playerActions[8] = "Pause";
        playerActions[9] = "OpenInventary";
        playerActions[10] = "SkipDialogue";
        playerActions[11] = "StarFruit";
        playerActions[12] = "Interaction";
        playerActions[13] = "Hotbar1";
        playerActions[14] = "Hotbar2";
        playerActions[15] = "Hotbar3";
        playerActions[16] = "Hotbar4";
        playerActions[17] = "Hotbar5";
        playerActions[18] = "Hotbar6";

        ControlInitialValues();
        SaveResetValuesKey();
        for(int i = 0; i < playerControls.Length; i ++) //Carrega todas as teclas que foram alteradas, se n houve alteração mantem a tecla incial 
        {
            if(LoadKeys(i) != "")
            {
                playerControls[i] = LoadKeys(i);
            }
        }
        SaveChanges(); //Salva mudanças
        ChangeAllImages(); //Troca todas as imagens das teclas pra novas teclas, toda vez q mudar de scene isso tem q acontecer
        LoadBindings(); //Carrega as mudanças (garantias)
    }

    private void ControlInitialValues()
    {
        playerControls[0] = "w"; //Armazena todas as teclas iniciais
        playerControls[1] = "s";
        playerControls[2] = "a";
        playerControls[3] = "d";
        playerControls[4] = "leftButton";
        playerControls[5] = "rightButton";
        playerControls[6] = "r";
        playerControls[7] = "leftShift";
        playerControls[8] = "escape";
        playerControls[9] = "i";
        playerControls[10] = "tab";
        playerControls[11] = "q";
        playerControls[12] = "e";
        playerControls[13] = "1";
        playerControls[14] = "2";
        playerControls[15] = "3";
        playerControls[16] = "4";
        playerControls[17] = "5";
        playerControls[18] = "6";
    }
    private void SaveResetValuesKey() //Salva os valores qnd abrir a sala de controles mas antes de salvar, caso precise resetar até iultimo dado salvo
    {
        for(int i =0; i < playerControls.Length; i ++)
        {
            keysBeforaSaving[i] = playerControls[i];
        }
    }
    private void ParcialResetKeys() //Reseta pros mesmo salvos da ultima vez
    {
        for(int i =0; i < playerControls.Length; i ++)
        {
            playerControls[i] = keysBeforaSaving[i];
        }
    }
    void Start()
    {
        StartCoroutine(ChangeColorKey()); //Chama a animação de engayzamento do texto de trocar as teclas
    }

    // Update is called once per frame
    void Update()
    {
        if(changeKeytxt.isActiveAndEnabled == true) //Verifica se o texto de mudar teclas ta habilitado
        {
            if(changingColortxt == false)
            {
                changingColortxt = true;
            }
        }
        else if(changingColortxt == true)
        {
            changingColortxt = false;
        }

        if(savedControl == true || sameKey == true) //verifica se houve mudanças nos controles e/ou se tem teclas repetidas
        {
            btnSalvar.interactable = false; //Desbilita o botao de salvar controles
        }
        else
        {
            btnSalvar.interactable = true; //Abilita o botao de salvar controles
        }

        if(keyId != playerActions.Length) //Trata exceção (teclas especiais e Mouse) qnd for trocar as teclas
        {
            if(esc.action.IsPressed())
            {
                SelectKey("escape");
            }
            if(tab.action.IsPressed())
            {
                SelectKey("tab");
            }
            if(leftShift.action.IsPressed())
            {
                SelectKey("leftShift");
            }
            if(leftAlt.action.IsPressed())
            {
                SelectKey("leftAlt");
            }
            if(enter.action.IsPressed())
            {
                SelectKey("enter");
            }
            if(rightButton.action.IsPressed())
            {
                SelectKey("rightButton");
            }
            if(leftButton.action.IsPressed())
            {
                SelectKey("leftButton");
            }
        }
    }

    public void OpenClosePnlChangeKey() //Se o texto tiver ativado habilita a tela de troca tecla, se n tiver desabilitas
    {
        if(changingColortxt == false)
        {
            pnlChangeKey.SetActive(true);
            pnlChangeKeytxt.SetActive(true);
        }
        else
        {
            pnlChangeKeytxt.SetActive(false);
            pnlChangeKey.SetActive(false);
        }
    } 
    
    public void SureRestoringKeys() //Promp de certeza que vai resetar as teclas
    {
        pnlSureRestoring.SetActive(true);
    } 
    public void CloseSureRestoringKeys() //Fechar o prompt ^
    {
        pnlSureRestoring.SetActive(false);
    } 
    public void Back() //Botao de voltar
    {
        if(savedControl == true) //Se tiver salvado as teclas, fecha a tela de botoes
        {
            this.GetComponent<Pause>().ClosePanelControls();
        }
        else //se n vem o prompt de certeza que n quer salvar
        {
            pnlBackNoSaving.SetActive(true);
        }
    }
    public void BackNoSavingYes() //Confirmando que não quer salvar os botoes qnd fechar
    {
        ParcialResetKeys();
        ChangeAllImages();
        savedControl = true;
        pnlBackNoSaving.SetActive(false);
        this.GetComponent<Pause>().ClosePanelControls();
    }
    public void BackNoSavingNo() //Não sair sem salvar (não sai praticamente)
    {
        pnlBackNoSaving.SetActive(false);
    }
    
    public void ChooseKey(int keyCode) //Quando clickar em alguma tecla pra ser alterada, muda arte dela pra ela soq pressionada
    {
        for(int i = 0; i < allPressedKeys.Length; i ++)
        {
            string[] key = keys[keyCode].sprite.ToString().Split(char.Parse(" "));
            string[] cKey = allPressedKeys[i].ToString().Split(char.Parse(" "));
            if((key[0] + "p") == cKey[0])
            {
                keys[keyCode].sprite = allPressedKeys[i];
            }
        }
        newKey.ActivateInputField();
        keyId = keyCode;
        savedControl = false;
    }

    public void SelectKey(string excep) //Só vai vir string se for alguma exceção (ta nome cortado pq exception é um metodo, achei melhor n manter mesmo nome tlg né :v)
    {
        if(excep != "null") //Trata casos especiais (tab, esc, space, etc)
        {
            Debug.Log(excep);
            for(int i = 0; i < allKeys.Length; i ++)
            {
                string[] key = allKeys[i].ToString().Split(char.Parse(" "));

                if(excep == key[0])
                {
                    ChangeKey(keyId, i);
                    break; 
                }
            }
            newKey.text = "";
        }
        else
        {
            for(int i = 0; i < allKeys.Length; i ++)
            {
                string[] key = allKeys[i].ToString().Split(char.Parse(" "));

                if(newKey.text == key[0])
                {
                    ChangeKey(keyId, i);
                    break; 
                }
            }
            newKey.text = "";
        }
        KeyRepeat(); //Verifica se tem repetições
    }

    public void KeyRepeat() //Verifica se alguma tecla se repete em duas ou mais ações (exceto pular dialogo);
    { //aproveitei a função pra resolver um B.O do pause não poder ser com o click, mais pratico :v
        sameKey = false;
        for(int i =0; i < playerControls.Length; i ++)
        {
            keysWarning[i].SetActive(false);
            for(int j =0; j < playerControls.Length; j ++)
            {
                if(i != j)
                {
                    if(i != 10 && j != 10)
                    {
                        if(playerControls[i] == playerControls[j])
                        {
                            keysWarning[i].SetActive(true);
                            sameKey = true;
                        }
                    }
                }
            }
        }
        if(playerControls[8] == "leftButton")
        {
            keysWarning[8].SetActive(true);
            sameKey = true;
        }
    }
    
    public void ChangeKey(int oldKey, int newKey)
    {
        savedControl = false;
        keys[oldKey].sprite = allKeys[newKey];
        string[] newControl = allKeys[newKey].ToString().Split(char.Parse(" "));
        playerControls[oldKey] = newControl[0];
        keyId = playerActions.Length;
        OpenClosePnlChangeKey();
    }

    private void ChangeAllImages()
    {
        for(int i = 0; i < keys.Length; i ++)
        {
            for(int j =0; j < allKeys.Length; j ++)
            {
                string[] key = allKeys[j].ToString().Split(char.Parse(" "));
                if(playerControls[i] == key[0])
                {
                    keys[i].sprite = allKeys[j];
                }
            }
        }
    }

    public IEnumerator ChangeColorKey() //Engayzar as letrinha, nada de mais 
    {
        for (int i = 255; i >=0; i -= 25)
        {
            changeKeytxt.color = new Color32(255 ,((byte)i) ,((byte)i), 255);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        for (int i = 0; i <= 255; i += 25)
        {
            changeKeytxt.color = new Color32(255 ,((byte)i) ,((byte)i), 255);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        StartCoroutine(ChangeColorKey());
    }

    public void SaveChanges()
    {
        for(int i =0; i < playerControls.Length; i ++)
        {
            SaveKey(i, playerControls[i]);
        }
        
        string[] movements = new string[4];
        for(int i = 0; i < 4; i ++)
        {
            movements[i] = playerControls[i];
        }
        ChangingCompositeBinding("<Keyboard>/", movements, "Move");

        for(int i = 4; i < playerControls.Length; i ++)
        {
            ChangingOneBiding("<Keyboard>/", playerControls[i], playerActions[i]);
        }
        SaveBindings();
        savedControl = true;
        SaveResetValuesKey();
    }

    public void ChangingOneBiding(string device, string newKey, string action) //Muda actions que usam uma tecla só (basicamente tudo menos andar)
    {
        if(newKey == "leftButton" || newKey == "rightButton")
        {
            string key = "<Mouse>/" + newKey;
            playerInput.actions[action].ChangeBinding(0)
            .WithPath(key);
        }
        else
        {
            string key = device + newKey;
            playerInput.actions[action].ChangeBinding(0)
            .WithPath(key);
        }
        SaveBindings();
    }
    public void ChangingCompositeBinding(string device, string[] newKeys, string action) //Muda actions que usam mais de umas tecla (basicamente só a movimentação até agora)
    {
        Debug.Log(newKeys[0] + " " + newKeys[1] + " " + newKeys[2] + " " + newKeys[3] + " " );
        playerInput.actions[action].ChangeBinding(0).Erase();

        playerInput.actions[action]
        .AddCompositeBinding("3DVector")
        .With("Up", device + newKeys[0])
        .With("Down", device + newKeys[1])
        .With("Left", device + newKeys[2])
        .With("Right", device + newKeys[3]);
        SaveBindings();
    }

    public void SaveBindings()
    {
        var rebinds = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
    public void LoadBindings()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        playerInput.actions.LoadBindingOverridesFromJson(rebinds);

    }
    public void RestoringBindings()
    {
        playerInput.actions.RemoveAllBindingOverrides();
        ControlInitialValues();
        ChangeAllImages();
        SaveChanges();
    } 

    public void SaveKey(int keyCode, string key)
    {
        PlayerPrefs.SetString(keyCode.ToString(), key);
        PlayerPrefs.Save();
        Debug.Log("Saving Keys: " + keyCode + "  " + key);
    }
    public string LoadKeys(int keyCode)
    {
        string ret;
        ret = PlayerPrefs.GetString(keyCode.ToString());
        Debug.Log("Loading Keys: " + keyCode + "  " + ret);
        return ret;
    }    
}
