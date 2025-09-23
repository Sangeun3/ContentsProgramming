using UnityEngine;

public class TemperatureHeight : MonoBehaviour
{
    // 온도 변수 inspector 창에서 조절 가능
    public float temperature = 25.0f; // 온도
    public float maxHeight = 5.0f; // 최대 높이
    private Transform myTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTransform = GetComponent<Transform>();

    }

    void Update()
    {

        float height = (temperature / 50.0f) * maxHeight; //온도를 높이로 변환

        myTransform.localScale = new Vector3(1, height, 1);
        Debug.Log("온도에 따른 높이 설정 완료: " + height);
    }
}
