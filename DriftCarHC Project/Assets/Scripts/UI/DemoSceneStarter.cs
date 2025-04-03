using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoSceneStarter : MonoBehaviour
{
    // Start is called before the first frame update
    public void DemoScene()
    {
        SceneManager.LoadScene("Demo");
    }
}