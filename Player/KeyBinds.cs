using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyBinds
{

    #region Default Keybinds

    
    //Basic Movement; Actaully useless, Unity uses a raw axis.
    public static KeyCode forward = KeyCode.W;
    public static KeyCode backwards = KeyCode.S;
    public static KeyCode left = KeyCode.A;
    public static KeyCode right = KeyCode.D;

    public static KeyCode jump = KeyCode.Space;

    public static KeyCode sprint = KeyCode.LeftShift;
    public static KeyCode toggleSprint = KeyCode.T;
    public static KeyCode crouch = KeyCode.C;


    //Interaction
    public static KeyCode interact = KeyCode.E;
    public static KeyCode interactSecondary = KeyCode.Q; //Used for things like dropping


    //Advanced Movement
    public static KeyCode slide = KeyCode.LeftControl;
    public static KeyCode decreaseRope = KeyCode.Space;
    public static KeyCode increaseRope = KeyCode.LeftControl;

    //Weapons
    public static KeyCode shoot = KeyCode.Mouse0;
    public static KeyCode ADS = KeyCode.Mouse1;
    public static KeyCode reload = KeyCode.R;
    public static KeyCode throwGrenade = KeyCode.G;


    //Misc
    public static KeyCode forceRespawn = KeyCode.Tab;
    public static KeyCode forceRestartLevel = KeyCode.Equals;

    public static KeyCode screenshot = KeyCode.Alpha6;

    #endregion

}
