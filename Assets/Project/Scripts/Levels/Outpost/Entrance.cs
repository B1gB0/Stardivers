using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Entrance : MonoBehaviour
{
    public readonly int IsOpened = Animator.StringToHash(nameof(IsOpened));
    
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerActor playerActor))
        {
            OpenGate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerActor playerActor))
        {
            CloseGate();
        }
    }

    public void OpenGate()
    {
        _animator.SetBool(IsOpened, true);
    }

    public void CloseGate()
    {
        _animator.SetBool(IsOpened, false);
    }
}
