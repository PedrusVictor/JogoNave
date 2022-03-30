using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public class Disparador3 : Disparador
{

    // Use this for initialization
    void Start()
    {

        //print (GetComponent<Transform>().eulerAngles);
        //GetComponent<Transform> ().right = (Camera.main.ScreenPointToRay (Input.mousePosition).origin - GetComponent<Transform>().position) / (Vector2.Distance (GetComponent<Transform>().position, Camera.main.ScreenPointToRay (Input.mousePosition).origin));

    }

    IEnumerator DispCircular(int forTiro, float distDisparo, UnityEditor.MonoScript mecBala, string tipoProjetil, int playerTipo)
    {

        float dist = (6 / (forTiro + 2));
        Vector3 dirBala;

        List<GameObject> inimigos = new List<GameObject>(GameObject.FindGameObjectsWithTag(identificarTInimigo(tipoProjetil)));

        ordenar(inimigos);
        List<GameObject> balas = new List<GameObject>();
        for (int a = 0; a <= forTiro; a++)
        {

            GameObject projetil = Instantiate(municao, (GetComponent<Transform>().position + GetComponent<Transform>().right - (GetComponent<Transform>().up.x * (new Vector3(Mathf.Cos(-a * 60 * Mathf.PI / 180), -Mathf.Sin(-a * 60 * Mathf.PI / 180), 0))) - (GetComponent<Transform>().up.y * 1.25f * (new Vector3(Mathf.Sin(-a * 60 * Mathf.PI / 180), Mathf.Cos(-a * 60 * Mathf.PI / 180), 0)))), municao.GetComponent<Transform>().rotation) as GameObject;
            balas.Add(projetil);
            dirBala = ((GetComponent<Transform>().right * 10) + (GetComponent<Transform>().up * 3) - (GetComponent<Transform>().up * dist * (a + 1)));

            projetil.AddComponent(mecBala.GetClass());
            projetil.GetComponent<Projetil>().splash = this.splash;
            projetil.GetComponent<Projetil>().tipo = playerTipo;
            if (projetil.GetComponent<TiroEspiral>())
            {
                projetil.GetComponent<TiroEspiral>().InAlvo = null;
                projetil.GetComponent<TiroEspiral>().TipoAlvo = identificarTInimigo(tipoProjetil);
                if (inimigos.Count > 0) { projetil.GetComponent<TiroEspiral>().InAlvo = inimigos[(a % inimigos.Count)]; }
            }

            projetil.gameObject.tag = tipoProjetil;

            projetil.GetComponent<Projetil>().calDist(forTiro, distDisparo);

            projetil.GetComponent<Projetil>().setDir(dirBala / (Vector2.Distance(GetComponent<Transform>().position, dirBala)));
            yield return StartCoroutine(Delay(0.1f));

        }
        yield return StartCoroutine(Delay(0.1f));
        for (int a = 0; a < balas.Count; a++)
        {
            if (balas[a] != null)
            {
                balas[a].GetComponent<Projetil>().vel = veldisp;
            }

            yield return StartCoroutine(Delay(0.1f));
        }
        Destroy(gameObject);
    }

    public override void Disparo(int forTiro, float distDisparo, MonoScript mecBala, string tipoProjetil, Vector2 direcao, int playerTipo)
    {

        GetComponent<Transform>().right = direcao;
        //GetComponent<Transform>().right=(new Vector2((Camera.main.ScreenPointToRay (Input.mousePosition).origin.x-GetComponent<Transform>().position.x),(Camera.main.ScreenPointToRay (Input.mousePosition).origin.y-(GetComponent<Transform>().position.y)))/(Vector2.Distance((new Vector2(GetComponent<Transform>().position.x,GetComponent<Transform>().position.y)),Camera.main.ScreenPointToRay (Input.mousePosition).origin)));
        Debug.DrawRay(new Vector2(GetComponent<Transform>().position.x, (GetComponent<Transform>().position.y)), 20 * (new Vector2((Camera.main.ScreenPointToRay(Input.mousePosition).origin.x - GetComponent<Transform>().position.x), (Camera.main.ScreenPointToRay(Input.mousePosition).origin.y - (GetComponent<Transform>().position.y))) / (Vector2.Distance((new Vector2(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y)), Camera.main.ScreenPointToRay(Input.mousePosition).origin))), Color.red);

        StartCoroutine(DispCircular(forTiro, distDisparo, mecBala, tipoProjetil, playerTipo));

    }

    // Update is called once per frame

}
