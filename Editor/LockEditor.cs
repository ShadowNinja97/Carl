using UnityEngine;
using UnityEditor;

public class LockEditor : MonoBehaviour
{
    [MenuItem("Edit/Toggle Inspector Lock %#q")] //Control-Shift-Q to trigger by default.
    public static void Lock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    [MenuItem("Edit/Toggle Inspector Lock %#q", true)]
    public static bool Valid()
    {
        return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
    }
}
