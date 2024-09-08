using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScoreActor : MonoBehaviour
{
    public abstract void Accept(IActorVisitor visitor);
}
