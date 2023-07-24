using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;
    public GameObject fadeInOut;

    private void Awake() 
    {
        instance = this;
    }
    public float FadeIn()
    {
        GameObject fade = Instantiate(fadeInOut, new Vector3(0,0,0), Quaternion.identity);
        fade.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        fade.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
        fade.GetComponent<Animator>().SetTrigger("Fade in");
        return fade.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
    }
    public float FadeOut()
    {
        GameObject fade = Instantiate(fadeInOut, new Vector3(0,0,0), Quaternion.identity);
        fade.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        fade.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
        fade.GetComponent<Animator>().SetTrigger("Fade out");
        return fade.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
    }

    
}
