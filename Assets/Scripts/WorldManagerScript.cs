using UnityEngine;

public static class WorldManagerScript
{
    public static void EntityElementRemove (EntityScript entity, SystemScript system, ElementScript element)
    {
        foreach (var systemMeta in entity.SystemsMeta)
        {
            system.System.Elements.Remove(element);

            if(systemMeta.SystemScript == system)
            {
                systemMeta.Elements.Remove(element);

                if(systemMeta.Elements.Count == 0)
                {
                    entity.SystemsMeta.Remove(systemMeta);

                    system.System.Entities.Remove(entity);
                }

                break;
            }
        }
    }

    public static void EntitySystemRemove (EntityScript entity, SystemScript system)
    {
        foreach (var systemMeta in entity.SystemsMeta)
        {
            if(systemMeta.SystemScript == system)
            {
                foreach (var element in systemMeta.Elements)
                {
                    foreach (var _entity in element.Entities)
                    {
                        EntityElementRemove(_entity, system, element);
                    }
                }
            }
        }
    }

    public static void EntitySystemRemove (EntitySystemMeta systemMeta)
    {
        for (int i = 0; i < systemMeta.Elements.Count; i++)
        {
            foreach (var _entity in systemMeta.Elements[i].Entities)
            {
                //EntityElementRemove(_entity, systemMeta.SystemScript, systemMeta.Elements[i]);

                systemMeta.Elements[i].Remove();
            }
        }
    }
}
