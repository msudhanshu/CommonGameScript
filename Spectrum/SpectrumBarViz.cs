//
//  SpectrumCubeViz.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpectrumBarViz : MonoBehaviour, ISpectrumVisualizer
{
		public List<ISpectrumBar> allSpecBars = new List<ISpectrumBar>();
		private int specBarIndex=0;
		SpectrumType specType = SpectrumType.Realtime;

		AudioSpectrum _spectrum;
		public AudioSpectrum spectrum {
			get {
				if(_spectrum==null)
					_spectrum = FindObjectOfType(typeof(AudioSpectrum)) as AudioSpectrum;
				_spectrum.spectrumType = specType;
				return _spectrum;
			}
			set {
			}
		}
		

		public void AddSpectrumBar(ISpectrumBar specBar) {
			specBar.BarIndex = specBarIndex;
			allSpecBars.Add(specBar);
			specBarIndex = (specBarIndex+1)%spectrum.Levels.Length;
		}

	void Start() {
		StartViz();
	}
	
	public void StartViz() {
		StartCoroutine("StartVizCoroutine");
	}
	
	public void StopViz() {
		StopCoroutine("StartVizCoroutine");
	}
	
	
	IEnumerator StartVizCoroutine() {
		while(true) {
			SpectrumParam specBarParamValue = new SpectrumParam();

			foreach(ISpectrumBar spectrumBar in allSpecBars ) {
				specBarParamValue.DefaultValue =  spectrum.Sample[spectrumBar.BarIndex];
				spectrumBar.SetVisualization(specBarParamValue);
			}
			yield return 0;
		}
	}
}

