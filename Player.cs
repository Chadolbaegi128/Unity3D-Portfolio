using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ������Ʈ�� �̵��ӵ��� �����Ϳ��� �����ϵ��� SerializeField �׸� ����
    [SerializeField]
    float _speed;

    //�÷��̾��� ������� �迭 2�� ����
    //�������� �迭, ���� ���� Ȯ�� ����
    [SerializeField]
    GameObject[] _weapons;

    [SerializeField]
    bool[] _hasWeapons;

    //�����ϴ� ����ź ������Ʈ�� �����ϱ� ���� ����
    [SerializeField]
    GameObject[] _grenades;

    [SerializeField]
    int _hasGrenades;

    [SerializeField]
    Camera _followCamera;

    //ź��, ����, ü��, ����ź  ������Ʈ�� ������ ���� ���� ����
    [SerializeField]
    int _ammo;

    [SerializeField]
    int _coin;

    [SerializeField]
    int _health;

    [SerializeField]
    int _hasMissile;

    //ź��, ����, ü��, ����ź  ������Ʈ�� �ִ� ��ġ�� �����ϴ� ���� ����
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
    // X��, Y�� �̵����� ���� �������� ����
    float _horizontalAxis;
    float _verticalAxis;

    // ĳ���Ͱ� ������ �� Ư�� Ű�� ���� �� ���� ��ȯ�ǵ���
    // ������ �޾ƿ��� ���� ����
    bool _walkDown;

    // ĳ���� ����
    bool _jumpDown;
    bool _isJump;

    // ĳ���� ���� ����-Ű�Է�
    bool _fireDown;

    // �ѱ�� ������ Ű�Է�
    bool _reloadDown;

    //ĳ���� ��ȣ�ۿ� ����
    bool _interactionDown;

    //���� ��ü ������ ���� ���� �� Input ��ư���� ���
    bool _swapDown1;
    bool _swapDown2;
    bool _swapDown3;

    //ĳ���� ȸ��
    bool _isDodge;

    //���ⱳü ���θ� Ȯ���ϴ� ����
    bool _isSwap;

    // ������ ���θ� Ȯ���ϴ� ����
    bool _isReload;

    // ĳ���� ���� ���� ����
    // Move �޼��忡�� _isFireReady�� false�� �����Ͽ�
    // ������ ������ ������ �������� ���ϱ� ������
    // ������������ true�� �����Ͽ� ó������ �����̰� �����.
    bool _isFireReady= true;

    // �� �浹 ���� Ȯ�ο� ����
    bool _isBorder;

    // X��, Y�� �̵����� ���ս�ų  ����3 ���� ����
    Vector3 _moveVec;

    //ȸ�ǵ��� ������ȯ�� �ȵǵ��� ȸ����ǥ ����
    Vector3 _dodgeVec;

    // ĳ���� ����ȿ�� ����
    Rigidbody _rigid;

    // Mesh Object�� �ִϸ����� ��Ʈ�ѷ� �ҷ�����
    Animator _animation;

    //�÷��̾� ĳ���Ͱ� ������ ������Ʈ ������ ��� ����(������Ʈ ����)
    GameObject _nearObject;

    //������ ������ ���� ������Ʈ�� �����ϴ� ���� ���� �� Ȱ��
    Weapon _equipWeapon;

    //�����ϰ� �ִ� ���� ������Ʈ�� �迭 �ε����� �����ϴ� ����
    int _equipWeaponIndex = -1;

    // ĳ���� ���ݵ����� ���� ����
    float _fireDelay;
    void Awake()
    {
        // ĳ���� ������Ʈ ��ü�� �����Ű�� ���̱� ������
        // GetComponent<>() �޼��带 ����Ѵ�.
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

        // 'Wall'�̶�� ���ڿ��� ���� ���̾�� �浹�ϸ� _isBorder�� ���� True�� �ٲ�� �ȴ�.
        _isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        // FreezeRotation �޼��忡�� angularVelocty�� ���� ȸ���ӵ��� 0���� �������
        // ������ ������ ȸ���ϴ� ������ �߻����� �ʴ´�.
        FreezeRotation();
        StopToWall();
    }

    // Update is called once per frame
    void Update()
    {
        // GetInput() �޼��忡�� �ʱ�ȭ�� �Լ�����
        // Move() �޼��带 ���� ������Ʈ�� �̵��� �� �ְ� �����
        // Turn() �޼��带 ���� ������Ʈ�� ȸ����Ų��.
        // Jump() �޼��带 ���� ������Ʈ ��ü�� �����Ų��.
        //Dodge()�޼��带 ���� ������Ʈ�� ȸ�� ������ �����Ѵ�.
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

        // ShiftŰ�� ������ ��Ȳ���� ���� �� �ֵ��� GetButton �޼��� ���
        _walkDown = Input.GetButton("Walk");

        // Ư��Ű�� �� ���� ������ �� ������ �� �ֵ��� GetButtonDown �޼��� ���
        _jumpDown = Input.GetButtonDown("Jump");

        // ���� ��� ��ư ����
        _fireDown = Input.GetButton("Fire1");

        // ������ ��ư ����
        _reloadDown = Input.GetButtonDown("Reload");

        //_interactionDown ������ ��ȣ�ۿ�Ű ����
        _interactionDown = Input.GetButtonDown("Interaction");

        //_swapDown1,2,3 ������ ��ȣ�ۿ�Ű ����
        _swapDown1 = Input.GetButtonDown("Swap1");
        _swapDown2 = Input.GetButtonDown("Swap2");
        _swapDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        // ��� �����̵� ������ ��ġ�� �� �� �ֵ��� ����ȭ(normalized) ����
        _moveVec = new Vector3(_horizontalAxis, 0, _verticalAxis).normalized;

        //ȸ�� �߿��� ������ ����->ȸ�ǹ��� ���ͷ� �ٲٵ��� ����
        //ȸ�� ���� ���� ��ȯ X
        if (_isDodge)
        {            
            _moveVec = _dodgeVec;                        
        }

        // ���� ��ü, �����غ�, ������ �߿��� �̵��Ұ� ����
        if (_isSwap || _isReload || !_isFireReady)
        {
            _moveVec = Vector3.zero;
        }

        // ����Ű�� �۵��ǵ��� transform �̵� ����
        // ���� ������ ���=>_walkDown ���� True�� 0.3f, False�� 1.0f
        // ���� �ε����� �ʾ��� ���� �̵� �����ϰ� ����
        // (�÷��� ����-Ȯ�ο� ������ �̵����� �������� Ȱ��)
        if (!_isBorder)
        {
            transform.position += _moveVec * _speed * (_walkDown ? 0.3f : 1.0f) * Time.deltaTime;
        }       

        // SetBool �޼���� �Ű����� ���� ����=>�� ��° �Ű�������
        // Ư�� �������� ������Ű�� "isRun" ������ True�� ��ȯ
        _animation.SetBool("isRun", _moveVec != Vector3.zero);

        // _walkDown �������� True�� �Ǹ� isWalk �������� True�� ��
        // �޸��⿡�� �ȱ�� ��ȯ
        _animation.SetBool("isWalk", _walkDown);
    }

    void Turn()
    {
        // #1. Ű���忡 ���� ȸ��
        // LookAt �޼��带 �̿��ؼ� ĳ���͸� ȸ����Ŵ
        // transform.position + _moveVec=>���������� �������� ȸ����
        transform.LookAt(transform.position + _moveVec);

        // #2. ���콺�� ���� ȸ��
        // ������ ���� ���콺 ȸ���ϵ��� ����
        if (_fireDown)
        {
            Ray ray = _followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                // �÷��̾ ���ƺ� ��ġ ���Ͱ� ���ϱ�
                Vector3 nextVec = rayHit.point - transform.position;
                // ���̰� ������Ʈ ������ ĳ���� �ü��� ���� ������ �ʵ���
                // ���ƺ� ��ġ ������ y�� ���� 0���� ������Ŵ
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
       
    }

    void Jump()
    {
        // ����Ű�� �����鼭 ���� ������ ���°� �ƴ� ���
        // ���� ����� �����Ѵ�.
        //ȸ�� ���۰� �������� ���� _moveVec�� ���� Vector3,zero
        //��, ������ �ִ� ���¸� ������ �� �� �ְ� �̵� ���� ����
        //ȸ�Ǹ� �� �� �ִ�.
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
        // ���Ⱑ ���� ���� ���ݸ���� ����ǰ� ���ǹ� �ۼ�
        // ���Ⱑ ������ ���� �ȵ�
        if (_equipWeapon == null)
        {
            return;
        }

        // ���ݵ����̿� �ð��� �����ְ� ���ݰ��� ���θ� Ȯ��
        _fireDelay += Time.deltaTime;

        // ���ݼӵ��� ���ݵ����� �ð����� ���� �� ������ ���°� �غ�Ǿ��ٰ� ������
        _isFireReady = _equipWeapon._rate < _fireDelay;

        // ���ݹ�ư �Է�+�����غ� �Ϸ�+ȸ�� ���� X+ ���ⱳü X ������ ��
        // Weapon ��ũ��Ʈ�� �ִ� Use �޼��� ����
        if (_fireDown && _isFireReady && !_isDodge && !_isSwap)
        {
            _equipWeapon.Use();
            // ������ ���� ������ �����̸� 'doSwing' Ʈ���Ÿ�, �ƴϸ� 'doShot'(���Ÿ� ����) Ʈ���� �ߵ�
            // ���� ������ ���
            _animation.SetTrigger(_equipWeapon._type.Equals(Weapon.Type.Melee) ? "doSwing" : "doShot");

            // ���ݵ����̸� 0���� ������ ���� ���ݱ��� ��ٸ��� ����
            _fireDelay = 0;
        }
    }

    void Reload()
    {
        // �Ʒ� 3���� ���ǿ����� �������� �ȵ�
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

        // ������ ��ư �Է�, ���� X, ȸ�� X, ���ⱳü X, �����غ� ���� ���� ���� ����
        if(_reloadDown && !_isJump && !_isDodge && !_isSwap && _isFireReady)
        {
            _animation.SetTrigger("doReload");
            _isReload = true;

            // 0.4���Ŀ� ������ ��� ����
            Invoke("ReloadOut", 0.1f);
        }
    }

    void ReloadOut()
    {
        // ������ �� �����Ǵ� ź���� ������ ���� ������ ź���� ������ ��ģ ����
        // ������ �� �ִ� �ִ� ź���� �������� ������ '�����Ǵ� ź���� ����'
        // �ƴ϶�� '������ �� �ִ� �ִ� ź���� �������� ���� ������ ź���� ������ �� ��'���� ����
        int reloadAmmo = _ammo+_equipWeapon._currentAmmo
            < _equipWeapon._maxAmmo ? _ammo :
            _equipWeapon._maxAmmo - _equipWeapon._currentAmmo;

        // �������Ǹ鼭 ź���� �߰���
        _equipWeapon._currentAmmo += reloadAmmo;

        // �����ִ� �� ź���� �������� �����Ǵ� ź���� ����.
        _ammo -= reloadAmmo;
        _isReload = false;
    }

    //ĳ���� ȸ�Ǵ� ����Ű�� �̿��� �����
    void Dodge()
    {
        // ����Ű�� �����鼭 ���� ������ ���°� �ƴ� ���
        // ȸ�� ����� �����Ѵ�.
        if (_jumpDown && !_moveVec.Equals( Vector3.zero) && !_isJump && !_isDodge && ! _isSwap)
        {
            //ȸ�� ���� ������ȯ�� ���� �ʵ��� ȸ�ǹ��� Vector3 �߰�
            _dodgeVec = _moveVec;            

            // ȸ�� ���۽� �̵��ӵ��� 2��� ����
            _speed *= 2;
            _animation.SetTrigger("doDodge");
            _isDodge = true;            

            //ȸ�� ������ ���� �� 0,4�� �ڿ� �⺻ �ڼ��� ����
            Invoke("DodgeOut", 0.4f);
        }
    }

    // ȸ�� ������ ������ �� �̵��ӵ��� ���� �ӵ��� �ǵ�����
    // _isDodge ���� false�� �ٲ�
    void DodgeOut()
    {
       _speed *= 0.5f;
        _isDodge = false;
    }

    //����Ű �� �� �ϳ��� ������ �ǵ��� OR ���� �ۼ�
    //���� �ٲ� �� ������ ȸ�� ������ �۵� �ȵǵ��� ���� �߰�
    void Swap()
    {
        //���� �ߺ� ��ü ����
        //�ش� ���� ��üŰ�� ������ �� �ش� ���Ⱑ �����Ǿ����� �ʰų�
        //�ش� ������ value ���̸� ���� ��ü�� �����
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

        //���� ��ü ������ �ߵ���Ű�� ���� ���� �ʱ�ȭ ��
        //_weapons �迭 �ҷ�����
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

        //���� ��ü ���� �Ǵ� �������� �ʰų� ȸ�� ��, ���� ��ü�� ����  ���� �� 
        //�߰�ȣ �ȿ� ���� ����(OR ����) 
        if ((_swapDown1 || _swapDown2 || _swapDown3) && ! _isJump && ! _isDodge)
        {            
            //������ ����ִ� ���⸦ ������ų �� �� ���⸦ ���� �� �ֵ��� ����
            if (_equipWeapon != null)
            {
                _equipWeapon.gameObject.SetActive(false);
            }

            //Weapon�� value ���� _equipWeaponIndex�� ����
            _equipWeaponIndex = weaponIndex;
            _equipWeapon = _weapons[weaponIndex].GetComponent<Weapon>();
            _equipWeapon.gameObject.SetActive(true);
            
            //���� ��ü �ִϸ��̼� �߰�
            _animation.SetTrigger("doSwap");

            //���� ��ü ������ ��� _isSwap�� �������� true�� �ٲ�
            _isSwap = true;

            //���� ��ü ���°� 0.4�� ���� ���� ���_isSwap�� �������� false�� �ٲ�
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
            // �±װ� "Weapon"�̶�� Item ��ũ��Ʈ�� �ִ�
            // weaponindex ������ ������ ������Ʈ�� ������ value���� �����ϰ�
            // ������ ������Ʈ�� tag�� "Weapon"�̸� Ư�� value ���� �ش��ϴ�  �ε����� �ִ� ������
            // true�� �ٲ� �� �ش� ������Ʈ�� ����
            if (_nearObject.CompareTag("Weapon"))
            {
                Item item = _nearObject.GetComponent<Item>();
                int weaponIndex = item._value;
                _hasWeapons[weaponIndex] = true;

                Destroy(_nearObject);
            }
        }
    }  

    //Item ��ũ��Ʈ���� ���� enum Ÿ�Կ� �°� ������ ��ġ�� �÷��̾� ��ġ�� ����
    //ĳ���Ͱ� ������ ������Ʈ�� �ε�ĥ �� �����ۿ� ������ ��ġ�� ������Ŵ
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
            //���� �������� ���� ó��
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //ĳ���Ͱ� Weapon�̶�� �±׸� ���� ������Ʈ�� �ε�������
        //�ش� ������Ʈ�� _nearObject ������ ����
        if (other.CompareTag("Weapon"))
        {
            _nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Weapon�±׸� ���� ������Ʈ���� ĳ���Ͱ� ��� ���
        //_nearObject�� ����� ���� ����ش�.
        if (other.CompareTag("Weapon"))
        {
            _nearObject = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // ���� ������Ʈ�� �ٴڿ� �ε����� ���� ���°� �ƴ� ���·�
        // _isJump�� ���� false�� ��ȯ
        if (collision.gameObject.CompareTag("Floor"))
        {
            _animation.SetBool("isJump", false);
            _isJump = false;
        }
    }
  
}
