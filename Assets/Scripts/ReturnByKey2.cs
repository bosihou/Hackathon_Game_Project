using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnByKey2 : MonoBehaviour
{
    void Update()
    {
        // 监听 R 键按下事件
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 检查当前场景是否为 SuccessScene
            if (SceneManager.GetActiveScene().name == "SuccessScene")
            {
                SceneManager.LoadScene("StartScene");  // 替换为你的 Start Scene 名称
            }
        }
    }
}