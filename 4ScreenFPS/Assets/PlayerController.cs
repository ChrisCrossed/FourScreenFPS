using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : PLAYER_INPUT
{
    [SerializeField] XInputDotNetPure.PlayerIndex playerIndex;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerIndex( playerIndex );
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();

        if( playerInput.Button_A == XInputDotNetPure.ButtonState.Pressed )
        {
            print( playerInput.Button_A );
        }
    }
}
