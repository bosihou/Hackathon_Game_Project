using UnityEngine;

public class GlobalExit : MonoBehaviour
{
    // 确保脚本在所有场景中都生效
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);  // 让该脚本的 GameObject 在场景切换时不会被销毁
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // 停止编辑器运行
#else
            Application.Quit();  // 退出程序
#endif
        }
    }
}