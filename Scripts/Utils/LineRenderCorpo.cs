using UnityEngine;
using System.Collections;

public class LineRenderCorpo : MonoBehaviour {
	LineRenderer Line;
	public Transform PedTras;
	// Use this for initialization
	void Start () {
		Line = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Line.SetPosition (0,GetComponent<Transform>().position);
		//line
	}
}
