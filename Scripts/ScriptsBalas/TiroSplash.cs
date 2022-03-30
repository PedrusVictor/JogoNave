using UnityEngine;
using System.Collections;

public class TiroSplash : Projetil {

	public override void calDist (int forTiro, float distDisparo){
		dist=5+((forTiro+1)*0.25f)+((distDisparo+1)*3);
	}
	// Use this for initialization

	
	// Update is called once per frame

	public override void EfeitoDest()
	{
		if(splash!=null){

			GameObject splsh=Instantiate(splash,transform.position,transform.rotation)as GameObject;

			}
		Destroy(gameObject);
	}
	void OnTriggerEnter2D(Collider2D Objeto){
		/*if (Objeto.tag == "Inimigo" && gameObject.tag == "Bala") {
			GameObject splsh=Instantiate(splash,t.position,t.rotation)as GameObject;
			splsh.gameObject.tag="Bala";
		}*/
		if (gameObject.tag == "BalaInimiga" && Objeto.gameObject.tag == "cauda") {
			//print ("dfdf");
			Destroy (gameObject);
		} 
		
		else if(gameObject.tag=="Bala"&&Objeto.tag=="BalaInimiga"){
			
			if(Objeto.GetComponent<Transform>().parent){
				Objeto.GetComponentInParent<CirculoDeFogo>().projeteis.Remove(Objeto.gameObject);
				Destroy (Objeto.gameObject);
				
			}

			EfeitoDest();
			//print ("colidiu com inimigo bala");
		}
	}
}
