using UnityEngine;

public class TemperatureColor : MonoBehaviour
{
    public float temperature = 20.0f; //온도
    public Color coldColor = Color.blue; // 차가운 색상
    public Color normalColor = Color.green; // 보통 색상
    public Color hotColor = Color.red; // 뜨거운 색상
    private Renderer myRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Renderer 컴포넌트 가져오기
        myRenderer = GetComponent<Renderer>();
    }
    // Update is called once per frame
    void Update()
    {
        // 온도에 따라 색상 결정
        if (temperature < 15.0f)
        {
            myRenderer.material.color = coldColor;
            Debug.Log(temperature + "도: 차가워요! (파란색)");
        }
        else if (temperature < 30.0f)
        {
            myRenderer.material.color = normalColor;
            Debug.Log(temperature + "도: 적당해요! (녹색)");
        }
        else
        {
            myRenderer.material.color = hotColor;
            Debug.Log(temperature + "도: 뜨거워요! (빨간색)");
        }
    }
}
