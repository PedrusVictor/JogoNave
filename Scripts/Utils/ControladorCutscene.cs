using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControladorCutscene : MonoBehaviour
{
    public Text frase;
    [SerializeField]private string[] textos;
    string textoAtual = "";
    public float vel,temp;
    int id=0,idtextos=0;
    int idmax;
    // Start is called before the first frame update
    void Start()
    {
        textoAtual = "";
        id = 0;
        idmax = textos[0].Length;
        //frase.text = textos[0];
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - temp > vel && id<idmax)
        {
            textoAtual += textos[idtextos][id];
            id++;
            temp = Time.time;
            frase.text = textoAtual;
        }
        else if (id == idmax&&Time.time-temp>3.5f&&idtextos<textos.Length-1)
        {
            textoAtual = "";
            idtextos++;
            
            id = 0;
            temp = Time.time;
            idmax = textos[idtextos].Length;
        }
    }
}
