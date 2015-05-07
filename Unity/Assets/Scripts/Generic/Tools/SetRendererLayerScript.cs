using UnityEngine;
using System.Collections;

public class SetRendererLayerScript : MonoBehaviour 
{
	public string sortingLayerName;
	public int sortingOrder;

	// Use this for initialization
	void Start () 
	{
		GetComponent<Renderer>().sortingLayerName = sortingLayerName;
		GetComponent<Renderer>().sortingOrder = sortingOrder;
	}
}
