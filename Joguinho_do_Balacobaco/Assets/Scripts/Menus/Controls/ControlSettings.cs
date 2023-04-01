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
    [SerializeField]
    private string[] playerActions = new string[19];
    public PlayerInput playerInput;
    public GameObject pnlControl;
    private int keyId; 
    public TMP_Text changeKeytxt;
    private bool changingColortxt;
    public GameObject pnlChangeKey;
    public TMP_InputField newKey;
    [SerializeField]
    private bool savedControl; //checa se todas alterações nos controles foram salvas
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
        SaveChanges();
        ChangeAllImages(); //Troca todas as imagens das teclas pra novas teclas, toda vez q mudar de scene isso tem q acontecer
        LoadBindings();
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
    private void SaveResetValuesKey()
    {
        for(int i =0; i < playerControls.Length; i ++)
        {
            keysBeforaSaving[i] = playerControls[i];
        }
    }
    private void ParcialResetKeys()
    {
        for(int i =0; i < playerControls.Length; i ++)
        {
            playerControls[i] = keysBeforaSaving[i];
        }
    }
    void Start()
    {
        StartCoroutine(ChangeColorKey());
    }

    // Update is called once per frame
    void Update()
    {
        if(changeKeytxt.isActiveAndEnabled == true )
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

        if(savedControl == true)
        {
            btnSalvar.interactable = false;
        }
        else
        {
            btnSalvar.interactable = true;
        }

        if(keyId != playerActions.Length)
        {
            if(esc.action.IsPressed())
            {
                Debug.Log("escape");
                SelectKey("escape");
            }
            if(tab.action.IsPressed())
            {
                Debug.Log("tab");
                SelectKey("tab");
            }
            if(leftShift.action.IsPressed())
            {
                Debug.Log("leftShift");
                SelectKey("leftShift");
            }
            if(leftAlt.action.IsPressed())
            {
                Debug.Log("leftAlt");
                SelectKey("leftAlt");
            }
            if(enter.action.IsPressed())
            {
                Debug.Log("enter");
                SelectKey("enter");
            }
            if(rightButton.action.IsPressed())
            {
                Debug.Log("rightButton");
                SelectKey("rightButton");
            }
            if(leftButton.action.IsPressed())
            {
                Debug.Log("leftButton");
                SelectKey("leftButton");
            }
        }
    }

    public void OpenClosePnlChangeKey()
    {
        if(changingColortxt == false)
        {
            pnlChangeKey.SetActive(true);
        }
        else
        {
            pnlChangeKey.SetActive(false);
        }
    } 
    
    public void SureRestoringKeys()
    {
        pnlSureRestoring.SetActive(true);
    } 
    public void CloseSureRestoringKeys()
    {
        pnlSureRestoring.SetActive(false);
    } 
    public void Back()
    {
        if(savedControl == true)
        {
            this.GetComponent<Pause>().ClosePanelControls();
        }
        else
        {
            pnlBackNoSaving.SetActive(true);
        }
    }
    public void BackNoSavingYes()
    {
        ParcialResetKeys();
        ChangeAllImages();
        savedControl = true;
        pnlBackNoSaving.SetActive(false);
        this.GetComponent<Pause>().ClosePanelControls();
    }
    public void BackNoSavingNo()
    {
        pnlBackNoSaving.SetActive(false);
    }
    
    public void ChooseKey(int keyCode)
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

    //Daqui pra baixo por hora é sucata, caso precise de alguma lógica, se n depois apaga a poha toda-------------------------------- 
    /*public void GetId(int controlId)
    {
        keyId = controlId;
    }
    public void ChangeControl(int controlId, string key)
    {
        keyRepeat = false;
        for (int i = 0; i < playerControls.Length; i ++)
        {
            if(i != controlId && key == playerControls[i])
            {
                playerControls[i] = PlayerPrefs.GetString(controlId.ToString());
                playerControls[controlId] = PlayerPrefs.GetString(i.ToString());
                SaveControl(controlId,playerControls[controlId]);
                SaveControl(i, playerControls[i]);
                keyRepeat = true;
                break;
            }
        }
        if(keyRepeat == false)
        {
            PlayerPrefs.SetString(controlId.ToString(), key);
            PlayerPrefs.Save();
            SaveControl(controlId, key);
            LoadControls(controlId, key);
        }
    }

    public void SaveControl(int controlId, string key)
    {
        switch(key)
        {
            case "Escape":
            keyControls[controlId] = KeyCode.Escape;
            break;

            case "Quote":
            keyControls[controlId] = KeyCode.Quote;
            break;

            case "Alpha1":
            keyControls[controlId] = KeyCode.Alpha1;
            break;

            case "Alpha2":
            keyControls[controlId] = KeyCode.Alpha2;
            break;

            case "Alpha3":
            keyControls[controlId] = KeyCode.Alpha3;
            break;

            case "Alpha4":
            keyControls[controlId] = KeyCode.Alpha4;
            break;

            case "Alpha5":
            keyControls[controlId] = KeyCode.Alpha5;
            break;

            case "Alpha6":
            keyControls[controlId] = KeyCode.Alpha6;
            break;

            case "Alpha7":
            keyControls[controlId] = KeyCode.Alpha7;
            break;

            case "Alpha8":
            keyControls[controlId] = KeyCode.Alpha8;
            break;

            case "Alpha9":
            keyControls[controlId] = KeyCode.Alpha9;
            break;

            case "Alpha0":
            keyControls[controlId] = KeyCode.Alpha0;
            break;

            case "Underscore":
            keyControls[controlId] = KeyCode.Underscore;
            break;

            case "Equals":
            keyControls[controlId] = KeyCode.Equals;
            break;

            case "Backspace":
            keyControls[controlId] = KeyCode.Backspace;
            break;

            case "Tab":
            keyControls[controlId] = KeyCode.Tab;
            break;

            case "Q":
            keyControls[controlId] = KeyCode.Q;
            break;

            case "W":
            keyControls[controlId] = KeyCode.W;
            break;

            case "E":
            keyControls[controlId] = KeyCode.E;
            break;

            case "R":
            keyControls[controlId] = KeyCode.R;
            break;

            case "T":
            keyControls[controlId] = KeyCode.T;
            break;

            case "Y":
            keyControls[controlId] = KeyCode.Y;
            break;

            case "U":
            keyControls[controlId] = KeyCode.U;
            break;

            case "I":
            keyControls[controlId] = KeyCode.I;
            break;

            case "O":
            keyControls[controlId] = KeyCode.O;
            break;

            case "P":
            keyControls[controlId] = KeyCode.P;
            break;

            case "BackQuote":
            keyControls[controlId] = KeyCode.BackQuote;
            break;

            case "LeftBracket":
            keyControls[controlId] = KeyCode.LeftBracket;
            break;

            case "CapsLock":
            keyControls[controlId] = KeyCode.CapsLock;
            break;

            case "A":
            keyControls[controlId] = KeyCode.A;
            break;

            case "S":
            keyControls[controlId] = KeyCode.S;
            break;

            case "D":
            keyControls[controlId] = KeyCode.D;
            break;

            case "F":
            keyControls[controlId] = KeyCode.F;
            break;

            case "G":
            keyControls[controlId] = KeyCode.G;
            break;

            case "H":
            keyControls[controlId] = KeyCode.H;
            break;

            case "J":
            keyControls[controlId] = KeyCode.J;
            break;

            case "K":
            keyControls[controlId] = KeyCode.K;
            break;

            case "L":
            keyControls[controlId] = KeyCode.L;
            break;

            case "Tilde":
            keyControls[controlId] = KeyCode.Tilde;
            break;

            case "RightBracket":
            keyControls[controlId] = KeyCode.RightBracket;
            break;

            case "LeftShift":
            keyControls[controlId] = KeyCode.LeftShift;
            break;

            case "Pipe":
            keyControls[controlId] = KeyCode.Pipe;
            break;

            case "Z":
            keyControls[controlId] = KeyCode.Z;
            break;
            
            case "X":
            keyControls[controlId] = KeyCode.X;
            break;

            case "C":
            keyControls[controlId] = KeyCode.C;
            break;

            case "V":
            keyControls[controlId] = KeyCode.V;
            break;

            case "B":
            keyControls[controlId] = KeyCode.B;
            break;

            case "N":
            keyControls[controlId] = KeyCode.N;
            break;

            case "M":
            keyControls[controlId] = KeyCode.M;
            break;

            case "Comma":
            keyControls[controlId] = KeyCode.Comma;
            break;

            case "Period":
            keyControls[controlId] = KeyCode.Period;
            break;

            case "Colon":
            keyControls[controlId] = KeyCode.Colon;
            break;

            case "Semicolon":
            keyControls[controlId] = KeyCode.Semicolon;
            break;

            case "Slash":
            keyControls[controlId] = KeyCode.Slash;
            break;

            case "RightShift":
            keyControls[controlId] = KeyCode.RightShift;
            break;

            case "LeftControl":
            keyControls[controlId] = KeyCode.LeftControl;
            break;

            case "leftAlt":
            keyControls[controlId] = KeyCode.LeftAlt;
            break;
            
            case "Space":
            keyControls[controlId] = KeyCode.Space;
            break;

            case "AltGr":
            keyControls[controlId] = KeyCode.AltGr;
            break;

            case "RightControl":
            keyControls[controlId] = KeyCode.RightControl;
            break;
        }
    }

    public void LoadControls(int controlId, string key)
    {
        playerControls[controlId] = PlayerPrefs.GetString(controlId.ToString());
    }*/
    
}
