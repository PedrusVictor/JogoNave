using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class Disparador4 : Disparador
{

    // Use this for initialization


    public override void Disparo(int forTiro, float distDisparo, MonoScript mecBala, string tipoProjetil, Vector2 direcao, int playerTipo)
    {

        GetComponent<Transform>().right = direcao;
        //GetComponent<Transform>().right=(new Vector2((Camera.main.ScreenPointToRay (Input.mousePosition).origin.x-GetComponent<Transform>().position.x),(Camera.main.ScreenPointToRay (Input.mousePosition).origin.y-(GetComponent<Transform>().position.y)))/(Vector2.Distance((new Vector2(GetComponent<Transform>().position.x,GetComponent<Transform>().position.y)),Camera.main.ScreenPointToRay (Input.mousePosition).origin)));

        Debug.DrawRay(new Vector2(GetComponent<Transform>().position.x, (GetComponent<Transform>().position.y)), 20 * (new Vector2((Camera.main.ScreenPointToRay(Input.mousePosition).origin.x - GetComponent<Transform>().position.x), (Camera.main.ScreenPointToRay(Input.mousePosition).origin.y - (GetComponent<Transform>().position.y))) / (Vector2.Distance((new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y)), Camera.main.ScreenPointToRay(Input.mousePosition).origin))), Color.red);
        float dist = (6 / (forTiro + 2));
        Vector3 dirBala;

        List<GameObject> inimigos = new List<GameObject>(GameObject.FindGameObjectsWithTag(identificarTInimigo(tipoProjetil)));

        ordenar(inimigos);

        GameObject projetil = Instantiate(municao, GetComponent<Transform>().position, municao.GetComponent<Transform>().rotation) as GameObject;


        //dirBala=new Vector3(GetComponent<Transform>().position.x+10,Ymax-(dist*(a+1)),0);
        //dirBala=GetComponent<Transform>().right*100;
        dirBala = ((GetComponent<Transform>().right * 10) + (GetComponent<Transform>().up * 3));
        //print ((Camera.main.ScreenPointToRay (Input.mousePosition).origin-dirBala)/(Vector2.Distance(dirBala,Camera.main.ScreenPointToRay (Input.mousePosition).origin)));
        //projetil.GetComponent<Transform>().position=new Vector2(GetComponent<Transform>().position.x,Ymax-(dist*(a+1)));
        /*projetil.GetComponent<Transform>().position=GetComponent<Transform>().position;
        projetil.AddComponent<SpriteRenderer> ();
        projetil.GetComponent<SpriteRenderer> ().sprite = Imgsbalas [forTiro];
        */
        projetil.GetComponent<Transform>().localScale = new Vector2(0.6f, 0.6f) + forTiro * new Vector2(0.25f, 0.25f);
        projetil.AddComponent(mecBala.GetClass());
        projetil.GetComponent<Projetil>().splash = this.splash;
        projetil.GetComponent<Projetil>().tipo = playerTipo;

        if (projetil.GetComponent<TiroEspiral>())
        {
            projetil.GetComponent<TiroEspiral>().InAlvo = null;
            projetil.GetComponent<TiroEspiral>().TipoAlvo = identificarTInimigo(tipoProjetil);
            if (inimigos.Count > 0) { projetil.GetComponent<TiroEspiral>().InAlvo = inimigos[0]; }
        }

        projetil.gameObject.tag = tipoProjetil;


        projetil.GetComponent<Projetil>().calDist(forTiro, distDisparo);

        //projetil.GetComponent<Projetil>().dist=5+((forTiro+1)*0.5f)+((distDisparo+1)*3);
        projetil.GetComponent<Projetil>().setDir(dirBala / (Vector2.Distance(GetComponent<Transform>().position, dirBala)));
        projetil.GetComponent<Projetil>().vel = veldisp;


        Destroy(gameObject);
    }

    // Update is called once per frame

}
