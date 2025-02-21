using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using System.Reflection;
using Unity.VisualScripting;

public class ScenarioManager : MonoBehaviour
{
    Dictionary<string, string[]> scenarioData;

    // Dialogue Canvas //
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] Text dialogueText;
    [SerializeField] Text dialogueName;
    [SerializeField] Text monologueText;
    [SerializeField] Sprite[] portrait;
    [SerializeField] Image middleSprite;
    public int tmpIndex;
    public int currentIndex = -1;
    string currentKey = null;
    public float fadeSpeed = 5f;

    // Option Canvas
    public GameObject optionCanvas;
    public Button[] optionButton;

    // Notification Canvas
    public GameObject notificationCanvas;
    public Image notificationBox;
    public Text notificationText;
    
    // Guide Canvas
    public GameObject guideCanvas;
    public Text guideText;

    //public GameObject overlayBlack;

    // Keyword
    public KeywordManager keywordManager;
    public ProfileManager profileManager;

    void Start()
    {
        scenarioData = new Dictionary<string, string[]>();
        middleSprite.color = new Color(1, 1, 1, 0);
        GenerateData();
        //overlayBlack.SetActive(false);     
        //dialogueCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        guideCanvas.SetActive(false);
        StartScenario("2pm");
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueCanvas.activeSelf && !optionCanvas.activeSelf && !guideCanvas.activeSelf &&
            !profileManager.profileCanvas.activeSelf
            && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            StartScenario(currentKey);
        }
    }

    public void StartScenario(string key)
    {
        // 선택지 닫기
        if(optionCanvas.activeSelf)
            optionCanvas.SetActive(false);
        
        currentKey = key;

        currentIndex++;
        PrintScenario(currentIndex);
    }

    void PrintScenario(int index)
    {
        string line = scenarioData[currentKey][index];

        Debug.Log(line);

        // 대사
        if (!line.StartsWith('*'))
        {
            // "이름*대사"
            if(line.Contains('*'))
            {
                string[] inst = line.Split('*');
                PrintDialogue(inst[0], inst[1]);
            }
            // 독백 대사
            else
            {
                PrintMonologue(line);
            }
        }
        else
        {
            // "[0]*cmd명[1]*추가명령어[2]"
            string[] cmd = line.Split("*");

            // 선택지 "*option*옵션1*옵션2"
            if(cmd[1] == "option")
            {
                tmpIndex = currentIndex;
                currentIndex = -1;
                ShowOptions(cmd[2], cmd[3]);
            }

            // 키워드 획득 "*keyword*이름:키워드명"
            else if(cmd[1] == "keyword")
            {
                KeywordNotification(keywordManager.AddKeyword(cmd[2]));
                StartScenario(currentKey);
            }

            // 키워드 업데이트 "*update*이름:키워드명"
            else if(cmd[1] == "update")
            {
                KeywordNotification(keywordManager.UpdateKeyword(cmd[2]));
                StartScenario(currentKey);
            }

            // 아이템 획득 "*item*아이템명"
            else if(cmd[1] == "item")
            {
                ItemNotification(cmd[2]);
                // break) 아이템 추가
                StartScenario(currentKey);
            }

            // 가이드 표시 "*guide*가이드 텍스트"
            else if(cmd[1] == "guide")
            {
                ShowGuide(cmd[2]);
            }

            // 다음 키 이동 "*next*다음 키"
            else if(cmd[1] == "next")
            {
                currentIndex = -1;
                StartScenario(cmd[2]);
            }

            // 새 프로필 "*profile*(int code)"
            else if(cmd[1] == "profile")
            {
                profileManager.AddProfileButton(int.Parse(cmd[2]));
            }

            // optional 이후 main 복귀 "*return*리턴 키"
            else if(cmd[1] == "return")
            {
                currentIndex = tmpIndex;
                StartScenario(cmd[2]);
            }

            else if(cmd[1] == "conversation")
            {
                // break
            }
        }
    }

    void ShowGuide(string guide)
    {
        guideCanvas.SetActive(true);
        guideText.text = "<color=cyan>Guide : </color> " + guide;
    }

    public void CloseGuide()
    {
        Debug.Log("CloseGuide");
        guideCanvas.SetActive(false);
    }

    void KeywordNotification(string message)
    {
        StopCoroutine(ShowNotification());
        notificationText.text = message;
        StartCoroutine(ShowNotification());
    }

    void ItemNotification(string item)
    {
        StopCoroutine(ShowNotification());
        notificationText.text = "새로운 아이템 <color=cyan>" + item + "</color> 획득";
        StartCoroutine(ShowNotification());
    }

    IEnumerator ShowNotification()
    {
        notificationCanvas.SetActive(true);
        notificationBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
        while (notificationBox.GetComponent<RectTransform>().anchoredPosition.y > 0)
        {
            notificationBox.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(
                notificationBox.GetComponent<RectTransform>().anchoredPosition, new Vector2(0, 0), 100f * Time.deltaTime);
            yield return null;
        }
        notificationBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSecondsRealtime(1f);
        }
        while (notificationBox.GetComponent<RectTransform>().anchoredPosition.y < 100)
        {
            notificationBox.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(
                notificationBox.GetComponent<RectTransform>().anchoredPosition, new Vector2(0, 100), 100f * Time.deltaTime);
            yield return null;
        }
        notificationCanvas.SetActive(false);
    }

    void ShowOptions(string firstOption, string secondOption)
    {
        optionCanvas.SetActive(true);
        optionButton[0].GetComponentInChildren<Text>().text = firstOption;
        optionButton[1].GetComponentInChildren<Text>().text = secondOption;
        optionButton[0].onClick.AddListener(() => StartScenario(firstOption));
        optionButton[1].onClick.AddListener(() => StartScenario(secondOption));
    }

    void PrintDialogue(string name, string line)
    {
        dialogueCanvas.SetActive(true);
        if(middleSprite.color.a < 1)
        {
            StartCoroutine(ImageFadeIn(middleSprite));
        }
        middleSprite.color = new Color(1, 1, 1, 1);
        dialogueText.text = line;
        if (name == "서연오")
        {
            dialogueName.text = "<color=#d48585>서연오</color>";
            middleSprite.sprite = portrait[0];
        }
        else if(name == "강재현")
        {
            dialogueName.text = "<color=#8ab8a7>강재현</color>";
            middleSprite.sprite = portrait[1];
        }
        else if(name == "하시윤")
        {
            dialogueName.text = "<color=#e3a1bf>하시윤</color>";
            middleSprite.sprite = portrait[2];
        }
        else if(name == "백세인")
        {
            dialogueName.text = "<color=#b885d4>백세인</color>";
            middleSprite.sprite = portrait[3];
        }
        else if(name == "도영하")
        {
            dialogueName.text = "<color=#a9add6>도영하</color>";
            middleSprite.sprite = portrait[4];
        }
        else if (name == "홍여진")
        {
            dialogueName.text = "<color=#d4ce85>홍여진</color>";
            middleSprite.sprite = portrait[5];
        }
        else if (name == "유하람")
        {
            dialogueName.text = "<color=#b9e5f0>유하람</color>";
            middleSprite.sprite = portrait[6];
        }
        else if (name == "조강희")
        {
            dialogueName.text = "<color=#b8a08a>조강희</color>";
            middleSprite.sprite = portrait[7];
        }
        else
        {
            dialogueName.text = name;
            middleSprite.color = Color.gray;
        }
        monologueText.text = "";
    }

    IEnumerator ImageFadeIn(Image img)
    {
        while (img.color.a < 1)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + (Time.deltaTime * fadeSpeed));
            yield return null;
        }
    }
    IEnumerator ImageFadeOut(Image img)
    {
        while (img.color.a > 0)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime * fadeSpeed));
            yield return null;
        }
    }

    void PrintMonologue(string line)
    {
        dialogueCanvas.SetActive(true);
        monologueText.text = line;
        if(middleSprite.color.a > 0)
        {
            middleSprite.color = Color.gray;
        }
        dialogueName.text = "";
        dialogueText.text = "";
    }

    void GenerateData()
    {
        scenarioData.Add("2pm", new string[] {
            "서연오*이런 곳에서 파티를 하는구나…",
            "얼마 전까지만 해도 이런 파티에 참석하는 건 상상도 못했던 일이다.",
            "수많은 무명 싱어송라이터 중 하나에 지나지 않았으니까.",
            "*keyword*서연오:프로필",
            "*profile*0",
            "*guide*오른쪽의 인물 아이콘을 클릭하면 인물 정보를 확인할 수 있습니다.",
            "소규모 무대는 많이 가봤어도, 고급 호텔이라니.",
            "기대보다는 역시 낯선 장소에 대한 불안부터 앞선다.",
            "*keyword*서연오:아웃사이더",
            "*guide*스토리를 진행하며 얻은 키워드는 인물 정보에 기록됩니다.",
            "잠시 호텔 건물을 구경하다가 안으로 들어갔다.",
            "로비와 2층을 지나자 초대장에 적힌 룸이 보인다.",
            "직원*초대장 확인하겠습니다.",
            "서연오*아, 네. 여기…",
            "초대장을 꺼내려던 순간, 직원의 어깨 너머로 익숙한 얼굴이 나타났다.",
            "강재현*연오 씨 왔구나! 기다리고 있었어요.",
            "서연오*아, 재현 씨. 오랜만이에요.",
            "강재현*급하게 불렀는데 와줘서 고마워요. 얼른 들어와요.",
            "이 사람은 <color=cyan>강재현</color>, 오늘 열린 파티의 주최자이자 손에 꼽을 만큼 적은 연예계 인맥 중 한 명이다.",
            "비교적 어린 나이에도 타고난 입담과 재치로 차세대 국민 MC로 주목받고 있는 인물이다.",
            "*keyword*강재현:프로필",
            "*profile*1",
            "들어오라며 손짓하는 강재현에게 이끌려 안쪽으로 향하자, 파티장 안은 이미 사람들로 북적이고 있다.",
            "배우, 가수, 아이돌, 방송인…",
            "하나같이 <color=yellow>유명인</color>들로 가득하다.",
            "무슨 별세계도 아니고…",
            "그런 생각을 읽었는지, 강재현이 웃으며 말을 걸었다.",
            "강재현*아는 사람이 없어서 영 어색하죠?",
            "강재현*이따 별장으로 이동한 후에는 인원이 적으니까 소개는 그때 천천히 시켜줄게요.",
            "강재현*일단은 여기 있는 음식이랑 음료부터 즐겨요. 이따 무대도 구경하고!",
            "강재현*대부분 외향적인 사람들이라 친해지기 어렵진 않을 거예요.",
            "서연오*네, 괜찮아요. 신경 써주셔서 감사해요.",
            "강재현은 가볍게 인사를 나누고 돌아가 다시 사람들 틈에 섞였다.",
            "주최자라 그런지, 모두와 인사를 나누느라 바빠 보인다.",
            "수많은 사람들과 반갑게 인사를 하는 모습에 새삼 거리감이 느껴진다.",
            "괜찮다고 말은 했지만… 역시 영 어색하다.",
            "이런 자리도, 사람들도, 전부 낯선 느낌.",
            "왜인지 내가 있으면 안 될 것 같은 자리처럼 느껴진다.",
            "서연오*방송인들은 왜 다들 저렇게 텐션이 좋은 거야…",
            "어색하게 주변을 둘러보는데, 수많은 사람들 사이로 낯익은 얼굴이 하나 보인다.",
            "눈이 마주치자, 상대방도 놀란 얼굴을 하고 다가왔다.",
            "하시윤*연오 언니!",
            "서연오*<color=cyan>시윤</color>아, 너도 왔어?",
            "하시윤*그럼요, 재현 오빠 초대인데 와야죠.",
            "*keyword*하시윤:프로필",
            "*profile*2",
            "하시윤*그나저나 오랜만이네요. 우리 <color=yellow>탑 싱어</color> 끝나고 처음 보는 거죠?",
            "서연오*요새 바쁘지 않아? 어딜 가도 너희 노래만 나오던데.",
            "서연오*오늘도 다른 스케줄 있을 줄 알았어.",
            "하시윤*아니에요. 저희 활동 끝난 지 얼마 안 돼서 한가해요.",
            "하시윤*그러는 언니야말로, 요새 카페 가면 언니 노래 자주 들리던데요.",
            "서연오*너네에 비하면 멀었지. 이번에 음방 1위한 거 축하해.",
            "하시윤*고마워요, 언니. 앞으로 열심히 해야죠.",
            "하시윤*휴, 그나저나 언니라도 있어서 다행이다…",
            "하시윤*이런 자리 처음이라 어색했거든요.",
            "서연오*사실 나도 그래. 나야 이런 데 올 일이 없었으니까…",
            "하시윤*저도 워낙 신인이다 보니까 너무 어색했던 거 있죠.",
            "그래도 친한 사람을 만나니 어색함이 완화되는 기분이다.",
            "거기다 혼자 외부인 같은 기분을 느끼는 게 아니라니… 묘한 <color=yellow>동질감</color>이 든다.",
            "하시윤*참, 언니도 이따 <color=cyan>애프터 파티</color> 같이 가죠?",
            "강재현이 빌린 별장에서 진행되는 애프터 파티 이야기다.",
            "초대를 받긴 했지만, 초면인 사람들과 함께 며칠을 지낼 걱정에 고민 중이었는데…",
            "*keyword*스토리:애프터 파티",
            "*guide*오른쪽의 책 아이콘을 클릭하면 스토리 키워드를 확인할 수 있습니다.",
            "서연오*아, 그게…",
            "*option*당연하지.*초대를 받긴 했는데…",
            "서연오*안 그래도 아는 연예인이라고는 <color=yellow>탑 싱어</color>에서 만난 사람들밖에 없는데, 가서 얼굴이라도 좀 익혀 둬야지.",
            "하시윤*하긴, 저도 이렇게 사람 많이 모인 곳보다는 몇 명이서 소규모로 만나는 게 친해지기 더 쉬운 것 같아요.",
            "서연오*그래도 너는 방송 다니면서 친해진 사람들 꽤 있지 않아?",
            "하시윤*음… 여기저기 인사는 많이 드렸는데, 사적으로 친해질 일은 없어서요.",
            "하시윤*언니도 알잖아요, 저 낯 많이 가리는 거.",
            "서연오*너나 나나 새로운 사람 사귀는 거 어려워하는 타입이긴 하지…",
            "하시윤*아, 저기 공연 시작한다.",
            "하시윤*같이 앉아서 봐요, 언니.",
            "*fadeout",
            "*fadein",
            "파티가 끝나자, 파티장을 가득 채웠던 사람들은 하나둘씩 떠나기 시작했다.",
            "어수선했던 분위기가 어느 정도 정리되고, 호텔 직원들이 분주하게 남은 자리를 정돈하고 있다.",
            "별장 애프터파티는 열 명 내외라고 했으니까…",
            "여기 남은 사람들이 애프터파티 참석하는 사람들인가?",
            "남은 인원을 세보니 총 여덟 명이다.",
            "TV에서 자주 보던 사람도, 그렇지 않은 사람도 있다.",
            "강재현*자자, 주목!",
            "강재현이 박수를 두 번 치며 시선을 집중시켰다.",
            "강재현*우선 참석해주셔서 감사해요.",
            "강재현*이제 여기 남은 인원은 별장으로 이동할 거예요.",
            "강재현*여객선은 미리 예약해뒀고, 항구까지 멀진 않으니 각자 편하신 대로 이동하죠.",
            "강재현*이동편이 마땅치 않으면 저랑 같이 이동하시면 되고요.",
            "강재현*그럼, 이따 항구에서 봐요!",
            "강재현이 말을 마치자 모였던 사람들이 각자 흩어졌다.",
            "어떻게 갈지 고민하던 중, 하시윤이 다가왔다.",
            "하시윤*언니, 어떻게 갈 생각이에요?",
            "서연오*어, 글쎄… 계획은 따로 없어.",
            "하시윤*그럼 저랑 같이 갈래요?",
            "이야기를 듣고 있던 강재현도 대화에 끼어들었다.",
            "강재현*아니면 나랑 같이 가도 되고.",
            "강재현*연오 씨 편한대로 해요.",
            "하시윤*어떻게 할래요?",
            "*option*강재현과 함께 간다.*하시윤과 함께 간다.*혼자 택시를 탄다."
        });
    
    scenarioData.Add("당연하지.", new string[] {
            "서연오*당연하지.",
            "하시윤*다행이다. 언니까지 없었으면 큰일날 뻔했어요.",
            "하시윤*너무 어색할 것 같아서 걱정했거든요…",
            "*return*2pm"
        });

        scenarioData.Add("초대를 받긴 했는데…", new string[] {
            "서연오*초대를 받긴 했는데…",
            "하시윤*설마 안 갈 생각이에요?",
            "…원래도 갈 생각이긴 했지만, 울상이 된 하시윤의 얼굴을 보니 안 간다는 말은 꺼낼 수도 없게 되어버렸다.",
            "서연오*가야지. 이왕 초대받은 건데, 거절하기도 그렇고.",
            "하시윤*다행이다. 언니까지 없으면 저 어색해서 못 있어요.",
            "*return*2pm"
        });

        scenarioData.Add("강재현과 함께 간다.", new string[] {
            "서연오*재현 씨랑 갈게.",
            "강재현*좋아요, 얼른 출발하죠!",
            "…",
            "강재현의 차 조수석에 탔다.",
            "강재현*<탑 싱어> 이후로 좀 어때요?",
            "서연오*아직 적응 중이에요.",
            "서연오*사람들이 알아보는 거나, SNS 팔로워 수 같은 게 실감이 안 나서…",
            "강재현*대중들이 연오 씨 매력을 알아본 거죠.",
            "강재현*나도 연오 씨 노래 잘 듣고 있어요.",
            "강재현*앨범 수록곡도 하나하나 명곡이더라고.",
            "서연오*아, 감사해요.",
            "강재현*사실 나는 연오 씨가 우승할 줄 알았어요.",
            "강재현*정말 아쉬웠죠. 상대가 마지막에 너무 찰떡인 곡을 가져오는 바람에…",
            "강재현*그래도 연오 씨 무대 영상이 조회수는 더 높잖아요.",
            "강재현*차트 순위도 더 높았던 것 같은데?",
            "서연오*하하… 운이 좋았던 거죠.",
            "이 말은 진심이다. 우승에 미련도 없다.",
            "오디션 프로그램에 참가한 것도 단순히 인지도를 높이려고 참가한 거니까.",
            "준우승도 충분히 기대 이상의 성과다.",
            "강재현*참, 혹시 오늘 애프터 오는 사람들 중에 아는 사람 있어요?",
            "서연오*어… 사실 누가 오는지 잘 몰라서…",
            "서연오*아마 시윤이 정도만 알 것 같아요.",
            "서연오*제가 원래 TV를 잘 안 봐서… 계신 분들도 이름이랑 얼굴 정도밖에 모를 것 같네요.",
            "강재현*하하, 연오 씨 은근히 <color=cyan>문외한</color>이구나?",
            "*keyword*서연오:문외한",
            "강재현*가서 친해지면 되니까 걱정 말아요.",
            "강재현*참, <color=yellow>탑 싱어</color> 끝나고 나서…",
            "… …",
            "강재현과 대화를 하다 보니 어느새 항구에 도착했다.",
            "아무래도 우리가 마지막으로 도착한 모양이다.",
            "강재현: 어라, 우리가 마지막이네. 다들 오래 기다렸어요?",
            "*next*6pm"
        });
    
        scenarioData.Add("하시윤과 함께 간다.", new string[] {
            "서연오*시윤이랑 갈게요.",
            "하시윤*매니저 오빠가 주차장에서 대기 중이니까 얼른 출발해요.",
            "…",
            "하시윤을 따라 차 뒷좌석에 탔다.",
            "하시윤*<탑 싱어> 이후로 바쁘죠?",
            "서연오*앨범 준비하느라 정신없긴 했지.",
            "서연오*소속사에 들어간 것도 처음이고, 앨범도 처음이니까…",
            "하시윤*저도 언니 앨범 잘 듣고 있어요.",
            "하시윤*거의 계약하자마자 앨범 낸 거 아녜요? 그 짧은 시간에 곡 작업은 어떻게 다 했어요?",
            "서연오*아, 예전에 써뒀던 곡들 중에 골라서 낸 거야.",
            "하시윤*수록곡 하나하나 다 좋더라고요.",
            "하시윤*언니 음색 진짜 좋은 거 알죠?",
            "하시윤*사실 나는 <탑 싱어>도 언니가 우승할 줄 알았거든요.",
            "하시윤*근데 상대가 마지막에 선곡을 너무 잘 해서 아쉬웠어요.",
            "하시윤*그래도 언니 노래가 조회수나 차트 순위는 더 높았잖아요.",
            "서연오*그냥 운이 좋았던 거라고 생각해.",
            "이 말은 진심이다. 우승에 미련도 없다.",
            "오디션 프로그램에 참가한 것도 단순히 인지도를 높이려고 참가한 거니까.",
            "준우승도 충분히 기대 이상의 성과다.",
            "하시윤*에이, 운도 실력이에요. 게다가 대중은 거짓말을 안 한다구요.",
            "하시윤*참, 오늘 애프터 가는 사람들 중에 아는 사람 있어요?",
            "서연오*사실 누가 오는지 잘 몰라서…",
            "하시윤*언니도 은근 <color=cyan>문외한</color>이네요.",
            "*keyword*서연오:문외한",
            "서연오*너는 어때? 아는 사람 있어?",
            "하시윤*있기는 한데… 아까도 얘기했지만 딱히 사적으로 친한 사람은 없어요.",
            "하시윤*그나마 친한 사람이…",
            "… …",
            "하시윤과 대화를 하다 보니 어느새 항구에 도착했다.",
            "아무래도 우리가 마지막으로 도착한 모양이다.",
            "강재현*거기 지각생들, 빨리 오세요~",
            "*next*6pm"
        });

        //* 6pm 시나리오    
        scenarioData.Add("6pm", new string[] {
            "섬에 다다른 후, 꽤나 근사한 별장에 도착했다.",
            "각자 방에 들어가서 간단하게 짐을 정리한 뒤 거실로 모였다.",
            "강재현: 자, 그럼 간단하게 자기소개부터 시작할까요?",
            "*keyword*스토리:자기소개",
            "강재현*대부분 얼굴은 아는 사이겠지만, 아이스 브레이킹 겸!",
            "강재현*저부터 할까요?",
            "강재현*오늘의 스페셜 MC, 강재현입니다.",
            "강재현*배우 겸 방송인으로 활동하고 있습니다.",
            "강재현*오늘 파티의 주최자이기도 하고요.",
            "모인 사람들이 가볍게 박수를 쳤다.",
            "강재현*다들 저에 대해서는 웬만큼 아실 테니까 이 정도만 소개해도 되겠죠?",
            "강재현*그럼 세인이가 시작하는 게 어때?",
            "강재현에게 지목당한 사람이 못 말린다는 표정으로 고개를 내저었다.",
            "백세인*나 시킬 줄 알았다니까?",
            "백세인*안녕하세요, 백세인입니다. 모델 일 하고 있어요.",
            "*keyword*백세인:프로필",
            "*profile*3",
            "광고에서 자주 봤는데, 역시 화면보다 실물이 훨씬 낫구나…",
            "강재현*패션쇼 끝나자마자 귀국해서 피곤하지?",
            "백세인*에이, 비행기 하루 이틀 타는 것도 아니고. 오빠가 부른 건데 당연히 와야지.",
            "강재현*자, 그럼 다음은… 오른쪽 방향으로 쭉 소개하면 되겠네요.",
            "하시윤*아, 다음은 저인가요?",
            "하시윤*아이돌 그룹 프론티어의 세아입니다. 본명은 하시윤이고요, 편하신 쪽으로 불러주세요.",
            "하시윤의 짧은 자기소개가 끝나자, 옆에 앉아있던 남자가 입을 뗐다.",
            "도영하*안녕하세요, 프론티어 매니저 도영하라고 합니다.",
            "*keyword*도영하:프로필",
            "*profile*4",
            "하시윤*아무래도 신인이다 보니 회사에서 그냥 보내주지는 않아서…",
            "하시윤은 멋쩍게 덧붙였다.",
            "강재현*원래 신인 아이돌이면 그렇지. 괜찮으니까 영하 씨도 부담 갖지 말고 편하게 놀다가 가요.",
            "도영하*이해해주셔서 감사합니다.",
            "자연스럽게 도영하의 옆에 앉아있던 남자가 자기소개를 이어갔다.",
            "유하람*알루어의 유하람입니다.",
            "유하람*파티는 자주 가봤어도, 이런 자리는 또 처음이네요.",
            "*keyword*유하람:프로필",
            "*profile*5",
            "여기 유하람이 온다고 얘기했을 때 알루어의 팬인 친구가 난리를 쳤던 기억이 있다.",
            "…이따 연락이나 해볼까?",
            "홍여진*…저는 홍여진이라고 해요.",
            "홍여진*하람 씨 개인활동 매니저를 맡고 있어요.",
            "*keyword*홍여진:프로필",
            "*profile*6",
            "여기도 매니저가 대동했구나.",
            "연차가 쌓여도 역시 아이돌은 사생활 관리가 우선인 모양이다.",
            "조강희*다음은 저인가요?",
            "조강희*배우 조강희입니다.",
            "조강희*젊은 친구들 사이에 낀 것 같아서 조금 민망하군요.",
            "강재현*에이, 그런 말씀 마세요. 선배 모르는 사람이 어디 있겠어요.",
            "강재현의 말대로다. 대한민국에 살면서 조강희를 모를 수가 있나…",
            "*keyword*조강희:프로필",
            "*profile*7",
            "특히 조강희와 강재현과의 관계는 나도 알 정도로 유명하다.",
            "강재현이 인터뷰 때마다 롤모델로 언급하는 존재니까.",
            "*keyword*조강희:롤모델",
            "강재현이 매번 롤모델로 언급하는 인물이다.",
            "강재현* 마지막으로, 연오 씨도 자기소개 해줄래요?",
            "… …",
            "…이 유명인들 사이에서 무슨 얘기를 해야 하지?",
            "잠시 어색한 침묵이 흘렀다.",
            "*option*간단하게 소개한다.*자세하게 소개한다.",
            "서연오*모쪼록 잘 부탁드립니다.",
            "자기소개가 끝나고, 서로 짧게 인사를 나누었다.",
            "다소 서먹해하는 눈치긴 했지만, 다들 같은 분야에 종사하다 보니 금세 친해질 수 있을 것 같은 분위기였다.",
            "강재현*그럼 저녁시간 전까지 각자 자유롭게 시간 보내는 걸로 할까요?",
            "강재현*서로 친해지는 시간 가지시면 더 좋고요.",
            "*conversation*6pm"
        });

        scenarioData.Add("간단하게 소개한다.", new string[] {
            "서연오*서연오입니다.",
            "서연오*싱어송라이터고요, 올해 초에 <color=yellow>탑 싱어</color>에 출연했어요.",
            "*return*6pm"
        });

        scenarioData.Add("자세하게 소개한다.", new string[] {
            "서연오*서연오입니다.",
            "서연오*싱어송라이터 일을 하고 있어요.",
            "서연오*올해 초에 오디션 프로그램 <color=yellow>탑 싱어</color>에 출연해서 준우승을 했습니다.",
            "서연오*이전에는 밴드에서 보컬로 활동했고요.",
            "*return*6pm"
        });
    }
}
