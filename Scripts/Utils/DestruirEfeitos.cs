using UnityEngine;
using System.Collections;

public class DestruirEfeitos : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter2D(Collider2D Objeto){

		if (Objeto.gameObject.tag == "Inimigo"&&Objeto.GetComponent<Inimigo>().vida>0) {
			Objeto.GetComponent<Inimigo>().vida--;
		}
	}
	// Update is called once per frame
	void Update () {
		if(GetComponent<ParticleSystem>().isStopped){
			Destroy(gameObject);
		}
	}
}
