using UnityEngine; // 41 Post - Created by DimasTheDriver on May/17/2012 . Part of the 'Unity: Making a simple audio visualization' post. http://www.41post.com/?p=4776
using System.Collections;

public class SpectrumLineVizAuto : MonoBehaviour ,ISpectrumVisualizer
{
	//A renderer that will draw a line at the screen
	private LineRenderer lRenderer;
	//A reference to the cube prefab
	public GameObject cube;
	//The transform attached to this game object
	private Transform goTransform;
	//The position of the current cube. Will also be the position of each point of the line.
	private Vector3 cubePos;
	//An array that stores the Transforms of all instantiated cubes
	private Transform[] cubesTransform;
	//The velocity that the cubes will drop
	private Vector3 gravity = new Vector3(0.0f,0.25f,0.0f);


	AudioSpectrum _spectrum;
	public AudioSpectrum spectrum {
		get {
			if(_spectrum==null)
				_spectrum = FindObjectOfType(typeof(AudioSpectrum)) as AudioSpectrum;
			return _spectrum;
		}
		set {
		}
	}

	SpectrumType specType = SpectrumType.Realtime;

	void Awake () 
	{
		//LineRenderer
		this.lRenderer = GetComponent<LineRenderer>();
		//Transform
		this.goTransform = GetComponent<Transform>();
	}
	
	void Start()
	{
		//The line should have the same number of points as the number of samples
		spectrum.spectrumType = specType;
		//spectrum.numberOfSamples= 64;

		lRenderer.SetVertexCount(spectrum.Length);
		//The cubesTransform array should be initialized with the same length as the samples array
		cubesTransform = new Transform[spectrum.Length];
		//Center the audio visualization line at the X axis, according to the samples array length
		goTransform.position = new Vector3(-spectrum.Length/2,goTransform.position.y,goTransform.position.z);
		
		//Create a temporary GameObject, that will serve as a reference to the most recent cloned cube
		GameObject tempCube;
		
		//For each sample
		for(int i=0; i<spectrum.Length;i++)
		{
			//Instantiate a cube placing it at the right side of the previous one
			tempCube = (GameObject) Instantiate(cube, new Vector3(goTransform.position.x + i, goTransform.position.y, goTransform.position.z),Quaternion.identity);
			//Get the recently instantiated cube Transform component
			cubesTransform[i] = tempCube.GetComponent<Transform>();
			//Make the cube a child of this game object
			cubesTransform[i].parent = goTransform;
		}

		StartViz();
	}

	public void StartViz() {
		StartCoroutine("StartVizCoroutine");
	}

	public void StopViz() {
		StopCoroutine("StartVizCoroutine");
	}


	IEnumerator StartVizCoroutine() {
		//Obtain the samples from the frequency bands of the attached AudioSource
		//	aSource.GetSpectrumData(this.samples,0,FFTWindow.BlackmanHarris);
		while(true) {
		//For each sample
		for(int i=0; i<spectrum.Length;i++)
		{
			/*Set the cubePos Vector3 to the same value as the position of the corresponding 
			 * cube. However, set it's Y element according to the current sample.*/
			cubePos.Set(cubesTransform[i].position.x, Mathf.Clamp(spectrum.Sample[i]*(50+i*i),0,50), cubesTransform[i].position.z);
			
			//If the new cubePos.y is greater than the current cube position
			if(cubePos.y >= cubesTransform[i].position.y)
			{
				//Set the cube to the new Y position
				cubesTransform[i].position = cubePos;
			}
			else
			{
				//The spectrum line is below the cube, make it fall
				cubesTransform[i].position -= gravity;
			}
			
			/*Set the position of each vertex of the line based on the cube position.
			 * Since this method only takes absolute World space positions, it has 
			 * been subtracted by the current game object position.*/
			lRenderer.SetPosition(i, cubePos - goTransform.position);
		}
			yield return 0;
		}
	}

}
