using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriInspector;
using System;

[Serializable]
public class ElementComponents
{
    [ListDrawerSettings(Draggable = false, HideAddButton = true, HideRemoveButton = true, AlwaysExpanded = true)]
    public List<ComponentScript> InputComponents = new List<ComponentScript>();

    public ElementComponents () {}

    public ElementComponents (int componentsCount)
    {
        InputComponents = new List<ComponentScript> (componentsCount);
    }
}

[DrawWithTriInspector]
[HideMonoScript]
public class ElementScript : MonoBehaviour
{
    [HideInInspector]
    public SystemScript SystemScript;

    [ListDrawerSettings(Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    public List<ElementComponents> InputComponents = new List<ElementComponents>();

    [HideInInspector]
    public bool Enable;

    //[HideInInspector]
    public List<EntityScript> Entities = new List<EntityScript>();

    [Button("Enable")]
    [GUIColor("$EnableColor")]
    [PropertyOrder(0)]
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

    [Button("Remove")]
    [GUIColor("red")]
    public void Remove ()
    {
        SystemScript.System.Elements.Remove(this);

        foreach (var entity in Entities)
        {
            foreach (var systemMeta in entity.SystemsMeta)
            {
                if(systemMeta.SystemScript == SystemScript)
                {
                    systemMeta.Elements.Remove(this);

                    if(systemMeta.Elements.Count == 0)
                    {
                        entity.SystemsMeta.Remove(systemMeta);

                        SystemScript.System.Entities.Remove(entity);
                    }

                    break;
                }
            }

            foreach (var component in entity.Components)
            {
                component.ElementScripts.Remove(this);
            }
        }

        DestroyImmediate(this);
    }
}
