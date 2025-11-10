using UnityEngine;
public class ArrayExample : MonoBehaviour
{
    public float[] monthlyTemps = new float[12]
    {
        -2f, 0f, 7f, 14f, 20f, 25f,
        28f, 27f, 22f, 15f, 7f, -5f
    };

    void Start()
    {
        // 특정 월 온도 읽기
        Debug.Log("1월 온도: " + monthlyTemps[0] + "°C");
        Debug.Log("7월 온도: " + monthlyTemps[6] + "°C");
        Debug.Log("12월 온도: " + monthlyTemps[11] + "°C");

        // 변수에 저장해서 사용
        float january = monthlyTemps[0];
        float july = monthlyTemps[6];
        Debug.Log("1월과 7월 온도 차: " + (july - january));
    }

    void Update()
    {
        // 6월 온도를 26도로 변경
        monthlyTemps[5] = 26f;

        // 12월 온도를 -3도로 변경
        monthlyTemps[11] = -3f;

        // 조건부 수정도 가능
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 스페이스바 누르면 7월을 30도로
            monthlyTemps[6] = 30f;
            Debug.Log("7월 온도 업데이트: 30°C");
        }
    }
}