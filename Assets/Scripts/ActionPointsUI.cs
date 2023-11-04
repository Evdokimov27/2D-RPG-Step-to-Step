using UnityEngine;
using System.Collections.Generic;

public class ActionPointsUI : MonoBehaviour
{
    public GameObject circlePrefab; // ������ ������
    public Transform circleGroup; // ������ ��� ������ ��� ���������� �������
    public int maxActionPoints = 10; // ������������ ���������� ��
    private int currentActionPoints = 10; // ������� ���������� ��
    private List<GameObject> circles = new List<GameObject>();

    void Start()
    {
        // �������������� ����������� ��
        UpdateActionPointsUI();
    }

    void UpdateActionPointsUI()
    {
        // ������� ��� ������� ������
        foreach (var circle in circles)
        {
            Destroy(circle);
        }
        circles.Clear();

        // ������� ����� ������ � ������������ � ����������� ��
        for (int i = 0; i < currentActionPoints; i++)
        {
            GameObject newCircle = Instantiate(circlePrefab, circleGroup);
            circles.Add(newCircle);
        }
    }

    // ������� ��� ���������� �������� ���������� ��
    public void UpdateActionPoints(int newActionPoints)
    {
        currentActionPoints = newActionPoints;
        UpdateActionPointsUI();
    }
}
