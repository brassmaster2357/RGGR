using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomScript : MonoBehaviour
{
    // Put enemies into the object you put into "epicreveal"
    // no need, it will auto spawn them

    public GameObject epicreveal;
    public GameObject[] enemies;
    private int enemyCount;
    private GameObject enemy;

    private void Start()
    {
        Debug.Log("set unactive");
        epicreveal.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Floor 1")
        {
            enemyCount = 3;
        }
        else if (SceneManager.GetActiveScene().name == "Floor 2")
        {
            enemyCount = 4;
        }
        else if (SceneManager.GetActiveScene().name == "Floor 3")
        {
            enemyCount = 5;
        }
        else if (SceneManager.GetActiveScene().name == "6 Toriels")
        {
            enemyCount = 1;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0), Quaternion.identity, epicreveal.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("set active");
            epicreveal.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
                epicreveal.SetActive(false);
        }
    }
}
