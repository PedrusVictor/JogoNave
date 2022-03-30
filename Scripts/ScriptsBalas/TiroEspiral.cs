using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TiroEspiral : Projetil {

	public float valor=0;
	public int dirgiro=-1;
	public int compor=0;
	public float distAlv=8;
	public GameObject InAlvo=null;
	public string TipoAlvo;
	public  List<GameObject>inimigos;

	//metodo para identificar o tipo do alvo
	public void identificarTInimigo(){
		if(gameObject.tag=="Bala"){
			TipoAlvo="Inimigo";
		}
		else if(gameObject.tag=="BalaInimiga"){
			TipoAlvo="Player";
		}
	}
	//cria um lista dos inimigos presentes na cena
	public  void LocalizarInimigos(string alvo1){
		inimigos=new List<GameObject>(GameObject.FindGameObjectsWithTag (alvo1));
	}
	//comparador que sera usado no metodo de ordenar
	public int compare(GameObject a,GameObject b){
		return   (Vector2.Distance(a.GetComponent<Transform>().position,GetComponent<Transform>().position)).CompareTo(Vector2.Distance(b.GetComponent<Transform>().position,GetComponent<Transform>().position));
	}
	//ordena usando a distancia dos inimigos para o projetil
	public void ordenar(){
		inimigos.Sort (compare);
	}
	// Use this for initialization
	void Start(){
		/*inimigos = null;
		identificarTInimigo ();
		LocalizarInimigos (TipoAlvo);
		ordenar ();
		if (inimigos.Count > 0) {
			InAlvo = inimigos [0];
		}
		*/
		//vel = 0;
		posRest = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControleFase>().areasFase;
		t = GetComponent<Transform> ();
		posIni = t.position;
	//	direcao = (InAlvo.GetComponent<Transform> ().position - GetComponent<Transform> ().position) / Vector2.Distance (InAlvo.GetComponent<Transform>().position,t.position);
		//inimigos = GameObject.FindGameObjectsWithTag (TipoAlvo).;
		/*for(int a=0;a<GameObject.FindGameObjectsWithTag (TipoAlvo).Length;a++){
			inimigos.Add (GameObject.FindGameObjectsWithTag (TipoAlvo)[a]);}*/
	}
	//implementaçao do metodo de calcular distancia
	public override void calDist (int forTiro, float distDisparo){
		dist=(5+((forTiro+1)*0.5f)+((distDisparo+1)*3))*1.5f;

	}
	//metodo de achar o inigo proximo. retorna um game object contendo o inimigo mais proximo
	public GameObject acharInimigoProx(){
		GameObject[] inim = GameObject.FindGameObjectsWithTag (TipoAlvo);
		GameObject alvo=null;
	//	InAlvo =ListInimigos [0];
		for(int a=0;a<inim.Length;a++){
			if(a==0){
				alvo=inim[0];
			}
			if(alvo!=null&&Vector2.Distance(GetComponent<Transform>().position,inim[a].GetComponent<Transform>().position)<Vector2.Distance(GetComponent<Transform>().position,alvo.GetComponent<Transform>().position)){
				alvo=inim[a];
			}
		}
		return alvo;
	}

	//metodo de delay. aqui coloco o tempo em segundos o quanto o objeto ficara parado. esse metodo so chama o metodo de espera
	IEnumerator Delay(float segs){

		yield return StartCoroutine (Temporarizador (segs));

	}
	//metodo que faz com que aja a espera
	IEnumerator Temporarizador(float tempdelay){

		yield return new WaitForSeconds (tempdelay);

	}
	//comportamentos
	 IEnumerator Comportamento(){
		switch(compor){
		case 0:
			//Comportamento '0' ou padrao. quando nao entra no modo de rastreio de alvo, o projetil se move em movimento ondulatorio

			//caso esteja muito perto ou a bala tenha andado um certo ponto e tiver um alvo
			if(InAlvo!=null&&Vector2.Distance(GetComponent<Transform>().position,InAlvo.GetComponent<Transform>().position)<distAlv||InAlvo!=null&&Vector2.Distance (posIni, t.position) > dist){
				vel=0;
				yield return StartCoroutine( Delay(0.1f));//da um delay de 0.5f segs para poder se mover novamente

				compor=1;
				vel=1;

			}
			//destroi a bala quando o tempo de vida da bala acabar
			else if (Vector2.Distance (posIni, t.position) > dist&&InAlvo==null) {
				vel=0;
				yield return StartCoroutine( Delay(0.125f));
				InAlvo= acharInimigoProx();
				if(InAlvo==null){
					GetComponent<CircleCollider2D>().enabled=false;
					Destroy(gameObject);}
			}
			//faz o movimento ondulatorio caso nao esteja muito perto do alvo ou nao tenha andado muito(tempo de vida da bala nao acabou)

			else{
				valor += Time.deltaTime*2;
				mover((vel*direcao*0.5f)+new Vector2(vel*direcao.y*0.1f,(Mathf.Cos(valor*20)*direcao.x*vel)/2));
				//mover(((direcao*1.5f+((Mathf.Cos(-valor*dirgiro*8/vel) *new Vector2 (-base.direcao.y,base.direcao.x))/vel))/4).normalized);
			}
			break;

		

		case 1:
			//modo rastreio de alvo. o projetil vai em direçao ao alvo mais proximo.
			if(InAlvo!=null){
				yield return StartCoroutine( Delay(0.125f));
			
				if(InAlvo!=null&&Vector2.Distance(GetComponent<Transform>().position,InAlvo.GetComponent<Transform>().position)>1){
					direcao=Vector2.Lerp(base.direcao,(InAlvo.GetComponent<Transform>().position-GetComponent<Transform>().position)/(Vector2.Distance(InAlvo.GetComponent<Transform>().position,GetComponent<Transform>().position)),0.3f);
					mover(direcao*vel*0.5f);}
			}
				
			 if(InAlvo==null){
				valor=0;
				compor=0;
				InAlvo= acharInimigoProx();
				vel=1;
			}
				break;
		}
	}
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
	void FixedUpdate () {

		StartCoroutine(Comportamento ());


	}

}
