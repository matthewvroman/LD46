using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://github.com/acron0/Easings/blob/master/Easings.cs
public class Easing : MonoBehaviour {

	public static float EaseOutBack(float t)
	{
		float f = (1 - t);
		return 1 - (f * f * f - f * Mathf.Sin(t * Mathf.PI));
	}

	public static float EaseOutQuad(float p)
	{
		return -(p * (p - 2));
	}
}