using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;

    private float startTime;
    private float timeTaken;

    private int collectablesPicked;
    public int maxCollectables = 10;

    public GameObject playButton;
    public GameObject leaderboard;
    public TextMeshProUGUI curTimeText;

    private bool isPlaying;
    
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isPlaying)
            return;

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);

        curTimeText.text = (Time.time - startTime).ToString("F2");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            Destroy(other.gameObject);
            if (collectablesPicked == maxCollectables)
                End();
        }
        if (other.gameObject.CompareTag("Slow"))
        {
            speed = speed - 2;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Speed"))
        {
            speed = speed + 2;
            Destroy(other.gameObject);
        }
    }

    public void Begin()
    {
        playButton.SetActive(false);
        leaderboard.SetActive(false);
        startTime = Time.time;
        isPlaying = true;
    }

    void End()
    {
        playButton.SetActive(true);
        leaderboard.SetActive(true);
        timeTaken = Time.time - startTime;
        isPlaying = false;
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
    }
}
