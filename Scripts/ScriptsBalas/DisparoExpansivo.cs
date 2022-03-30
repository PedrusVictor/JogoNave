using UnityEngine;
using System.Collections;

public class DisparoExpansivo : Projetil {
	public float TamMax=4;
	public float velExpansao=0.04f;
	public override void calDist (int forTiro, float distDisparo){
		dist=5+((forTiro+1)*0.5f)+((distDisparo+1)*3);
	}
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		mover ((direcao*(t.localScale.x/5)));
		t.localScale = Vector2.Lerp (t.localScale,(new Vector2(this.TamMax,this.TamMax)),(velExpansao));
		if (Vector2.Distance (posIni, t.position) > dist) {
			Destroy(gameObject);
		}
	}
}
