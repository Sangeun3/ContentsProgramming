using UnityEngine;

public class MyFirstScript : MonoBehaviour
{
    public int playerLevel = 1; // 플레이어 레벨
    public float walkSpeed = 5.0f; // 걸음 속도
    public string playerName = "김철수"; // 플레이어 이름
    public bool canJump = true; // 점프 가능 여부
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start는 게임 시작할 때 한 번 실행됩니다.
    void Start()
    {
        Debug.Log("플레이어 레벨: " + playerLevel);
        Debug.Log("걸음 속도: " + walkSpeed + "m/s");
        Debug.Log("플레이어 이름: " + playerName);
        Debug.Log("점프 가능: " + canJump);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
