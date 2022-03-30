using UnityEngine;
using System.Collections;

public class BalaNormal : Projetil {
	public override void calDist (int forTiro, float distDisparo){
		dist=6+((forTiro+1)*0.25f)+((distDisparo+1)*4);
	}
	// Use this for initialization

	void OnTriggerEnter2D(Collider2D Objeto){
		
		if (gameObject.tag == "BalaInimiga" && Objeto.gameObject.tag == "cauda") {
			//print ("dfdf");
			Destroy (gameObject);
		} 
		
		else if(gameObject.tag=="Bala"&&Objeto.tag=="BalaInimiga"){
			
			if(Objeto.GetComponent<Transform>().parent){
				Objeto.GetComponentInParent<CirculoDeFogo>().projeteis.Remove(Objeto.gameObject);
				Destroy (Objeto.gameObject);
				
			}
			Destroy(gameObject);
			//print ("colidiu com inimigo bala");
		}
	}
	// Update is called once per frame

}
