using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriInspector;
using System;


public interface ISystem
{
    public string Name {get; set;}

    public abstract void ElementsInvoke();
}

[Serializable]
[DrawWithTriInspector]
public class SystemGroup : ISystem
{
    public string Name {get; set;}

    [SerializeReference]
    public List<ISystem> Systems = new List<ISystem>();

    public void ElementsInvoke ()
    {
        for (int i = 0; i < Systems.Count; i++)
        {
            Systems[i].ElementsInvoke();
        }
    }
}

[DeclareHorizontalGroup("System", Sizes = new float[] { 75, 0, 50 })]
[DrawWithTriInspector]
public class LifecycleScript : MonoBehaviour
{
            [InlineEditor, ListDrawerSettings(ShowElementLabels = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    public List<SystemScript> StartSystemGroups = new List<SystemScript>();

    public WorldScript World;

    [Group("System")]
    [TriInspector.HideLabel]
    public string SystemName;

    [Group("System")]
    [TriInspector.HideLabel]
    [TriInspector.LabelWidth(15)]
    public _System NewSystem;

    [Button("Add")]
    [Group("System")]
    public void AddStartSystem ()
    {
        SystemScript system = World.SystemsGO.AddComponent<SystemScript>();
        system.Name = SystemName;
        system.System = NewSystem;
        system.World = World;
        system.LifecycleScript = this;

        system.System.Name = SystemName;
        system.System.World = World;
        system.System.SystemScript = system;

        StartSystemGroups.Add(system);

        NewSystem = null;
    }



    void Start()
    {
        for (int i = 0; i < StartSystemGroups.Count; i++)
        {
            StartSystemGroups[i].System.Invoke();
        }
    }
}
