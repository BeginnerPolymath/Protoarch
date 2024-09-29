using System.Collections.Generic;
using UnityEngine;

public class PrintSystem : _System
{
    public PrintSystem ()
    {
        InputComponents = new List<InputComponents>()
        {
            new InputComponents
            (
                new InputComponent("AnyComponent", typeof(object).FullName)
            ),
            new InputComponents
            (
                new InputComponent("AnyComponent", typeof(object).FullName)
            )
        };
    }

    public override void Invoke(List<ElementComponents> components)
    {
        for (int i = 0; i < components.Count; i++)
        {
            Debug.Log(components[0].InputComponents[0].Component.GetObject());
        }
    }
}


public class PrintHealthSystem : _System
{
    public PrintHealthSystem ()
    {
        InputComponents = new List<InputComponents>()
        {
            new InputComponents
            (
                new InputComponent("Health", typeof(object).FullName)
            )
        };
    }

    public override void Invoke(List<ElementComponents> components)
    {
        for (int i = 0; i < components.Count; i++)
        {
            Debug.Log(components[0].InputComponents[0].Component.GetObject());
        }
    }
}
