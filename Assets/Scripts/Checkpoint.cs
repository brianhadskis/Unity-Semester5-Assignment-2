using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool checkpointReached = false;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetComponent<AudioSource>().time = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckpointReached()
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        checkpointReached = true;

        foreach (Transform child in this.transform)
        {
            if (child.transform.GetComponent<ParticleSystem>())
            {
                child.transform.GetComponent<ParticleSystem>().Stop();
            }
            if (child.transform.GetComponent<CapsuleCollider>())
            {
                child.gameObject.SetActive(false);
            }
        }

        this.transform.GetComponent<AudioSource>().Play();

        while (this.transform.GetComponent<AudioSource>().isPlaying && checkpointReached)
        {
            yield return new WaitForSeconds(0.2f);
        }

        Destroy(this.gameObject);
    }
}
