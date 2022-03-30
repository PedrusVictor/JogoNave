using UnityEngine;
using System.Collections;

public class BalaRicochete :Projetil{
	int ricochete=0;
	// Use this for initialization

	void Start () {
		
		//calDist (forc, dist);
		t = GetComponent<Transform> ();
		posRest = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControleFase>().areasFase;
		//vel = 0;
		//GetComponent<Rigidbody2D> ().velocity = direcao * vel;
	}
	public override void calDist (int forTiro, float distDisparo){
		dist=(forTiro/3)+distDisparo;
	}
	public void Ricochete(){

		if (GetComponent<Transform> ().position.y > (posRest[3] + 1) || GetComponent<Transform> ().position.y < -1*(posRest[3] + 1)) {
			direcao=new Vector2(direcao.x,direcao.y*-1*Mathf.Cos(45/180*Mathf.PI));
			if(ricochete>dist){
				
				Destroy(gameObject);
			}
			ricochete++;
			//Destroy(gameObject);
		}
		else if (GetComponent<Transform> ().position.x > (posRest[1] + 1) || GetComponent<Transform> ().position.x <= -(posRest[1] + 1)) {
			direcao=new Vector2(direcao.x*-1,direcao.y*Mathf.Cos(45/180*Mathf.PI));

			if(ricochete>dist){
				print("recdestroi2");
				Destroy(gameObject);
			}ricochete++;

			//Destroy(gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D Obstaculo){
		if(Obstaculo.gameObject.tag=="Inimigo"){
			if(ricochete>dist){
				Destroy(gameObject);
			}
			if(Mathf.Abs(Obstaculo.GetComponent<Transform>().position.y-GetComponent<Transform>().position.y)>0f){
				setDir(new Vector2(direcao.x,direcao.y*2*Mathf.Cos(45/180*Mathf.PI)));
				ricochete++;
			}
			//Destroy(gameObject);
			//print ("bateu" +Obstaculo.gameObject.name);
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		Ricochete ();

		mover (direcao);

	}
}
