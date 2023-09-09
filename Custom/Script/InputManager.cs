using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public enum IngameInputHardWare
{
    Mouse = 0,
    JoyStick = 1,
}

public enum InputMode
{
    InGame = 0,
    UI = 1,
}

public enum InputType
{
    Up = 0,
    Down = 1,
    Right = 2,
    Left = 3,
    Shovel = 4,
    Flag = 5,
    Interact = 6,
}

public class InputManager : MonoBehaviour
{
    public static IngameInputHardWare currentInputHardware = IngameInputHardWare.Mouse;

    #region InputCheck
    public class InputCheck
    {
        private Player player;
        public InputCheck(Player _player)
        {
            player = _player;
        }

        public bool isPressingUP()
        {
            return player.GetButton("MoveUP");
        }

        public bool isPressingDown()
        {
            return player.GetButton("MoveDown");
        }

        public bool isPressingRight()
        {
            return player.GetButton("MoveRight");
        }

        public bool isPressingLeft()
        {
            return player.GetButton("MoveLeft");
        }


        public bool isInteractiveButtonDown()
        {
            return player.GetButtonDown("Interact");
        }

    }
    #endregion

    public class InputEvent
    {
        static bool isCurrentInput(InputMode type)
        {
            bool flag = false;

            if(inputControlStack.Count == 0)
            {
                return flag;
            }

            if(inputControlStack.Peek() == type)
            {
                flag = true;
            }

            return flag;
        }

        #region Event
        public static event Action<InputType> MovePressEvent;
        public static void Invoke_MovePressed(InputType inputType)
        {
            MovePressEvent.Invoke(inputType);
        }

        #endregion
    }

    #region static Field
    public static InputManager instance = null;
    private static ControllerMapEnabler.RuleSet[] ruleSets;
    private static ControllerMapEnabler mapEnabler;
    public static Stack<InputMode> inputControlStack = new Stack<InputMode>();
    public static int currentInputRule = 0;
    public static InputCheck inputCheck;
    #endregion

    private Player player;
    
    void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(this.gameObject);
        }

        player = ReInput.players.GetPlayer(0);
        mapEnabler = player.controllers.maps.mapEnabler;

        inputCheck = new InputCheck(player);

        ruleSets = new ControllerMapEnabler.RuleSet[mapEnabler.ruleSets.Count];
        ruleSets[0] = mapEnabler.ruleSets.Find(x => x.tag == "InGame");
        ruleSets[1] = mapEnabler.ruleSets.Find(x => x.tag == "UI");
    }

    public static void getInput(InputMode type)
    {
        if(inputControlStack.Count != 0 && inputControlStack.Peek() == type)
        {
            return;
        }

        inputControlStack.Push(type);
        changePlayerInputRule();
    }

    private static void changePlayerInputRule(int ruleNum)
    {
        foreach(var rule in ruleSets)
        {
            rule.enabled = false;
        }
        ruleSets[ruleNum].enabled = true;

        currentInputRule = ruleNum;

        mapEnabler.Apply();
    }

    private static void changePlayerInputRule()
    {
        if(inputControlStack.Count == 0)
        {
            return;
        }

        switch(inputControlStack.Peek())
        {
            case InputMode.InGame :
                changePlayerInputRule(0);
                break;
            case InputMode.UI :
                changePlayerInputRule(1);
                break;
        }
    }

    private void OnEnable() {
        delegateInputFunctions();
    }

    private void OnDisable() {
        removeInputFunctions();
    }

    public void delegateInputFunctions()
    {
        player.AddInputEventDelegate(UPPressed, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "MoveUp");
        player.AddInputEventDelegate(DownPressed, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "MoveDown");
        player.AddInputEventDelegate(RightPressed, UpdateLoopType.Update, InputActionEventType.ButtonPressed,"MoveRight");
        player.AddInputEventDelegate(LeftPressed, UpdateLoopType.Update, InputActionEventType.ButtonPressed,"MoveLeft");
    }

    public void removeInputFunctions()
    {
        player.RemoveInputEventDelegate(UPPressed);
        player.RemoveInputEventDelegate(DownPressed);
        player.RemoveInputEventDelegate(RightPressed);
        player.RemoveInputEventDelegate(LeftPressed);
    }

    #region moveInputFunctions
    public void UPPressed(InputActionEventData data)
    {
        InputEvent.Invoke_MovePressed(InputType.Up);
    }

    public void DownPressed(InputActionEventData data)
    {
        InputEvent.Invoke_MovePressed(InputType.Down);
    }

    public void RightPressed(InputActionEventData data)
    {
        InputEvent.Invoke_MovePressed(InputType.Right);
    }

    public void LeftPressed(InputActionEventData data)
    {
        InputEvent.Invoke_MovePressed(InputType.Left);
    }
    #endregion


}
