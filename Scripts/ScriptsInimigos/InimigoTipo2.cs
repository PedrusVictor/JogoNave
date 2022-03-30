using UnityEngine;
using System.Collections;

public class InimigoTipo2 :Inimigo {

	public float vel;
	Transform t;
	public int dir;
	public float distMax;
	// Use this for initialization
	void Start () {
		setCorOriginalMonstro(GetComponent<SpriteRenderer>().color);
		t = GetComponent<Transform> ();
		setMorteDist(true);
		vida = 1;
	}
	
	 void Comportamento(){
		t.Translate (vel*dir*Time.deltaTime,distMax*Time.deltaTime*vel*(Mathf.Cos(t.position.x)),0);
		Matar ();
	}
	void Comportamento2()
	{
		t.Translate(distMax * Time.deltaTime * -vel * (Mathf.Cos(t.position.y)),vel * dir * Time.deltaTime, 0);
		Matar();
	}


	// Update is called once per frame
	void Update () {
		if (getTypeMonster() == 1)
		{
			Comportamento2();
		}
		else
		{
			Comportamento();
		}
	}


}
