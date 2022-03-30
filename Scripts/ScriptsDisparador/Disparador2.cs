using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class Disparador2 : Disparador
{


    // Use this for initialization
    void Start()
    {

        //print (GetComponent<Transform>().eulerAngles);
        //GetComponent<Transform> ().right = (Camera.main.ScreenPointToRay (Input.mousePosition).origin - GetComponent<Transform>().position) / (Vector2.Distance (GetComponent<Transform>().position, Camera.main.ScreenPointToRay (Input.mousePosition).origin));

    }
    public override void Disparo(int forTiro, float distDisparo, MonoScript mecBala, string tipoProjetil, Vector2 direcao, int playerTipo)
    {
        GetComponent<Transform>().right = direcao;
        //GetComponent<Transform>().right=(new Vector2((Camera.main.ScreenPointToRay (Input.mousePosition).origin.x-GetComponent<Transform>().position.x),(Camera.main.ScreenPointToRay (Input.mousePosition).origin.y-(GetComponent<Transform>().position.y)))/(Vector2.Distance((new Vector2(GetComponent<Transform>().position.x,GetComponent<Transform>().position.y)),Camera.main.ScreenPointToRay (Input.mousePosition).origin)));

        Debug.DrawRay(new Vector2(GetComponent<Transform>().position.x, (GetComponent<Transform>().position.y)), 20 * (new Vector2((Camera.main.ScreenPointToRay(Input.mousePosition).origin.x - GetComponent<Transform>().position.x), (Camera.main.ScreenPointToRay(Input.mousePosition).origin.y - (GetComponent<Transform>().position.y))) / (Vector2.Distance((new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y)), Camera.main.ScreenPointToRay(Input.mousePosition).origin))), Color.red);

        float dist = (6 / (forTiro + 2));
        //Vector3 dirBala;
        List<GameObject> inimigos = new List<GameObject>(GameObject.FindGameObjectsWithTag(identificarTInimigo(tipoProjetil)));
        ordenar(inimigos);
        for (int a = 0; a <= forTiro; a++)
        {
            GameObject projetil = Instantiate(municao, GetComponent<Transform>().position + a * 0.5f * GetComponent<Transform>().right, municao.GetComponent<Transform>().rotation) as GameObject;
            projetil.name = "tipo2";

            projetil.gameObject.tag = tipoProjetil;

            projetil.AddComponent(mecBala.GetClass());
            projetil.GetComponent<Projetil>().splash = this.splash;
            projetil.GetComponent<Projetil>().tipo = playerTipo;
            if (projetil.GetComponent<TiroEspiral>())
            {
                projetil.GetComponent<TiroEspiral>().InAlvo = null;
                projetil.GetComponent<TiroEspiral>().TipoAlvo = identificarTInimigo(tipoProjetil);
                if (inimigos.Count > 0) { projetil.GetComponent<TiroEspiral>().InAlvo = inimigos[(a % inimigos.Count)]; }
            }

            projetil.GetComponent<Projetil>().calDist(forTiro, distDisparo);
            projetil.GetComponent<Projetil>().setDir(GetComponent<Transform>().right);
            projetil.GetComponent<Projetil>().vel = veldisp;


        }

        Destroy(gameObject);
    }

    // Update is called once per frame

}
