using UnityEngine;
using UnityEngine.AI;

public class AnimController : MonoBehaviour
{
    [SerializeField] AudioClip walksound;
    public ParticleSystem rightParticle;
    public ParticleSystem leftParticle;
    public ParticleSystem scaredParticle;

    private Animator _animator;
    private NavMeshAgent _agent;
    private StateComponent _playerState;

    private bool _isWalking = false;

    private bool _rightOrLeftParticle = true;
    private bool _isAlreadyScared = false;

    private float _stepTime = 0.6f;
    private float _currentStepTimer = 0f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _playerState = GetComponentInChildren<StateComponent>();
        _playerState.changingSize.AddListener(PlayStateChangeAnim);

        _currentStepTimer = _stepTime;
    }

    void Update()
    {
        if (_agent)
        {
            if (_agent.velocity.magnitude > 0.1f && !_isWalking)
            {
                _isWalking = true;
                ResetScared();
                _animator.ResetTrigger("StopWalk");
                _animator.SetTrigger("StartWalk");

            }
            else if (_agent.velocity.magnitude<0.1f && _isWalking) 
            {
                _isWalking = false;
                ResetScared();
                _animator.SetTrigger("StopWalk");
                _currentStepTimer = 0f;
                _rightOrLeftParticle = true;
            }
        }

        if (_isWalking)
        {
            _currentStepTimer = Mathf.Clamp(_currentStepTimer + Time.deltaTime, 0f, _stepTime);

            if (Mathf.Approximately(_currentStepTimer, _stepTime))
            {
                AudioManager.instance.PlaySFXOnce(walksound);
                _currentStepTimer = 0f;
                if (_rightOrLeftParticle)
                {
                    _rightOrLeftParticle = false;
                    rightParticle.Play();

                }
                else
                {
                    _rightOrLeftParticle = true;
                    leftParticle.Play();
                }
            }
        }

        if (_isAlreadyScared == false && _animator.GetBool("Scared")) 
        {
            _isAlreadyScared = true;
            scaredParticle.Play();
        }

        if (Input.GetMouseButtonDown(0))
        {
            ResetScared();
        }
    }

    private void ResetScared()
    {
        _animator.SetBool("Scared", false);
        _isAlreadyScared = false;
    }

    private void PlayStateChangeAnim(float f)
    {
        if (_animator.GetBool("Scared"))
        {
            ResetScared();
            return;
        }
        _animator.SetTrigger("StateChange");
    }
}
