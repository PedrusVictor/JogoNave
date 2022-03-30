using UnityEngine;
using System.Collections;

public class InimigoTipo1 : Inimigo {
	public float vel;
	Transform t;
	public int dir;
	// Use this for initialization
	void Start () {
		setCorOriginalMonstro(GetComponent<SpriteRenderer>().color);
		t = GetComponent<Transform> ();
		setMorteDist(true);
		vida = 1;
	}

	 void Comportamento(){
		t.Translate (vel*dir*Time.deltaTime,0,0);
		Matar ();
		}
	void Comportamento2()
	{
		t.Translate(0,-vel * Time.deltaTime, 0);
		Matar();
	}

	// Update is called once per frame
	void Update () {
		if (getTypeMonster() == 1)
		{
			Comportamento2();
		}
		else {
			Comportamento();
		}
		
	}
}
