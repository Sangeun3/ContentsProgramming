using System; // Action 사용을 위해 추가!
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeatherManager : MonoBehaviour
{
    private const string BASE_URL = "https://api.openweathermap.org/data/2.5/weather";
    public string API_KEY = ""; // Inspector에서 입력!

    // 콜백 함수를 매개변수로 받도록 수정!
    public IEnumerator GetWeatherData(string cityName, Action<WeatherData> onSuccess)
    {
        string url = $"{BASE_URL}?q={cityName}&appid={API_KEY}&units=metric&lang=kr";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ API 호출 실패: " + request.error);
                onSuccess?.Invoke(null);  // 실패 시 null 전달
                yield break;
            }

            string jsonResponse = request.downloadHandler.text;
            WeatherData weatherData = JsonUtility.FromJson<WeatherData>(jsonResponse);

            Debug.Log($"✅ API 호출 성공: {weatherData.name}");

            // 콜백 함수 호출 (WeatherUI.UpdateUI)
            onSuccess?.Invoke(weatherData);
        }
    }
}