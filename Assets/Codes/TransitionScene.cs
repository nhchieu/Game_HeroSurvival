using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    public void LoadScene(int a)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(a, LoadSceneMode.Single);
    }
}
