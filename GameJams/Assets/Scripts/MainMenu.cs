using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Specify the name of the next scene, or leave empty to go to next scene in build order")]
    public string nextSceneName = "";

    void Update()
    {
        // Check if Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

           
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No next scene available. This is the last scene.");
                
            }
        }
    }
}