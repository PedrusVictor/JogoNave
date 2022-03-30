using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptArm : MonoBehaviour
{
    public float dano = 2;
    public Vector2 area;
    [SerializeField] private LayerMask player;
    public Transform CenterPoint;
    public void Soco()
    {
        Collider2D[] col = Physics2D.OverlapBoxAll(CenterPoint.transform.position,area,0,player);
        if(col!=null && col.Length > 0)
        {
           
            foreach (Collider2D c in col)
            {
                print(c.gameObject);
                if (c.gameObject.tag == "Player" && c.gameObject.GetComponentInParent<movBoitata>()&& c.gameObject.GetComponentInParent<movBoitata>().hp>0)
                {
                    c.gameObject.GetComponentInParent<movBoitata>().dano();
                    
                }

            }
        }
    }
    public void Porrada()
    {
        Collider2D[] col = Physics2D.OverlapBoxAll(CenterPoint.transform.position, new Vector2( 3*area.x,area.y), 0, player);
        if (col != null && col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                print(c.gameObject);
                if (c.gameObject.tag == "Player" && c.gameObject.GetComponentInParent<movBoitata>()&& c.gameObject.GetComponentInParent<movBoitata>().hp > 0)
                {
                    c.gameObject.GetComponentInParent<movBoitata>().dano();

                }

            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(CenterPoint.transform.position,area);
    }
}
