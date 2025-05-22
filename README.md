## 3D 프로젝트 개인 과제 : 네모의 모험

### 기본 이동 및 점프
설명: Input System을 활용한 플레이어 이동(WASD), 점프(Space) 구현

기술: Input System, Rigidbody, ForceMode

### ❤️ 체력바 UI
설명: 플레이어의 체력을 반영하는 UI 바 구현

기술: Canvas UI, Slider, 실시간 UI 갱신

### 🔍 동적 환경 조사
설명: Raycast로 플레이어가 바라보는 오브젝트 정보를 화면에 출력

기술: Raycast, UI, Tag, Component Access


### 🏗️ 점프대
설명: 캐릭터가 닿으면 위로 튀어오르는 점프대 구현

기술: OnCollisionEnter, Rigidbody.AddForce, ForceMode.Impulse

### 🧰 아이템 데이터 관리
설명: 다양한 아이템 속성들을 ScriptableObject로 정의하여 효율적인 데이터 관리

기술: ScriptableObject, Inspector Customization

### ⚡ 아이템 사용 및 효과 지속
설명: 특정 아이템 사용 시, 일정 시간 동안 효과가 유지되는 시스템

기술: Coroutine, IEnumerator, WaitForSeconds

### 🪂 낙하 데미지 시스템
설명: 플레이어가 일정 높이 이상에서 떨어졌을 때 낙하 거리만큼 물리 데미지를 받도록 처리하는 시스템입니다.

기술: OnCollisionEnter, Rigidbody, velocity, 거리 계산, 데미지 전달

#### 주요 기능
플레이어가 떨어진 높이를 계산하여 데미지 적용

임계 높이(fallThreshold) 이하의 낙하는 데미지 없음

낙하 거리 초과분에 damageMultiplier를 곱해 데미지 계산

PlayerCondition.TakePysicalDamage()를 통해 데미지 전달

Rigidbody의 velocity.y를 통해 낙하 여부 판단

