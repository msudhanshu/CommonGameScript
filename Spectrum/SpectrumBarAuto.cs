//
//  SpectrumCube.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;

public class SpectrumBarAuto : MonoBehaviour, ISpectrumBar
{
	private int _BarIndex =1;
	public int BarIndex {
		get {
			return _BarIndex;
		}
		set {
			_BarIndex = value;
		}
	}

	public void SetVisualization(SpectrumParam sParam) {
		var vs = transform.localScale;
		vs.y = sParam.DefaultValue * 20.0f;
		transform.localScale = vs;
	}

}

