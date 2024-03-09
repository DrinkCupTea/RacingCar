using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{
    [SerializeField] private Animator UIAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UIAnimator.Play("StartGame");
            StartGame();
        }
    }

    void StartGame()
    {
        // UIAnimator.SetTrigger("");
        StartCoroutine(LoadScene("Game"));
    }

    IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(scene);
    }
}
