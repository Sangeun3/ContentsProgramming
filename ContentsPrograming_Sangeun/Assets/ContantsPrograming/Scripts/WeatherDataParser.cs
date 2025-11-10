using UnityEngine;
using System.IO;      // File 클래스 사용
using System.Text;    // Encoding 클래스 사용

public class WeatherDataParser : MonoBehaviour
{
    // 12개월 평균기온 저장 배열
    private float[] monthlyTemps = new float[12];

    void Start()
    {
        LoadWeatherData();
        DisplayResults();
    }

    void LoadWeatherData()
    {
        // 1. StreamingAssets 폴더 경로 + 파일명
        string csvPath = Path.Combine(
            Application.streamingAssetsPath,
            "extremum_20251104104449.csv"
        );

        // 2. 파일 존재 여부 확인
        if (!File.Exists(csvPath))
        {
            Debug.LogError("❌ CSV 파일을 찾을 수 없습니다: " + csvPath);
            return;
        }

        // 3. UTF-8 인코딩으로 파일 읽기
        string content = File.ReadAllText(csvPath, Encoding.UTF8);

        // 4. 줄 단위로 분리
        string[] lines = content.Split('\n');

        // 5. 2024년 12개월 데이터 파싱 (8번째 줄부터 19번째 줄까지)
        for (int i = 13; i < 25; i++)
        {
            // 각 줄을 쉼표로 분리
            string[] values = lines[i].Split(',');

            // 평균기온은 3번째 컬럼 (index 2)
            string avgTempStr = values[6].Trim();

            // 문자열을 float로 변환
            float avgTemp = float.Parse(avgTempStr);

            // 배열에 저장 (0월~11월 인덱스로 저장)
            monthlyTemps[i - 13] = avgTemp;
        }

        Debug.Log("✅ 날씨 데이터 로드 성공!");
    }

    void DisplayResults()
    {
        Debug.Log("=== 2024년 서울 월별 평균기온 ===");

        for (int i = 0; i < monthlyTemps.Length; i++)
        {
            Debug.Log($"{i + 1}월: {monthlyTemps[i]}℃");
        }
    }
}