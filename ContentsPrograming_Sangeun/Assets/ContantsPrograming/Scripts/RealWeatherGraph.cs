using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;      // File 클래스 사용
using System.Text;    // Encoding 클래스 사용

public class RealWeatherGraph : MonoBehaviour
{
    [Header("12개의 막대 그래프")]
    public Image[] barImages = new Image[12];

    [Header("12개의 월 라벨")]
    public TextMeshProUGUI[] monthLabels = new TextMeshProUGUI[12];

    [Header("12개의 온도 텍스트")]
    public TextMeshProUGUI[] tempTexts = new TextMeshProUGUI[12];

    [Header("온도 범위 설정")]
    public float minTemp = -10f;
    public float maxTemp = 40f;

    [Header("색상 설정")]
    public Color coldColor = new Color(0f, 0.5f, 1f);      // 파란색
    public Color warmColor = new Color(1f, 0.8f, 0f);      // 노란색
    public Color hotColor = new Color(1f, 0.2f, 0f);       // 빨간색

    // 실제 날씨 데이터 저장
    private float[] monthlyTemps = new float[12];

    void Start()
    {
        LoadWeatherData();
        UpdateGraph();
    }

    // 2교시에서 배운 CSV 파싱 (StreamingAssets + UTF-8)
    void LoadWeatherData()
    {
        Debug.Log("===== CSV 파일 로드 시작 =====");

        // 1단계: StreamingAssets 경로 + 파일명
        string csvPath = Path.Combine(
            Application.streamingAssetsPath,
            "ta_20251104111223.csv"
        );

        // 2단계: 파일 존재 여부 확인
        if (!File.Exists(csvPath))
        {
            Debug.LogError("❌ CSV 파일을 찾을 수 없습니다: " + csvPath);
            return;
        }

        // 3단계: UTF-8 인코딩으로 파일 읽기
        string content = File.ReadAllText(csvPath, Encoding.UTF8);

        // 4단계: 줄로 나누기
        string[] lines = content.Split('\n');
        Debug.Log($"총 줄 수: {lines.Length}");

        // 5단계: 8번째 줄부터 12개월 데이터 파싱
        for (int i = 8; i < 20; i++)
        {
            string[] values = lines[i].Split(',');
            string avgTempStr = values[2].Trim();  // 평균기온
            float avgTemp = float.Parse(avgTempStr);

            monthlyTemps[i - 8] = avgTemp;
            Debug.Log($"{i - 7}월: {avgTemp}℃");
        }

        Debug.Log("✅ CSV 파일 로드 완료!");
    }

    // 7주차에서 배운 그래프 업데이트
    void UpdateGraph()
    {
        Debug.Log("===== 그래프 업데이트 시작 =====");

        for (int i = 0; i < 12; i++)
        {
            // 온도 가져오기
            float temp = monthlyTemps[i];

            // 1. 월 라벨 설정
            monthLabels[i].text = (i + 1) + "월";

            // 2. 온도 텍스트 설정
            tempTexts[i].text = temp + "°C";

            // 3. 막대 길이 설정 (정규화)
            float normalized = Normalize(temp, minTemp, maxTemp);
            barImages[i].fillAmount = normalized;

            // 4. 막대 색상 설정
            barImages[i].color = GetTemperatureColor(temp);

            Debug.Log($"{i + 1}월: {temp}°C, 막대길이: {normalized:F2}, 색상: {barImages[i].color}");
        }

        Debug.Log("✅ 그래프 업데이트 완료!");
    }

    // 정규화 함수 (7주차)
    float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    // 온도별 색상 함수 (7주차)
    Color GetTemperatureColor(float temp)
    {
        float t = Normalize(temp, minTemp, maxTemp);

        if (t < 0.5f)
        {
            // 추운 온도 (파란색 → 노란색)
            return Color.Lerp(coldColor, warmColor, t * 2f);
        }
        else
        {
            // 더운 온도 (노란색 → 빨간색)
            return Color.Lerp(warmColor, hotColor, (t - 0.5f) * 2f);
        }
    }
}