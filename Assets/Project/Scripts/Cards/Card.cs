using UnityEngine;

[CreateAssetMenu(menuName = "Cards/New Card")]
public class Card : ScriptableObject
{
    [field: SerializeField] public string Label { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string Level { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
}
