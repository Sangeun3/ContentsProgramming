using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용
using System.IO;
using System.Text;

public class WeatherManagerFinal : MonoBehaviour
{
    [Header("UI 버튼 (20개 연결)")]
    public Button[] weatherButtons;

    [Header("정보 표시 텍스트 (우측/하단 패널)")]
    public TextMeshProUGUI textCountry;
    public TextMeshProUGUI textCity;
    public TextMeshProUGUI textTemp;
    public TextMeshProUGUI textRain;

    [Header("카운트 표시 텍스트 (예: 5/12)")]
    public TextMeshProUGUI textSunCount;   // 해 카운트 텍스트
    public TextMeshProUGUI textCloudCount; // 구름 카운트 텍스트
    public TextMeshProUGUI textSnowCount;  // 눈 카운트 텍스트

    [Header("날씨 이미지 소스 (클릭 전/후)")]
    // 클릭 전 (기본/흑백 등)
    public Sprite sunNormal;
    public Sprite cloudNormal;
    public Sprite snowNormal;

    // 클릭 후 (색칠됨)
    public Sprite sunColored;
    public Sprite cloudColored;
    public Sprite snowColored;

    // 내부 데이터 클래스
    private class WeatherData
    {
        public string country;
        public string city;
        public float avgTemp;
        public float rain;

        public WeatherType type; // 날씨 타입
        public bool isClicked;   // 중복 클릭 방지 체크
    }

    // 날씨 타입 열거형
    private enum WeatherType { Sun, Cloud, Snow }

    // 카운트용 변수
    private int totalSun = 0, totalCloud = 0, totalSnow = 0;
    private int currentSun = 0, currentCloud = 0, currentSnow = 0;

    void Start()
    {
        LoadWeatherData();
    }

    void LoadWeatherData()
    {
        Debug.Log("===== CSV 파일 로드 시작 =====");

        string fileName = "1STCS_세계기후평년값_MNH_20251202112552.txt";
        string csvPath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(csvPath))
        {
            Debug.LogError("❌ CSV 파일을 찾을 수 없습니다: " + csvPath);
            return;
        }

        string content = File.ReadAllText(csvPath, Encoding.UTF8);
        string[] lines = content.Split('\n');

        int buttonIndex = 0;

        // 변수 초기화
        totalSun = 0; totalCloud = 0; totalSnow = 0;
        currentSun = 0; currentCloud = 0; currentSnow = 0;

        // 6행부터 데이터 시작
        for (int i = 6; i < lines.Length; i++)
        {
            if (buttonIndex >= weatherButtons.Length || string.IsNullOrWhiteSpace(lines[i])) break;

            string[] row = lines[i].Split(',');

            WeatherData data = new WeatherData();
            data.country = row[0];
            data.city = row[1];
            data.isClicked = false; // 아직 클릭 안함

            float.TryParse(row[6], out data.avgTemp);
            float.TryParse(row[9], out data.rain);

            // 날씨 타입 결정 및 전체 개수 카운트
            if (data.avgTemp >= 20.0f)
            {
                data.type = WeatherType.Sun;
                totalSun++;
            }
            else if (data.avgTemp >= 10.0f)
            {
                data.type = WeatherType.Cloud;
                totalCloud++;
            }
            else
            {
                data.type = WeatherType.Snow;
                totalSnow++;
            }

            // 버튼 세팅
            SetupButton(buttonIndex, data);

            buttonIndex++;
        }

        // 초기 카운트 텍스트 갱신 (0 / 전체개수)
        UpdateCountTexts();
    }

    void SetupButton(int index, WeatherData data)
    {
        Button btn = weatherButtons[index];
        Image btnImage = btn.GetComponent<Image>();

        // 초기 이미지 설정 (Normal 이미지)
        switch (data.type)
        {
            case WeatherType.Sun:
                btnImage.sprite = sunNormal;
                break;
            case WeatherType.Cloud:
                btnImage.sprite = cloudNormal;
                break;
            case WeatherType.Snow:
                btnImage.sprite = snowNormal;
                break;
        }

        // 버튼 리스너 연결
        btn.onClick.RemoveAllListeners();
        // 람다식을 통해 버튼 객체(btn)와 데이터(data)를 함께 넘김
        btn.onClick.AddListener(() => OnWeatherButtonClicked(data, btn));

        btn.gameObject.SetActive(true);
    }

    void OnWeatherButtonClicked(WeatherData data, Button btn)
    {
        // 1. 하단/우측 정보 텍스트 갱신 (항상 실행)
        textCountry.text = "국가: " + data.country;
        textCity.text = "도시: " + data.city;
        textTemp.text = $"평균 기온: {data.avgTemp:F1}°C";
        textRain.text = $"강수량: {data.rain:F1}mm";

        // 2. 이미 클릭한 버튼이면 카운트/이미지 변경 로직 건너뜀
        if (data.isClicked) return;

        // 3. 첫 클릭 처리 (중복 방지)
        data.isClicked = true;
        Image btnImage = btn.GetComponent<Image>();

        // 타입별 로직: 이미지 변경(Colored) & 카운트 증가
        switch (data.type)
        {
            case WeatherType.Sun:
                btnImage.sprite = sunColored; // 색칠된 해
                currentSun++;
                break;

            case WeatherType.Cloud:
                btnImage.sprite = cloudColored; // 색칠된 구름
                currentCloud++;
                break;

            case WeatherType.Snow:
                btnImage.sprite = snowColored; // 색칠된 눈
                currentSnow++;
                break;
        }

        // 4. 카운트 텍스트 UI 갱신
        UpdateCountTexts();
    }

    // 카운트 텍스트 형식 (현재 / 전체) 업데이트 함수
    void UpdateCountTexts()
    {
        if (textSunCount != null)
            textSunCount.text = $"{currentSun} / {totalSun}";

        if (textCloudCount != null)
            textCloudCount.text = $"{currentCloud} / {totalCloud}";

        if (textSnowCount != null)
            textSnowCount.text = $"{currentSnow} / {totalSnow}";
    }
}