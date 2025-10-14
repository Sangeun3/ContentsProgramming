using UnityEngine;
using TMPro;  // TextMeshPro 사용을 위해 필요

public class WeatherController : MonoBehaviour
{
    [Header("온도계 프리팹 설정")]
    public GameObject thermometerPrefab;      // 온도계 프리팹 루트
    public Transform thermometerParent;       // Thermometer_Parent (크기 조절용)
    public float currentTemperature = 25.0f;  // 현재 온도
    public float maxHeight = 2.0f;           // 최대 높이 (프리팹에 맞게 조정)

    [Header("디버깅")]
    public bool showDebugInfo = true;    // 디버그 정보 표시

    private Renderer barRenderer;       // Thermometer_Bar의 렌더러

    [Header("UI 텍스트")]
    public TextMeshProUGUI temperatureText;  // 온도 표시 텍스트

    void Start()
    {
        // Thermometer_Parent에서 Thermometer_Bar의 Renderer 가져오기
        if (thermometerParent != null)
        {
            Transform barChild = thermometerParent.Find("Thermometer_Bar");
            if (barChild != null)
            {
                barRenderer = barChild.GetComponent<Renderer>();
            }
        }

        // 초기 온도 적용
        UpdateTemperature(currentTemperature);

        if (showDebugInfo)
        {
            Debug.Log("날씨 컨트롤러 초기화 완료! 초기 온도: " + currentTemperature + "도");
        }
    }

    // 온도 업데이트 함수 (UI에서 호출할 함수)
    public void UpdateTemperature(float newTemperature)
    {
        currentTemperature = newTemperature;

        // 높이 조절
        UpdateThermometerHeight();

        // 색상 조절
        UpdateThermometerColor();

        // 온도 UI 조절
        UpdateTemperatureDisplay(newTemperature);
        if (showDebugInfo)
        {
            Debug.Log("온도 업데이트: " + currentTemperature + "도");
        }
    }

    // 높이 조절 함수 - Thermometer_Parent의 Y 스케일 조정
    private void UpdateThermometerHeight()
    {
        if (thermometerParent == null) return;

        // 온도를 높이로 변환 (0~50도 범위)
        float height = currentTemperature / 50.0f * maxHeight;
        if (height < 0.1f) height = 0.1f;  // 최소 높이 보장

        // Thermometer_Parent의 Y 스케일만 조정 (Bar 크기 조절)
        thermometerParent.localScale = new Vector3(1, height, 1);
    }

    // 색상 조절 함수 - Thermometer_Bar의 색상 변경
    private void UpdateThermometerColor()
    {
        if (barRenderer == null) return;

        if (currentTemperature < 15.0f)
        {
            // 추운 날씨 - 파란색
            barRenderer.material.color = Color.blue;
        }
        else if (currentTemperature < 30.0f)
        {
            // 적당한 날씨 - 녹색
            barRenderer.material.color = Color.green;
        }
        else
        {
            // 더운 날씨 - 빨간색
            barRenderer.material.color = Color.red;
        }
    }


    public void UpdateTemperatureDisplay(float temperature)
    {
        if (temperatureText != null)
        {
            temperatureText.text = "현재 온도: " + temperature + "°C";
        }
    }

    // UI 버튼용 온도 설정 함수들
    public void SetColdWeather()  // 추운 날씨
    {
        UpdateTemperature(10.0f);
    }

    public void SetMildWeather()  // 적당한 날씨
    {
        UpdateTemperature(20.0f);
    }

    public void SetWarmWeather()  // 따뜻한 날씨
    {
        UpdateTemperature(35.0f);
    }
}