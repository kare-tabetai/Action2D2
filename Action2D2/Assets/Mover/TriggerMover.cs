using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerMover : Mover {
	
    protected override abstract void Move();

    public virtual void ChildTriggerEnter2D(Collider2D col) { }
    public virtual void ChildTriggerStay2D(Collider2D col) { }
    public virtual void ChildTriggerExit2D(Collider2D col) { }
}
