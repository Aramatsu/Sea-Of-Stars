using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EnemySpawner_Script : MonoBehaviour
{
    //WARNING: STUPIDLY MESSY ASS CODE PLS HELP ME I WANNA DIE
    public delegate void Enemy_Delegate();//the delegate that 
    public static Enemy_Delegate Onkill;

    private Slider waveSlider = null;
    private Text waveText;

    [Serializable]
    public struct Enemy
    {
        public GameObject Prefab;
        public float Rareness;
    }

    //Enemy types
    [SerializeField] private Enemy[] Enemys;

    //variables for enemy spawning
    [SerializeField] private Transform spawn_area;

    private int wave_number = 0;
    [SerializeField]private float _difficultyModifier;
    [SerializeField]private float _spawningSpeed;
    private int Enemy_num; //number of enemys 
    private GameObject _enemyType; 
    private float _enemyWeights; 

    private void Awake()
    {
        Onkill += Enemy_died; // on awake add the method that handles when an enemy dies to the onkill delegate
    }


    //start function
    private void Start()
    {
        waveSlider = UI_Handler.SharedInstance.GetElement("Wave Slider")
            .Element
            .GetComponent<Slider>();
        waveText = UI_Handler.SharedInstance.GetElement("Wave Text")
            .Element
            .GetComponent<Text>();

        // start
        for (int j = 0; j < Enemys.Length; j++)
        {
            _enemyWeights += 1 / Enemys[j].Rareness;
        }
        StartCoroutine("WaveStart");
    }

    
    public IEnumerator WaveStart()
    {

        wave_number++; // wave number is used both as a counter as a difficulty level
        for (int i = 3; i > 0; i--)
        {
            waveText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        StartCoroutine("OnWave");
        waveText.text = "Go!";
        yield return new WaitForSeconds(1);
        waveText.text = "";

    }

    IEnumerator OnWave()
    {
        Enemy_num = Mathf.RoundToInt((wave_number * _difficultyModifier) + 4);
        for (int i = 0; i < Mathf.RoundToInt((wave_number * _difficultyModifier) + 4); i++)
        {
            for (int j = 0; j < Enemys.Length; j++)
            {
                if (UnityEngine.Random.Range(0.0f, _enemyWeights) < 1 / Enemys[j].Rareness)
                {
                    _enemyType = Enemys[j].Prefab;
                }
                else
                {
                    _enemyType = Enemys[0].Prefab;
                }
            }
            
            Instantiate(_enemyType, new Vector2(spawn_area.position.x + UnityEngine.Random.Range(-1 * (spawn_area.localScale.x / 2), spawn_area.localScale.x / 2), spawn_area.position.y), Quaternion.identity);
            
            yield return new WaitForSeconds(_spawningSpeed);
        }
        waveSlider.value = 1f;
    }

    private void Enemy_died()
    {
        Enemy_num--;

        waveSlider.value -= 1.0f / Mathf.RoundToInt((wave_number * _difficultyModifier) + 5);
        if (Enemy_num == 0)
        {
            Debug.Log("Wave over!!!");
            StartCoroutine(WaveOver());
            Debug.Log("Next wave");

        }
    }

    IEnumerator WaveOver()
    {
        wave_number++;
        for (int i = 0; i < 50; i++)
        {
            waveSlider.value = 0.02f * i;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(WaveStart());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(-1 * (spawn_area.localScale.x / 2), spawn_area.position.y),new Vector2(spawn_area.localScale.x / 2, spawn_area.position.y));
    }
}

#region Enemy Code

//The main class of all enemies
public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Player_Controller.Weapon current_player_weapon;
    public ParticleSystem ParticleHurt;
    public Slider HealthBar;
    public Rigidbody2D rb; // the rigidbody of the star
    public Animator anim;// the anim of the star

    [Space]
    [Header("Prefabs")]
    private GameObject current_mana; //variable used in the create method

    [Space]
    [Header("Values")]
    public Vector2 planet_pos = new Vector2(0, -225);
    public float health; //the enemies health
    public float speed; // their speed
    public float damage; // the amount of damage they deal to the player 
    public int Mana; // the amount of mana given when killed 

    public static Transform PlayerTransform;

    private void Awake()
    {
        PlayerTransform = Physics2D.CircleCast(transform.position, Mathf.Infinity, Vector2.zero, Mathf.Infinity, (1 << 10)).transform;
    }

    private void Start()
    {
        HealthBar.maxValue = health;
        HealthBar.value = HealthBar.maxValue;
        Player_Controller.Onshot += Shot;
        MoveToPlanet(transform.position, planet_pos, rb, speed);

    }
    //function that hurts the star and does the coresponding other crap
    public void Damage(float damage)
    {
        health -= damage;
        HealthBar.value = health;
        ParticleHurt.Play();
        AudioManager.audioManager.PlaySound("EnemyHurt");
        if (health <= 0)
        {
            Die(Mana); // gio sucks
        }
        anim.SetTrigger("Hurt");
    }

    //the function that sets the weapon of the player
    public void Shot(Quaternion rotation, Vector2 direction, Player_Controller.Weapon weapon, string[] tags, GameObject Whoshot)
    {
        current_player_weapon = weapon;
    }

    // function that moves the star toward the planet
    public void MoveToPlanet(Vector2 r_pos, Vector2 p_pos, Rigidbody2D rb2D, float speed)
    {
        rb2D.AddForce(Vector2.down * speed, ForceMode2D.Force);
    }
    
    //the function that used to kill the star
    public void Die(int mana)
    {
        Player_Controller.Onshot -= Shot;
        EnemySpawner_Script.Onkill();
        for (int i = 0; i < mana; i++)
        {
            current_mana = Object_pool.Shared_instance.Create(Object_pool.Shared_instance.Pooled_Squares, transform.position + new Vector3(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2), 0), Quaternion.identity);
            if (current_mana)
            {
                current_mana.GetComponent<Object_script>().Explode(transform.position);
            }
        }
        AudioManager.audioManager.PlaySound("EnemyDie", 0.5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player_Controller>().damage(damage);
        }
    }
}
#endregion