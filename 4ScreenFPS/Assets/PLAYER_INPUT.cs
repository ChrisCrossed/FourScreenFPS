using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public struct PlayerInput
{
    // Game Settings
    private float _f_zDir; // Forward or Backward
    private float _f_xDir; // Strafe Left or Right
    private float _f_LookHoriz;
    private float _f_LookVert;
    private bool _b_JumpPressed;
    private bool _b_JumpHeld;
    private bool _b_DPad_LeftPressed;
    private bool _b_DPad_RightPressed;
    private bool _b_DPad_UpPressed;
    private bool _b_DPad_DownPressed;
    private Vector2 _v2_DPad;
    private ButtonState _d_Button_A;
    private ButtonState _d_Button_B;
    private ButtonState _d_Button_X;
    private ButtonState _d_Button_Y;
    private ButtonState _Start;
    private ButtonState _Select;

    // Analog Sticks
    public float zDir
    {
        internal set
        {
            float f_Temp = value;

            // Cap the value between -1 and 1
            if (f_Temp < -1.0f) f_Temp = -1.0f;
            if (f_Temp > 1.0f) f_Temp = 1.0f;

            // If very close to 0f, set to 0f
            if (f_Temp < .05f && f_Temp > -0.05f) f_Temp = 0f;

            _f_zDir = f_Temp;
        }
        get
        {
            return _f_zDir;
        }
    }
    public float xDir
    {
        set
        {
            float f_Temp = value;

            // Cap the value between -1 and 1
            if (f_Temp < -1.0f) f_Temp = -1.0f;
            if (f_Temp > 1.0f) f_Temp = 1.0f;

            // If very close to 0f, set to 0f
            if (f_Temp < .05f && f_Temp > -0.05f) f_Temp = 0f;

            _f_xDir = f_Temp;
        }
        get
        {
            return _f_xDir;
        }
    }

    public float LookHoriz
    {
        set
        {
            _f_LookHoriz = value;
        }
        get { return _f_LookHoriz; }
    }
    public float LookVert
    {
        set
        {
            _f_LookVert = value;
        }
        get { return _f_LookVert; }
    }

    // DPad
    public bool DPad_Pressed_Left
    {
        set { _b_DPad_LeftPressed = value; }
        get { return _b_DPad_LeftPressed; }
    }
    public bool DPad_Pressed_Right
    {
        set { _b_DPad_RightPressed = value; }
        get { return _b_DPad_RightPressed; }
    }
    public bool DPad_Pressed_Up
    {
        set { _b_DPad_UpPressed = value; }
        get { return _b_DPad_UpPressed; }
    }
    public bool DPad_Pressed_Down
    {
        set { _b_DPad_DownPressed = value; }
        get { return _b_DPad_DownPressed; }
    }

    public Vector2 DPadVector
    {
        set { _v2_DPad = value; }
        get { return _v2_DPad; }
    }

    // A/B/X/Y
    public ButtonState Button_A
    {
        internal set { _d_Button_A = value; }
        get { return _d_Button_A; }
    }
    public ButtonState Button_B
    {
        internal set { _d_Button_B = value; }
        get { return _d_Button_B; }
    }
    public ButtonState Button_X
    {
        internal set { _d_Button_X = value; }
        get { return _d_Button_X; }
    }
    public ButtonState Button_Y
    {
        internal set { _d_Button_Y = value; }
        get { return _d_Button_Y; }
    }

    // Bumpers
    public ButtonState Bumper_Left
    {
        internal set;
        get;
    }
    public ButtonState Bumper_Right
    {
        internal set;
        get;
    }

    public ButtonState Trigger_Left
    {
        internal set;
        get;
    }
    public ButtonState Trigger_Right
    {
        internal set;
        get;
    }

    // Start & Select
    public ButtonState Button_Start
    {
        set { _Start = value; }
        internal get { return _Start; }
    }
    public ButtonState Button_Select
    {
        set { _Select = value; }
        get { return _Select; }
    }
}

public class PLAYER_INPUT : MonoBehaviour
{
    PlayerIndex playerIndex;
    GamePadState playerState;
    GamePadState playerState_Previous;

    internal PlayerInput playerInput;

    // Start is called before the first frame update
    internal void InputStart()
    {
        playerInput = new PlayerInput();
        playerState = new GamePadState();

        playerIndex = gameObject.GetComponent<PlayerController>().playerIndex;
    }

    internal void SetPlayerIndex( PlayerIndex _index )
    {
        playerIndex = _index;
    }

    internal PlayerIndex GetPlayerIndex()
    {
        return playerIndex;
    }

    float f_InputMinimum = 0.01f;
    Vector2 v2_DPad;
    Vector2 v2_DPad_Old;

    // Update is called once per frame
    internal void InputUpdate()
    {
        playerState = GamePad.GetState( playerIndex );

        #region Analog Sticks
        // Move Horizontally
        if (playerState.ThumbSticks.Left.X >= f_InputMinimum || playerState.ThumbSticks.Left.X <= -f_InputMinimum) playerInput.xDir = playerState.ThumbSticks.Left.X;
        else playerInput.xDir = 0f;

        // Move Forward / Backward
        if (playerState.ThumbSticks.Left.Y >= f_InputMinimum || playerState.ThumbSticks.Left.Y <= -f_InputMinimum) playerInput.zDir = playerState.ThumbSticks.Left.Y;
        else playerInput.zDir = 0f;

        // Look Up / Down
        if (playerState.ThumbSticks.Right.X >= f_InputMinimum || playerState.ThumbSticks.Right.X <= -f_InputMinimum) playerInput.LookHoriz = playerState.ThumbSticks.Right.X;
        else playerInput.LookHoriz = 0f;

        // Look Up / Down
        if (playerState.ThumbSticks.Right.Y >= f_InputMinimum || playerState.ThumbSticks.Right.Y <= -f_InputMinimum) playerInput.LookVert = playerState.ThumbSticks.Right.Y;
        else playerInput.LookVert = 0f;
        #endregion

        #region DPad Bools and Vector2
        // Reset values
        playerInput.DPad_Pressed_Left = false;
        playerInput.DPad_Pressed_Right = false;
        playerInput.DPad_Pressed_Up = false;
        playerInput.DPad_Pressed_Up = false;
        v2_DPad = new Vector2();

        if (playerState.DPad.Left == ButtonState.Pressed)
        {
            if (v2_DPad_Old.x >= 0f) playerInput.DPad_Pressed_Left = true;

            v2_DPad.x = -1f;
        }
        else if (playerState.DPad.Right == ButtonState.Pressed)
        {
            if (v2_DPad_Old.x <= 0f) playerInput.DPad_Pressed_Right = true;

            v2_DPad.x = 1f;
        }

        if (playerState.DPad.Up == ButtonState.Pressed)
        {
            if (v2_DPad_Old.y <= 0f) playerInput.DPad_Pressed_Up = true;

            v2_DPad.y = 1f;
        }
        else if (playerState.DPad.Down == ButtonState.Pressed)
        {
            if (v2_DPad_Old.y >= 0f) playerInput.DPad_Pressed_Down = true;

            v2_DPad.y = -1f;
        }

        // Normalize
        v2_DPad.Normalize();

        // Set value
        playerInput.DPadVector = v2_DPad;

        // Store old value for evaluation
        v2_DPad_Old = v2_DPad;
        #endregion

        #region A/B/X/Y
        playerInput.Button_A = ButtonState.Released;
        playerInput.Button_B = ButtonState.Released;
        playerInput.Button_X = ButtonState.Released;
        playerInput.Button_Y = ButtonState.Released;
        if (playerState.Buttons.A == ButtonState.Pressed)
        {
            playerInput.Button_A = ButtonState.Pressed;
        }
        if (playerState.Buttons.B == ButtonState.Pressed)
        {
            playerInput.Button_B = ButtonState.Pressed;
        }
        if (playerState.Buttons.X == ButtonState.Pressed)
        {
            playerInput.Button_X = ButtonState.Pressed;
        }
        if (playerState.Buttons.Y == ButtonState.Pressed)
        {
            playerInput.Button_Y = ButtonState.Pressed;
        }
        #endregion

        #region Trigger Input
        if (playerState.Triggers.Left > 0.4f) playerInput.Trigger_Left = ButtonState.Pressed;
        else playerInput.Trigger_Left = ButtonState.Released;

        if (playerState.Triggers.Right > 0.4f) playerInput.Trigger_Right = ButtonState.Pressed;
        else playerInput.Trigger_Right = ButtonState.Released;
        #endregion

        #region Bumpers
        if (playerState.Buttons.LeftShoulder == ButtonState.Pressed)
            playerInput.Bumper_Left = ButtonState.Pressed;
        else
            playerInput.Bumper_Left = ButtonState.Released;

        if (playerState.Buttons.RightShoulder == ButtonState.Pressed)
            playerInput.Bumper_Right = ButtonState.Pressed;
        else
            playerInput.Bumper_Right = ButtonState.Released;
        #endregion

        #region Start & Select
        if (playerState.Buttons.Start == ButtonState.Pressed) playerInput.Button_Start = ButtonState.Pressed;
        else playerInput.Button_Start = ButtonState.Released;

        if (playerState.Buttons.Back == ButtonState.Pressed) playerInput.Button_Select = ButtonState.Pressed;
        else playerInput.Button_Select = ButtonState.Released;
        #endregion

        playerState_Previous = playerState;
    }
}
