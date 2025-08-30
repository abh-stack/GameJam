using UnityEngine;
using UnityEngine.SceneManagement;

public class EndReset : MonoBehaviour
{
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.R))
        {
           
            SceneManager.LoadScene(0);
        }
    }
}
