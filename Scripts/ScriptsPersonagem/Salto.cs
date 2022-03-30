using UnityEngine.UI;
using UnityEngine;

public class Salto : MonoBehaviour
{
	GameObject player;
	public GameObject botao;
	public string atalho;


	public void esquiva()
	{
		
		
		StartCoroutine( player.GetComponent<movBoitata>().salto(botao));
		
		
	}
	public void setButton(bool estado)
	{
		botao = GameObject.Find("esquiva");
		botao.GetComponent<Button>().onClick.AddListener(esquiva);
		botao.SetActive(estado);
	}
	// Start is called before the first frame update
	void Start()
    {
		player = gameObject;
		
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(atalho)) {
			esquiva();
		};
    }
}
