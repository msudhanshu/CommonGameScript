using UnityEngine;
using System.Collections;

public class GridBlock : MonoBehaviour {
	//[HideInInspector]
	public bool isDummyTarget = false;
	//[HideInInspector]
	public bool isMoving = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GridBlock() {

	}
	public GridBlock(bool dummy) {
		this.isDummyTarget = dummy;
	}

	public static GridBlock dummyTargetBlock = new GridBlock(true);
}