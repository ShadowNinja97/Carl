using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CriticalAttribute : PropertyAttribute //Give this attribute to variables that are important for the GAME itself to function, not just a script.
{ 
    public Color color = Color.red;

} 


