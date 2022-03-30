using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class InimigoTipo3 : Inimigo {
	public float vel;
	Transform t;
	public int dir;
	public float distPer;
	public GameObject img;
	public int comportamentos=1;
	public float ang=0;
	public Vector3 posIn;
	public float raio;
	public Vector2 pos2;
	public float tempo;
	public int contTiro;
	public GameObject preFCirc;
	public GameObject circuloDfog;
	public GameObject explosao;
	public GameObject expl;
	public MonoScript mecBala;

	// Use this for initialization
	void Start () {
		setCorOriginalMonstro(GetComponent<SpriteRenderer>().color);
		vida = 1;
		VidaMax = vida;
		t = GetComponent<Transform> ();
		posIn = t.position;
		circuloDfog=Instantiate(preFCirc,t.position,t.rotation)as GameObject;
		setMorteDist(true);
	}

	void Mover(){
		if (getTypeMonster() == 1) {

			if (Mathf.Abs(t.position.y - posIn.y) < distPer)
			{
				t.Translate(Time.deltaTime * vel * Mathf.Cos(t.position.y),-vel * Time.deltaTime, 0);

			}
			else
			{
				this.tempo = 0;
				comportamentos = 2;
				contTiro = 3;
			}
		}
		else {

			if (Mathf.Abs(t.position.x - posIn.x) < distPer)
			{
				t.Translate(vel * dir * Time.deltaTime, Time.deltaTime * vel * Mathf.Cos(t.position.x), 0);

			}
			else
			{
				this.tempo = 0;
				comportamentos = 2;
				contTiro = 3;
			}
		}
		
	}


	void Atirar(){
		List<GameObject>alvs=new List<GameObject> (GameObject.FindGameObjectsWithTag("Player"));
		ordenar (alvs);

		if (alvs.Count > 0) {

			circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].GetComponent<Transform> ().SetParent (null);
			circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].GetComponent<Transform> ().eulerAngles = new Vector3 (0, 0, 0);
			circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].AddComponent<BalaNormal> ().direcao = ((alvs [0].GetComponent<Transform> ().position - circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].GetComponent<Transform> ().position) / (Vector2.Distance (alvs [0].GetComponent<Transform> ().position, circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].GetComponent<Transform> ().position)));
			//print (((posalvo.position-circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].GetComponent<Transform> ().position)/(Vector2.Distance(posalvo.position,circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].GetComponent<Transform> ().position)))+"direcao");

			circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].GetComponent<Projetil> ().dist = 12;
			circuloDfog.GetComponent<CirculoDeFogo> ().projeteis [0].GetComponent<Projetil> ().vel=0.25f;
			circuloDfog.GetComponent<CirculoDeFogo> ().projeteis.RemoveAt (0);
			this.tempo = 0;
		}
	}


		

		
IEnumerator ExplosaoMorte()
	{
		for (float a=0;a<1;a+=0.05f)
		{
			
			t.Translate(Mathf.Sin(a*9), Mathf.Cos(a * 9),0);
			for (int b = 0; b < circuloDfog.GetComponent<CirculoDeFogo>().projeteis.Count; b++)
			{
				circuloDfog.GetComponent<CirculoDeFogo>().projeteis[b].GetComponent<Transform>().position = Vector2.MoveTowards(circuloDfog.GetComponent<CirculoDeFogo>().projeteis[b].GetComponent<Transform>().position, t.position, 0.1f);
				circuloDfog.GetComponent<CirculoDeFogo>().projeteis[b].GetComponent<Transform>().localScale = new Vector2(circuloDfog.GetComponent<CirculoDeFogo>().projeteis[b].GetComponent<Transform>().localScale.x + 0.05f, circuloDfog.GetComponent<CirculoDeFogo>().projeteis[b].GetComponent<Transform>().localScale.y + 0.05f);
			}
			t.localScale = new Vector2(t.localScale.x+0.05f, t.localScale.y + 0.05f);
			yield return new WaitForSeconds(0.03f);
		}
		if (expl == null)
		{
			expl = Instantiate(explosao, t.position, t.rotation) as GameObject;
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<CircleCollider2D>().enabled = false;
			if (circuloDfog != null && circuloDfog.GetComponent<CirculoDeFogo>().projeteis.Count > 0)
			{
				for (int a = 0; a < (circuloDfog.GetComponent<CirculoDeFogo>().projeteis.Count * 3); a++)
				{
					GameObject bala = Instantiate(img, t.position, t.rotation) as GameObject;

					bala.AddComponent(mecBala.GetClass());
					bala.GetComponent<Projetil>().direcao = new Vector2((Mathf.Sin((360 / (circuloDfog.GetComponent<CirculoDeFogo>().projeteis.Count * 3)) * a * Mathf.PI / 180)), (Mathf.Cos((360 / (circuloDfog.GetComponent<CirculoDeFogo>().projeteis.Count * 3)) * a * Mathf.PI / 180)));
					bala.GetComponent<Projetil>().calDist(0, 1.5f);
					bala.GetComponent<Projetil>().vel = 1;
					bala.tag = "BalaInimiga";

					bala.GetComponent<Transform>().localScale = new Vector2(1.5f, 1.5f);
				}

			}
			Destroy(circuloDfog.gameObject);
		}
		if (expl)
		{
			Destroy(expl.gameObject);
			vida = 0;
			Matar();
		}

	}



	void Comportamento(){
		
		switch (comportamentos) {
		case 1:
			if(circuloDfog!=null&&circuloDfog.GetComponent<CirculoDeFogo>().cont==3){
				Mover();}
			break;
		case 2:
			if(circuloDfog!=null&&circuloDfog.GetComponent<CirculoDeFogo>().projeteis.Count>0&&this.tempo>1){
				Atirar();

				//print("entrou porra");
			}
			else if(circuloDfog!=null&&circuloDfog.GetComponent<CirculoDeFogo>().projeteis.Count==0&&this.tempo>1){
					
					comportamentos = 3;}
			break;
		//case 3:
				
			//CirculoDeFogo();
			//Destroy(circuloDfog);

			//CriarCirculodeFogo();
			//break;
		
		}

	}
	// Update is called once per frame
 void Update () {
		/*if (Input.GetKeyDown (KeyCode.Backspace)&&comportamentos==0) {
			circuloDfog=Instantiate(preFCirc,t.position,t.rotation)as GameObject;
			ang=0;
			comportamentos=1;
			posIn = t.position;
			t.localScale=new Vector2(1,1);
		}*/
		tempo += Time.deltaTime;
		if (vida > 0) { Comportamento(); }
		else if(vida <1 && !ivunerabilidade)
		{
			
				
			ivunerabilidade = true;
				
			this.tempo = 0;
			StartCoroutine(ExplosaoMorte());

			
		}
		
		if (circuloDfog != null) {
			circuloDfog.GetComponent<Transform> ().position = t.position;
		}
		

	}
}
