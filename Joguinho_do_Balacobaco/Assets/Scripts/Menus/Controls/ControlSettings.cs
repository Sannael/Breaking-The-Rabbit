using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlSettings : MonoBehaviour
{
    public string[] playerControls = new string[19];
    public KeyCode[] keyControls = new KeyCode[19];
    public Image[] keys = new Image[19];
    private bool keyRepeat;
    public TesteControl testeControl;
    public GameObject pnlControl;
    public string a;
    private int keyId; 
    void Start()
    {
        testeControl = pnlControl.GetComponent<TesteControl>();
        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            playerControls[0] = "W";
            playerControls[1] = "S";
            playerControls[2] = "A";
            playerControls[3] = "D";
            playerControls[4] = "0";
            playerControls[5] = "1";
            playerControls[6] = "R";
            playerControls[7] = "LeftShift";
            playerControls[8] = "Escape";
            playerControls[9] = "I";
            playerControls[10] = "Tab";
            playerControls[11] = "Q";
            playerControls[12] = "E";
            playerControls[13] = "Alpha1";
            playerControls[14] = "Alpha2";
            playerControls[15] = "Alpha3";
            playerControls[16] = "Alpha4";
            playerControls[17] = "Alpha5";
            playerControls[18] = "Alpha6";
            
            for (int i = 0; i < playerControls.Length; i ++)
            {
                SaveControl(i, playerControls[i]);
            }
        }

        else
        {
            for (int i = 0; i < playerControls.Length; i ++)
            {
                LoadControls(i, playerControls[i]);
                ChangeControl(i, playerControls[i]);
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetId(int controlId)
    {
        keyId = controlId;
        if(Input.GetKeyDown(KeyCode.E))
        {
            a = "E";
        }
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
    }
    
}
