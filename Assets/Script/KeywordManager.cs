using System;
using System.Collections;
using  System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeywordManager : MonoBehaviour
{

    public Dictionary<string, Keyword> keywordData;
    public List<Keyword> obtainedKeywords;

    void Start()
    {
        keywordData = new Dictionary<string, Keyword>();
        GenerateData();
        obtainedKeywords = new List<Keyword>();
    }

    public string AddKeyword(string keyword)
    {
        if(obtainedKeywords.Exists(key => key.keyword.Equals(keyword)))
            return "이미 획득한 키워드입니다.";
        else
        {
            obtainedKeywords.Add(keywordData[keyword]);
            return "새로운 키워드 <color=cyan>" + keyword + "</color> 획득";
        }
    }

    public string UpdateKeyword(string keyword)
    {
        obtainedKeywords.Find(key => key.keyword.Equals(keyword)).detailIdx++;
        return "키워드 <color=cyan>" + keyword + "</color> 업데이트";
    }

    void GenerateDataHelper(Keyword keyword)
    {
        keywordData.Add(keyword.keyword, keyword);
    }

    public List<Keyword> GetCharacterKeywords(string characterName)
    {
        List<Keyword> keywordList = new List<Keyword>();
        foreach(Keyword keyword in obtainedKeywords)
        {
            if(keyword.characterName.Equals(characterName))
            {
                keywordList.Add(keyword);
            }
        }
        return keywordList;
    }

    private void GenerateData()
    {
        GenerateDataHelper(new Keyword("스토리:애프터 파티", "연말 파티의 연장선으로 섬 별장에서 3박 4일로 속규모 애프터 파티가 진행될 예정이다."));
        GenerateDataHelper(new Keyword("스토리:자기소개", "별장에서 함께 애프터 파티를 즐길 8명. 스페셜 MC를 자처한 강재현의 주도 하에 자기소개가 진행되었다."));
        GenerateDataHelper(new Keyword("스토리:와이파이", "별장의 와이파이에는 비밀번호가 걸려있다. 강재현에게 물어보자.*별장 와이파이의 비밀번호는 '12345678'이다."));
        GenerateDataHelper(new Keyword("스토리:사라진 MP3", "유하람의 장난에 놀라 작곡용 MP3를 잃어버리고 말았다."));
        GenerateDataHelper(new Keyword("스토리:기상 악화", "새벽에 폭풍우가 온다는 소식을 받았다. 덕분에 여행의 실외 일정이 모조리 취소되었다. 집순이, 집돌이들에게는 행운일지도."));
        GenerateDataHelper(new Keyword("스토리:러버덕후", "이유현의 추천으로 연예계 정보가 모이는 익명 커뮤니티 '러버덕후'를 설치했다."));
        GenerateDataHelper(new Keyword("스토리:고립", "폭풍우 때문에 경찰이 접근할 수도 없고, 당장 서울로 돌아갈 방법도 없다."));
        GenerateDataHelper(new Keyword("스토리:내부자", "범인은 우리 중에 있다."));
        GenerateDataHelper(new Keyword("스토리:신호 불량", "네트워크와 전화, 문자까지 전부 끊긴 상태가 되어버렸다."));

        // 서연오 키워드
        GenerateDataHelper(new Keyword("서연오:프로필", "음악 오디션 프로그램 '탑 싱어'에 출연하여 이름을 알린 싱어송라이터."));
        GenerateDataHelper(new Keyword("서연오:아웃사이더", "무명 싱어송라이터에서 한순간에 유명세를 얻게 되어 아직 연예계의 분위기에 익숙하지 않다."));
        GenerateDataHelper(new Keyword("서연오:탑 싱어", "음악 오디션 프로그램 '탑 싱어'에 출연하여 이름을 알렸다."));
        GenerateDataHelper(new Keyword("서연오:문외한", "평소 TV를 자주 보지 않아 연예인에 대한 정보가 전혀 없다."));
        GenerateDataHelper(new Keyword("서연오:밴드부원", "미국에서 고등학교를 다니던 시절 밴드부에 합류하며 처음 음악을 시작했다."));
        GenerateDataHelper(new Keyword("서연오:미국 국적", "미국에서 태어나 미국 국적을 가지고 있지만, 고등학교 시절을 제외하고는 줄곧 한국에서 지냈기 때문에 영어를 잘 하지는 않는다."));
        GenerateDataHelper(new Keyword("서연오:블루워터 하이스쿨", "서연오의 모교인 고등학교이다."));
        GenerateDataHelper(new Keyword("서연오:애스터", "한국에 돌아온 뒤 애스터(Aster)라는 밴드로 활동했으나 현재는 해체된 상태다.*비록 멤버가 교체되었으나 고등학교 시절 밴드부 이름이었던 애스터를 그대로 활용하여 한국에서도 활동했다."));
        GenerateDataHelper(new Keyword("서연오:한인 학교", "미국에 위치한 한인 고등학교를 졸업했으며, 재학 당시 밴드부를 모아 애스터(Aster)라는 이름으로 활동했다."));
        GenerateDataHelper(new Keyword("서연오:박민오", "고등학교 밴드부 ‘애스터’ 활동 당시 기타리스트는 같은 고등학교에 다니는 박민오였다.*한국으로 돌아와 ‘애스터’ 활동을 할 때도 박민오와 함께 활동했다."));

        // 강재현 키워드
        GenerateDataHelper(new Keyword("강재현:프로필", "배우 출신의 방송인. 여러 방송에 고정으로 출연하며 유망한 MC로 주목받고 있다."));
        GenerateDataHelper(new Keyword("강재현:조언", "연예계에서 거짓말은 당연한 생존 수단으로 여겨진다는 조언을 했다."));
        GenerateDataHelper(new Keyword("강재현:터닝포인트", "우연히 참석한 조강희의 연설이 인생의 터닝포인트가 되었다고 한다."));
        GenerateDataHelper(new Keyword("강재현:다정보스", "방송계 스탭과 출연진 사이에서 친절한 성격으로 널리 알려져 있다."));
        GenerateDataHelper(new Keyword("강재현:불화", "조강희의 방에서 굳은 얼굴로 나오는 것을 목격했다."));
        GenerateDataHelper(new Keyword("강재현:배우 데뷔", "조강희의 추천으로 드라마 엑스트라 역을 맡아 배우로 데뷔했다."));
        GenerateDataHelper(new Keyword("강재현:금오 엔터", "금오 엔터테인먼트 소속이다."));
        GenerateDataHelper(new Keyword("강재현:승진의 아이콘", "처음 엑스트라 역으로 데뷔한 후 다양한 주조연 역할에 캐스팅되며 경력을 쌓았다."));
        GenerateDataHelper(new Keyword("강재현:선", "조강희의 불화에 대해 묻자 단호하게 선을 그었다."));
        GenerateDataHelper(new Keyword("강재현:죄책감", "자신이 주최한 모임에서 생긴 사건에 죄책감이 든다고 말했다."));
        GenerateDataHelper(new Keyword("강재현:부탁", "조강희와 있었던 불화를 사람들에게 비밀로 해달라고 부탁했다."));

        // 하시윤
        GenerateDataHelper(new Keyword("하시윤:프로필", "신인 5인조 걸그룹 FRONTIER의 메인 보컬이자 비주얼 멤버."));
        GenerateDataHelper(new Keyword("하시윤:팀메이트", "'탑 싱어'에서 서연오와 같은 팀이 되어 압도적인 표 차이로 상대팀을 이겼다."));
        GenerateDataHelper(new Keyword("하시윤:실력파", "개인활동으로 <탑 싱어>에 출연해 보컬 실력을 증명하고 실력파로 인정받았다."));
        GenerateDataHelper(new Keyword("하시윤:중소기업의 기적", "FRONTIER은 팬덤 내에서 ‘중소기업의 기적’이라고 불린다고 한다. 무슨 의미일까?*FRONTIER은 중소기업의 첫 데뷔 그룹으로 이례적인 성공을 거두어 ‘중소기업의 기적’이라고 불린다."));
        GenerateDataHelper(new Keyword("하시윤:천상 아이돌", "회사에서 전혀 걱정하지 않을만큼 인성을 갖춘, 그야말로 천상 아이돌이라고 한다."));
        GenerateDataHelper(new Keyword("하시윤:일본어 능력자", "일본에서 고등학교를 나와 일본어를 유창하게 한다고 한다."));
        GenerateDataHelper(new Keyword("하시윤:화월 엔터", "화월 엔터테인먼트 소속이다."));
        GenerateDataHelper(new Keyword("하시윤:소녀가장", "팀의 인기에 가장 큰 역할을 하고 있다.*팬들 사이에서도 팀을 캐리하고 있다는 평이 대다수이다."));
        GenerateDataHelper(new Keyword("하시윤:안티팬", "익명 커뮤니티 러버덕후에서 꾸준히 활동하는 하시윤의 안티팬 계정이 있다."));
        GenerateDataHelper(new Keyword("하시윤:단기 연습생", "6개월 가량의 짧은 연습생 기간을 거치고 곧바로 데뷔했다고 한다."));

        // 백세인
        GenerateDataHelper(new Keyword("백세인:프로필", "패션 매거진, 광고, 패션쇼 등 패션계 전반에서 섭외 1순위로 꼽히는 유명 모델."));
        GenerateDataHelper(new Keyword("백세인:성덕", "유하람의 팬임을 밝히며 스스로를 성덕이라고 칭했다."));
        GenerateDataHelper(new Keyword("백세인:뉴블랙 에이전시", "뉴블랙 에이전시 소속이다."));
        GenerateDataHelper(new Keyword("백세인:진성 덕후", "Allure 팬들 사이에서도 널리 알려진 유하람의 팬이다."));
        GenerateDataHelper(new Keyword("백세인:기절", "죽어있는 조강희를 목격하고 충격을 받아 쓰러졌다."));
        GenerateDataHelper(new Keyword("백세인:쎈언니", "방송에서는 쎈언니 캐릭터, 사이다 캐릭터로 활약하고 있으나, 유하람은 그녀의 실제 성격이 매우 여린 것 같다고 말했다."));

        // 도영하
        GenerateDataHelper(new Keyword("도영하:프로필", "아이돌 그룹 FRONTIER의 매니저."));
        GenerateDataHelper(new Keyword("도영하:츤데레", "무뚝뚝한 인상과 달리 세심하게 멤버들을 챙겨준다고 한다."));
        GenerateDataHelper(new Keyword("도영하:중소기업", "도영하와 하시윤이 소속된 곳은 중소기업으로, 프론티어가 처음으로 런칭한 아이돌 그룹이다."));
        GenerateDataHelper(new Keyword("도영하:충성심", "회사의 입장을 먼저 생각하는 충성스러운 직원의 입장을 고수했다."));

        // 유하람
        GenerateDataHelper(new Keyword("유하람:프로필", "6년째 꾸준한 인기를 누리고 있는 6인조 남자 아이돌 그룹 Allure의 멤버."));
        GenerateDataHelper(new Keyword("유하람:루머", "붙임성이 좋은 탓에 이상한 루머에 휘말린다고 한다."));
        GenerateDataHelper(new Keyword("유하람:진입장벽", "위계질서가 확고한 아이돌계에서 후배들에게 서슴없이 다가가는 편이라고 한다. 후배들 입장에서는 최고의 선배인 듯하다."));
        GenerateDataHelper(new Keyword("유하람:반항아", "회사 말을 잘 듣지 않는다는 홍여진의 증언이 있었다. 하지만 성격이 나쁜 것은 아니라고 한다."));
        GenerateDataHelper(new Keyword("유하람:미필", "아직 군대를 다녀오지 않아 공백기를 걱정 중이다."));
        GenerateDataHelper(new Keyword("유하람:귀농", "은퇴 후 시골로 내려가 토끼를 키우고 싶다고 말했다. 이름도 정해두었다고 한다.*자체 컨텐츠 영상에서 토끼 이름을 ‘마루’로 할 것이라 밝혔다."));
        GenerateDataHelper(new Keyword("유하람:단속", "소속사에서 열애설을 방지하기 위해 온 신경을 기울이는 것 같다."));
        GenerateDataHelper(new Keyword("유하람:여미새", "데뷔 때부터 지금까지 열애설이 끊이지 않았으며, 팬들 사이에서도 여미새라는 말이 나온다고 한다.*데뷔하는 여자 아이돌 그룹들을 유심히 보는 모습이 자주 포착되었다."));
        GenerateDataHelper(new Keyword("유하람:모태솔로설", "수많은 열애설과는 별개로, 데뷔 이후 연애를 하는 정황이 포착된 적은 없다고 한다."));
        GenerateDataHelper(new Keyword("유하람:솔라라이즈 엔터", "솔라라이즈 엔터테인먼트 소속이다."));
        GenerateDataHelper(new Keyword("유하람:탈덕한 팬", "수많은 열애설을 몰고 다니는 탓에 탈덕한 팬이 작성한 일명 ‘탈덕문’에 갑론을박이 펼쳐지고 있다."));
        GenerateDataHelper(new Keyword("유하람:'마루'", "휴대폰에 달고 다니는 토끼 인형의 이름이 ‘마루’라고 한다."));
        GenerateDataHelper(new Keyword("유하람:걱정", "사건에 큰 충격을 받았을 강재현의 상태를 시종일관 걱정하는 모습을 보였다."));

        // 홍여진
        GenerateDataHelper(new Keyword("홍여진:프로필", "유하람의 매니저."));
        GenerateDataHelper(new Keyword("홍여진:개인 매니저", "Allure의 매니저가 아닌, 유하람의 개인활동 매니저이다."));
        GenerateDataHelper(new Keyword("홍여진:1년차", "1년 전 유하람이 솔로 활동을 시작하면서 새로 고용되었다. 그 전에는 평범한 회사 생활을 했다고 한다."));
        GenerateDataHelper(new Keyword("홍여진:직장인의 고충", "회사에 이 사건에 대해 어떻게 보고해야 할지 고민하고 있다."));

        // 조강희
        GenerateDataHelper(new Keyword("조강희:프로필", "일명 '국민 배우'로 통하는 20여 년 차 배우."));
        GenerateDataHelper(new Keyword("조강희:롤모델", "강재현이 매번 롤모델로 언급하는 인물이다."));
        GenerateDataHelper(new Keyword("조강희:근황", "스릴러 드라마에 캐스팅되어 촬영을 준비 중이라고 한다."));
        GenerateDataHelper(new Keyword("조강희:첫만남", "조강희의 강연에 온 강재현을 만난 것을 계기로 많은 도움을 주었다고 한다."));
        GenerateDataHelper(new Keyword("조강희:두번째 아버지", "강재현이 아버지라고 부를 만큼 강재현의 인생에서 빼놓을 수 없는 인물이다."));
        GenerateDataHelper(new Keyword("조강희:비혼주의자", "자신은 연애와 연애하고 있기 때문에 앞으로도 연애나 결혼은 생각이 없다고 한다."));
        GenerateDataHelper(new Keyword("조강희:장기연애", "20대 초반에 오래 만났던 상대가 있었다고 한다. 그 뒤로 기억에 남는 연애를 한 적은 없는 것 같다."));
        GenerateDataHelper(new Keyword("조강희:금오 엔터", "금오 엔터테인먼트 소속이다."));
        GenerateDataHelper(new Keyword("조강희:소속사 이적", "2018년에 금오 엔터테인먼트로 소속사를 이전했다고 한다."));

        // 이유현
        GenerateDataHelper(new Keyword("이유현:프로필", "서연오의 절친한 친구."));
        GenerateDataHelper(new Keyword("이유현:오타쿠", "Allure의 팬이며, 현재 유하람을 가장 좋아한다."));
        GenerateDataHelper(new Keyword("이유현:팬의 입장", "팬들은 자신이 좋아하는 연예인의 비밀을 알면서도 숨겨주는 경우가 종종 있다고 한다."));
        GenerateDataHelper(new Keyword("이유현:배우 덕질", "Allure의 팬이 되기 전에는 배우를 좋아한 전적이 있다."));
    }

}
