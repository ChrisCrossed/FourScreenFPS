using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : PLAYER_INPUT
{
    

    // Start is called before the first frame update
    void Start()
    {
        InputStart();
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();

        if( playerState.Buttons.A == XInputDotNetPure.ButtonState.Pressed )
        {
            print("A Held");
        }
    }
}
