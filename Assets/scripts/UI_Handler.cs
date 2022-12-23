

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    public static UI_Handler SharedInstance;

    public static bool IsPaused = false;

    [System.Serializable] 
    public class UIElement // just for clarification, this is a class because i need to be a nullable type
    {
        public string Name;
        public GameObject Element;
    }

    public UIElement[] UIElements;

    public string NextScene;

    private float _pausePanelStartPos;
    private RectTransform _rectTransform;
    private bool _pauseExist = false;

    [SerializeField] private RectTransform SceneTransition;
 
    private void Awake()
    {
        SharedInstance = this;
        IsPaused = false;
        if (TryGetElement("Pause Panel"))
        {
            _pauseExist = true; 
            _rectTransform = UI_Handler.SharedInstance.GetElement("Pause Panel").Element.GetComponent<RectTransform>();
            _pausePanelStartPos = UI_Handler.SharedInstance.GetElement("Pause Panel").Element.GetComponent<RectTransform>().localPosition.y;
        }



    }


    private void Start()
    {
        
        EndSceneTransition();
    }

    private void Update()
    {
        if (_pauseExist)
        {
            if (!IsPaused)
            {
                //do the shit underneath me when pause is NOT on



                _rectTransform.localPosition = new Vector2(_rectTransform.localPosition.x, Mathf.Lerp(_rectTransform.localPosition.y, _pausePanelStartPos, 0.1f));


            }
            else //do these things when pause IS on
            {



                _rectTransform.localPosition = new Vector2(_rectTransform.localPosition.x, Mathf.Lerp(_rectTransform.localPosition.y, 0, 0.1f));
            }
        }

    }

    //-----------------------------------------------------------
    public UIElement GetElement(string ElementName)
    {
        foreach (UIElement element in UIElements)
        {
            if (string.Equals(ElementName, element.Name))
            {
                return element;

            }
        }
        Debug.LogWarning("No such UIElement!");
        return null;
    }

    public bool TryGetElement(string ElementName)
    {
        foreach (UIElement element in UIElements)
        {
            if (string.Equals(ElementName, element.Name))
            {
                return true;

            }
        }
        Debug.LogWarning("No such UIElement!");
        return false;
    }

    public void EndSceneTransition()
    {
        LeanTween.alpha(SceneTransition, 0, 1);
    }

    public void PlaySceneTransition(string ToScene)
    {
        Time.timeScale = 1;
        NextScene = ToScene;
        LeanTween.alpha(SceneTransition, 1, 1).setOnComplete(loadScene);
    }

    private void loadScene()
    {
        SceneManager.LoadScene(NextScene);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    //function that defines what happens when the player pauses the game, not entirely sure why im defining it here though but oh well
    public static  void Pause()
    {
        if (IsPaused)
        {
            IsPaused = false;
            Time.timeScale = 1;
        }
        else if (!IsPaused)
        {
            IsPaused = true;
            Time.timeScale = 0;
        }




    }
}
