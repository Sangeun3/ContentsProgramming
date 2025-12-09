using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;

public class WeatherManagerFinal : MonoBehaviour
{
    [Header("UI 버튼 (20개 연결)")]
    public Button[] weatherButtons;

    [Header("각 버튼별 닫기 버튼 (20개 연결)")]
    public Button[] closeButtons;

    [Header("★ 힌트 기능")]
    public Button hintButton;
    public GameObject hintImageObject;

    [Header("★ 완료 기능 (모든 카운트 달성 시)")]
    public GameObject completionImageObject; // 다 찾았을 때 뜰 이미지 오브젝트

    [Header("정보 표시 텍스트")]
    public TextMeshProUGUI textCountry;
    public TextMeshProUGUI textCity;
    public TextMeshProUGUI textTemp;
    public TextMeshProUGUI textRain;

    [Header("★ 원 그래프 (게이지) 이미지")]
    public Image tempGaugeImage;
    public Image rainGaugeImage;

    [Header("카운트 표시 텍스트")]
    public TextMeshProUGUI textSunCount;
    public TextMeshProUGUI textCloudCount;
    public TextMeshProUGUI textSnowCount;

    [Header("날씨 이미지 소스")]
    public Sprite sunNormal; public Sprite sunColored;
    public Sprite cloudNormal; public Sprite cloudColored;
    public Sprite snowNormal; public Sprite snowColored;

    private class WeatherData
    {
        public string country;
        public string city;
        public float avgTemp;
        public float rain;
        public WeatherType type;
        public bool isClicked;
    }

    private enum WeatherType { Sun, Cloud, Snow }

    private int totalSun = 0, totalCloud = 0, totalSnow = 0;
    private int currentSun = 0, currentCloud = 0, currentSnow = 0;

    private Coroutine hintCoroutine;

    void Start()
    {
        // 1. 닫기 버튼 초기 설정
        if (closeButtons != null)
        {
            for (int i = 0; i < closeButtons.Length; i++)
            {
                if (closeButtons[i] != null)
                {
                    int index = i;
                    closeButtons[i].gameObject.SetActive(false);
                    closeButtons[i].onClick.RemoveAllListeners();
                    closeButtons[i].onClick.AddListener(() =>
                    {
                        closeButtons[index].gameObject.SetActive(false);
                    });
                }
            }
        }

        // 2. 힌트 및 완료 이미지 초기화
        if (hintImageObject != null) hintImageObject.SetActive(false);
        if (completionImageObject != null) completionImageObject.SetActive(false); // 시작할 땐 숨김

        if (hintButton != null)
        {
            hintButton.onClick.RemoveAllListeners();
            hintButton.onClick.AddListener(OnHintButtonClicked);
        }

        // 3. 그래프 초기화
        if (tempGaugeImage != null) tempGaugeImage.fillAmount = 0;
        if (rainGaugeImage != null) rainGaugeImage.fillAmount = 0;

        LoadWeatherData();
    }

    void OnHintButtonClicked()
    {
        if (hintImageObject == null) return;
        if (hintCoroutine != null) StopCoroutine(hintCoroutine);
        hintCoroutine = StartCoroutine(ShowHintRoutine());
    }

    IEnumerator ShowHintRoutine()
    {
        hintImageObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        hintImageObject.SetActive(false);
    }

    void LoadWeatherData()
    {
        Debug.Log("===== CSV 파일 로드 시작 =====");
        string fileName = "1STCS_세계기후평년값_MNH_20251202112552.txt";
        string csvPath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(csvPath))
        {
            Debug.LogError("❌ 파일을 찾을 수 없습니다: " + csvPath);
            return;
        }

        string content = File.ReadAllText(csvPath, Encoding.UTF8);
        string[] lines = content.Split('\n');
        int buttonIndex = 0;

        totalSun = 0; totalCloud = 0; totalSnow = 0;
        currentSun = 0; currentCloud = 0; currentSnow = 0;

        for (int i = 6; i < lines.Length; i++)
        {
            if (buttonIndex >= weatherButtons.Length || string.IsNullOrWhiteSpace(lines[i])) break;

            string[] row = lines[i].Split(',');

            WeatherData data = new WeatherData();
            data.country = row[0];
            data.city = row[1];
            data.isClicked = false;

            float.TryParse(row[6], out data.avgTemp);
            float.TryParse(row[9], out data.rain);

            if (data.avgTemp >= 20.0f) { data.type = WeatherType.Sun; totalSun++; }
            else if (data.avgTemp >= 10.0f) { data.type = WeatherType.Cloud; totalCloud++; }
            else { data.type = WeatherType.Snow; totalSnow++; }

            SetupButton(buttonIndex, data);
            buttonIndex++;
        }
        UpdateCountTexts();
    }

    void SetupButton(int index, WeatherData data)
    {
        Button btn = weatherButtons[index];
        Image btnImage = btn.GetComponent<Image>();

        switch (data.type)
        {
            case WeatherType.Sun: btnImage.sprite = sunNormal; break;
            case WeatherType.Cloud: btnImage.sprite = cloudNormal; break;
            case WeatherType.Snow: btnImage.sprite = snowNormal; break;
        }

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => OnWeatherButtonClicked(index, data, btn));
        btn.gameObject.SetActive(true);
    }

    void OnWeatherButtonClicked(int index, WeatherData data, Button btn)
    {
        textCountry.text = "국가: " + data.country;
        textCity.text = "도시: " + data.city;
        textTemp.text = $"평균 기온: {data.avgTemp:F1}°C";
        textRain.text = $"강수량: {data.rain:F1}mm";

        UpdateGauges(data.avgTemp, data.rain);

        if (closeButtons != null && index < closeButtons.Length && closeButtons[index] != null)
        {
            closeButtons[index].gameObject.SetActive(true);
        }

        if (data.isClicked) return;

        data.isClicked = true;
        Image btnImage = btn.GetComponent<Image>();

        switch (data.type)
        {
            case WeatherType.Sun: btnImage.sprite = sunColored; currentSun++; break;
            case WeatherType.Cloud: btnImage.sprite = cloudColored; currentCloud++; break;
            case WeatherType.Snow: btnImage.sprite = snowColored; currentSnow++; break;
        }
        UpdateCountTexts();

        // ★ [추가됨] 모든 카운트가 찼는지 확인
        CheckAllCompleted();
    }

    void UpdateGauges(float temp, float rain)
    {
        if (tempGaugeImage != null)
            tempGaugeImage.fillAmount = Mathf.InverseLerp(-5f, 30f, temp);

        if (rainGaugeImage != null)
            rainGaugeImage.fillAmount = Mathf.InverseLerp(0f, 500f, rain);
    }

    void UpdateCountTexts()
    {
        if (textSunCount != null) textSunCount.text = $"{currentSun} / {totalSun}";
        if (textCloudCount != null) textCloudCount.text = $"{currentCloud} / {totalCloud}";
        if (textSnowCount != null) textSnowCount.text = $"{currentSnow} / {totalSnow}";
    }

    // ★ [추가됨] 완료 체크 함수
    void CheckAllCompleted()
    {
        // 3가지 날씨 모두 현재 개수와 전체 개수가 같은지 확인
        if (currentSun == totalSun && currentCloud == totalCloud && currentSnow == totalSnow)
        {
            Debug.Log("🎉 모든 아이콘 찾기 완료!");
            if (completionImageObject != null)
            {
                completionImageObject.SetActive(true);
            }
        }
    }
}