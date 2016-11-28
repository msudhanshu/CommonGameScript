using UnityEngine;
using System.Collections;


/*
 * while rearranging grid , //occupy targetpos first , so that no one dares to move there
 */
  
public class DummyTargetBlock : GridBlock {
	void Start() {
	    isDummyTarget = true;
	}
}
