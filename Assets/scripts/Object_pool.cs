using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_pool : MonoBehaviour
{
    public static Object_pool Shared_instance;
    public List<GameObject> Pooled_Squares;
    public List<GameObject> Pooled_bullets;
    public int Number_of_Squares;
    public int Number_of_Bullets;
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
        for (int i = 0; i < Number_of_Squares; i++)
        {
            GameObject clone = Instantiate(Squares, Vector2.zero, Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().sprite = Mana_sprites[Random.Range(0, Mana_sprites.Length)]; 
            clone.SetActive(false);
            Pooled_Squares.Add(clone);
            
        }

        for (int i = 0; i < Number_of_Bullets; i++)
        {
            GameObject clone = Instantiate(Bullets, Vector2.zero, Quaternion.identity);
            clone.SetActive(false);
            Pooled_bullets.Add(clone);

        }
    }

    public GameObject Create(List<GameObject> pool, Vector2 position, Quaternion rotation)
    {
        //Check if there are any unactive objects
        for(int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                //and if so activate that object and return it
                pool[i].transform.position = position;
                pool[i].transform.localRotation = rotation;
                pool[i].SetActive(true);

                return pool[i];
            }
        }
        //if they are all active create a new one
        if (pool[0].CompareTag("Mana"))
        {
            GameObject clone = Instantiate(Squares, Vector2.zero, Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().sprite = Mana_sprites[Random.Range(0, Mana_sprites.Length)];
            clone.SetActive(true);
            pool.Add(clone);
            return pool[pool.Count - 1];
        }
        else if (pool[0].CompareTag("Bullet1"))
        {
            GameObject clone = Instantiate(Bullets, Vector2.zero, Quaternion.identity);
            clone.SetActive(true);
            pool.Add(clone);
            return pool[pool.Count - 1];
        }

        //If all else fails, return null
        return null;
        
    }
}
