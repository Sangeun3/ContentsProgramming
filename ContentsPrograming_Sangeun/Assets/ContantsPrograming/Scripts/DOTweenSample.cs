using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DOTweenSample : MonoBehaviour
{
    private bool isImportant;
    public float duration = 2f;
    public float temperature = 0.5f;
    public TextMeshProUGUI temperatureText;

    public Image slider;
    public RectTransform backgroundPanel;
    public CanvasGroup canvasGroup;
    void Start()
    {
        temperatureText.text = "Temperature: " + temperature.ToString("F1") + "C";
        DOTween.To(() => temperature, x => temperature = x, 20f, duration).OnUpdate(
            () => temperatureText.text = "Temperature: " + temperature.ToString("F1") + "C"
        );

        slider.fillAmount = 0f;
        slider.DOFillAmount(0.8f, duration).SetEase(Ease.OutQuad);

        backgroundPanel.DOAnchorPos(new Vector2(0f, 0f), duration).SetEase(Ease.Linear).OnUpdate(
            () => canvasGroup.DOFade(1f, 5f)
        ).OnComplete(
            () => slider.DOColor(new Color(0f, 0f, 1f, 1f), duration)
        );
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start() =>
    // Animator: Animation Clip 미리 제작 필요
    // DOTween: 코드 한 줄로 즉시 실행

    // transform.DOScale(3f, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutBack);  // 2초 동안 3배 크기로
    //무한반복 요요처럼, 아웃백 이지
    //transform.DOScale(3f, duration).SetLoops(-1, LoopType.Restart);  // 2초 동안 3배 크기로
    //무한반복 리스타트처럼
    //transform.DOMove(new Vector3(10f, 0f, 0f), duration).SetLoops(-1, LoopType.Yoyo);  // 2초 동안 3배 크기로
    //무한반복 리스타트처럼
    //transform.DOPunchScale(Vector3.one * 2f, duration, 5);
    //펀치

    // // 예시 1: 애니메이션 후 오브젝트 비활성화
    //         transform.DOScale(0f, 0.5f).SetDelay(1f).OnComplete(
    //             () => gameObject.SetActive(false).OnComplete(
    //                 () => transform.DOScale(3f, 2f))
    // );

    // 예시 2: 애니메이션 체인 (연속 실행)
    // 체이닝 = 메서드를 점(.)으로 연결

    // transform.DOScale(2f, 1f)
    //     .SetEase(Ease.OutBack)       // 이징 설정
    //     .SetDelay(0.5f)              // 0.5초 후 시작
    //     .OnComplete(() =>            // 완료 후 실행
    //     {
    //         Debug.Log("완료!");
    //     });


    // 조건에 따라 동적으로 변경 가능
    // if (isImportant)
    // {
    //     transform.DOScale(2f, 0.5f);  // 중요하면 더 크게, 빠르게
    // }
    // else
    // {
    //     transform.DOScale(1.2f, 1f);  // 일반은 작게, 천천히
    // }

    //➡️ 런타임에 값을 자유롭게 변경!
}
