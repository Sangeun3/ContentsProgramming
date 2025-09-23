using UnityEngine;

public class WeatherData : MonoBehaviour
{
    //변수 선언 (데이터를 담을 상자 만들기)
    public float temperature = 25.0f; // 온도
    public float humidity = 60.0f; // 습도
    public string weatherType = "맑음"; // 날씨 상태
    public bool isRaining = true; // 비 오는지

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 변수 값 출력하기
        Debug.Log("현재 온도: " + temperature + "도");
        Debug.Log("현재 습도: " + humidity + "%");
        Debug.Log("날씨 상태: " + weatherType);

        if (isRaining)
        {
            Debug.Log("우산을 챙기세요! 비가 오고 있습니다!");
        }
        else
        {
            Debug.Log("비가 오지 않습니다.");
        }
    }



}
