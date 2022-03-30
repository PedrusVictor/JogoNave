using UnityEngine;
using System.Collections;

public class ListaDeItens : MonoBehaviour {
	public float PorDrop;
	public GameObject[] prefItens = new GameObject[4];
	public struct cItem{
		public GameObject obj;
		public float max;
	};

	public cItem[]itens=new cItem[4];
	//public GameObject item;

	public void carregarItens(){
		for (int a=0; a<4; a++) {
			
			itens[a].obj = prefItens[a];
			
			if(a==0){
				itens[0].max=(PorDrop/100)*(100000)*(itens[a].obj.GetComponent<Item>().chanceDrop/100);
			}
			else{
				itens[a].max=itens[a-1].max+((PorDrop/100)*(100000)*(itens[a].obj.GetComponent<Item>().chanceDrop/100));
				
			}
			
		}
	}
	/*public void carregarItens(){
		
		for (int a=0; a<4; a++) {

			itens[a].obj = (Resources.LoadAssetAtPath ("Assets/Prefabs/item" +(a+1).ToString()+".prefab", typeof(GameObject))) as GameObject;

			if(a==0){
				itens[0].max=(PorDrop/100)*(100000)*(itens[a].obj.GetComponent<Item>().chanceDrop/100);
			}
			else{
				itens[a].max=itens[a-1].max+((PorDrop/100)*(100000)*(itens[a].obj.GetComponent<Item>().chanceDrop/100));
				
			}

		}
	}*/
	// Use this for initialization
	void Start () {
		carregarItens ();

		//for (int a=0; a<4; a++) {
			//print("item:"+a+" max="+itens[a].max+"\n");
		//}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
