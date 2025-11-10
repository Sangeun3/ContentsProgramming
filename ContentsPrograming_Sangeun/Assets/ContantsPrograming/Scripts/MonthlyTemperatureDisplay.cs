using UnityEngine;

public class MonthlyTemperatureDisplay : MonoBehaviour
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

    void Start()
    {
        DisplayAllMonths();
        ShowStatistics();
    }

    // 모든 달의 온도 출력
    void DisplayAllMonths()
    {
        Debug.Log("===== 2024년 서울 평균 온도 =====");

        for (int i = 0; i < monthlyTemperatures.Length; i++)
        {
            // i+1을 해야 1월부터 12월로 표시됨 (i는 0부터 시작)
            Debug.Log((i + 1) + "월: " + monthlyTemperatures[i] + "°C");
        }

        Debug.Log("================================");
    }

    // 통계 정보 출력
    void ShowStatistics()
    {
        // 배열 크기 확인
        Debug.Log("총 데이터 개수: " + monthlyTemperatures.Length + "개");

        // 가장 추운 달 찾기
        float minTemp = monthlyTemperatures[0];
        int coldestMonth = 0;

        for (int i = 1; i < monthlyTemperatures.Length; i++)
        {
            if (monthlyTemperatures[i] < minTemp)
            {
                minTemp = monthlyTemperatures[i];
                coldestMonth = i;
            }
        }

        Debug.Log("가장 추운 달: " + (coldestMonth + 1) + "월 (" + minTemp + "°C)");

        // 가장 더운 달 찾기
        float maxTemp = monthlyTemperatures[0];
        int hottestMonth = 0;

        for (int i = 1; i < monthlyTemperatures.Length; i++)
        {
            if (monthlyTemperatures[i] > maxTemp)
            {
                maxTemp = monthlyTemperatures[i];
                hottestMonth = i;
            }
        }

        Debug.Log("가장 더운 달: " + (hottestMonth + 1) + "월 (" + maxTemp + "°C)");

        // 평균 온도 계산
        float sum = 0;
        for (int i = 0; i < monthlyTemperatures.Length; i++)
        {
            sum += monthlyTemperatures[i];
        }
        float average = sum / monthlyTemperatures.Length;

        Debug.Log("연평균 온도: " + average.ToString("F1") + "°C");
    }
}