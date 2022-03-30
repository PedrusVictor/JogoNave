using UnityEngine;
using System.Collections;
using System;
public abstract class Projetil : MonoBehaviour {
	public float vel=0;
	public float dist=0;
	public Vector2 direcao=new Vector2(0,0);
	public Vector2 posIni;
	public Transform t;
	public float temPDisp;
	public GameObject splash;
	public int tipo = 0;
	public bool destDist = true;
	public float[] posRest;
	// Use this for initialization
	public void mover (Vector2 dir){
		t.Translate (dir * vel);
		if(!(-15 < transform.position.x && transform.position.x < 15 && -1*(posRest[3]+2) < transform.position.y && transform.position.y < (posRest[3] + 2)))
		{
			try { Destroy(gameObject); }
			catch
			{
				print("erro");
			}
			
		}
			
		
		
	}
	public void setDir(Vector2 dir){
		this.direcao = dir;
	}

	public abstract void calDist (int forTiro, float distDisparo);

	void Start () {
		posRest = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControleFase>().areasFase;
		t = GetComponent<Transform> ();
		posIni = t.position;
		
		//vel = 0;
		//GetComponent<Rigidbody2D> ().velocity = direcao * vel;
	}

	public virtual void EfeitoDest()
	{
		Destroy(gameObject);
	}
	void OnTriggerEnter2D(Collider2D Objeto){

		 if(gameObject.tag=="Bala"&&Objeto.tag=="BalaInimiga"){

			if(Objeto.GetComponent<Transform>().parent){
				try
				{
					Objeto.GetComponentInParent<CirculoDeFogo>().projeteis.Remove(Objeto.gameObject);
					Destroy(Objeto.gameObject);
				}
				catch
				{
					if (Objeto != null)
					{
						Destroy(Objeto.gameObject);
					}
					

				}
				

			}
			EfeitoDest();
			//print ("colidiu com inimigo bala");
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		mover (direcao);
		if (Vector2.Distance(posIni, t.position) > dist

			)
		{
			if (gameObject)
			{
				Destroy(gameObject);
			}
			
		}
	}
}
