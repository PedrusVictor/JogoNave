using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;
public class GerenciadorJogo : MonoBehaviour {

	public VideoPlayer cutscene;
	//private bool Exibircutscene = false;
	// Use this for initialization
	void Start () {
		//Exibircutscene = false;
		int valor =  new List<string> { "Android", "Iphone" }.Contains(SystemInfo.operatingSystem.Split(' ')[0])?1:0;
		
		PlayerPrefs.SetInt("mobile",valor );
	}

	public void Sair(){
		Application.Quit ();
		print ("saindo");
	
	}
	public void Menu()
	{
		SceneManager.LoadScene("menu");
	}
	public void Fase1(){
		SceneManager.LoadScene ("fase1");
	}
	public void TelaVitoria()
	{
		SceneManager.LoadScene("congratulations");
	}
	public void players(int quantidade)
	{
		PlayerPrefs.SetInt("player", quantidade);
		//Exibircutscene = true;
		StartCoroutine(cutscene1());
	}
	
	
	public IEnumerator cutscene1()
	{
		print("entrou");
		yield return new WaitForSeconds((float)cutscene.length);
	
		Fase1();
		
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			Sair();
		}
		
		
	}
}
