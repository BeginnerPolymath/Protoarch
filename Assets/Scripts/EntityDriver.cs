using TriInspector;
using UnityEngine;

public class EntityDriver : MonoBehaviour
{
    public WorldScript World;

    [InlineEditor(Mode = InlineEditorModes.GUIOnly)]
    public EntityScript Entity;

    void Reset ()
    {
        World = FindFirstObjectByType<WorldScript>();

        Entity = World.CreateEntity();
    }
}
