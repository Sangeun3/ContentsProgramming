using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;
using DG.Tweening; // ★ DOTween 필수

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
    public GameObject completionImageObject;

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

                    // ★ 혹시 모를 기존 CanvasGroup 초기화
                    CanvasGroup cg = closeButtons[i].GetComponent<CanvasGroup>();
                    if (cg == null) cg = closeButtons[i].gameObject.AddComponent<CanvasGroup>();
                    cg.alpha = 1f;

                    closeButtons[i].onClick.RemoveAllListeners();
                    closeButtons[i].onClick.AddListener(() =>
                    {
                        closeButtons[index].gameObject.SetActive(false);
                        closeButtons[index].transform.DOKill();
                        if (cg != null) cg.DOKill(); // CanvasGroup 트윈도 종료
                    });
                }
            }
        }

        if (hintImageObject != null) hintImageObject.SetActive(false);
        if (completionImageObject != null) completionImageObject.SetActive(false);

        if (hintButton != null)
        {
            hintButton.onClick.RemoveAllListeners();
            hintButton.onClick.AddListener(OnHintButtonClicked);
        }

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

        // ★ [수정됨] CanvasGroup을 사용한 확실한 애니메이션 처리
        if (closeButtons != null && index < closeButtons.Length && closeButtons[index] != null)
        {
            GameObject closeObj = closeButtons[index].gameObject;

            // 1. CanvasGroup 가져오기 (없으면 추가)
            // CanvasGroup은 Button 컴포넌트의 색상 간섭을 무시하고 전체 투명도를 조절함
            CanvasGroup cg = closeObj.GetComponent<CanvasGroup>();
            if (cg == null) cg = closeObj.AddComponent<CanvasGroup>();

            // 2. 초기화
            closeObj.transform.DOKill();
            cg.DOKill();

            closeObj.SetActive(true);
            closeObj.transform.localScale = Vector3.one;
            closeObj.transform.localRotation = Quaternion.identity;

            // 투명하게 시작
            cg.alpha = 0f;

            // 3. 시퀀스 생성
            Sequence seq = DOTween.Sequence();

            // 단계 1: 페이드 인 (0.5초) - Image가 아닌 CanvasGroup의 alpha를 조절
            seq.Append(cg.DOFade(1f, 0.5f));

            // 단계 2: 랜덤 액션 (2.5초 진행)
            int randomAction = Random.Range(0, 3);
            switch (randomAction)
            {
                case 0: // 깜박이기 (Blink)
                    // CanvasGroup의 alpha를 0까지 내렸다가 올림
                    seq.Append(cg.DOFade(0f, 0.25f).SetLoops(10, LoopType.Yoyo));
                    break;
                case 1: // 한바퀴 돌기
                    seq.Join(closeObj.transform.DORotate(new Vector3(0, 0, 720), 2.5f, RotateMode.FastBeyond360));
                    break;
                case 2: // 흔들리기
                    seq.Join(closeObj.transform.DOShakePosition(2.5f, strength: 15f, vibrato: 15));
                    break;
            }

            // 단계 3: 2초 대기
            seq.AppendInterval(2.0f);

            // 단계 4: 종료
            seq.OnComplete(() =>
            {
                closeObj.SetActive(false);
            });
        }
        // ---------------------------------------------------------

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

    void CheckAllCompleted()
    {
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