using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Play1 : MonoBehaviour
{
    [SerializeField]
    private InputActionReference move;
    public PlayerInput playerInput;
    public float speed;
    public Vector3 moveInput;
    
    void Start()
    {
        //Isso aqui é pra trocar os controles, funcionando até então
        //playerInput.actions["Move"].ChangeCompositeBinding("3DVector").Erase();

        /*playerInput.actions["Move"].AddCompositeBinding("3DVector")
        .With("Up", "<Keyboard>/u")
        .With("Left", "<Keyboard>/h")
        .With("Down", "<Keyboard>/j")
        .With("Right", "<Keyboard>/k")
        .With("Foward", "")
        .With("Backward", "");*/
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        moveInput = move.action.ReadValue<Vector3>();
        transform.position = transform.position + speed * moveInput * Time.deltaTime;
    }
}
