using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    public static UI_Handler SharedInstance;

    [System.Serializable] 
    public struct UIElement
    {
        public string Name;
        public GameObject Element;
    }

    public UIElement[] UIElements;

    public string NextScene;

    [SerializeField] private RectTransform SceneTransition;
 
    private void Awake()
    {
        SharedInstance = this;
    }

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
        return UIElements[0];
    }

    private void Start()
    {
        EndSceneTransition();
    }

    public void EndSceneTransition()
    {
        LeanTween.alpha(SceneTransition, 0, 1);
    }

    public void PlaySceneTransition(string ToScene)
    {
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


}
