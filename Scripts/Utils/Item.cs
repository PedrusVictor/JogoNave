using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	private int tipo = 0;
	public float chanceDrop;
	public float tempo;
	public float vel = 0.1f;
	private Vector2 tplayer;
	// Use this for initialization
	void Start () {
		tempo = Time.time;
		
	}
	public void setType(int tipo)
	{
		this.tipo = tipo;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player") ;
		
		foreach (GameObject g in players)
		{
			
			if (g!=null&& g.GetComponent<movBoitata>().nPlayer == tipo)
			{
				tplayer = g.transform.position;
				break;
			}
		}
	}
	void OnTriggerEnter2D(Collider2D Obj){
		if(Obj.gameObject.transform.parent && Obj.transform.parent.tag=="Player"){
			switch(gameObject.tag){
				case "AumDist":

					if(Obj.GetComponentInParent<Atirar>().DistTiro<4){
						Obj.GetComponentInParent<Atirar>().DistTiro++;}
					break;

				case "aumfor":
					if(Obj.GetComponentInParent<Atirar>().forTiro<3){
						Obj.GetComponentInParent<Atirar>().forTiro++;}
					break;

				case "hp":
					Obj.GetComponentInParent<movBoitata>().recHp();
					break;

				case "projetil1":
					Obj.GetComponentInParent<Atirar>().tipoTiro=0;
					Obj.GetComponentInParent<Atirar>().tempDisp=0.3f;
					break;

				case "projetil2":
					Obj.GetComponentInParent<Atirar>().tipoTiro=1;
					Obj.GetComponentInParent<Atirar>().tempDisp=0.4f;
					break;

			}

			Destroy(gameObject);
		}
	}
	private void FixedUpdate()
	{
		//Vector2 tplayer = GameObject.FindGameObjectWithTag("Player").transform.position;
		if (Vector2.Distance(transform.position, tplayer) > 5) {

			transform.position = Vector2.MoveTowards(transform.position, tplayer, vel);
		}
		
	}
	// Update is called once per frame
	void Update () {
		if(Time.time-tempo>5){
			Destroy(gameObject);
		}
	}
}
