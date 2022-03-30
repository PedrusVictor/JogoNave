using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public class Inimigo : MonoBehaviour {
	public float vida;
	public float VidaMax;
	public bool ivunerabilidade=false;
	private Color corOriginal;//cor original do monstro
	public GameObject objtipo;
	private Color[]coresTipo= { new Color(1,0.2f,0.2f,0.2f), new Color(0.2f, 0.2f, 1, 0.2f) };

	public void setCorOriginalMonstro(Color cor)
	{
		this.corOriginal = cor;

	}
	public Color getColorOriginalMonster()
	{
		return this.corOriginal;
	}

	[SerializeField]private int tipo = 0;
	bool morteDist = false;
	// Use this for initialization
	public int compare(GameObject a,GameObject b){
		return   (Vector2.Distance(a.GetComponent<Transform>().position,GetComponent<Transform>().position)).CompareTo(Vector2.Distance(b.GetComponent<Transform>().position,GetComponent<Transform>().position));
	}
	//ordena usando a distancia dos inimigos para o projetil
	public void ordenar(List<GameObject> inimigos){
		inimigos.Sort (this.compare);
	}
	public void setType(int type)
	{
		this.tipo = type;
		if (objtipo)
		{
			objtipo.GetComponent<SpriteRenderer>().color = coresTipo[tipo];

		}

	}
	public void setMorteDist(bool valor)
	{
		morteDist=valor;

	}
	public int getTypeMonster()
	{
		return tipo;
	}

	IEnumerator PerderHp()
	{
		for (int a = 0; a < 3; a++)
		{
			Color c = GetComponent<SpriteRenderer>().color;
			
			
			c.a = 0.5f;
			c.g = 0.25f;
			c.b = 0.25f;
			c.r = 0.25f;
			GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(0.1f);

			c = getColorOriginalMonster();
			c.a = 1;
			
			GetComponent<SpriteRenderer>().color = c;
			yield return new WaitForSeconds(0.1f);
		}

	}

	void OnTriggerEnter2D(Collider2D Objeto){
		if(Objeto.gameObject.tag=="Bala"){
			//print("dfdf");
			/*if(Objeto.GetComponent<TiroSplash>()&&Objeto.GetComponent<TiroSplash>().splash!=null){

				GameObject splsh=Instantiate(Objeto.GetComponent<TiroSplash>().splash,Objeto.GetComponent<Transform>().position,Objeto.GetComponent<Transform>().rotation)as GameObject;

			}*/
			if (Objeto.GetComponent<TiroSplash>())
			{
				Objeto.GetComponent<TiroSplash>().EfeitoDest();
			}
			if (!ivunerabilidade){
				if(Objeto.gameObject.GetComponent<Projetil>() && Objeto.gameObject.GetComponent<Projetil>().tipo==tipo || Objeto.gameObject.GetComponent<Projetil>()&& tipo == -1)
				{
					vida--;
					StopCoroutine(PerderHp());
					StartCoroutine(PerderHp());
				}
				
			}

			Destroy(Objeto.gameObject);
		}
	}
	public	IEnumerator Delay(float segs){
		
		yield return StartCoroutine (Temporarizador (segs));
		//StopAllCoroutines ();
		
	}
	//metodo que faz com que aja a espera
	public	IEnumerator Temporarizador(float tempdelay){
		
		yield return new WaitForSeconds (tempdelay);

	}
	

	public void Matar(){
		if(vida<=0){

			float drop=(Random.Range(1,100000)+1);
			//print("drop="+drop);
			for(int a=0;a<4;a++){
				if(drop>0&&drop<=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ListaDeItens>().itens[a].max){
					GameObject obj= Instantiate( GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ListaDeItens>().itens[a].obj,GetComponent<Transform>().position,GetComponent<Transform>().rotation)as GameObject;
					obj.GetComponent<Item>().setType( tipo);
					break;}
		
			}
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControleFase>().Pontuacao();
			Destroy(gameObject);
	}
		else if (tipo==2&& GetComponent<Transform>().position.y < -8 && morteDist == true
			)
		{
			Destroy(gameObject);
		}
		else if(GetComponent<Transform>().position.x<-13&&morteDist==true
			)
		{
			Destroy(gameObject);
		}
	// Update is called once per frame

}
}