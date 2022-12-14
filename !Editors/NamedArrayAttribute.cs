using UnityEngine;
using UnityEditor;

/*
 * Use [NamedArray (string array)] to give the array elements names in the inspector.
 * To assign an existing array, use [NamedArray (existingArray)]
 * To use a new array, use [NamedArray (new string[] {"Option 1", "Option 2", "Option 3", "etc." })]
 */

    //DO NOT EDIT THIS CLASS


public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    /// <summary>
    /// Standard Format: [NamedArray(new string[] {"Item1", "Item2", "Item3"})]
    /// </summary>
    /// <param name="names"></param>
    public NamedArrayAttribute(string[] names) { this.names = names; }
}
