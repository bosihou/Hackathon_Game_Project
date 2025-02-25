using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))  // 按下 R 键返回第一个场景
        {
            SceneManager.LoadScene(0);  // 0 是第一个场景的索引
        }
    }
}