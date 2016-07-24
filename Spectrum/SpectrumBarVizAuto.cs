using UnityEngine;
using System.Collections;

public class SpectrumBarVizAuto : MonoBehaviour, ISpectrumVisualizer
{
    public GameObject barPrefab;
    public GUIStyle labelStyle;

	AudioSpectrum _spectrum;
	public AudioSpectrum spectrum {
		get {
			if(_spectrum==null)
				_spectrum = FindObjectOfType(typeof(AudioSpectrum)) as AudioSpectrum;
			_spectrum.spectrumType = SpectrumType.Realtime;
			return _spectrum;
		}
		set {
		}
	}

	SpectrumType barType;
    int barCount;

	void Start() {
        
		StartViz();
	}

    void SetBarVizCamera() {
        
    }

	public void StartViz() {
		StartCoroutine("StartVizCoroutine");
	}
	
	public void StopViz() {
		StopCoroutine("StartVizCoroutine");
	}
	
	
	IEnumerator StartVizCoroutine() {
		while(true) {
		        if (barCount == spectrum.Length && !Input.GetMouseButtonDown(0)) {
		            yield  return 0;
		        } else {

			        // Change the bar type on mouse click.
			        if (Input.GetMouseButtonDown(0)) {
						if (barType == SpectrumType.MeanLevel) {
							barType = SpectrumType.Realtime;
			            } else {
			                barType++;
			            }
			        }

			        // Destroy the old bars.
			        foreach (var child in transform) {
			            Destroy ((child as Transform).gameObject);
			        }

			        // Change the number of bars.
			        barCount = spectrum.Length;
			        var barWidth = 6.0f / barCount;
			        var barScale = new Vector3 (barWidth * 0.9f, 1, 0.75f);

			        // Create new bars.
			        for (var i = 0; i < barCount; i++) {
			            var x = 6.0f * i / barCount - 3.0f + barWidth / 2;

			            var bar = Instantiate (barPrefab, Vector3.right * x, transform.rotation) as GameObject;
						ISpectrumBar specBar =  bar.GetComponent<SpectrumBarOld> ();
						bar.transform.parent = transform;
						bar.transform.localScale = barScale;

			           specBar.BarIndex = i;
						//bar.GetComponent<SpectrumBarOld> ().barType = barType;
						SpectrumParam specBarParamValue = new SpectrumParam();

					specBarParamValue.DefaultValue = spectrum.Sample[i];
					/*	switch (barType) {
							case SpectrumType.Realtime:
								specBarParamValue.DefaultValue =  spectrum.Levels[i];
								break;
							case SpectrumType.PeakLevel:
								specBarParamValue.DefaultValue  = spectrum.PeakLevels[i];
								break;
							case SpectrumType.MeanLevel:
								specBarParamValue.DefaultValue = spectrum.MeanLevels[i];
								break;
						}
						*/
						specBar.SetVisualization(specBarParamValue);
			        }
							yield return 0;
				}
		}
    }

	
	void OnGUI ()
	{
		string text = "Current mode: " + barType + "\n";
		text += "Click the screen to change the mode.";
		GUI.Label (new Rect(0, 0, Screen.width, Screen.height), text, labelStyle);
	}

}