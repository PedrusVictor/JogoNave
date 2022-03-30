using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class InimigoSubChefe : Inimigo {
	Transform t;
	public List< GameObject> municao=new List<GameObject>();
	public GameObject disparador;
	public List<UnityEditor.MonoScript> mecbala=new List<UnityEditor.MonoScript>();
	public Transform mira;
	public float disMover;
	public float vel;
	public float tempvel;
	public bool testar=false;
	public float taxAumVelInv;
	public float quantRecInv;
	public float teste;
	public float velMaxRec;
	public bool rotina= false;
	Animator anim;
	public GameObject zona;
	public int tipoAtaque=0;
	public float PorParaActEspecial;
	bool atkspecial = false;
	public int dirMov=-1;
	private ControleFase control;
	// Use this for initialization
	void Start () {
		setCorOriginalMonstro(GetComponent<SpriteRenderer>().color);
		control = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControleFase>();
		t = GetComponent<Transform> ();
		VidaMax = vida;
		anim = GetComponent<Animator> ();
		t.position = new Vector2 (5.1f,dirMov*3);
		anim.SetFloat("hp", vida);
	}
	IEnumerator Movimento(int dirMv){
		StartCoroutine (rajadaTiros(5,0,1.2f,disparador,municao[0],mecbala[0],0));
		for(float a=0;a<disMover;a+=(2*tempvel)){
			//caso nao use time.delta time, o valor da velocidade deve ser 0.25
			t.Translate(vel*Time.deltaTime*Mathf.Cos(2*a*dirMv),vel*Time.deltaTime*Mathf.Sin(a*dirMv),0);
			yield return new WaitForSeconds(Time.deltaTime);

		}
		//StopCoroutine (Movimento ());
	}
	IEnumerator rajadaTiros(int quantTiros,int quantPrjDisp,float tempDisp,GameObject disparador,GameObject municao,UnityEditor.MonoScript MecBalas,float quantRecInv){
		for(int a=0;a<quantTiros;a++){
			GameObject bala= Instantiate (disparador, mira.position, GetComponent<Transform>().rotation) as GameObject;
			bala.GetComponent<Disparador> ().municao = municao;
			bala.GetComponent<Disparador> ().Disparo (quantPrjDisp, 4,MecBalas,"BalaInimiga",-t.right,0);
			for(float b=quantRecInv;b>0;b-=vel*Time.deltaTime){
				t.Translate(t.right*b*vel*Time.deltaTime);
				yield return new WaitForSeconds(tempvel);
			}
			yield return new WaitForSeconds(tempDisp);
		}
	}


	IEnumerator Recuo(Vector2 posRecuo,float quantRecInv,float velMax){
		float velAtual = 0;
		anim.SetBool ("Recuo",true);
		while ((Vector2)t.position!=posRecuo) {
			if((Vector2)t.position==posRecuo){
				break;
			}
			velAtual=Mathf.Lerp(velAtual,velMax,0.25f*Time.deltaTime);
			if (posRecuo.y < -5f)
			{
				posRecuo.y = -5f;
			}
			t.position=Vector2.MoveTowards(t.position,posRecuo,velAtual*vel*quantRecInv*0.9f);


			yield return new WaitForSeconds(tempvel);

		}
		anim.SetBool ("Recuo",false);
	}

	IEnumerator Perseguir(Transform player,float tempPers){
		float temp=Time.time;
		StartCoroutine (rajadaTiros((int)tempPers,0,tempPers*0.25f,disparador,municao[1],mecbala[1],0));
		while(Time.time-temp<tempPers){
			if(Time.time-temp>tempPers){
				break;
			}
			float posy = player.position.y;
			if (posy < -4.5f)
			{
				posy = -4.5f;
			}
			t.position=Vector2.MoveTowards(t.position,(new Vector2(11,posy)),vel*Time.deltaTime*1.5f);
			yield return new WaitForSeconds(tempvel);
		}

	}
	IEnumerator Investida(float TaxAumVel,float quantRecInv){
		anim.SetBool ("Inv",true);
		for(float a=0;t.position.x>-11.5f;a+=taxAumVelInv){

			t.position=Vector2.MoveTowards(t.position,new Vector2(-11.5f,t.position.y),(vel+a)*Time.deltaTime);
			//t.Translate(-t.right*(vel+a)*Time.deltaTime);
			yield return new WaitForSeconds(tempvel);
		}
		yield return new WaitForSeconds (0.15f);
		anim.SetBool ("Inv",false);
		for(float b=quantRecInv;b>0;b-=vel*Time.deltaTime){
			t.Translate(t.right*b*vel*Time.deltaTime);
			yield return new WaitForSeconds(tempvel);
		}

	}
	IEnumerator ActEspecial(){
		ivunerabilidade = true;
		anim.Play ("carregandoExplMulasemcabec");
		yield return new WaitForSeconds (2);
		for(int a=0;a<8;a++){
			GameObject bala= Instantiate(municao[0],t.position,t.rotation) as GameObject;
			
			bala.AddComponent (mecbala[0].GetClass());
			bala.GetComponent<Projetil>().direcao = new Vector2((Mathf.Sin(45*a*Mathf.PI/180)),(Mathf.Cos(45*a*Mathf.PI/180)));
			bala.GetComponent<Projetil>().calDist(0,1.5f);
			bala.GetComponent<Projetil>().vel=1;
			bala.tag = "BalaInimiga";
			bala.GetComponent<Transform>().localScale=new Vector2(1.5f,1.5f);
		}

		anim.Play ("AnimMulSemCab");
		dirMov = -1;
		yield return StartCoroutine(Recuo((new Vector2(11,5*dirMov)),quantRecInv/2,velMaxRec/4));
		PorParaActEspecial+=PorParaActEspecial;
		rotina=false;
		ivunerabilidade = false;
		tipoAtaque = 0;

		atkspecial = false;
		StopAllCoroutines();
	}

	IEnumerator ActPerdaDeHp(){
		if (rotina&&!atkspecial) {
			
			if((vida/VidaMax)<=(1-PorParaActEspecial)+0.1f){
				//Debug.Log("entrou na açao de hp");
				//explosao
				atkspecial = true;
				StopAllCoroutines();
				yield return StartCoroutine(ActEspecial());
				

			}
			yield return new WaitForSeconds(tempvel);
			Matar();
		}

	}

	IEnumerator Comportamento(){

		if (vida > 0)
		{
			GameObject[] pers = GameObject.FindGameObjectsWithTag("Player");
			Transform pPers = pers[0].transform;
			int lengthPers = pers.Length;
			float dist = Vector2.Distance(transform.position, pPers.position);
			for (int a = 1; a < lengthPers; a++)
			{

				if (Vector2.Distance(pers[a].transform.position, transform.position) < dist)
				{
					dist = Vector2.Distance(pers[a].transform.position, transform.position);
					pPers = pers[a].transform;
				}
			}
			//print("COMPORTAMENTO");
			StartCoroutine(ActPerdaDeHp());
			yield return new WaitForSeconds(2);
			yield return StartCoroutine(Movimento(dirMov));
			yield return new WaitForSeconds(2);
			yield return StartCoroutine(Perseguir(pPers, 4));
			yield return StartCoroutine(Recuo((new Vector2(t.position.x, pPers.position.y * 0.9f)), quantRecInv, velMaxRec * 1.5f));
			switch ((int)(tipoAtaque % 2))
			{
				case 0:
					if (control)
						control.AtivarAviso(new Vector2((-30 / 2) - t.right.x + t.position.x, t.position.y), Vector3.zero);
					yield return new WaitForSeconds(1);


					yield return StartCoroutine(Investida(taxAumVelInv, quantRecInv));
					break;

				case 1:

					yield return new WaitForSeconds(1);
					yield return StartCoroutine(rajadaTiros(1, 3, 0, disparador, municao[0], mecbala[0], quantRecInv / 2));
					break;
			}

			tipoAtaque = ((tipoAtaque % 2) + 1);
			yield return new WaitForSeconds(2);
			yield return StartCoroutine(Recuo((new Vector2(11, 5 * dirMov)), quantRecInv, velMaxRec));
			StopAllCoroutines();
			rotina = false;
		}

		

		
	}


	// Update is called once per frame
	void FixedUpdate () {

		if(!rotina){
			rotina=true;
			StopAllCoroutines();
			StartCoroutine(Comportamento());
			dirMov*=-1;
		}
		if (vida < 1 && ivunerabilidade==false) {
			StopAllCoroutines();
			anim.SetFloat("hp", vida);
			ivunerabilidade = true;
		}
		
		/*if (vida < 1)
		{
			StopAllCoroutines();
			Matar();
		}*/
		/*if(Input.GetKeyDown(KeyCode.L)){
			StopAllCoroutines();
			StartCoroutine(Comportamento());
			//testar=false;
			//StartCoroutine(Investida(taxAumVelInv,quantRecInv));
			//StartCoroutine(Perseguir(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(),3));
			//StartCoroutine(Recuo((new Vector2(11,1)),quantRecInv));
			//StartCoroutine(Movimento());
		}*/
	}
}
