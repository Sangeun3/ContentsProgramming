using UnityEngine;
using TMPro;

public class MonthlyTemperatureData : MonoBehaviour
{
    [Header("12개월 평균 온도 데이터")]
    public float[] monthlyTemperatures = new float[12]
    {
        -2f,   // 1월
        0f,    // 2월
        7f,    // 3월
        14f,   // 4월
        20f,   // 5월
        25f,   // 6월
        28f,   // 7월
        27f,   // 8월
        22f,   // 9월
        15f,   // 10월
        7f,    // 11월
        -5f    // 12월
    };

    [Header("특정 월 온도 표시")]
    public TextMeshProUGUI januaryText;
    public TextMeshProUGUI julyText;
    public TextMeshProUGUI decemberText;

    void Start()
    {
        DisplayMonthTemperatures();
        ShowArrayInfo();
    }

    void DisplayMonthTemperatures()
    {
        // 1월 온도 표시 (인덱스 0)
        januaryText.text = "1월: " + monthlyTemperatures[0] + "°C";

        // 7월 온도 표시 (인덱스 6)
        julyText.text = "7월: " + monthlyTemperatures[6] + "°C";

        // 12월 온도 표시 (인덱스 11)
        decemberText.text = "12월: " + monthlyTemperatures[11] + "°C";
    }

    void ShowArrayInfo()
    {
        // Console에 배열 정보 출력
        Debug.Log("=== 12개월 온도 데이터 ===");
        Debug.Log("배열 크기: " + monthlyTemperatures.Length);
        Debug.Log("마지막 인덱스: " + (monthlyTemperatures.Length - 1));
        Debug.Log("");

        // 모든 달 온도 출력 (지금은 12줄, 2교시에는 3줄로 줄일 예정!)
        Debug.Log("1월: " + monthlyTemperatures[0] + "°C");
        Debug.Log("2월: " + monthlyTemperatures[1] + "°C");
        Debug.Log("3월: " + monthlyTemperatures[2] + "°C");
        Debug.Log("4월: " + monthlyTemperatures[3] + "°C");
        Debug.Log("5월: " + monthlyTemperatures[4] + "°C");
        Debug.Log("6월: " + monthlyTemperatures[5] + "°C");
        Debug.Log("7월: " + monthlyTemperatures[6] + "°C");
        Debug.Log("8월: " + monthlyTemperatures[7] + "°C");
        Debug.Log("9월: " + monthlyTemperatures[8] + "°C");
        Debug.Log("10월: " + monthlyTemperatures[9] + "°C");
        Debug.Log("11월: " + monthlyTemperatures[10] + "°C");
        Debug.Log("12월: " + monthlyTemperatures[11] + "°C");
    }
}