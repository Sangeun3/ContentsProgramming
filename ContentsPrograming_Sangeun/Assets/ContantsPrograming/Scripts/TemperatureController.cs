using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemperatureController : MonoBehaviour
{
    [Header("UI References")]
    public Slider temperatureSlider;
    public Image thermometerFilled;  // 채워지는 온도계 이미지
    public Image weatherIcon;         // 날씨 아이콘 이미지
    public TextMeshProUGUI temperatureText;

    [Header("Temperature Settings")]
    public float minTemperature = -10f;
    public float maxTemperature = 40f;

    [Header("Weather Icons")]
    public Sprite snowIcon;      // ❄️ 눈 (영하)
    public Sprite coldIcon;      // 🌨️ 추움 (0~10도)
    public Sprite mildIcon;      // 🌤️ 선선 (10~20도)
    public Sprite warmIcon;      // ☀️ 따뜻 (20~30도)
    public Sprite hotIcon;       // 🔥 더움 (30도 이상)

    void Start()
    {
        // 초기값 적용
        OnTemperatureChanged(temperatureSlider.value);

        Debug.Log("온도 제어 시스템 초기화 완료!");
    }

    // 슬라이더 값이 변경될 때마다 호출되는 함수
    public void OnTemperatureChanged(float temperature)
    {
        // 1. 텍스트 업데이트
        temperatureText.text = temperature.ToString("F1") + "°C";

        // 2. Fill Amount 업데이트 (정규화)
        float normalizedValue = (temperature - minTemperature) / (maxTemperature - minTemperature);
        thermometerFilled.fillAmount = normalizedValue;

        // 3. 온도계 색상 변경
        UpdateThermometerColor(temperature);

        // 4. 날씨 아이콘 변경
        UpdateWeatherIcon(temperature);

        // 5. 디버그 정보
        Debug.Log($"온도: {temperature}°C, Fill: {normalizedValue:F2}");
    }

    // 온도에 따른 온도계 색상 변경
    void UpdateThermometerColor(float temperature)
    {
        if (temperature < 0f)
        {
            // 영하 - 진한 파란색
            thermometerFilled.color = new Color(0.2f, 0.5f, 1f);
        }
        else if (temperature < 10f)
        {
            // 추운 날씨 - 하늘색
            thermometerFilled.color = new Color(0.4f, 0.7f, 1f);
        }
        else if (temperature < 20f)
        {
            // 선선한 날씨 - 녹색
            thermometerFilled.color = new Color(0.3f, 1f, 0.3f);
        }
        else if (temperature < 30f)
        {
            // 따뜻한 날씨 - 주황색
            thermometerFilled.color = new Color(1f, 0.6f, 0.2f);
        }
        else
        {
            // 더운 날씨 - 빨간색
            thermometerFilled.color = new Color(1f, 0.3f, 0.3f);
        }
    }

    // 온도에 따른 날씨 아이콘 변경
    void UpdateWeatherIcon(float temperature)
    {
        if (temperature < 0f)
            weatherIcon.sprite = snowIcon;       // ❄️ 눈
        else if (temperature < 10f)
            weatherIcon.sprite = coldIcon;       // 🌨️ 추움
        else if (temperature < 20f)
            weatherIcon.sprite = mildIcon;       // 🌤️ 선선
        else if (temperature < 30f)
            weatherIcon.sprite = warmIcon;       // ☀️ 따뜻
        else
            weatherIcon.sprite = hotIcon;        // 🔥 더움
    }
}