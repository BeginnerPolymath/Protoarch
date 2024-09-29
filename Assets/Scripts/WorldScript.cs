using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriInspector;

[DeclareVerticalGroup("Entity")]
public class WorldScript : MonoBehaviour
{
    [Group("Entity")]
    public string EntityName;
    [InlineEditor]
    [Group("Entity")]
    [ListDrawerSettings(ShowElementLabels = true)]
    public List<EntityScript> Entities = new List<EntityScript>();

    public GameObject SystemsGO;
    public GameObject ElementsGO;
    public GameObject GlobalComponentsGO;



    public LifecycleScript Lifecycle;


    [Button("Create Entity")]
    [Group("Entity"), PropertyOrder(1)]
    public EntityScript CreateEntity ()
    {
        GameObject entityGO = new GameObject();
        entityGO.name = $"Entity {Entities.Count}";

        EntityScript entity = entityGO.AddComponent<EntityScript>();

        entity.World = this;
        entity.Name = EntityName;

        Entities.Insert(0, entity);

        return entity;
    }

}
