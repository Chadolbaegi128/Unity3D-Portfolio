using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 오브젝트의 이동속도를 에디터에서 설정하도록 SerializeField 항목 설정
    [SerializeField]
    float _speed;

    //플레이어의 무기관련 배열 2개 선언
    //무기정보 배열, 무기 습득 확인 변수
    [SerializeField]
    GameObject[] _weapons;

    [SerializeField]
    bool[] _hasWeapons;

    //공전하는 수류탄 오브젝트를 저장하기 위한 변수
    [SerializeField]
    GameObject[] _grenades;

    [SerializeField]
    int _hasGrenades;

    [SerializeField]
    Camera _followCamera;

    //탄약, 동전, 체력, 수류탄  오브젝트의 정보를 담은 변수 생성
    [SerializeField]
    int _ammo;

    [SerializeField]
    int _coin;

    [SerializeField]
    int _health;

    [SerializeField]
    int _hasMissile;

    //탄약, 동전, 체력, 수류탄  오브젝트의 최대 수치를 저장하는 변수 생성
    [SerializeField]
    int _maxAmmo;

    [SerializeField]
    int _maxCoin;

    [SerializeField]
    int _maxHealth;
     
    [SerializeField]
    int _maxHasGrenades;

    [SerializeField]
    int _maxHasMissile;
    // X축, Y측 이동값을 받을 전역변수 선언
    float _horizontalAxis;
    float _verticalAxis;

    // 캐릭터가 움직일 때 특정 키를 누를 때 긷기로 전환되도록
    // 조건을 받아오는 변수 선언
    bool _walkDown;

    // 캐릭터 점프
    bool _jumpDown;
    bool _isJump;

    // 캐릭터 공격 설정-키입력
    bool _fireDown;

    // 총기류 재장전 키입력
    bool _reloadDown;

    //캐릭터 상호작용 설정
    bool _interactionDown;

    //무기 교체 설정용 변수 생성 및 Input 버튼으로 등록
    bool _swapDown1;
    bool _swapDown2;
    bool _swapDown3;

    //캐릭터 회피
    bool _isDodge;

    //무기교체 여부를 확인하는 변수
    bool _isSwap;

    // 재장전 여부를 확인하는 변수
    bool _isReload;

    // 캐릭터 공격 변수 선언
    // Move 메서드에서 _isFireReady가 false로 설정하여
    // 게임을 시작할 때부터 움직이지 못하기 때문에
    // 전역변수에서 true로 설정하여 처음에는 움직이게 만든다.
    bool _isFireReady= true;

    // 벽 충돌 여부 확인용 변수
    bool _isBorder;

    // X축, Y축 이동값을 통합시킬  벡터3 변수 선언
    Vector3 _moveVec;

    //회피도중 방향전환이 안되도록 회피좌표 설정
    Vector3 _dodgeVec;

    // 캐릭터 물리효과 적용
    Rigidbody _rigid;

    // Mesh Object의 애니메이터 컨트롤러 불러오기
    Animator _animation;

    //플레이어 캐릭터가 접근한 오브젝트 정보가 담길 변수(오브젝트 감지)
    GameObject _nearObject;

    //기존에 장착된 무기 오브젝트를 저장하는 변수 선언 및 활용
    Weapon _equipWeapon;

    //장착하고 있는 무기 오브젝트의 배열 인덱스를 저장하는 변수
    int _equipWeaponIndex = -1;

    // 캐릭터 공격딜레이 변수 선언
    float _fireDelay;
    void Awake()
    {
        // 캐릭터 오브젝트 전체를 도약시키는 것이기 때문에
        // GetComponent<>() 메서드를 사용한다.
        _rigid = GetComponent<Rigidbody>();

        _animation = GetComponentInChildren<Animator>();    
    }

    void FreezeRotation()
    {
        _rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);

        // 'Wall'이라는 문자열을 가진 레이어와 충돌하면 _isBorder의 값이 True로 바뀌게 된다.
        _isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        // FreezeRotation 메서드에서 angularVelocty를 통해 회전속도를 0으로 만들었기
        // 때문에 저절로 회전하는 현상은 발생하지 않는다.
        FreezeRotation();
        StopToWall();
    }

    // Update is called once per frame
    void Update()
    {
        // GetInput() 메서드에서 초기화된 함수들이
        // Move() 메서드를 통해 오브젝트가 이동할 수 있게 만들고
        // Turn() 메서드를 통해 오브젝트를 회전시킨다.
        // Jump() 메서드를 통해 오브젝트 전체를 도약시킨다.
        //Dodge()메서드를 통해 오브젝트에 회피 동작을 수행한다.
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Reload();
        Dodge();
        Swap();
        Interaction();
    }
    void GetInput()
    {
        _horizontalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        // Shift키를 누르는 상황에서 걸을 수 있도록 GetButton 메서드 사용
        _walkDown = Input.GetButton("Walk");

        // 특정키를 한 번만 눌렀을 때 점프할 수 있도록 GetButtonDown 메서드 사용
        _jumpDown = Input.GetButtonDown("Jump");

        // 공격 모션 버튼 설정
        _fireDown = Input.GetButton("Fire1");

        // 재장전 버튼 설정
        _reloadDown = Input.GetButtonDown("Reload");

        //_interactionDown 변수에 상호작용키 설정
        _interactionDown = Input.GetButtonDown("Interaction");

        //_swapDown1,2,3 변수에 상호작용키 설정
        _swapDown1 = Input.GetButtonDown("Swap1");
        _swapDown2 = Input.GetButtonDown("Swap2");
        _swapDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        // 어느 방향이든 동일한 수치로 갈 수 있도록 정규화(normalized) 설정
        _moveVec = new Vector3(_horizontalAxis, 0, _verticalAxis).normalized;

        //회피 중에는 움직임 벡터->회피방향 벡터로 바꾸도록 구현
        //회피 도중 방향 전환 X
        if (_isDodge)
        {            
            _moveVec = _dodgeVec;                        
        }

        // 무기 교체, 공격준비, 재장전 중에는 이동불가 설정
        if (_isSwap || _isReload || !_isFireReady)
        {
            _moveVec = Vector3.zero;
        }

        // 조작키가 작동되도록 transform 이동 설정
        // 삼항 연산자 사용=>_walkDown 값이 True면 0.3f, False면 1.0f
        // 벽에 부딪히지 않았을 때만 이동 가능하게 설정
        // (플래그 변수-확인용 변수를 이동제한 조건으로 활용)
        if (!_isBorder)
        {
            transform.position += _moveVec * _speed * (_walkDown ? 0.3f : 1.0f) * Time.deltaTime;
        }       

        // SetBool 메서드로 매개변수 값을 설정=>두 번째 매개변수로
        // 특정 변수값을 만족시키면 "isRun" 변수가 True로 전환
        _animation.SetBool("isRun", _moveVec != Vector3.zero);

        // _walkDown 변수값이 True가 되면 isWalk 변수값도 True가 됨
        // 달리기에서 걷기로 전환
        _animation.SetBool("isWalk", _walkDown);
    }

    void Turn()
    {
        // #1. 키보드에 의한 회전
        // LookAt 메서드를 이용해서 캐릭터를 회전시킴
        // transform.position + _moveVec=>움직여야할 방향으로 회전함
        transform.LookAt(transform.position + _moveVec);

        // #2. 마우스에 의한 회전
        // 공격할 때만 마우스 회전하도록 설정
        if (_fireDown)
        {
            Ray ray = _followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                // 플레이어가 돌아볼 위치 벡터값 구하기
                Vector3 nextVec = rayHit.point - transform.position;
                // 높이가 오브젝트 때문에 캐릭터 시선이 위로 향하지 않도록
                // 돌아볼 위치 벡터의 y축 값은 0으로 고정시킴
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
       
    }

    void Jump()
    {
        // 점프키를 누르면서 현재 점프인 상태가 아닐 경우
        // 점프 명령을 수행한다.
        //회피 동작과 구분짓기 위해 _moveVec의 값이 Vector3,zero
        //즉, 가만히 있는 상태면 점프만 할 수 있고 이동 중일 때는
        //회피만 할 수 있다.
        if (_jumpDown && _moveVec == Vector3.zero && !_isJump && !_isDodge && !_isSwap)
        {
            _rigid.AddForce(Vector3.up*15, ForceMode.Impulse);
            _animation.SetBool("isJump", true);
            _animation.SetTrigger("doJump");
            _isJump = true;
        }
    }

    void Attack()
    {
        // 무기가 있을 때만 공격모션이 실행되게 조건문 작성
        // 무기가 없으면 실행 안됨
        if (_equipWeapon == null)
        {
            return;
        }

        // 공격딜레이에 시간을 더해주고 공격가능 여부를 확인
        _fireDelay += Time.deltaTime;

        // 공격속도가 공격딜레이 시간보다 적을 때 공격할 상태가 준비되었다고 간주함
        _isFireReady = _equipWeapon._rate < _fireDelay;

        // 공격버튼 입력+공격준비 완료+회피 상태 X+ 무기교체 X 상태일 때
        // Weapon 스크립트에 있는 Use 메서드 실행
        if (_fireDown && _isFireReady && !_isDodge && !_isSwap)
        {
            _equipWeapon.Use();
            // 착용한 무기 종류가 근접이면 'doSwing' 트리거를, 아니면 'doShot'(원거리 무기) 트리거 발동
            // 삼항 연산자 사용
            _animation.SetTrigger(_equipWeapon._type.Equals(Weapon.Type.Melee) ? "doSwing" : "doShot");

            // 공격딜레이를 0으로 돌려서 다음 공격까지 기다리는 로직
            _fireDelay = 0;
        }
    }

    void Reload()
    {
        // 아래 3가지 조건에서는 재장전이 안됨
        if (_equipWeapon == null)
        {
            return;
        }

        if (_equipWeapon._type.Equals(Weapon.Type.Melee))
        {
            return;
        }

        if (_ammo.Equals(0))
        {
            return;
        }

        // 재장전 버튼 입력, 점프 X, 회피 X, 무기교체 X, 공격준비 됐을 때만 장전 가능
        if(_reloadDown && !_isJump && !_isDodge && !_isSwap && _isFireReady)
        {
            _animation.SetTrigger("doReload");
            _isReload = true;

            // 0.4초후에 재장전 모션 종료
            Invoke("ReloadOut", 0.1f);
        }
    }

    void ReloadOut()
    {
        // 재장전 시 장전되는 탄약의 갯수와 현재 장착된 탄약의 갯수를 합친 값이
        // 장착할 수 있는 최대 탄약의 갯수보다 적으면 '장전되는 탄약의 갯수'
        // 아니라면 '장착할 수 있는 최대 탄약의 갯수에서 현재 장착된 탄약의 갯수를 뺀 값'으로 설정
        int reloadAmmo = _ammo+_equipWeapon._currentAmmo
            < _equipWeapon._maxAmmo ? _ammo :
            _equipWeapon._maxAmmo - _equipWeapon._currentAmmo;

        // 재장전되면서 탄약이 추가됨
        _equipWeapon._currentAmmo += reloadAmmo;

        // 남아있는 총 탄약의 갯수에서 장전되는 탄약을 뺀다.
        _ammo -= reloadAmmo;
        _isReload = false;
    }

    //캐릭터 회피는 점프키를 이용해 실행됨
    void Dodge()
    {
        // 점프키를 누르면서 현재 점프인 상태가 아닐 경우
        // 회피 명령을 수행한다.
        if (_jumpDown && !_moveVec.Equals( Vector3.zero) && !_isJump && !_isDodge && ! _isSwap)
        {
            //회피 도중 방향전환이 되지 않도록 회피방향 Vector3 추가
            _dodgeVec = _moveVec;            

            // 회피 동작시 이동속도가 2배로 증가
            _speed *= 2;
            _animation.SetTrigger("doDodge");
            _isDodge = true;            

            //회피 동작을 취한 후 0,4초 뒤에 기본 자세로 복귀
            Invoke("DodgeOut", 0.4f);
        }
    }

    // 회피 동작이 끝났을 때 이동속도를 원래 속도로 되돌리고
    // _isDodge 값을 false로 바꿈
    void DodgeOut()
    {
       _speed *= 0.5f;
        _isDodge = false;
    }

    //단축키 셋 중 하나만 눌러도 되도록 OR 조건 작성
    //무기 바꿀 때 점프나 회피 동작이 작동 안되도록 조건 추가
    void Swap()
    {
        //무기 중복 교체 방지
        //해당 무기 교체키를 눌렀을 때 해당 무기가 장착되어있지 않거나
        //해당 무기의 value 값이면 무기 교체가 종료됨
        if(_swapDown1 && (!_hasWeapons[0] || _equipWeaponIndex.Equals("0")))
        {
            return;
        }
        if (_swapDown2 && (!_hasWeapons[1] || _equipWeaponIndex.Equals("1")))
        {
            return;
        }
        if (_swapDown3 && (!_hasWeapons[2] || _equipWeaponIndex.Equals("2")))
        {
            return;
        }

        //무기 교체 조건을 발동시키기 위한 변수 초기화 및
        //_weapons 배열 불러오기
        int weaponIndex = -1;
        if (_swapDown1)
        {
            weaponIndex = 0;
        }
        if (_swapDown2)
        {
            weaponIndex = 1;
        }
        if(_swapDown3)
        {
            weaponIndex = 2;
        }

        //무기 교체 도중 또는 점프하지 않거나 회피 중, 무기 교체를 하지  않을 때 
        //중괄호 안에 로직 실행(OR 조건) 
        if ((_swapDown1 || _swapDown2 || _swapDown3) && ! _isJump && ! _isDodge)
        {            
            //기존에 들어있는 무기를 해제시킬 때 새 무기를 얻을 수 있도록 설정
            if (_equipWeapon != null)
            {
                _equipWeapon.gameObject.SetActive(false);
            }

            //Weapon의 value 값을 _equipWeaponIndex에 저장
            _equipWeaponIndex = weaponIndex;
            _equipWeapon = _weapons[weaponIndex].GetComponent<Weapon>();
            _equipWeapon.gameObject.SetActive(true);
            
            //무기 교체 애니메이션 추가
            _animation.SetTrigger("doSwap");

            //무기 교체 상태일 경우 _isSwap의 변수값을 true로 바꿈
            _isSwap = true;

            //무기 교체 상태가 0.4초 정도 지날 경우_isSwap의 변수값을 false로 바꿈
            Invoke("SwapOut", 0.4f);
        }
       
    }
        
    void SwapOut()
    {
        _isSwap = false;
    }

    void Interaction()
    {
        if(_interactionDown && _nearObject != null && ! _isJump)
        {
            // 태그가 "Weapon"이라면 Item 스크립트에 있는
            // weaponindex 변수에 아이템 오브젝트에 설정된 value값을 저장하고
            // 접근한 오브젝트의 tag가 "Weapon"이면 특정 value 값에 해당하는  인덱스에 있는 변수를
            // true로 바꾼 뒤 해당 오브젝트를 제거
            if (_nearObject.CompareTag("Weapon"))
            {
                Item item = _nearObject.GetComponent<Item>();
                int weaponIndex = item._value;
                _hasWeapons[weaponIndex] = true;

                Destroy(_nearObject);
            }
        }
    }  

    //Item 스크립트에서 정한 enum 타입에 맞게 아이템 수치를 플레이어 수치에 적용
    //캐릭터가 아이템 오브젝트에 부딪칠 때 아이템에 설정된 수치를 증가시킴
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            switch (item._type)
            {
                case Item.Type.Ammo:
                    _ammo += item._value;
                    if(_ammo>_maxAmmo)
                    {
                        _ammo = _maxAmmo;
                    }
                    break;
                case Item.Type.Coin:
                    _coin += item._value;
                    if (_coin > _maxCoin)
                    {
                        _coin = _maxCoin;
                    }
                    break;
                case Item.Type.Heart:
                    _health += item._value;
                    if (_health > _maxHealth)
                    {
                        _health = _maxHealth;
                    }
                    break;
                case Item.Type.Grenade:
                    if (_hasGrenades.Equals(_maxHasGrenades))
                    {
                        return;
                    }
                    _grenades[_hasGrenades].SetActive(true);
                    _hasGrenades += item._value;
                    break;
                case Item.Type.Missile:
                    _hasMissile += item._value;
                    if (_hasMissile > _maxHasMissile)
                    {
                        _hasMissile = _maxHasMissile;
                    }
                    break;
                default:
                    Debug.Assert(false,"Unknown Type");
                    break;
            }
            //먹은 아이템은 삭제 처리
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //캐릭터가 Weapon이라는 태그를 가진 오브젝트에 부딪혔을때
        //해당 오브젝트를 _nearObject 변수에 저장
        if (other.CompareTag("Weapon"))
        {
            _nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Weapon태그를 가진 오브젝트에서 캐릭터가 벗어날 경우
        //_nearObject에 저장된 값을 비워준다.
        if (other.CompareTag("Weapon"))
        {
            _nearObject = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 게임 오브젝트가 바닥에 부딪히면 점프 상태가 아닌 상태로
        // _isJump의 값이 false로 전환
        if (collision.gameObject.CompareTag("Floor"))
        {
            _animation.SetBool("isJump", false);
            _isJump = false;
        }
    }
  
}
