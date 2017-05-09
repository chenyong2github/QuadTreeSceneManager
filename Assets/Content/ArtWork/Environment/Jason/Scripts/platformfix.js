#pragma strict

function OnTriggerEnter (other : Collider) { other.transform.parent = gameObject.transform; } function OnTriggerExit (other : Collider) { other.transform.parent = null; }