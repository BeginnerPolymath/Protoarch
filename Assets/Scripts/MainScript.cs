using TriInspector;
using UnityEngine;

[HideMonoScript]
public abstract class Base : MonoBehaviour
{
    [HideInInspector]
    public WorldScript World;

    [PropertyOrder(0)]
    [HideLabel]
    [Group("Name")]
    public string Name;

    void Start ()
    {
        
    }
}