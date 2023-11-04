using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;

	private void Start()
	{
        LoadSceneWithTransfer();
    }
	public void LoadSceneWithTransfer()
    {
        ObjectToTransfer[] objectsToTransfer = FindObjectsOfType<ObjectToTransfer>();

        // Перемещаем найденные объекты в новую сцену
        foreach (var obj in objectsToTransfer)
        {
            DontDestroyOnLoad(obj);
        }
    }
    public void SwitchToNewScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Q)) // Пример: удаление предмета из инвентаря по нажатию клавиши R
        {
            SwitchToNewScene();
        }
    }
}
