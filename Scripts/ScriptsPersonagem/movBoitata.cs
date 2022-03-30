using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class movBoitata : MonoBehaviour
{
    public int nPlayer = 0;
    public GameObject mira;
    Transform t;
    public GameObject cabeca;
    public float vel;
    public GameObject cam;
    public float angulo;
    public float angulMax;
    Quaternion foco;
    public GameObject bala;
    public GameObject[] corpo = new GameObject[6];
    public Vector3[] posCorpo = new Vector3[16];
    public float velCorpo;
    public Vector2 dir;
    public RaycastHit2D[] raio;

    //hp
    public int hpmax = 10, hp;
    public Image barraHp;


    [SerializeField] private bool iv = false;//variavel de invencibilidade
    public float timeCom = 1;
    public float temp;

    public float tempEspSalto = 3;
    private bool Actsalto = false;
    // Use this for initialization
    public float dis = 0.3f;
    [System.Serializable]
    public struct playerConf
    {
        public string[] direcional;
        public float[] areasFase;
    }
    public playerConf[] playersconf = new playerConf[2];


    public int mobile = 0;
    public SimpleTouchController controleMobile;


    private void Awake()
    {



        try
        {
            mobile = PlayerPrefs.GetInt("mobile");
        }
        catch
        {
            string[] plataformas = new string[] { "Android", "Iphone" };
            string dispositivo = SystemInfo.operatingSystem.Split(' ')[0];
            int valor = 0;

            foreach (string a in plataformas)
            {
                if (dispositivo == a)
                {
                    valor = 1;
                    break;
                }

            }

            PlayerPrefs.SetInt("mobile", valor);
            mobile = valor;
        }



    }

    void Start()
    {

        cam = GameObject.FindGameObjectWithTag("MainCamera");
        hp = hpmax;


        barraHp = GameObject.Find("hp" + nPlayer.ToString()).GetComponent<Image>();

        AjustarBarraHp();
        try
        {

            controleMobile = GameObject.Find("controle movimento").GetComponent<SimpleTouchController>();
        }
        catch
        {

            controleMobile = null;
        }



        t = gameObject.GetComponent<Transform>();
        for (int a = 0; a < 16; a++)
        {
            posCorpo[a] = new Vector3(cabeca.transform.position.x - 0.25f * a, t.position.y, 0);

        }

        for (int a = 0; a < 6; a++)
        {

            corpo[a].GetComponent<Transform>().position = posCorpo[(3 * a)];
        }

        dir = new Vector2(1, 0);


        //DesenharCorpo ();
    }
    public void AplicarModAparencia(Character personagem)
    {
        cabeca.GetComponent<SpriteRenderer>().sprite = personagem.cabeca;
        for (int a = 0; a < 5; a++)
        {
            corpo[a].GetComponent<SpriteRenderer>().sprite = personagem.corpo;

        }
        corpo[5].GetComponent<SpriteRenderer>().sprite = personagem.cauda;
    }
    //método de esquiva/ salto do personagem
    public IEnumerator salto(GameObject botao = null)
    {
        if (!Actsalto)
        {
            if (botao)
            {
                botao.GetComponent<Button>().interactable = false;
            }
            iv = true;
            Actsalto = true;
            cabeca.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.5f, 1, 0.5f);
            for (int a = 0; a < 6; a++)
            {
                corpo[a].GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.5f, 1, 0.5f);
            }

            for (float a = 0; a < 2; a += 0.5f)
            {
                transform.Translate(a * corpo[0].transform.right.x, Mathf.Cos(a * 2) * 2.5f, 0);
                //Vector3 aux = posCorpo[0];
                //Vector3 aux2;

                posCorpo[0] = transform.position;
                Vector3 aux = posCorpo[0];
                //posCorpo[0] = new Vector2(0,posCorpo[0].y+ Mathf.Cos(a * 2) * 10000);


                for (int x = 15; x > 0; x--)
                {

                    posCorpo[x] = Vector3.MoveTowards(posCorpo[x], posCorpo[x - 1], 0.9f);



                }

                for (int b = 0; b < 6; b++)
                {
                    corpo[b].GetComponent<Transform>().position = posCorpo[(3 * b)];

                }




                yield return new WaitForSeconds(0.04f);
            }
            cabeca.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            for (int a = 0; a < 6; a++)
            {
                corpo[a].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }

            iv = false;

            yield return new WaitForSeconds(tempEspSalto);
            if (botao)
            {
                botao.GetComponent<Button>().interactable = true;
            }
            Actsalto = false;
        }


    }
    public void ControleMovimento()
    {
        if (mobile < 1)
        {
            if (transform.position.x > playersconf[nPlayer].areasFase[0] && Input.GetKey(playersconf[nPlayer].direcional[2]) && !Input.GetKey(playersconf[nPlayer].direcional[0]) && !Input.GetKey(playersconf[nPlayer].direcional[1]))
            {
                MovimentoBoitata(-vel * timeCom, 0, 0);
                cabeca.GetComponent<Transform>().localPosition = new Vector2(-1, 1.5f);
                corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 180);


            }
            else if (transform.position.x < playersconf[nPlayer].areasFase[1] && Input.GetKey(playersconf[nPlayer].direcional[3]) && !Input.GetKey(playersconf[nPlayer].direcional[0]) && !Input.GetKey(playersconf[nPlayer].direcional[1]))
            {
                MovimentoBoitata(vel * timeCom, 0, 0);
                cabeca.GetComponent<Transform>().localPosition = new Vector2(1, 1.5f);
                corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);


            }
            else if (transform.position.y < playersconf[nPlayer].areasFase[3] && Input.GetKey(playersconf[nPlayer].direcional[0]) && !Input.GetKey(playersconf[nPlayer].direcional[2]) && !Input.GetKey(playersconf[nPlayer].direcional[3]))
            {
                MovimentoBoitata(0, vel * timeCom, 0);
                cabeca.GetComponent<Transform>().localPosition = new Vector2(cabeca.GetComponent<Transform>().localPosition.x, 1.5f);
                corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 90);

            }
            else if (transform.position.y < playersconf[nPlayer].areasFase[3] && transform.position.x < playersconf[nPlayer].areasFase[1] && Input.GetKey(playersconf[nPlayer].direcional[0]) && Input.GetKey(playersconf[nPlayer].direcional[3]))
            {
                MovimentoBoitata(vel * ((Mathf.Sqrt(2)) / 2) * timeCom, vel * ((Mathf.Sqrt(2)) / 2) * timeCom, 0);
                cabeca.GetComponent<Transform>().localPosition = new Vector2(1, 1.5f);
                corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 45);


            }
            else if (transform.position.y < playersconf[nPlayer].areasFase[3] && transform.position.x > playersconf[nPlayer].areasFase[0] && Input.GetKey(playersconf[nPlayer].direcional[0]) && Input.GetKey(playersconf[nPlayer].direcional[2]))
            {
                MovimentoBoitata(-vel * ((Mathf.Sqrt(2)) / 2) * timeCom, vel * ((Mathf.Sqrt(2)) / 2) * timeCom, 0);
                cabeca.GetComponent<Transform>().localPosition = new Vector2(-1, 1.5f);
                corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 135);

            }
            else if (transform.position.y > playersconf[nPlayer].areasFase[2] && Input.GetKey(playersconf[nPlayer].direcional[1]) && !Input.GetKey(playersconf[nPlayer].direcional[2]) && !Input.GetKey(playersconf[nPlayer].direcional[3]))
            {
                MovimentoBoitata(0, -vel * timeCom, 0);
                cabeca.GetComponent<Transform>().localPosition = new Vector2(cabeca.GetComponent<Transform>().localPosition.x, -1.5f);
                corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 270);

            }
            else if (transform.position.y > playersconf[nPlayer].areasFase[2] && transform.position.x < playersconf[nPlayer].areasFase[1] && Input.GetKey(playersconf[nPlayer].direcional[1]) && Input.GetKey(playersconf[nPlayer].direcional[3]))
            {
                MovimentoBoitata(vel * ((Mathf.Sqrt(2)) / 2), -vel * ((Mathf.Sqrt(2)) / 2) * timeCom, 0);
                cabeca.GetComponent<Transform>().localPosition = new Vector2(1, -1.5f);
                corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 315);

            }
            else if (transform.position.y > playersconf[nPlayer].areasFase[2] && transform.position.x > playersconf[nPlayer].areasFase[0] && Input.GetKey(playersconf[nPlayer].direcional[1]) && Input.GetKey(playersconf[nPlayer].direcional[2]))
            {
                MovimentoBoitata(-vel * ((Mathf.Sqrt(2)) / 2) * timeCom, -vel * ((Mathf.Sqrt(2)) / 2) * timeCom, 0);
                cabeca.GetComponent<Transform>().localPosition = new Vector2(-1, -1.5f);
                corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 225);

            }

        }

        else
        {
            if (controleMobile != null)
            {
                if (transform.position.x > playersconf[nPlayer].areasFase[0] && controleMobile.GetTouchPosition.x < 0 && Mathf.Abs(controleMobile.GetTouchPosition.y) < 0.5f)
                {
                    MovimentoBoitata(-vel * timeCom, 0, 0);
                    cabeca.GetComponent<Transform>().localPosition = new Vector2(-1, 1.5f);
                    corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 180);


                }
                else if (transform.position.x < playersconf[nPlayer].areasFase[1] && controleMobile.GetTouchPosition.x > 0 && Mathf.Abs(controleMobile.GetTouchPosition.y) < 0.5f)
                {
                    MovimentoBoitata(vel * timeCom, 0, 0);
                    cabeca.GetComponent<Transform>().localPosition = new Vector2(1, 1.5f);
                    corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);


                }
                else if (transform.position.y < playersconf[nPlayer].areasFase[3] && Mathf.Abs(controleMobile.GetTouchPosition.x) < 0.5f && controleMobile.GetTouchPosition.y > 0)
                {
                    MovimentoBoitata(0, vel * timeCom, 0);
                    cabeca.GetComponent<Transform>().localPosition = new Vector2(cabeca.GetComponent<Transform>().localPosition.x, 1.5f);
                    corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 90);

                }
                else if (transform.position.y > playersconf[nPlayer].areasFase[2] && Mathf.Abs(controleMobile.GetTouchPosition.x) < 0.5f && controleMobile.GetTouchPosition.y < 0)
                {
                    MovimentoBoitata(0, -vel * timeCom, 0);
                    cabeca.GetComponent<Transform>().localPosition = new Vector2(cabeca.GetComponent<Transform>().localPosition.x, -1.5f);
                    corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 270);

                }
                else if (transform.position.y < playersconf[nPlayer].areasFase[3] && transform.position.x < playersconf[nPlayer].areasFase[1] && controleMobile.GetTouchPosition.x > 0.5f && controleMobile.GetTouchPosition.y > 0.5f)
                {
                    MovimentoBoitata(vel * ((Mathf.Sqrt(2)) / 2) * timeCom, vel * ((Mathf.Sqrt(2)) / 2) * timeCom, 0);
                    cabeca.GetComponent<Transform>().localPosition = new Vector2(1, 1.5f);
                    corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 45);


                }
                else if (transform.position.y < playersconf[nPlayer].areasFase[3] && transform.position.x > playersconf[nPlayer].areasFase[0] && controleMobile.GetTouchPosition.x < -0.5f && controleMobile.GetTouchPosition.y > 0.5f)
                {
                    MovimentoBoitata(-vel * ((Mathf.Sqrt(2)) / 2) * timeCom, vel * ((Mathf.Sqrt(2)) / 2) * timeCom, 0);
                    cabeca.GetComponent<Transform>().localPosition = new Vector2(-1, 1.5f);
                    corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 135);

                }

                else if (transform.position.y > playersconf[nPlayer].areasFase[2] && transform.position.x < playersconf[nPlayer].areasFase[1] && controleMobile.GetTouchPosition.x > 0.5f && controleMobile.GetTouchPosition.y < -0.5f)
                {
                    MovimentoBoitata(vel * ((Mathf.Sqrt(2)) / 2), -vel * ((Mathf.Sqrt(2)) / 2) * timeCom, 0);
                    cabeca.GetComponent<Transform>().localPosition = new Vector2(1, -1.5f);
                    corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 315);

                }
                else if (transform.position.y > playersconf[nPlayer].areasFase[2] && transform.position.x > playersconf[nPlayer].areasFase[0] && controleMobile.GetTouchPosition.x < -0.5f && controleMobile.GetTouchPosition.y < -0.5f)
                {
                    MovimentoBoitata(-vel * ((Mathf.Sqrt(2)) / 2) * timeCom, -vel * ((Mathf.Sqrt(2)) / 2) * timeCom, 0);
                    cabeca.GetComponent<Transform>().localPosition = new Vector2(-1, -1.5f);
                    corpo[0].GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 225);

                }
            }


        }

    }
    public void AjustarBarraHp()
    {
        if (barraHp != null)
        {
            barraHp.fillAmount = (float)hp / (float)hpmax;
        }

    }
    public void recHp()
    {
        if (hp < hpmax)
        {
            hp++;
            AjustarBarraHp();
        }
    }
    public void dano()
    {
        if (!iv && hp > 0)
        {
            hp--;
            StartCoroutine(golpe());
            if (hp < 1)
            {
                cam.GetComponent<GerenciadorJogo>().Fase1();
            }

            AjustarBarraHp();
        }
    }

    Vector3 Ang(Vector3 obj, Vector3 alvo, float Angmax)
    {

        //(obj(o,1) olhando para cima
        //alvo = posalv-posobj
        //return (Mathf.Acos((alvo.y - obj.y)/((Mathf.Sqrt((Mathf.Pow((alvo.x - obj.x),2))+(Mathf.Pow((alvo.y - obj.y),2))))))*(180/Mathf.PI));
        float v;
        if (alvo.x < obj.x)
        {
            v = Angmax - (Mathf.Acos((alvo.y - obj.y) / ((Mathf.Sqrt((Mathf.Pow((alvo.x - obj.x), 2)) + (Mathf.Pow((alvo.y - obj.y), 2)))))) * (180 / Mathf.PI));
            if (float.IsNaN(v))
            {
                v = 0;
            }
            return new Vector3(0, 180, v);
            //return new Vector3 (0, 180, Angmax-(Mathf.Acos ((alvo.y - obj.y) / ((Mathf.Sqrt ((Mathf.Pow ((alvo.x - obj.x), 2)) + (Mathf.Pow ((alvo.y - obj.y), 2)))))) * (180 / Mathf.PI)));
        }
        v = Angmax - (Mathf.Acos((alvo.y - obj.y) / ((Mathf.Sqrt((Mathf.Pow((alvo.x - obj.x), 2)) + (Mathf.Pow((alvo.y - obj.y), 2)))))) * (180 / Mathf.PI));
        if (float.IsNaN(v))
        {
            v = 0;
        }
        return new Vector3(0, 0, v);
        //return new Vector3(0,0,Angmax-(Mathf.Acos((alvo.y - obj.y)/((Mathf.Sqrt((Mathf.Pow((alvo.x - obj.x),2))+(Mathf.Pow((alvo.y - obj.y),2))))))*(180/Mathf.PI)));
    }

    public void MovimentoBoitata(float x, float y, float z)
    {
        Vector3 aux = posCorpo[0];
        Vector3 aux2;
        t.Translate(x, y, z);
        posCorpo[0] = t.position;

        for (int a = 0; a < 6; a++)
        {
            if (a < 5)
            {
                aux2 = posCorpo[(3 * a) + 1];
                posCorpo[(3 * a) + 1] = Vector2.Lerp(posCorpo[(3 * a) + 1], aux, velCorpo * timeCom);
                aux = posCorpo[(3 * a) + 2];
                posCorpo[(3 * a) + 2] = Vector2.Lerp(posCorpo[(3 * a) + 2], aux2, velCorpo * timeCom);
                aux2 = posCorpo[(3 * a) + 3];
                posCorpo[(3 * a) + 3] = Vector2.Lerp(posCorpo[(3 * a) + 3], aux, velCorpo * timeCom);
                aux = aux2;
            }
            corpo[a].GetComponent<Transform>().position = posCorpo[(3 * a)];
            if (a > 0)
            {
                //Vector3 newDirection = Vector3.RotateTowards(corpo[a].transform.right, corpo[a-1].transform.position - corpo[a].transform.position,vel*Time.deltaTime,0);
                //corpo[a].transform.eulerAngles = new Vector3(0, 0, Quaternion.LookRotation(newDirection, corpo[a].transform.forward).eulerAngles.z+90);
                //print(Quaternion.LookRotation(newDirection, corpo[a].transform.up).eulerAngles - new Vector3(0, 0, +90));
                corpo[a].transform.eulerAngles = Ang(corpo[a].transform.position, corpo[a - 1].transform.position, angulMax);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D Objeto)
    {
        if (Objeto.gameObject.tag == "Inimigo" || Objeto.gameObject.tag == "BalaInimiga")
        {
            dano();
        }


    }
    IEnumerator golpe()
    {
        for (int a = 0; a < 3; a++)
        {
            Color c = cabeca.GetComponent<SpriteRenderer>().color;
            c.a = 0;
            cabeca.GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(0.1f);


            c.a = 1;
            cabeca.GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(0.1f);
        }

    }
    void DesenharCorpo()
    {
        //print (cabeca.GetComponent<Transform> ().position);


        for (int b = 1; b < 6; b++)
        {

            if (Vector2.Distance(posCorpo[(b * 3) - 2], posCorpo[(b * 3) - 3]) > dis)
            {
                posCorpo[(b * 3) - 2] = Vector3.MoveTowards(posCorpo[(b * 3) - 2], posCorpo[(b * 3) - 3], 0.5f);
            }

            if (Vector2.Distance(posCorpo[(b * 3) - 1], posCorpo[(b * 3) - 2]) > dis)
            {
                posCorpo[(b * 3) - 1] = Vector3.MoveTowards(posCorpo[(b * 3) - 1], posCorpo[(b * 3) - 2], 0.5f);
            }
            if (Vector2.Distance(posCorpo[(b * 3)], posCorpo[(b * 3) - 1]) > dis)
            {
                posCorpo[(b * 3)] = Vector3.MoveTowards(posCorpo[(b * 3)], posCorpo[(b * 3) - 1], 0.5f);
            }
            corpo[b].GetComponent<Transform>().position = posCorpo[(3 * b)];
        }





    }
    //método que localiza alvo e mira logo em seguida no alvo mais próximo

    public void LocalizarMirar()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        mira.GetComponent<Transform>().position = cabeca.transform.position + new Vector3(3, 0, 0);
        cabeca.GetComponent<Transform>().eulerAngles = Ang(t.position, mira.GetComponent<Transform>().position, angulMax);
        foreach (GameObject inimi in inimigos)
        {
            if (nPlayer == 0 && Mathf.Abs(inimi.transform.position.y - transform.position.y) < 6.5f + (GetComponent<Atirar>().DistTiro / 2) ||
                nPlayer == 1 && Mathf.Abs(inimi.transform.position.x - transform.position.x) < 6.5f + (GetComponent<Atirar>().DistTiro / 2)
                )
            {
                mira.GetComponent<Transform>().position = new Vector2(inimi.transform.position.x, inimi.transform.position.y);
                cabeca.GetComponent<Transform>().eulerAngles = Ang(t.position, mira.GetComponent<Transform>().position, angulMax);

                break;
            }

        }
    }
    private void FixedUpdate()
    {
        LocalizarMirar();
        
        DesenharCorpo();
        ControleMovimento();
    }

    // Update is called once per frame
    void Update()
    {
        temp = Time.deltaTime;
        /*
        if (Time.deltaTime < 0.007f)
        {

            if (Time.deltaTime < 0.0055f)
            {
                timeCom = 0.5f;
            }
            else
            {
                timeCom = Time.deltaTime;
            }
        }
        else if (Time.deltaTime > 0.1f)
        {
            timeCom = Time.deltaTime;
        }
        else
        {
            timeCom = 1;
        }
        */

        /*if (GetComponent<Atirar>() && GetComponent<Atirar>().tiro == false) {

			mira.GetComponent<Transform>().position = new Vector2((Camera.main.ScreenPointToRay(Input.mousePosition).origin.x), (Camera.main.ScreenPointToRay(Input.mousePosition).origin.y));
			cabeca.GetComponent<Transform>().eulerAngles = Ang(t.position, mira.GetComponent<Transform>().position, angulMax);
		}*/
        //localizando e mirando nos inimigos




        






    }
}