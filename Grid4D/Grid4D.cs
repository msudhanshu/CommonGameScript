using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Album3D;

public class Grid4D : MonoBehaviour {

	public Vector3 GridSize = new Vector3(5,5,5);
	public GridBlock dummyTargetBlock;
	public float speed = 1f;
//	public int GridSizeY = 5;
//	public int GridSizeZ = 5;
	GridBlock[ , , ] gridArray;

	// Use this for initialization
	void Start () {
		CreateGrid();
		StartCoroutine(InfiniteRearrangeGrid());
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A)) {
			Debug.LogError("..............");
			StartCoroutine(InfiniteRearrangeGrid());
		}
	}

	public IEnumerator InfiniteRearrangeGrid() {
		while(true) {
			RearrangeGrid();
			yield return new WaitForSeconds(1);
		}
	}

	public void CreateGrid() {
		gridArray =  new GridBlock[(int)GridSize.x,(int)GridSize.y,(int)GridSize.z];
		StartCoroutine(CreatingGrid());
	}

	void OnDrawGizmos() {
		//depth = 2*bubbleRadius.y;
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(GridSize*0.5f-Vector3.one*0.5f,GridSize+Vector3.one*0.1f);
		Gizmos.color = Color.green;
		for(int k=0;k<GridSize.z;k++) {
			for(int j=0;j<GridSize.y;j++) {
				for(int i=0;i<GridSize.x;i++) {
						Gizmos.DrawWireCube(new Vector3(i,j,k),Vector3.one);
				}
			}
		}
	}


	private IEnumerator CreatingGrid() {
		for(int k=0;k<GridSize.z;k++) {
			for(int j=0;j<GridSize.y;j++) {
				for(int i=0;i<GridSize.x;i++) {
					if(ShouldRandomCreate()) {
						gridArray[i,j,k] = CreateBlock(i,j,k);
						//yield return 0;//new WaitForSeconds(0.01);
					}
				}
			}
		}
		yield break;
	}

	private bool ShouldRandomCreate() {
		//if(Random.Range(0,2)==1) {
		if(Random.value > 0.8f) return true;
		return false;
	}


	//this should go somewhere else and should be a prefab.
	private GridBlock CreateBlock(int x, int y, int z) {
		GameObject bgo = GameObject.CreatePrimitive(PrimitiveType.Cube);
		bgo.transform.position = new Vector3(x,y,z);
		GridBlock b = bgo.AddComponent<GridBlock>();
		bgo.AddComponent<Album3D.Screen>();
	//	bgo.AddComponent<SpectrumBar>();
		bgo.AddComponent<Target>();
		return b;
	}

	public void RearrangeGrid() {
		for(int k=0;k<GridSize.z;k++) {
			for(int j=0;j<GridSize.y;j++) {
				for(int i=0;i<GridSize.x;i++) {
					if(gridArray[i,j,k]!=null) {
						Vector3 curPos = new Vector3(i,j,k);
						GridBlock gb = GetGridBlocks(curPos);
						if(gb!=null && !gb.isDummyTarget && !gb.isMoving) {
							Vector3? vtargetPos = FindFreeNeighbour(curPos);
							if(vtargetPos!=null && vtargetPos.HasValue ) {
								Vector3 targetPos = vtargetPos??curPos;
								//occupy targetpos first , so that no one dares to move there
								SetGridBlocks(targetPos,dummyTargetBlock);
								gb.isMoving = true;
								StartCoroutine(MovingGridBlock(curPos,targetPos));
							} else {
//								Debug.LogError("No neigbour found for "+curPos);
							}
						}
					}
				}
			}
		}
	}

	private float GetSpeed() {
		return speed;
	}

	private IEnumerator MovingGridBlock(Vector3 curPos, Vector3 targetPos) {
		int i =0;
		//Debug.Log("entered "+curPos +" , "+targetPos);

		GridBlock gb = GetGridBlocks(curPos);

	  float startTime = Time.time;
		float distCovered =0;
		float fracJourney=0;
		while(fracJourney<=0.95f) {
//			Debug.Log("........"+fracJourney);
			 distCovered = (Time.time - startTime) * GetSpeed();
			 fracJourney = distCovered / 1.0f;
			if(gb==null) break;
			 gb.transform.position = Vector3.Lerp(curPos, targetPos,fracJourney );
			 yield return 0 ;
			//yield return new WaitForSeconds(0.0f);
		}

		//yield return 0;
		if(gb!=null)
		gb.transform.position = targetPos;
		//Debug.Log("final pos="+gb.transform.position+"..final pos index="+targetPos+"...., curPos index="+curPos);
		//gb.transform.position = curPos;
		SetGridBlocks(targetPos,gb);
		SetGridBlocks(curPos,null);
		gb.isMoving = false;

		//Debug.Log("exit "+curPos +" , "+targetPos);

		yield break;
	}

	private Vector3? FindFreeNeighbour(Vector3 curPos) {
		List<Vector3> allNeighbour = new List<Vector3>();
		AddN(curPos,new Vector3(-1,0,0), allNeighbour);
		AddN(curPos,new Vector3(1,0,0), allNeighbour);
		AddN(curPos,new Vector3(0,-1,0), allNeighbour);
		AddN(curPos,new Vector3(0,1,0), allNeighbour);
		AddN(curPos,new Vector3(0,0,-1), allNeighbour);
		AddN(curPos,new Vector3(0,0,1), allNeighbour);

		int i = Random.Range(0,allNeighbour.Count);
		//Debug.Log("random selected n="+i+", total n="+allNeighbour.Count);
		if(allNeighbour.Count==0) return null;
		return allNeighbour[i];
	}

	private void AddN (Vector3 curPos, Vector3 nOffset,  List<Vector3> allNeighbour) {
		Vector3 nPos = curPos + nOffset;
		//Debug.Log("expected nepos for "+curPos +" is="+nPos.ToString());
		if( IsInRange(nPos , Vector3.zero, GridSize ) )
		{
			GridBlock gb = GetGridBlocks(nPos);
           		 if( gb==null) {
					//Debug.Log("neigbhour null");
					allNeighbour.Add(nPos);
				} else {
					//Debug.Log("occupied"+gb+"..isdummy="+gb.isDummyTarget);
				}
		} else {
			//Debug.LogError("out of range");
		}
	}

	private bool IsInRange(Vector3 v, Vector3 startRange, Vector3 endRange) {
		if(v.x >= startRange.x && v.y >= startRange.y && v.z >= startRange.z &&
		   v.x < endRange.x && v.y < endRange.y && v.z < endRange.z) return true;
		return false;
	}
	

	private GridBlock GetGridBlocks(Vector3 pos) {
		if(IsInRange(pos , Vector3.zero, GridSize ) )
			return gridArray[(int)pos.x,(int)pos.y,(int)pos.z];
		else
			return null;
	}

	private void SetGridBlocks(Vector3 pos,GridBlock gb) {
		//if(IsInRange(pos , Vector3.zero, GridSize ))
		 gridArray[(int)pos.x,(int)pos.y,(int)pos.z] = gb;
	}
}
