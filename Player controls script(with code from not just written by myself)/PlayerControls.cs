using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerControls : MonoBehaviour
{
    private Camera _camera;
    private NavMeshAgent _agent;
    private StateComponent _playerState;
    public bool controlsLocked;
    [SerializeField] AudioClip pettingsound;
    
    public Slider shrinkIndicatorPrefab;
    private Slider _shrinkIndicatorInstance;
    private bool _playerClicked = false;
    [SerializeField] private float _holdShrinkTimer = 0.7f;
    private float _currentShrinkTime = 0f;
    // Update is called once per frame
    private void Awake()
    {
        controlsLocked = false;
        _camera = FindObjectOfType<Camera>();
        _agent = GetComponent<NavMeshAgent>();
        _playerState = GetComponentInChildren<StateComponent>();
        _playerState.changingState.AddListener(UpdateSize);
       

    }
    void Update()
    {
        //Checks what happens when the left mouse button is clicked.
        if (!controlsLocked && Input.GetMouseButtonDown(0))
        {
            //Converts our current mouse position in screenspace to world space
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            //Stores information about what the ray hits. 
            RaycastHit hit;

            //Caststs a ray and stores the information in hit & check if Mouse is overUI.
            if (Physics.Raycast(ray, out hit) == true && !EventSystem.current.IsPointerOverGameObject())
            {
                var interactComp = hit.transform.GetComponentInParent<Clickable>();
                if (hit.transform.CompareTag("Player") && _playerState.stateIndex != 0)
                {
                    _playerClicked = true;
                    _currentShrinkTime = 0f;
                   

                    var canvas = GetComponentInChildren<Canvas>();

                    if (shrinkIndicatorPrefab)
                        _shrinkIndicatorInstance = Instantiate(shrinkIndicatorPrefab, canvas.transform);

                }
                else if (interactComp != null)
                {
                    interactComp.Onclicked();
                }
                else
                {
                    //move player to the point we hit with our ray.
                    _agent.SetDestination(hit.point);
                }
            }
        }

        if (!controlsLocked && Input.GetMouseButtonUp(0))
        {
            _playerClicked = false;
            if (_shrinkIndicatorInstance)
                Destroy(_shrinkIndicatorInstance.gameObject);
        }

        if (_playerClicked)
        {
            _currentShrinkTime = Mathf.Clamp(_currentShrinkTime + Time.deltaTime, 0f, _holdShrinkTimer);

            if (_shrinkIndicatorInstance)
            {
                _shrinkIndicatorInstance.value = 1f - (_currentShrinkTime / _holdShrinkTimer);
            }

            AudioManager.instance.PlaySFXOnce(pettingsound);
            
            if (Mathf.Approximately(_currentShrinkTime, _holdShrinkTimer))
            {
                _playerClicked = false;
                _playerState.ClickPlayerReduceSize();
                

                if (_shrinkIndicatorInstance)
                    Destroy(_shrinkIndicatorInstance.gameObject);
            }
        }
    }
    void UpdateSize()
    {
        _agent.agentTypeID = NavMesh.GetSettingsByIndex(_playerState.stateIndex).agentTypeID;
    }
}
