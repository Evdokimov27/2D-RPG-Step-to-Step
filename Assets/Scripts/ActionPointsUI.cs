using UnityEngine;
using System.Collections.Generic;

public class ActionPointsUI : MonoBehaviour
{
    public GameObject circlePrefab; // Префаб кружка
    public Transform circleGroup; // Панель или группа для размещения кружков
    public int maxActionPoints = 10; // Максимальное количество ОД
    private int currentActionPoints = 10; // Текущее количество ОД
    private List<GameObject> circles = new List<GameObject>();

    void Start()
    {
        // Инициализируем отображение ОД
        UpdateActionPointsUI();
    }

    void UpdateActionPointsUI()
    {
        // Удаляем все текущие кружки
        foreach (var circle in circles)
        {
            Destroy(circle);
        }
        circles.Clear();

        // Создаем новые кружки в соответствии с количеством ОД
        for (int i = 0; i < currentActionPoints; i++)
        {
            GameObject newCircle = Instantiate(circlePrefab, circleGroup);
            circles.Add(newCircle);
        }
    }

    // Функция для обновления текущего количества ОД
    public void UpdateActionPoints(int newActionPoints)
    {
        currentActionPoints = newActionPoints;
        UpdateActionPointsUI();
    }
}
