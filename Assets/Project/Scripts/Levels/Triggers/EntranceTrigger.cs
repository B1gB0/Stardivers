using Project.Scripts.Levels.Outpost;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EntranceTrigger : Trigger
    {
        [field: SerializeField] public Entrance Entrance { get; private set; }
    }
}