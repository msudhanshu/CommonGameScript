using UnityEngine;
using System.Collections;

public class SpectrumBarOld : MonoBehaviour,ISpectrumBar
{
    public enum BarType { Realtime, PeakLevel, MeanLevel };

    public int index;
    public BarType barType=BarType.Realtime;

    AudioSpectrum spectrum;

    void Awake()
    {
        spectrum = FindObjectOfType(typeof(AudioSpectrum)) as AudioSpectrum;
    }

	private int _BarIndex =1;
	public int BarIndex {
		get {
			return _BarIndex;
		}
		set {
			_BarIndex = value;
			index = _BarIndex;
		}
	}
	public void SetVisualization(SpectrumParam sParam) {
			var vs = transform.localScale;
			vs.y = sParam.DefaultValue * 20.0f;
			transform.localScale = vs;
	}


    void Update ()
    {
		if (index < spectrum.Levels.Length) {
			float scale = 0.0f;
			SpectrumParam s =new SpectrumParam();
			switch (barType) {
			case BarType.Realtime:
				scale = spectrum.Levels[index];
				break;
			case BarType.PeakLevel:
				scale = spectrum.PeakLevels[index];
				break;
			case BarType.MeanLevel:
				scale = spectrum.MeanLevels[index];
				break;
			}
			s.DefaultValue = scale;
			SetVisualization(s);
    	}
	}
}