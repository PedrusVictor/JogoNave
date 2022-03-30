using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CirculoDeFogo : MonoBehaviour {
	public List<GameObject>projeteis = new List<GameObject> ();

	public GameObject img;
	Transform t;
	float tempo;
	public float velRot;
	public int cont=0;
	// Use this for initialization
	void Start () {
		gameObject.name = "CirculoDeFogo";
		t = GetComponent<Transform> ();
		tempo = Time.time;
	}
	/*void CriarCirculodeFogo(int cont){
	
			projeteis[cont]= new GameObject();
		projeteis [cont].GetComponent<Transform> ().SetParent(t);
			projeteis[cont].GetComponent<Transform> ().position =t.position +1.5f*(new Vector3((Mathf.Sin(120*cont*Mathf.PI/180)),(Mathf.Cos(120*cont*Mathf.PI/180)),0));
			projeteis[cont].AddComponent <SpriteRenderer> ().sprite = img;
			projeteis[cont].GetComponent<SpriteRenderer> ().color = Color.blue;
			projeteis[cont].AddComponent<Projetil> ().vel=0;
			projeteis[cont].GetComponent<Projetil> ().direcao = new Vector2((Mathf.Sin(45*cont*Mathf.PI/180)),(Mathf.Cos(45*cont*Mathf.PI/180)));
			projeteis[cont].GetComponent<Projetil> ().dist = 12;
			projeteis[cont].tag = "BalaInimiga";
			projeteis[cont].AddComponent<Rigidbody2D> ().gravityScale = 0;
			//projeteis[cont].AddComponent<CircleCollider2D> ().isTrigger = true;
			projeteis[cont].GetComponent<Transform>().localScale=new Vector2(1.5f,1.5f);

	}*/
	void CriarCirculodeFogo(){

		projeteis.Add (Instantiate(img,t.position + 1.5f * (new Vector3 ((Mathf.Sin (120 * (projeteis.Count - 1) * Mathf.PI / 180)), (Mathf.Cos (120 * (projeteis.Count - 1) * Mathf.PI / 180)), 0)),t.rotation)as GameObject);

		if (projeteis.Count > 0) {
			projeteis [(projeteis.Count - 1)].name = "p" + projeteis.Count.ToString ();
			//	projeteis[(projeteis.Count-1)]= new GameObject();
			projeteis [(projeteis.Count - 1)].GetComponent<Transform> ().SetParent (t);
			//projeteis [(projeteis.Count - 1)].GetComponent<Transform> ().position = t.position + 1.5f * (new Vector3 ((Mathf.Sin (120 * (projeteis.Count - 1) * Mathf.PI / 180)), (Mathf.Cos (120 * (projeteis.Count - 1) * Mathf.PI / 180)), 0));
			/*projeteis [(projeteis.Count - 1)].AddComponent <SpriteRenderer> ().sprite = img;
			projeteis [(projeteis.Count - 1)].GetComponent<SpriteRenderer> ().color = Color.blue;*/
			//projeteis[(projeteis.Count-1)].AddComponent<Projetil> ().vel=0;
			//projeteis[(projeteis.Count-1)].GetComponent<Projetil> ().dist = 12;
			projeteis [(projeteis.Count - 1)].tag = "BalaInimiga";//mudei de BalaInimiga para Inimigo
		/*	projeteis [(projeteis.Count - 1)].AddComponent<Rigidbody2D> ().gravityScale = 0;
			projeteis [projeteis.Count-1].AddComponent<CircleCollider2D> ().isTrigger = true;*/
			projeteis [(projeteis.Count - 1)].GetComponent<Transform> ().localScale = new Vector2 (1.5f, 1.5f);
		}
		//print ("adicionou");
	}

	void girar(){
		for(int a=0;a<projeteis.Count;a++){

			projeteis[a].GetComponent<Transform>().RotateAround(t.position,new Vector3(0,0,1),-velRot);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {


		if (cont<3&& Time.time - tempo > 0.5f) {
			CriarCirculodeFogo ();
			tempo = Time.time;
			cont++;

		} else if(cont>2){
			girar();

		}
	}
}
