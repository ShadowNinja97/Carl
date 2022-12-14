using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as a base for standard events from interactibles like triggers and buttons.
/// </summary>
public abstract class EventTriggerBase : MonoBehaviour
{
    public abstract void Trigger();
}
