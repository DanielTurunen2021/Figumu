using UnityEngine;
using UnityEngine.AI;

public class RadialTrigger : MonoBehaviour
{
    [SerializeField] float radius = 10f;
    [SerializeField] AudioClip scaredsound;
    
    NavMeshAgent player;

    private void Start()
    {
        player = FindObjectOfType<PlayerControls>().GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        Vector3 center = transform.position;
        
        Vector3 playerpos = player.transform.position;

        Vector3 runto =  -2 *(transform.position - playerpos) + transform.position;

        float distance = Vector3.Distance(playerpos, center);

        bool isinside = distance <= radius;
        if (isinside)
        {
            player.SetDestination(runto);
            StateComponent stateComponent = player.GetComponentInChildren<StateComponent>();
            player.GetComponent<Animator>().SetBool("Scared", true);          
            stateComponent.InitiateStateChange(- 2);

            //if the audio isn't playing while inside the trigger, play audio once
            if(!AudioManager.instance.sfx.isPlaying)
            AudioManager.instance.PlaySFXOnce(scaredsound);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
