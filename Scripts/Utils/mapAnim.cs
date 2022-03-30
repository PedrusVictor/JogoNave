using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class mapAnim : MonoBehaviour
{
    public string sortingLayerName; // inicialização antes dos métodos
    public int orderInLayer = 0;
    

    public float vel=1;
    Renderer mat;
    public float valor = 0;
    public Color cor;
    void SetSortingLayer()
    {
        if(mat == null)
        {
            mat = GetComponent<Renderer>();
        }
        mat.sortingLayerName = sortingLayerName;
        mat.sortingOrder = orderInLayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        //print(gameObject.GetComponent<Image>().material);
        
       mat = gameObject.GetComponent<Renderer>();
       SetSortingLayer();
        mat.material.color = cor;
      
    }
    
    // Update is called once per frame
    void Update()
    {
        valor += vel * Time.deltaTime;
        mat.material.mainTextureOffset= new Vector2(valor, 0);
        
    }
}
