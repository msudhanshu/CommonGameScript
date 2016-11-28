using UnityEngine;
using System.Collections;

public class MazeDoor : MazePassage {

	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

	public Transform hinge;
	public Transform hinge_handle;

	private bool isMirrored;

	private MazeDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}
	
	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);
		if (OtherSideOfDoor != null) {
			isMirrored = true;
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			if (child != hinge) {
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
			}
		}
	}

	public override void OnPlayerEntered () {
		StartCoroutine(DoorOpeningCoroutine());
	}
	
	public override void OnPlayerExited () {
		StartCoroutine(DoorClosingCoroutine());
	}

	IEnumerator DoorClosingCoroutine() {
		GetComponent<Animation>()["DoorOpen"].speed = -0.5f;
		GetComponent<Animation>().Play();
		OtherSideOfDoor.GetComponent<Animation>()["DoorOpen"].speed = -0.5f;
		OtherSideOfDoor.GetComponent<Animation>().Play();
		
		//OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
		//OtherSideOfDoor.cell.room.Hide();

		yield return 0;
	}

	IEnumerator DoorOpeningCoroutine() {
		//while()
		//hinge_handle.Translate(0,0.1,0);
		GetComponent<Animation>()["DoorOpen"].speed = 0.5f;
		OtherSideOfDoor.GetComponent<Animation>()["DoorOpen"].speed = 0.5f;
		GetComponent<Animation>().Play();
		OtherSideOfDoor.GetComponent<Animation>().Play();
		//OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
		//OtherSideOfDoor.cell.room.Show();
		yield return 0;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.O) )
			OnPlayerEntered();
		if(Input.GetKeyDown(KeyCode.P) )
			OnPlayerExited();
	}
}