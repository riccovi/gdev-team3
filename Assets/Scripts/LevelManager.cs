using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<Scene> SceneList;
    public static LevelManager instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;

    public float _target;

    private IEnumerator loadCor;


    void Awake()
    {
        if(instance==null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public async void LoadScene(string SceneName)
    {
        _target =0;
        var scene= SceneManager.LoadSceneAsync(SceneName);
        //Nie przechodź automatycznie do następnej sceny
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        StartCoroutine(loadBar (1.2f,scene,SceneName));
        

    }

    public async void LoadScene(string SceneName,float Time)
    {
        _target =0;
        var scene= SceneManager.LoadSceneAsync(SceneName);
        //Nie przechodź automatycznie do następnej sceny
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        StartCoroutine(loadBar (Time,scene,SceneName));
        

    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Load the current scene again
        LoadScene(currentScene.name);


        // Find and unload all inactive scenes
        UnloadInactiveScenes();


    }

    public void MoveToNextScene()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Calculate the next scene's build index
        int nextSceneIndex = currentScene.buildIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            LoadScene(currentScene.name);
        }
        else
        {
            // If no more scenes exist, return to the "0.MainMenu" scene
            LoadScene("0.MainMenu");
        }

        // Unload all inactive scenes to free up memory
        UnloadInactiveScenes();
    }

    public void BackMainMenu()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Load the current scene again
        LoadScene("0.MainMenu");


        // Find and unload all inactive scenes
        UnloadInactiveScenes();


    }

    private void UnloadInactiveScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene != SceneManager.GetActiveScene())
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }

    public void StartGame()
    {
        Debug.Log("Load first level");
        LevelManager.instance.LoadScene("1.Level0");
    }

    IEnumerator loadBar(float  duration,AsyncOperation scene,string SceneName)
    {
        float time = 0.0f;
        bool completedLoading=false;
        while(time < duration)
        {


            _progressBar.fillAmount = time / duration;
            time += Time.deltaTime;
            if(time>=duration)
            {
                completedLoading=true;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(completedLoading && scene.progress>=0.9f)
        {
            _progressBar.fillAmount = 1;
            scene.allowSceneActivation=true;

            yield return new WaitForSeconds(0.5f);

            if(SceneName !="0.MainMenu")
            {
                FindObjectOfType<GameManager>().LevelManager=this;
            }   
            else
            {
                var GameButtons = FindObjectsOfType<Button>();

                foreach(Button but in GameButtons)
                {
                    if(but.name=="StartGame")
                    {
                        but.onClick.AddListener(StartGame);
                    }
                }
            }         
            _loaderCanvas.SetActive(false);
        }

        if(SceneName=="MainMenu")
        {
            //MainMenu_Handler.instance.Activate_MainMenu();
        }

    }

}
