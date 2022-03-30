using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class ChefeMapinguari : Inimigo {
	Transform t;
	public GameObject projetil;
	public GameObject disparador;
	public MonoScript mecBala;
	public GameObject[] bracos=new GameObject[2];
	public GameObject corpo;
	public float velRevCorpo;
	public Color corFinal;
	public bool rev=false;
	public int modo=0;
	public float Tempvel;
	public float vel;
	Animator anim;
	public Material[] matRaio=new Material[2];
	public GameObject esfRaio;
	public int dirMov=-1;
	public float ValorhpTroca = 0.5f;
	[SerializeField] private LayerMask player;
	public Transform p;
	public GameObject boca;
	private bool open = false;
	public GameObject inim;
	private Transform pers;
	private ControleFase control;
	
	// Use this for initialization
	void Start () {
		setCorOriginalMonstro(GetComponent<SpriteRenderer>().color);
		ivunerabilidade = true;
		control = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControleFase>();
		VidaMax = vida;
		pers = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		t = GetComponent<Transform> ();
		anim = GetComponent<Animator> ();
		
		StartCoroutine(RevelarBoss());

	//	Vector2[] vetores = areaColisao ((new Vector2(0,0)),(new Vector2(0,1)));
		//	Vector2[] vetores = new Vector2[4];
/*
		vetores [0] = new Vector2 (0, 0);
		vetores [1] = new Vector2 (0, 5);
		vetores [2] = new Vector2 (1, 5);
		vetores [3] = new Vector2 (1, 0);*/
					        

		//print (teste.GetComponent<Collider2D> ().IsTouching (GetComponent<Collider2D>()));
			//print("colidiu com o raio");

	}

	IEnumerator RevelarBoss()
	{
		gameObject.SetActive(true);
		for (float a=0;a<1;a+=velRevCorpo) {
			gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, a);
			yield return new WaitForSeconds(0.05f);
		}
		gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1); 
		rev = true;
		ivunerabilidade = false;
	}
	Vector2[] areaColisao(Vector2 posIn, Vector2 posFinal){
		Vector2 dirColisor=((posFinal-posIn)/(Vector2.Distance(posIn,posFinal)));
		Vector2[] area = new Vector2[4];
		// 0 e 1 vetores proximos a esfera e 2 e 3 distante

		dirColisor.Set(-dirColisor.y,dirColisor.x);
		area [0] = (dirColisor*0.3f);
		area [1] = (-1*dirColisor*0.3f);
		area [2] = ((posFinal-posIn)*0.7f)+(-1*dirColisor*0.3f);
		area [3] = ((posFinal-posIn)*0.7f)+(dirColisor*0.3f);
	
		return area;

	}

	/*void OnTriggerEnter2D(Collider2D Obj){
		if(Obj.gameObject.tag=="Raio"){
			print("colidiu com o boss");
		}
	}*/
	IEnumerator MovAlv(Vector3 posAlv){
		//print ("entrou");
		while(t.position!=posAlv){
			t.position=Vector2.MoveTowards(t.position,posAlv,vel);
			yield return new WaitForSeconds(Tempvel);
		}
	}

	IEnumerator LaserOcular(Vector2 pos,Vector2 pos2){
		anim.SetBool ("MovOlho", true);
		yield return new WaitForSeconds (2.3f);
		anim.SetBool ("MovOlho", false);
		anim.CrossFade ("MapinguariCarregandoRaio", 0.1f);
		yield return new WaitForSeconds (2);
		anim.CrossFade ("mapinguariCabParada", 0.1f);
		GameObject esfLaser = Instantiate(esfRaio, t.position-new Vector3(0,1,0),t.rotation) as GameObject;

		//MiraLaser.GetComponent<Transform> ().position = t.position-new Vector3(0,1.32f,0);
		esfLaser.AddComponent<LineRenderer> ();
		esfLaser.GetComponent<LineRenderer>().sortingOrder = 2;
		esfLaser.GetComponent<LineRenderer> ().material = matRaio[0];
		Vector3 MiraLaser = t.position - new Vector3 (0, 1.32f, 0);
		Vector3 posFinal = new Vector2 (dirMov*13,-8)+ new Vector2( pos.x*dirMov,pos.y);
		esfLaser.GetComponent<LineRenderer> ().SetPosition (0, esfLaser.GetComponent<Transform>().position);
		esfLaser.GetComponent<LineRenderer> ().SetPosition (1, MiraLaser);
	//	StartCoroutine (ColRaio (esfRaio.GetComponent<Transform> ().position, MiraLaser, true,"Player"));
		esfLaser.AddComponent<PolygonCollider2D> ();
		esfLaser.GetComponent<PolygonCollider2D> ().isTrigger = true;
		esfLaser.tag="Raio";
		esfLaser.GetComponent<PolygonCollider2D>().SetPath (0, areaColisao (esfLaser.GetComponent<Transform>().position, new Vector2(MiraLaser.x,MiraLaser.y)));
		
		while (MiraLaser!=(posFinal)){
			
			yield return new WaitForSeconds(Tempvel);
			esfLaser.GetComponent<PolygonCollider2D>().SetPath (0, areaColisao (esfLaser.GetComponent<Transform>().position, new Vector2(MiraLaser.x,MiraLaser.y)));
			esfLaser.GetComponent<LineRenderer> ().SetPosition (1, MiraLaser);
			MiraLaser=Vector2.MoveTowards(MiraLaser,(posFinal),vel*5);
			Debug.DrawRay(esfLaser.transform.position, new Vector2(MiraLaser.x * 0.7f, MiraLaser.y), Color.blue);
			RaycastHit2D raio = Physics2D.Raycast(esfLaser.transform.position, new Vector2(MiraLaser.x * 0.7f, MiraLaser.y),Mathf.Infinity,player);
			if (raio
				)
			{
				raio.collider.GetComponentInParent<movBoitata>().dano();
			}
		}
		
		yield return new WaitForSeconds (1);
		posFinal = new Vector2 (((dirMov*13)+(-dirMov*16)),-8)+ new Vector2(pos2.x * dirMov, pos2.y);

		while(MiraLaser!=(posFinal)){
			Debug.DrawRay(esfLaser.transform.position, new Vector2(MiraLaser.x * 0.7f, MiraLaser.y), Color.blue);
			RaycastHit2D raio = Physics2D.Raycast(esfLaser.transform.position, new Vector2(MiraLaser.x * 0.7f, MiraLaser.y), Mathf.Infinity, player);
			if (raio
				)
			{
				//print("colidiu" + raio.collider.gameObject.transform.parent);
				raio.collider.GetComponentInParent<movBoitata>().dano();
			}
			yield return new WaitForSeconds(Tempvel);
			esfLaser.GetComponent<PolygonCollider2D>().SetPath (0, areaColisao (esfLaser.GetComponent<Transform>().position, new Vector2(MiraLaser.x,MiraLaser.y)));
			esfLaser.GetComponent<LineRenderer> ().SetPosition (1, MiraLaser);
			MiraLaser=Vector2.MoveTowards(MiraLaser,(posFinal),vel*2);
			
			
		}
		yield return new WaitForSeconds (0.5f);
		esfLaser.GetComponent<LineRenderer> ().material = matRaio[1];

		/*while(MiraLaser!=esfLaser.GetComponent<Transform>().position){
			yield return new WaitForSeconds(Tempvel);
			esfLaser.GetComponent<PolygonCollider2D>().SetPath (0, areaColisao (esfLaser.GetComponent<Transform>().position, new Vector2(MiraLaser.x,MiraLaser.y)));
			esfLaser.GetComponent<LineRenderer> ().SetPosition (1, MiraLaser);
			MiraLaser=Vector2.MoveTowards(MiraLaser,esfLaser.GetComponent<Transform>().position,vel*8*Time.deltaTime);


		}*/
	
		Destroy (esfLaser.gameObject);
		dirMov *= -1;
		//Destroy (MiraLaser);
		//anim.Play ("MapinguariCarregandoRaio");
	}
	void invMonstros()
	{
		if (open)
		{
			GameObject enemy = Instantiate(inim, boca.transform.position + (Vector3.up * 2), GetComponent<Transform>().rotation) as GameObject;
		}
		

		
		

	}
	IEnumerator MovCab(int quanMov){
		for (int a=0; a<quanMov; a++) {
			
			yield return StartCoroutine (MovAlv (pers.position));
			yield return new WaitForSeconds(0.5f);
			for (int b=0; b<5; b++) {
				yield return StartCoroutine (rajadaTiros (1, 0, 0.3f, disparador, projetil, mecBala, 0, (new Vector2 ((Mathf.Sin (72 * b * Mathf.PI / 180)), ((Mathf.Cos(72 * b * Mathf.PI / 180)))))));
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
	IEnumerator rajadaTiros(int quantTiros,int quantPrjDisp,float tempDisp,GameObject disparador,GameObject municao,MonoScript MecBalas,float quantRecInv,Vector2 direct){
		for(int a=0;a<quantTiros;a++){
			GameObject bala= Instantiate (disparador, t.position, GetComponent<Transform>().rotation) as GameObject;
			bala.GetComponent<Disparador> ().municao = municao;

			bala.GetComponent<Disparador> ().Disparo (quantPrjDisp, 6,MecBalas,"BalaInimiga",direct,0);
			for(float b=quantRecInv;b>0;b-=vel){
				t.Translate(t.right*b*vel);
				yield return new WaitForSeconds(Tempvel);
			}
			yield return new WaitForSeconds(tempDisp);
		}
	}
	//IEnumerator DispararFeixes(){}
	IEnumerator RevelarCorpo(float velRev){
		for(int a=0;a<2;a++){
			bracos[a].SetActive(true);
		}
		corpo.SetActive (true);
		bracos [0].GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, 0);
		bracos[1].GetComponent<SpriteRenderer>().color= new Color (255, 255, 255, 0);
		corpo.GetComponent<SpriteRenderer>().color= new Color (255, 255, 255, 0);
		for(float a=0;a<1.1f;a+=velRev){

			yield return new WaitForSeconds(0.025f);
			if(a>=1){

				break;
				
			}
			for(int b=0;b<2;b++){
				
				bracos[b].GetComponent<SpriteRenderer>().color=new Color(255,255,255,a);
			}
			corpo.GetComponent<SpriteRenderer>().color=new Color(255,255,255,a);


		}
	}

	IEnumerator comportamento1(){
		
		 if (vida > ValorhpTroca * VidaMax)
		{
			while (vida > ValorhpTroca * VidaMax)
			{

				yield return new WaitForSeconds(2);
				yield return StartCoroutine(MovCab(3));
				yield return new WaitForSeconds(0.5f);
				yield return StartCoroutine(MovAlv((new Vector2(0, 4.22f))));
				if (control)
					control.AtivarAviso(new Vector2(5 * (t.right.x * dirMov) + t.position.x, 0), new Vector3(0, 0, -90));
				yield return StartCoroutine(LaserOcular(Vector2.zero, Vector2.zero));

				yield return new WaitForSeconds(2);
				if (vida <= ValorhpTroca * VidaMax)
				{


					yield return StartCoroutine(RevelarCorpo(velRevCorpo));

					yield return new WaitForSeconds(1);




					break;
				}
			}
		}
		else if(vida > 0)
		{
			while (vida > 0)
			{
				invMonstros();
				
				yield return new WaitForSeconds(2);
				for (int b = 0; b < 5; b++)
				{
					yield return StartCoroutine(rajadaTiros(1, 0, 0.3f, disparador, projetil, mecBala, 0, (pers.position - transform.position) / Vector2.Distance(transform.position, pers.position)));


				}
				yield return new WaitForSeconds(1);

				int idBraco = 0;
				if (dirMov > 0)
				{
					idBraco = 1;
				}
				bracos[idBraco].GetComponent<Animator>().ResetTrigger("soco");
				bracos[idBraco].GetComponent<Animator>().SetTrigger("soco");
				yield return new WaitForSeconds(bracos[idBraco].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
				yield return new WaitForSeconds(1);
				bracos[idBraco].GetComponent<Animator>().ResetTrigger("soco");
				invMonstros();
				bracos[idBraco].GetComponent<Animator>().SetTrigger("porrada");
				yield return new WaitForSeconds(bracos[idBraco].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
				yield return new WaitForSeconds(1);
				for (int b = 0; b < 5; b++)
				{
					yield return StartCoroutine(rajadaTiros(1, 0, 0.3f, disparador, projetil, mecBala, 0, (new Vector2((Mathf.Sin(72 * b * Mathf.PI / 180)), ((Mathf.Cos(72 * b * Mathf.PI / 180)))))));

				}
				yield return new WaitForSeconds(1);
				if (control)
					control.AtivarAviso(new Vector2(-(t.right.x * dirMov) + t.position.x, 0), new Vector3(0, 0, -90));
				yield return StartCoroutine(LaserOcular(Vector2.one * 12, new Vector2(0, -12)));
				open = !open;
				boca.GetComponent<Animator>().SetBool("abrir", open);
				yield return new WaitForSeconds(1);
			}
		}
		
		//matar falta colocar uma animação de morte




	}
	// Update is called once per frame
	void Update () {
		
		if (rev){
			rev=false;
			
			StartCoroutine(comportamento1());
			
			
		}
		if (vida < 1&& !ivunerabilidade)
		{
			ivunerabilidade = true;
			
			StopAllCoroutines();
			Matar();
			

		}

	}
}
