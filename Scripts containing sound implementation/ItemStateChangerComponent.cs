using System.Collections;
using UnityEngine;

public class ItemStateChangerComponent : MonoBehaviour
{
    [SerializeField] AudioClip eatingsound;
    [Range(-5, 5)] public int changeValue;
    [Range(-1f, 10f)] public float respawnTime = 1.5f;

    //private MeshRenderer mesh;
    private Collider col;

    private bool isRespawning = false;

    private void Start()
    {
        //mesh = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRespawning) { return; }
        if (other.CompareTag("Player"))
        {
            StateComponent.stateComponentInstance.InitiateStateChange(changeValue);
            AudioManager.instance.PlaySFXOnce(eatingsound);
            if (respawnTime < 0f)
            {

                Destroy(gameObject);

            }
            else if (col)
            {
                StartCoroutine(RespawnTimer());
            }

        }
    }

    private IEnumerator RespawnTimer()
    {
        isRespawning = true;
        col.enabled = false;
        //mesh.enabled = false;
        var children = GetComponentsInChildren<MeshRenderer>();

        foreach (var item in children)
        {
            item.enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        isRespawning = false;
        col.enabled = true;

        foreach (var item in children)
        {
            item.enabled = true;
        }
        //mesh.enabled = true;
    }
}
