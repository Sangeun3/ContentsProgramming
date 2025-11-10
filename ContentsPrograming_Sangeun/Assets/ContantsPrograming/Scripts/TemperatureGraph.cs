using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemperatureGraph : MonoBehaviour
{
    [Header("12개월 온도 데이터")]
    public float[] monthlyTemperatures = new float[12]
    {
        -2f, 0f, 7f, 14f, 20f, 25f,
        28f, 27f, 22f, 15f, 7f, -5f
    };

    [Header("12개의 막대 그래프")]
    public Image[] barImages = new Image[12];

    [Header("12개의 월 라벨")]
    public TextMeshProUGUI[] monthLabels = new TextMeshProUGUI[12];

    [Header("12개의 온도 텍스트")]
    public TextMeshProUGUI[] tempTexts = new TextMeshProUGUI[12];

    [Header("온도 범위 설정")]
    public float minTemp = -10f;  // 최저 온도
    public float maxTemp = 40f;   // 최고 온도

    [Header("색상 설정")]
    public Color coldColor = Color.blue;      // 추운 색
    public Color warmColor = Color.yellow;    // 따듯한 색
    public Color hotColor = Color.red;        // 더운 색

    void Start()
    {
        // 12개월 그래프 초기화
        InitializeGraph();
    }

    void InitializeGraph()
    {
        Debug.Log("===== 그래프 초기화 시작 =====");

        // for문으로 12개월 모두 처리
        for (int i = 0; i < monthlyTemperatures.Length; i++)
        {
            // 1) 월 라벨 설정 (1월, 2월, ...)
            monthLabels[i].text = (i + 1) + "월";

            // 2) 온도 텍스트 설정
            tempTexts[i].text = monthlyTemperatures[i] + "°C";

            // 3) 막대 길이 계산 (정규화)
            float normalizedValue = Normalize(monthlyTemperatures[i], minTemp, maxTemp);
            barImages[i].fillAmount = normalizedValue;

            // 4) 온도에 따른 색상 설정
            barImages[i].color = GetTemperatureColor(monthlyTemperatures[i]);

            Debug.Log((i + 1) + "월: " + monthlyTemperatures[i] + "°C, 비율: " +
                      normalizedValue.ToString("F2"));
        }

        Debug.Log("===== 그래프 초기화 완료 =====");
    }

    // 정규화: 온도를 0~1 범위로 변환
    float Normalize(float value, float min, float max)
    {
        // (value - min) / (max - min)
        return (value - min) / (max - min);
    }

    // 온도에 따른 색상 결정
    Color GetTemperatureColor(float temp)
    {
        // 온도를 0~1로 정규화
        float t = Normalize(temp, minTemp, maxTemp);

        // 0~0.5: 파란색 → 노란색 (추움 → 따듯함)
        // 0.5~1.0: 노란색 → 빨간색 (따듯함 → 더움)
        if (t < 0.5f)
        {
            // 0~0.5 범위를 0~1로 확장
            float blendAmount = t * 2f;
            return Color.Lerp(coldColor, warmColor, blendAmount);
        }
        else
        {
            // 0.5~1.0 범위를 0~1로 확장
            float blendAmount = (t - 0.5f) * 2f;
            return Color.Lerp(warmColor, hotColor, blendAmount);
        }
    }
}