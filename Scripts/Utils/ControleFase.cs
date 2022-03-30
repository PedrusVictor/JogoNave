using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ControleFase : MonoBehaviour
{
    public GameObject[] ObjPlayers = new GameObject[2];//personagens
    public Character[] visualPlayers = new Character[2];//visual personagens
    /// v
    /// </summary>
    public Transform[] pontoInicial = new Transform[2];//pontos iniciais dele
    public GameObject[] barrasHp = new GameObject[2];
    public GameObject controlMob;
    public float tempo;
    public GameObject[] inimigos = new GameObject[2];
    public float[] intervaloEntreboss = new float[2];//intervalos para aparecerem o subchefe e o chefe
    private float tempISpawn=1.25f,tempIn;
    private int boss = 0;
    public bool lutaboss = false;
    public GameObject[] InimBoss = new GameObject[2];
    private GameObject bossativo = null;
    public GameObject objaviso;
    int quantplayers = 1;
    public float[] areasFase = new float[4];


    public GameObject buttonAtirar, buttonEsq;

    private GameObject TelaFim;

    private int pontos = 0;
    [SerializeField]private Text pontuacao;

    public Image MudTemp;//objeto que vai mudar a aparência do jogo conforme o tempo passa
    public Color tomCOr = new Color();
    public void Pontuacao()
    {
        pontos += 5;
        pontuacao.text = pontos.ToString();
    }
    // Use this for initialization
    public void AtivarAviso(Vector2 pos, Vector3 rot)
    {

        StartCoroutine(msgPerigo(pos, rot));
    }
    IEnumerator msgPerigo(Vector2 pos, Vector3 rot)
    {
        GameObject aviso = Instantiate(objaviso, pos, transform.rotation) as GameObject;
        aviso.transform.eulerAngles = rot;
        aviso.GetComponent<SpriteRenderer>().color = Color.red;
        aviso.GetComponent<Transform>().localScale = new Vector2(30, 3.5f);

        yield return new WaitForSeconds(1);

        Destroy(aviso.gameObject);
    }

    IEnumerator telaCongratulations()
    {
        for (float a = 0; a < 1; a += 0.025f)
        {
            Color c = TelaFim.GetComponent<Image>().color;
            c.a = a;
            TelaFim.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(Time.deltaTime);

        }

        yield return new WaitForSeconds(1);
        GetComponent<GerenciadorJogo>().TelaVitoria();
    }

    void Start()
    {

        quantplayers = PlayerPrefs.GetInt("player");

        for (int a = 0; a < quantplayers; a++)
        {
            barrasHp[a].SetActive(true);
            GameObject player = Instantiate(ObjPlayers[a], pontoInicial[a].position, transform.rotation) as GameObject;
            player.GetComponent<movBoitata>().AplicarModAparencia(visualPlayers[a]);
            player.GetComponent<movBoitata>().nPlayer = a;
            if (a == 0 && PlayerPrefs.GetInt("mobile") > 0)
            {
                controlMob.SetActive(true);
                buttonAtirar.SetActive(true);
                buttonEsq.SetActive(true);
                buttonAtirar.GetComponent<Button>().onClick.AddListener(player.GetComponent<Atirar>().atirar);
                buttonEsq.GetComponent<Button>().onClick.AddListener(player.GetComponent<Salto>().esquiva);
                player.GetComponent<Atirar>().setButton(true);
                player.GetComponent<Salto>().setButton(true);
            }



        }



        boss = 0;
       
        tempo = Time.time;
        tempIn = Time.time;
    }
    float temptotal(int valor)
    {
        float total = 0;
        for (int a = 0; a < valor; a++)
        {
            total += intervaloEntreboss[a];
        }
        return total;

    }
    private void spawnMonster()
    {
        GameObject inim;
        int nInimigo = Random.Range(0, 3);
        inim = Instantiate(inimigos[nInimigo], new Vector2(9, Random.Range(-3, 3) + 1), GetComponent<Transform>().rotation) as GameObject;
        inim.gameObject.GetComponent<Inimigo>().setType(0);
        inim = Instantiate(inimigos[3], new Vector2(Random.Range(-5, 7) + 1, 7), GetComponent<Transform>().rotation) as GameObject;

        //parte referente ao outro jogador
        if (quantplayers > 1)
        {
            nInimigo = Random.Range(0, 3);
            inim = Instantiate(inimigos[nInimigo], new Vector2(Random.Range(-7, 7) + 1, 9), GetComponent<Transform>().rotation) as GameObject;
            inim.gameObject.GetComponent<Inimigo>().setType(1);



        }


    }
  
    // Update is called once per frame
    void Update()
    {


        if (boss < 2 && Time.time-tempIn < temptotal(boss + 1))
        {
            if (GameObject.FindGameObjectsWithTag("Inimigo").Length < 7
                && Time.time-tempo >= tempISpawn )
            {

                spawnMonster();
                float caltemp = (Time.time - tempIn) / temptotal(boss + 1);
                Color cornova = Color.Lerp(MudTemp.color, tomCOr, caltemp );
                MudTemp.color = cornova;
                tempo = Time.time;
                if (tempISpawn > .25f)
                {
                    tempISpawn =(( (temptotal(boss + 1)-(Time.time - tempIn))/ temptotal(boss + 1))*2)+0.25f;
                }
                else
                {
                    tempISpawn = 0.25f;
                }
            }
            //fazendo transição de dia para noite através de cor
            
        }
        else if (boss < 2)
        {//&& GameObject.FindGameObjectsWithTag("Inimigo").Length<1
            if (!lutaboss && bossativo == null)
            {
                bossativo = Instantiate(InimBoss[boss], new Vector2(6, 0), GetComponent<Transform>().rotation) as GameObject;

                lutaboss = true;

            }
            else if (lutaboss && bossativo == null)
            {
                lutaboss = false;
                boss++;
            }

        }
        else if (boss > 1 && lutaboss)
        {
            StartCoroutine(telaCongratulations());
            lutaboss = false;
        }
    }

}
