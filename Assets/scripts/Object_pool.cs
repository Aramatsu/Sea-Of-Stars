using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_pool : MonoBehaviour
{
    public static Object_pool Shared_instance;
    public GameObject[] Pooled_Squares = new GameObject[100];
    public int Number_Squares;
    public GameObject Squares;

    [SerializeField] private Sprite[] Mana_sprites;
    
    // Start is called before the first frame update
    void Awake()
    {
        Shared_instance = this;
    }

    //
    private void Start()
    {
        for (int i = 0; i < Number_Squares; i++)
        {
            GameObject clone = Instantiate(Squares, Vector2.zero, Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().sprite = Mana_sprites[Random.Range(0, Mana_sprites.Length)]; 
            clone.SetActive(false);
            Pooled_Squares[i] = clone;
            
        }
    }

    public GameObject Create(GameObject[] pool, Vector2 position)
    {
        for(int i = 0; i < Number_Squares; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                pool[i].transform.position = position;
                return pool[i];
            }
        }
        return null;
    }
}
