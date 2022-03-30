using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
public class Atirar : MonoBehaviour
{
    public int tipo = 0; //tipo de player. se é o jogador 1 ou 2. 0= jogador 1. 1=jogador 2
    public int tipoTiro;
    public int forTiro;
    public float DistTiro;
    public GameObject[] disparador = new GameObject[2];
    public GameObject[] proj = new GameObject[2];
    public MonoScript[] MecBalas = new MonoScript[2];
    Transform t;
    public GameObject mira;
    public bool tiro = false;
    public float tempDisp = 0.3f;
    public GameObject splash;

    private GameObject botao;//botao/objeto de atirar
    public string atalho;//atalho para atirar
                         // Use this for initialization
   private void Start()
    {
        t = GetComponent<Transform>();
        tipoTiro = 0;
        forTiro = 0;
        DistTiro = 0;

        //setando o botão de atiar logo no inicio do jogo

    }

    public void setButton(bool estado)
    {
        botao = GameObject.Find("atirar");
        botao.GetComponent<Button>().onClick.AddListener(atirar);
        botao.SetActive(estado);
    }
    public void atirar()
    {
        if (!tiro)
        {

            StartCoroutine(Disparar(tipoTiro, forTiro, DistTiro));
        }

    }

    IEnumerator Disparar(int tipoTiro, int forTiro, float disTiro)
    {
        if (!tiro)
        {
            tiro = true;

            if (botao) { botao.GetComponent<Button>().interactable = false; }
            GetComponent<movBoitata>().LocalizarMirar();
            GameObject bala = Instantiate(disparador[tipoTiro], mira.GetComponent<Transform>().position, GetComponent<Transform>().rotation) as GameObject;

            bala.GetComponent<Disparador>().splash = splash;

            bala.GetComponent<Disparador>().municao = proj[tipoTiro];

            GameObject cab = transform.Find("cab").gameObject;
            if (cab)
            {
                bala.GetComponent<Disparador>().Disparo(forTiro, disTiro, MecBalas[tipoTiro], "Bala", cab.transform.right, tipo);
            }


            ///bala.GetComponent<Disparador> ().Disparo (forTiro, disTiro,MecBalas[tipoTiro],"Bala",(new Vector2((Camera.main.ScreenPointToRay (Input.mousePosition).origin.x-GetComponent<Transform>().position.x),(Camera.main.ScreenPointToRay (Input.mousePosition).origin.y-(GetComponent<Transform>().position.y)))/(Vector2.Distance((new Vector2(GetComponent<Transform>().position.x,GetComponent<Transform>().position.y)),Camera.main.ScreenPointToRay (Input.mousePosition).origin))),tipo);

            /*switch (tipoTiro) {
			case 0:
				bala.GetComponent<bala1> ().Disparo (forTiro, disTiro,MecBalas[tipoTiro]);
				break;
			case 1:
				bala.GetComponent<bala2> ().Disparo (forTiro, disTiro,MecBalas[tipoTiro]);
				break;
			default:
				break;
			}*/
            float delayTiro = tempDisp;
            if (disparador[tipoTiro].name == "disparadorTipo3" || MecBalas[tipoTiro].GetType().Name == "TiroEspiral")
            {
                delayTiro = tempDisp * 2.25f;

            }

            yield return new WaitForSeconds(delayTiro);
            this.tiro = false;
            if (botao) { botao.GetComponent<Button>().interactable = true; }
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(atalho))
        {
            StartCoroutine(Disparar(tipoTiro, forTiro, DistTiro));

        }
        //Debug.DrawRay (t.position,direcao,Color.black);
        //if(Input.GetMouseButtonDown(0)&&!tiro){
        //StartCoroutine(Disparar(tipoTiro,forTiro,DistTiro));
        //bala.GetComponent<Transform>().position=new Vector2(cabeca.GetComponent<Transform>().position.x,cabeca.GetComponent<Transform>().position.y-0.5f);
        //bala.GetComponent<Rigidbody2D>().AddForce(cabeca.GetComponent<Transform>().right*1000);
        //}
        /*if (Input.GetKeyDown (KeyCode.P)&&tipoTiro<1) {
			tipoTiro++;
			tempDisp+=0.2f;
		}
		else if (Input.GetKeyDown (KeyCode.O)&&tipoTiro>0) {
			tipoTiro--;
			tempDisp-=0.2f;
		}*/
    }
}
