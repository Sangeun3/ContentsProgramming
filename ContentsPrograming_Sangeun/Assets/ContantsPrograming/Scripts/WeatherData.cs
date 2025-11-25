using System;
using System.Collections.Generic;

[System.Serializable]
public class WeatherData
{
    public string name;         // 도시 이름 (data.name)
    public MainData main;       // 메인 데이터 (온도, 습도 등)
    public Weather[] weather;   // 날씨 설명 배열
}

[System.Serializable]
public class MainData
{
    public float temp;          // 온도 (data.main.temp)
    public int humidity;        // 습도 (data.main.humidity)
    // 필요한 경우 pressure(기압) 등을 추가할 수 있습니다.
}

[System.Serializable]
public class Weather
{
    public int id;
    public string main;
    public string description;  // 날씨 설명 (data.weather[0].description)
    public string icon;
}