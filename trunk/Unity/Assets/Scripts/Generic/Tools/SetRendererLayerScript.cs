using UnityEngine;
using System.Collections;

public class SetRendererLayerScript : MonoBehaviour 
{
	public string sortingLayerName;
	public int sortingOrder;

	// Use this for initialization
	void Start () 
	{
		renderer.sortingLayerName = sortingLayerName;
		renderer.sortingOrder = sortingOrder;
	}
}
