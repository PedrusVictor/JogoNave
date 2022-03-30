using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public abstract class Disparador : MonoBehaviour {
	public GameObject municao;
	public GameObject splash;
	public float veldisp = 0.5f;
	public int compare(GameObject a,GameObject b){
		return   (Vector2.Distance(a.GetComponent<Transform>().position,GetComponent<Transform>().position)).CompareTo(Vector2.Distance(b.GetComponent<Transform>().position,GetComponent<Transform>().position));
	}
	//ordena usando a distancia dos inimigos para o projetil
	public void ordenar(List<GameObject> inimigos){
		inimigos.Sort (this.compare);
	}
	public string identificarTInimigo(string tProj){

		if(tProj=="Bala"){
			return "Inimigo";
		}
		else if(tProj=="BalaInimiga"){
			return "Player";
		}
		return "";
	}
	//metodo de delay. aqui coloco o tempo em segundos o quanto o objeto ficara parado. esse metodo so chama o metodo de espera
public	IEnumerator Delay(float segs){
		
		yield return StartCoroutine (Temporarizador (segs));
		
	}
	//metodo que faz com que aja a espera
public	IEnumerator Temporarizador(float tempdelay){
		
		yield return new WaitForSeconds (tempdelay);
		
	}

	public abstract void Disparo (int forTiro, float distDisparo, MonoScript mecBala, string tipoProjetil,Vector2 direcao,int playerTipo);
}
