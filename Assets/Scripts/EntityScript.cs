using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;


[Serializable]
public class EntitySystemMeta
{
        [InlineEditor]
    public SystemScript SystemScript;

        [InlineEditor, HideLabel, ListDrawerSettings(ShowElementLabels = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    public List<ElementScript> Elements = new List<ElementScript>();

    public EntitySystemMeta (SystemScript system)
    {
        SystemScript = system;
    }

    public EntitySystemMeta (SystemScript system, ElementScript element)
    {
        SystemScript = system;
        Elements.Add(element);
    }

        [Button("Remove System"), GUIColor("red")]
    public void Remove ()
    {
        WorldManagerScript.EntitySystemRemove(this);
    }
}

[DeclareHorizontalGroup("Component", Sizes = new float[] { 75, 0, 50})]
[DeclareHorizontalGroup("System", Sizes = new float[] { 0, 75})]
[DeclareHorizontalGroup("Name", Sizes = new float[] { 0, 100})]
public class EntityScript : Base
{
        [InlineEditor, ListDrawerSettings(ShowElementLabels = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    public List<ComponentScript> Components = new List<ComponentScript>();

        [Group("Component"), PropertyOrder(0), HideLabel, GUIColor("$ComponentNameColor")]
    public string ComponentName;

        [Group("Component"), PropertyOrder(1), HideLabel, LabelWidth(15), InlineProperty]
    public ComponentBase NewComponent;

        [Group("Component"), PropertyOrder(2), Button("Add"), DisableIf(nameof(NewComponentNull)), GUIColor("$ComponentNameColor")]
    public void AddComponent ()
    {
        ComponentScript component = gameObject.AddComponent<ComponentScript>();

        component.Component = NewComponent;
        component.Type = NewComponent.GetValueType().FullName;
        component.Name = ComponentName;

        component.World = World;
        component.Entities.Add(this);

        Components.Insert(0, component);

        NewComponent = null;
    }

    bool NewComponentNull ()
    {
        foreach (var component in Components)
        {
            if(component.Name == ComponentName)
                return true;
        }

        if(ComponentName == "")
            return true;

        return NewComponent == null;
    }

    public Color ComponentNameColor ()
    {
        foreach (var component in Components)
        {
            if(component.Name == ComponentName)
                return Color.yellow;
        }

        if(ComponentName == "")
            return Color.red;

        return Color.white;
    }

        [Button("Remove Entity"), GUIColor("red"), Group("Name")]
    public void Remove ()
    {
        World.Entities.Remove(this);

        for (int i = 0; i < Components.Count; i++)
        {
            Components[i].Remove();
        }

        DestroyImmediate(gameObject);
    }

        [Dropdown(nameof(GetSystems), ValidationMessageType = TriMessageType.None), HideLabel, Group("System")]
    public SystemScript SystemScript;

        [Button("Register"), Group("System"), DisableIf("$SystemScriptNull")]
    public void RegisterInSystem ()
    {
        ElementScript element = SystemScript.AddElementByEntities(this);

        if(!SystemScript.System.Entities.Contains(this))
        {
            SystemScript.System.Entities.Add(this);

            SystemsMeta.Add(new EntitySystemMeta(SystemScript, element));
        }
        else
        {
            foreach (var systemMeta in SystemsMeta)
            {
                systemMeta.Elements.Add(element);
            }
        }

        element.Entities.Add(this);
    }

    bool SystemScriptNull ()
    {
        if(!SystemScript)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<EntitySystemMeta> SystemsMeta = new List<EntitySystemMeta>();

    private IEnumerable<TriDropdownItem<SystemScript>> GetSystems()
    {
        TriDropdownList<SystemScript> systems = new TriDropdownList<SystemScript>();

        foreach (var system in World.Lifecycle.StartSystemGroups)
        {
            systems.Add(system.Name, system);
        }

        return systems;
    }
}
