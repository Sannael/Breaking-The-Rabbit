using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool cameraCanMove = false; //Determina se a c�mera pode se mover, para evitar bugs
    private float CamMoveSpeed = 5f; //Velocidade do movimento de c�mera
    private Vector3 currentCameraPos; //Armazena as coordenadas da c�mera antes da mudan�a de cen�rio
    private Vector3 newCameraPos; //Armazena o valor da nova coordenada para onde a c�mera ir� se mover
    
    private bool cameraIsMoving = false; //checa se a camera ta em movimento (Transição)
    
    void Start()
    {
        Camera.main.transform.position = new Vector3(0f, 0f, -10.0f); //No come�o da cena centraliza a c�mera no primeiro cen�rio
    }

    void Update()
    {
        if (cameraCanMove) //Usa o m�todo Lerp para fazer uma transi��o suave entre as posi��es de c�mera: Lerp(posi��o atual, posi��o final, intervalo de tempo)
        {
            cameraIsMoving = true; //camera em transição
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCameraPos, (CamMoveSpeed += 50f * Time.deltaTime) * Time.deltaTime); // O intervalo de tempo tem uma f�rmula maluca pra ir acrescentando o valor aos poucos para acelerar o processo
            GameObject.Find("Player").GetComponent<PlayerScript>().canMove = false; //Impede o player de se movimentar, para evitar bugs
        }
        if (Camera.main.transform.position == newCameraPos && cameraIsMoving == true) //Quando a c�mera chegar na posi��o final
        {
            cameraCanMove = false; //Impede a c�mera de continuar movendo 
            CamMoveSpeed = 5f; //Reseta a velocidade de movimento para 5
            GameObject.Find("Player").GetComponent<PlayerScript>().canMove = true; //Libera o jogador para se mover
            cameraIsMoving = false; //camera parada
        }
    }

    public void MoveCamUp() //Metodo chamado no script "RoomChangeControl"
    {
        if (!cameraCanMove) //If para checar se a c�mera j� est� se movendo e evitar bugs (se estiver ele n�o faz nada)
        {
            GameObject.Find("Player").transform.position += new Vector3(0.0f, 2.0f, 0.0f); //Transporta o player para o outro cen�rio
            currentCameraPos = Camera.main.transform.position; //Armazena a posi��o atual da c�mera
            newCameraPos = currentCameraPos += new Vector3(0.0f, 10.8f, 0.0f); //Calcula a posi��o final da c�mera
            cameraCanMove = true;
        }
    }

    public void MoveCamDown() //Metodo chamado no script "RoomChangeControl"
    {
        if (!cameraCanMove) //If para checar se a c�mera j� est� se movendo e evitar bugs (se estiver ele n�o faz nada)
        {
            GameObject.Find("Player").transform.position -= new Vector3(0.0f, 2.0f, 0.0f); //Transporta o player para o outro cen�rio
            currentCameraPos = Camera.main.transform.position; //Armazena a posi��o atual da c�mera
            newCameraPos = currentCameraPos -= new Vector3(0.0f, 10.8f, 0.0f); //Calcula a posi��o final da c�mera
            cameraCanMove = true; //Permite que a c�mera possa se mover
        }
    }
    public void MoveCamLeft() //Metodo chamado no script "RoomChangeControl"
    {
        if (!cameraCanMove) //If para checar se a c�mera j� est� se movendo e evitar bugs (se estiver ele n�o faz nada)
        {
            GameObject.Find("Player").transform.position -= new Vector3(2.0f, 0.0f, 0.0f); //Transporta o player para o outro cen�rio
            currentCameraPos = Camera.main.transform.position; //Armazena a posi��o atual da c�mera
            newCameraPos = currentCameraPos -= new Vector3(19.18f, 0.0f, 0.0f); //Calcula a posi��o final da c�mera
            cameraCanMove = true; //Permite que a c�mera possa se mover
        }
    }

    public void MoveCamRight() //Metodo chamado no script "RoomChangeControl"
    {
        if (!cameraCanMove) //If para checar se a c�mera j� est� se movendo e evitar bugs (se estiver ele n�o faz nada)
        {
            GameObject.Find("Player").transform.position += new Vector3(2.0f, 0.0f, 0.0f); //Transporta o player para o outro cen�rio
            currentCameraPos = Camera.main.transform.position; //Armazena a posi��o atual da c�mera
            newCameraPos = currentCameraPos += new Vector3(19.18f, 0.0f, 0.0f); //Calcula a posi��o final da c�mera
            cameraCanMove = true; //Permite que a c�mera possa se mover
        }
    }
}
