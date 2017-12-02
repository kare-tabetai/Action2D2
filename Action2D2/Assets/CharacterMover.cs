using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMover : TriggerMover
{
    public enum State { Standing,Moving, Dying, Damaging, }

    public int hp;

    protected override abstract void Move();

    public abstract void Damaged(int dmg);
}
