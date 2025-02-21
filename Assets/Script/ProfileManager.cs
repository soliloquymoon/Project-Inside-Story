using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public GameObject profileCanvas;
    public GameObject characterProfile;
    public GameObject profileButtonCanvas;
    public Button[] profileButtons;
    public GameObject keywordButtonPrefab; // 키워드 버튼 프리팹
    private float spacingX = 10f; // 버튼 간 좌우 간격
    private float spacingY = 30f; // 버튼 간 상하 간격
    private float paddingX = 50f; // 좌우 여백
    private float paddingY = 80f; // 상하 여백
    private GameObject profileContent;
    private Text keywordDetailText;
    private Text profileNameText;
    private Text profileBioText;
    private KeywordManager keywordManager;

    void Start()
    {
        keywordManager = GameObject.Find("KeywordManager").GetComponent<KeywordManager>();
        profileContent = GameObject.Find("ProfileContent");
        keywordDetailText = GameObject.Find("KeywordDetailText").GetComponent<Text>();
        profileNameText = GameObject.Find("ProfileNameText").GetComponent<Text>();
        profileBioText = GameObject.Find("ProfileBioText").GetComponent<Text>();
        profileCanvas.SetActive(false);
        characterProfile.SetActive(false);
        foreach(Button ProfileButton in profileButtons)
            ProfileButton.GetComponent<Button>().image.color = new Color(1, 1, 1, 0);
    }

    public void OpenProfileWindow()
    {
        profileCanvas.SetActive(true);
        characterProfile.SetActive(false);
        profileButtonCanvas.SetActive(true);
    }

    public void AddProfileButton(int code)
    {
        profileButtons[code].image.color = new Color(1, 1, 1, 1);
    }

    public void CloseProfileWindow()
    {
        if(characterProfile.activeSelf)
        {
            characterProfile.SetActive(false);
            keywordDetailText.text = "키워드를 클릭하면 상세정보를 확인할 수 있습니다.";
            profileButtonCanvas.SetActive(true);
        }
        else
            profileCanvas.SetActive(false);
    }

    public void OpenCharacterProfile(string name)
    {
        List<Keyword> keywordList = keywordManager.GetCharacterKeywords(name);
        profileNameText.text = name;
        profileBioText.text = keywordList[0].detail[0];
        keywordList.Remove(keywordList[0]);

        profileButtonCanvas.SetActive(false);
        characterProfile.SetActive(true);
        PopulateKeywords(keywordList);
    }

    public void CloseCharacterProfile()
    {
        profileButtonCanvas.SetActive(true);
        characterProfile.SetActive(false);
    }

    private void PopulateKeywords(List<Keyword> keywords)
    {
        // 초기화
        foreach (Transform child in profileContent.transform)
        {
            Destroy(child.gameObject);
        }

        RectTransform profileContentRect = profileContent.GetComponent<RectTransform>();
        float currentX = paddingX; // 현재 X 위치
        float currentY = -paddingY; // 현재 Y 위치 (RectTransform은 Y가 위->아래로 감소)

        foreach (var keyword in keywords)
        {
            Debug.Log(keyword.keyword);
            // 키워드 버튼 생성
            GameObject button = Instantiate(keywordButtonPrefab, profileContent.transform);
            Text buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = keyword.word;

            RectTransform buttonRect = button.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRect); // 즉시 레이아웃 계산
            float buttonWidth = buttonRect.rect.width;
            float buttonHeight = buttonRect.rect.height;

            // 다음 줄로 넘어갈지 확인
            if (currentX + buttonWidth + paddingX > profileContentRect.rect.width)
            {
                currentX = paddingX;
                currentY -= paddingY + spacingY;
            }

            // 버튼 위치 설정
            buttonRect.anchoredPosition = new Vector2(currentX, currentY);
            currentX += buttonWidth + spacingX;

            button.GetComponent<Button>().onClick.AddListener(() => keywordDetailText.text = keyword.detail[0]);
        }

        // Content 크기 업데이트
        profileContentRect.sizeDelta = new Vector2(profileContentRect.rect.width, Mathf.Abs(currentY) + paddingY);
    }
}
