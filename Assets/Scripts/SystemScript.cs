using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TriInspector;
using Unity.VisualScripting;

[Serializable]
[DeclareHorizontalGroup("Component")]
public class InputComponent
{
    [TriInspector.HideLabel]
    [Group("Component")]
    public string Name;

    [TriInspector.HideLabel]
    [Group("Component")]
    public string Type;

    public InputComponent (string name, string type)
    {
        Name = name;
        Type = type;
    }
}

[Serializable]
public class InputComponents
{
    [ListDrawerSettings(Draggable = false, HideAddButton = true, HideRemoveButton = true, AlwaysExpanded = true)]
    public List<InputComponent> Components = new List<InputComponent>();

    public InputComponents(params InputComponent[] inputComponent)
    {
        Components.AddRange(inputComponent);
    }
}

[Serializable]
[DeclareHorizontalGroup("Element", Sizes = new float[] {0, 50} )]
[DeclareHorizontalGroup("Buttons", Sizes = new float[] {60, 0, 75} )]
public abstract class _System
{
    [HideLabel]
    [HideInInspector]
    public string Name;

    [HideInInspector]
    public bool Enable;

    [ListDrawerSettings(Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    [ReadOnly]
    public List<InputComponents> InputComponents = new List<InputComponents>();

    [InlineEditor]
    [ListDrawerSettings(ShowElementLabels = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    [Group("Element")]
    public List<ElementScript> Elements = new List<ElementScript>();

    [ListDrawerSettings(ShowElementLabels = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    public List<EntityScript> Entities = new List<EntityScript>();

    public void ElementsInvoke ()
    {
        if(!Enable)
            return;

        for (int i = 0; i < Elements.Count; i++)
        {
            if(Elements[i].Enable)
                Invoke(Elements[i].InputComponents);
        }
    }

    [Button("Enable")]
    [GUIColor("$EnableColor")]
    [PropertyOrder(0)]
    [Group("Buttons")]
    public void EnableFunc()
    {
        Enable = !Enable;
    }

    public Color EnableColor ()
    {
        if(Enable)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }

    [Group("Element")]
    [Button("Add")]
    public void AddElement ()
    {
        ElementScript element = World.ElementsGO.AddComponent<ElementScript>();

        element.SystemScript = SystemScript;

        element.InputComponents = new List<ElementComponents>(InputComponents.Count);

        Elements.Insert(0, element);
    }

    [HideInInspector]
    public WorldScript World;

    [HideInInspector]
    public SystemScript SystemScript;


    [Button("Invoke")]
    [Group("Buttons")]
    [EnableIf("Enable")]
    public void Invoke ()
    {
        ElementsInvoke();
    }

    [Button("Remove")]
    [GUIColor("red")]
    [Group("Buttons")]
    public void Remove ()
    {
        
        SystemScript.LifecycleScript.StartSystemGroups.Remove(SystemScript);

        GameObject.DestroyImmediate(SystemScript);
    }

    public abstract void Invoke (List<ElementComponents> components);
}

[DrawWithTriInspector]
[HideMonoScript]
public class SystemScript : Base
{
    [HideInInspector]
    public LifecycleScript LifecycleScript;

    [SerializeReference]
    [InlineProperty]
    [HideLabel]
    public _System System;

    public ElementScript AddElementByEntities (params EntityScript[] entity)
    {
        ElementScript element = World.ElementsGO.AddComponent<ElementScript>();

        element.SystemScript = this;

        for (int i = 0; i < System.InputComponents.Count; i++)
        {
            element.InputComponents.Add(new ElementComponents(System.InputComponents[i].Components.Count));
            
            foreach (var component in System.InputComponents[i].Components)
            {
                foreach (var entityComponent in entity[i].Components)
                {
                    if(component.Name == entityComponent.Name)
                    {
                        element.InputComponents[i].InputComponents.Add(entityComponent);

                        entityComponent.ElementScripts.Add(element);
                    }
                }
            }
        }

        System.Elements.Insert(0, element);

        return element;
    }
}
