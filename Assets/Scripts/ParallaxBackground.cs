using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform player;
    public float parallaxEffect = 0.5f;
    private Vector3 startOffset;
    private float backgroundWidth;
    void Start()
    {
        FindPlayer(); // 在 Start 里找到 Player
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            backgroundWidth = sr.bounds.size.x;
        }
    }

    void Update()
    {
        if (player == null) // 如果 Player 还没找到，持续查找
        {
            FindPlayer();
        }

        if (player != null)
        {
            // 计算新的背景位置，使其中点对准 Player
            float newX = player.position.x * parallaxEffect;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); // 找到带有 "Player" Tag 的物体
        if (playerObj != null)
        {
            player = playerObj.transform;
            startOffset = transform.position - player.position; // 计算偏移量
        }
    }
}
