using UnityEngine;
using TMPro;

public class ScreenHeartManager : MonoBehaviour
{
    public static ScreenHeartManager Instance;
    public TextMeshProUGUI heartsText; // 绑定 UI 文字

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // UI 不会随场景切换消失
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateHeartsUI(); // 初始化 Hearts 显示
    }

    public void UpdateHeartsUI()
    {
        if (heartsText != null)
        {
            heartsText.text = " " + GameManager.Instance.hearts;
        }
    }
}
