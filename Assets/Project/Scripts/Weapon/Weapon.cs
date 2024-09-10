using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [field: SerializeField] public string Type { get; private set; }

    public abstract void Shoot();
}