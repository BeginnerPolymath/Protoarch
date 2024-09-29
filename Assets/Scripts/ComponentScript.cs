using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TriInspector;

[Serializable]
public abstract class ComponentBase
{
    public Component<S> Get<S> ()
    {
        return (Component<S>)this;
    }

    public S GetValue<S> ()
    {
        return ((Component<S>)this).Value;
    }

    public ComponentBase Get ()
    {
        return this;
    }

    public abstract A GetExplicitValue<A> ();

    public abstract object GetObject ();

    public abstract Type GetValueType ();

    public abstract void SetValue (object _value);
}

[Serializable]
public abstract class Component<T> : ComponentBase
{
    [TriInspector.HideLabel]
    public T Value;

    public override A GetExplicitValue<A> ()
    {
        return (A)(object)Value;
    }

    public override object GetObject ()
    {
        return Value;
    }

    public override void SetValue (object value)
    {
        Value = (T)value;
    }

    public override Type GetValueType ()
    {
        return Value.GetType();
    }
}

[DeclareHorizontalGroup("Component", Sizes = new float[] {0, 0} )]
[DeclareHorizontalGroup("Name", Sizes = new float[] {0, 65})]
[DeclareHorizontalGroup("Meta", Sizes = new float[] {0, 0})]
public class ComponentScript : Base
{
    [SerializeReference]
    [HideReferencePicker]
    [InlineProperty]
    [TriInspector.HideLabel]
    [Group("Component")]
    public ComponentBase Component;

    [TriInspector.HideLabel]
    [Group("Component")]
    [ReadOnly]
    public string Type;

    [Group("Meta")]
    [ListDrawerSettings(ShowElementLabels = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    public List<EntityScript> Entities = new List<EntityScript>();

    [Group("Meta")]
    [ListDrawerSettings(ShowElementLabels = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    public List<ElementScript> ElementScripts = new List<ElementScript>();

    [Button("Remove")]
    [GUIColor("red")]
    [Group("Name")]
    public void Remove ()
    {
        foreach (var entity in Entities)
        {
            entity.Components.Remove(this);
        }

        foreach (var element in ElementScripts)
        {
            element.Remove();
        }
        
        DestroyImmediate(this);
    }
}