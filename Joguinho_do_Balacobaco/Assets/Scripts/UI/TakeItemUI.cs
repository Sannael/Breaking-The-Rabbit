using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItemUI : MonoBehaviour
{
    public SpriteRenderer interaction;
    public Sprite interactionIdle, interactionPressed;
    public GameObject uiChange;
    void Start()
    {
        ChangeImage(); //Checa se teve alteração na tecla
    }
    public void ChangeImage()
    {
        StartCoroutine(InteracrionAnimation()); //Começa o coroutine de animação da tecla
    }

    public IEnumerator InteracrionAnimation()
    {
        interaction.sprite = interactionIdle; //arte n pressionada
        yield return new WaitForSeconds(1);
        interaction.sprite = interactionPressed; //arte pressionada
        yield return new WaitForSeconds(1);
        ChangeImage(); //Resetar a logica (faz um loop)
    }
}
