using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_pool : MonoBehaviour
{
    public static Object_pool Shared_instance;
    public GameObject[] Pooled_Squares = new GameObject[100];
    public GameObject[] Pooled_bullets = new GameObject[100];
    public int Number_Squares;
    public int Number_bullets;
    public GameObject Squares;
    public GameObject Bullets;

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

        for (int i = 0; i < Number_bullets; i++)
        {
            GameObject clone = Instantiate(Bullets, Vector2.zero, Quaternion.identity);
            clone.SetActive(false);
            Pooled_bullets[i] = clone;

        }
    }

    public GameObject Create(GameObject[] pool, Vector2 position, Vector3 Rotation)
    {
        for(int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].transform.position = position;
                pool[i].transform.localRotation = Quaternion.Euler(Rotation);
                pool[i].SetActive(true);

                return pool[i];
            }
        }
        return null;
    }
}
