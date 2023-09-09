using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private void OnEnable() {
        InputManager.InputEvent.MovePressEvent += MovePlayer;
    }

    private void OnDisable() {
        InputManager.InputEvent.MovePressEvent -= MovePlayer;
    }

    private void MovePlayer(InputType inputType)
    {
        switch(inputType)
        {
            case InputType.Up :
                
                break;
            case InputType.Down :

                break;
            case InputType.Right :

                break;
            case InputType.Left :

                break;
        }   
    }
}
