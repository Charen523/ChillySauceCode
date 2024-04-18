
# ChillySauceCode
<img src="https://notion-emojis.s3-us-west-2.amazonaws.com/prod/svg-twitter/1f336-fe0f.svg" height="300px" width="300px">
<br>
<br>

### 프로젝트 소개(Introduction).

${\textsf{\color{red}7}}$
조 조원들이 가지고 있는
${\textsf{\color{red}리소스}}$
들을 소개하는 게임을 C# 
${\textsf{\color{red}코드}}$
로 구현한 프로젝트입니다.

### 프로젝트 기간(Schedule).
2024년 4월 15일 ~ 2024년 4월 19일

### 프로젝트 핵심 기능.
배경음 및 효과음 사용자 설정 기능 및 설정 저장<br>
여러 기기에서 동작이 가능하도록 자동 해상도 설정<br>
스테이지 선택 및 해금 기능 구현<br>
선택한 스테이지에 따라 난이도 증가<br>
같은 이미지를 가진 1쌍의 카드를 보드에 랜덤하게 배열<br>
클릭으로 카드를 뒤집고 두 카드가 같으면 파괴, 다르면 다시 뒤집는 매칭 시스템 구현<br>
매칭 결과에 따라 점수 및 시간이 증감하는 시스템 구현

  
### 팀원 소개
#### 정승연 (팀장)
  * GITHUB : https://github.com/Charen523
  * 블로그 : https://better-constructor123.tistory.com/manage/posts/    
#### 김영선 (팀원)
  * GITHUB : https://github.com/Mrdosim
  * 블로그 : https://mrdosim.tistory.com/     
#### 이승영 (팀원)
  * GITHUB : https://github.com/lsy1304
  * 블로그 : https://lsy130425.tistory.com/   
#### 박재균 (팀원)
  * GITHUB : https://github.com/JaeGyoon
  * 블로그 : https://velog.io/@balance/posts

### 맡은 역할

#### 김영선
한 번 뒤집은 카드는 색을 다르게 표시하기
<br>나만의 카드 등장 효과 연출하기
<br>카드 뒤집기에서 실제로 카드가 뒤집어지는 모습 연출하기
<br>스타트 씬 UI 수정
<br>카드 사라지는 버그 수정
<br>오디오 조절 바
<br>다른 기기에서(시뮬레이션에서) 변환이 잘 구현되어 있는지 확인.
<br>start버튼 깜빡이는 효과 추가

#### 정승연
매칭 성공 시/ 실패시 UI 만들기.
<br>결과에 매칭 시도 횟수 표시.
<br>매칭 시도 횟수가 게임 종료시 움직이는 애니메이션 구현
<br>클릭할때, 시작할때, 진행중일때 성공, 실패 소리 넣어보기.
<br>시도 횟수 애니메이션 수정
<br>스타트화면/배경화면 삽입 및  UI 수정.
<br>MBTI 카드 UI 만들기.
<br>TryBox 움직이는 애니메이션 통일.
<br>시간 줄어드는 슬라이드바.
<br>GameManager 코드 정리.
<br>LevelScene 개편.
<br>bgmChangeTime 수정.

#### 이승영
한 번 뒤집힌 카드의 색을 바꾸기
<br>결과에 점수 표시 (남은 시간 * 100 - 시도 횟수 - 10)
<br>첫번째 카드 선택 후 5초간 카운트 다운 > 안고르면 닫힘
<br>카드 섞기 기능 개선
<br>틀릴 시 시간 패널티 부여 / 맞을 시 보너스 타임 부여
<br>레벨이 올라갈수록 제한 시간 감소
<br>Pause 버튼 오류 해결
<br>시간 추가, 감점 시 UI를 통해 시간이 늘고 줄었다는 것을 보여주기.
<br>StartScene 재진입시 애니메이션 재생 오류 수정
<br>stage2 레벨 디자인

#### 박재균
타이머 시간이 촉박 할 때 게이머에게 경고하는 기능 작성해보기(시간이 붉게 변하거나 긴박한 배경음악으로 변경)
<br>스테이지 선택과 현재 해금한 스테이지가 구분 가능한 시작 화면 만들기, 플레이 중 해당 스테이지의 최단 기록 띄워주기
<br>AudioManager에 모든 음원 통합시켜버리기.

