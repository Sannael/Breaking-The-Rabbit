using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class RoomChangeControl : MonoBehaviour
{
    public string direction; //Determina a dire��o da troca de sala

    private void OnTriggerEnter2D(Collider2D other) //Checa se a collisão foi com o player
    {
        if(other.gameObject.name == "Player") //Para cada direção, chama o método de movimento de câmera no script "CameraController"
        {
            switch (direction) //Para cada dire��o, chama o m�todo de movimento de c�mera no script "CameraController"
            {
                case "up":
                    Camera.main.GetComponent<CameraController>().MoveCamUp();
                    break;
                case "down":
                    Camera.main.GetComponent<CameraController>().MoveCamDown();
                    break;
                case "right":
                    Camera.main.GetComponent<CameraController>().MoveCamRight();
                    break;
                case "left":
                    Camera.main.GetComponent<CameraController>().MoveCamLeft();
                    break;
                default:
                    // code block
                    break;
            }
        }
    }
}
