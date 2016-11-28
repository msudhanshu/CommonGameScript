//
//  ISpectrumBar.cs
//
//  Author:
//       Manjeet <msudhanshu@kiwiup.com>
//
using UnityEngine;
using System.Collections;

public interface ISpectrumBar 
{
	int BarIndex {get;set;}
	void SetVisualization(SpectrumParam sParam);
}
