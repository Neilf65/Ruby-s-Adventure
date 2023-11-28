using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject endPanel;
    [SerializeField] private AudioClip LossFanfare;

    private AudioSource audioSource;


    bool gameHasEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Call when player HP reaches 0
    public void LoseGame()
    {
        endPanel.SetActive(true);
        endPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Game Over! Press R to restart!";

        PlaySound(LossFanfare); 
                    

        if (gameHasEnded == false)
        {
            gameHasEnded = true;


        }
 
    }

    //Call when score reaches 5
    public void WinGame()
    {

        endPanel.SetActive(true);
        endPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "You Win! Press R to restart!";
        

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
