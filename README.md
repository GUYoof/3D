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

### 🦘 점프
설명: 플레이어 입력에 따라 점프하는 기능 구현, 바닥에 닿아 있을 때만 점프 가능하도록 처리

기술: InputSystem, Rigidbody2D.AddForce, 조건 검사(IsGrounded), ForceMode2D.Impulse

#### 주요 기능

- 플레이어가 점프 입력 시 바닥에 있으면 점프력(jumpPower) 만큼 위쪽으로 힘을 가함

- 점프 가능 여부를 IsGrounded() 메서드로 판단

- 점프 시 스태미나 일정량 사용(예: 50)

- 점프 부스트 아이템 효과 적용 시 점프력 증가

### 🏗️ 점프대
설명: 캐릭터가 닿으면 위로 튀어오르는 점프대 구현

기술: OnCollisionEnter, Rigidbody.AddForce, ForceMode.Impulse

### 🧰 아이템 데이터 관리
설명: 다양한 아이템 속성들을 ScriptableObject로 정의하여 효율적인 데이터 관리

기술: ScriptableObject, Inspector Customization

### ⚡ 점프 부스트 아이템 사용 및 UI 반영

설명: 점프 부스트 아이템 사용 시 일정 시간 동안 점프력이 증가하고, UI에 효과 지속 시간을 표시하는 시스템 구현

기술: Coroutine, IEnumerator, WaitForSeconds, UI fillAmount, 텍스트 업데이트, 상태 변수 관리

#### 주요 기능

- 점프 부스트 아이템 사용 시 Coroutine을 통해 점프력 1.2배 증가 효과를 일정 시간 지속

- 점프 부스트 효과 중복 사용 시 기존 효과 초기화 후 재적용

- UI 바와 텍스트를 통해 남은 점프 부스트 지속 시간 실시간 표시

- 효과 종료 시 점프력을 기본값으로 복구하고 UI 초기화

### 🪂 낙하 데미지 시스템
설명: 플레이어가 일정 높이 이상에서 떨어졌을 때 낙하 거리만큼 물리 데미지를 받도록 처리하는 시스템입니다.

기술: OnCollisionEnter, Rigidbody, velocity, 거리 계산, 데미지 전달

#### 주요 기능
- 플레이어가 떨어진 높이를 계산하여 데미지 적용

- 임계 높이(fallThreshold) 이하의 낙하는 데미지 없음

- 낙하 거리 초과분에 damageMultiplier를 곱해 데미지 계산

- PlayerCondition.TakePysicalDamage()를 통해 데미지 전달

- Rigidbody의 velocity.y를 통해 낙하 여부 판단

