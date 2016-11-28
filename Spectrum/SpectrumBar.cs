//
//  SpectrumCube.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;

public class SpectrumBar : MonoBehaviour, ISpectrumBar
{
	SpectrumBarViz specCubeViz;
	public int _BarIndex =1;
	public int BarIndex {
		get {
			return _BarIndex;
		}
		set {
			_BarIndex = value;
		}
	}

	void Start() {
		specCubeViz = FindObjectOfType(typeof(SpectrumBarViz)) as SpectrumBarViz;
		specCubeViz.AddSpectrumBar(this);
	}

	public void SetVisualization(SpectrumParam sParam) {
		var vs = transform.localScale;
		vs.z = sParam.DefaultValue * 20.0f;
		transform.localScale = vs;
	}

}

