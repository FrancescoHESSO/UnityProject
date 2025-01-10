using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void TryAgain()
    {
        // Ricarica la scena corrente
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LoadScene(0);
    }

    public void LoadScene(int indexLevel)
    {
        // Ricarica la scena corrente
        SceneManager.LoadScene(indexLevel);

    }
    public void QuitGame()
    {
        Debug.Log("Quit Game clicked!");
        // Funzione per uscire dal gioco
        Application.Quit();
    }
}
