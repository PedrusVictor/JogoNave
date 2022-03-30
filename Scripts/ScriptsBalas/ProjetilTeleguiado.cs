using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public abstract class ProjetilTeleguiado : Projetil {
	public GameObject alvo;
	public  List<GameObject>inimigos;
	public string TipoAlvo;

	public void identificarTInimigo(){
		if(gameObject.tag=="Bala"){
			TipoAlvo="Inimigo";
		}
		else if(gameObject.tag=="BalaInimiga"){
			TipoAlvo="Player";
		}
	}
	public  void LocalizarInimigos(string alvo1){
		inimigos=new List<GameObject>(GameObject.FindGameObjectsWithTag (alvo1));
	}
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
	public int compare(GameObject a,GameObject b){
		return   (Vector2.Distance(a.GetComponent<Transform>().position,GetComponent<Transform>().position)).CompareTo(Vector2.Distance(b.GetComponent<Transform>().position,GetComponent<Transform>().position));
	}
	//ordena usando a distancia dos inimigos para o projetil
	public void ordenar(){
		inimigos.Sort (compare);
	}

}
