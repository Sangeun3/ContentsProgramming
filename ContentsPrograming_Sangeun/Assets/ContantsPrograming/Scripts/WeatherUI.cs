using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI cityNameText;
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI weatherDescText;
    public TextMeshProUGUI humidityText;

    [Header("Buttons")]
    public Button seoulButton;
    public Button busanButton;
    public Button jejuButton;

    [Header("Weather Manager")]
    public WeatherManager weatherManager;

    void Start()
    {
        // 버튼 클릭 이벤트 등록
        seoulButton.onClick.AddListener(() => OnCityButtonClicked("Seoul"));
        busanButton.onClick.AddListener(() => OnCityButtonClicked("Busan"));
        jejuButton.onClick.AddListener(() => OnCityButtonClicked("Jeju"));

        Debug.Log("✅ WeatherUI 초기화 완료!");
    }

    void OnCityButtonClicked(string cityName)
    {
        Debug.Log($"🔘 {cityName} 버튼 클릭!");

        // WeatherManager의 GetWeatherData 코루틴 시작
        StartCoroutine(weatherManager.GetWeatherData(cityName, UpdateUI));
    }

    // WeatherManager에서 호출될 콜백 함수
    public void UpdateUI(WeatherData data)
    {
        if (data == null)
        {
            Debug.LogError("❌ 날씨 데이터가 없습니다!");
            return;
        }

        // UI 업데이트
        cityNameText.text = $"도시: {data.name}";
        temperatureText.text = $"온도: {data.main.temp}°C";
        weatherDescText.text = $"날씨: {data.weather[0].description}";
        humidityText.text = $"습도: {data.main.humidity}%";

        Debug.Log($"✅ UI 업데이트 완료: {data.name}");
    }
}