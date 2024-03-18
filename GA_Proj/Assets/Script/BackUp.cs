using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
public class BallGenerator : MonoBehaviour
{
    public GameObject ballPrefab; // �� ������
    public GameObject selectPin;

    public Dropdown encodingLengthDropdown; // ���ڵ� ���� Dropdown
    public Dropdown numberDropdown; // �α� ���� Dropdown
    public Button autoSelectButton; // �ڵ� ���� ��ư
    public Dropdown crossoverMethodDropdown; // ���� ��� ���� Dropdown
    public Dropdown mutationDropdown; // Mutation rate ���� Dropdown
    public Dropdown replacementDropdown; // ��ü ��� ���� Dropdown

    public int numberOfBalls = 10; // ������ ���� ��
    public int stringLength = 10; // ���� ���ڿ��� ����
    public float selectTime = 0.5f; // �ڵ� ���� �ð�
    public Text generationText; // ���� ������ ǥ���� UI �ؽ�Ʈ
    public Text selectedBallText; // ���õ� ���� ���� ���ڿ��� ǥ���� UI �ؽ�Ʈ
    private float[] mutationRates = new float[] { 1f, 0.1f, 0.01f, 0.001f }; // Mutation Ȯ��

    private int xRange = 10;
    private int yRange = 2;
    private int zRange = 10;
    private List<GameObject> balls = new List<GameObject>(); // ������ ������ �����ϴ� ����Ʈ


    void Start()
    {
        InitializeDropdown();
        InitializeEncodingLengthDropdown(); // ���ڵ� ���� Dropdown �ʱ�ȭ
        InitializeMutationDropdown(); // Mutation Dropdown �ʱ�ȭ
        InitializeReplacementDropdown(); // ��ü ��� Dropdown �ʱ�ȭ
        UpdateGenerationText(); // UI �ؽ�Ʈ �ʱ�ȭ

        // ��ư Ŭ�� ������ �߰�
        autoSelectButton.onClick.AddListener(AutoSelectBalls);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (balls.Contains(hitObject))
                {
                    ToggleBallSelection(hitObject);
                    PerformReplacementIfReady(); // ���� ���� ������Ʈ �� ���� ���� ���� Ȯ��
                }
            }
        }
    }

    // �ѤѤѤѤѤѤѤѤѤ� �ʱ�ȭ �ѤѤѤѤѤѤѤѤѤ�

    void InitializeDropdown() // �α� Dropdown �ʱ�ȭ
    {
        numberDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 10; i <= 100; i += 10)
        {
            options.Add(i.ToString());
        }
        numberDropdown.AddOptions(options);
        numberDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(numberDropdown); });
    }

    void InitializeEncodingLengthDropdown() // ���ڵ� ���� Dropdown �ʱ�ȭ
    {
        encodingLengthDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 10; i <= 100; i += 10)
        {
            options.Add(i.ToString());
        }
        encodingLengthDropdown.AddOptions(options);
        encodingLengthDropdown.onValueChanged.AddListener(delegate { EncodingLengthDropdownValueChanged(encodingLengthDropdown); });
    }

    void InitializeReplacementDropdown() // ���� ��� �ʱ�ȭ
    {
        replacementDropdown.ClearOptions();
        List<string> options = new List<string> { "Steady-State", "Generational" };
        replacementDropdown.AddOptions(options);
    }

    void InitializeMutationDropdown() // Mutation Dropdown �ʱ�ȭ
    {
        mutationDropdown.ClearOptions();
        List<string> options = new List<string> { "1%", "0.1%", "0.01%", "0.001%" };
        mutationDropdown.AddOptions(options);
    }


    // �ѤѤѤѤѤѤѤѤѤ� �α� ���� �ѤѤѤѤѤѤѤѤѤ�

    public void GenerateBalls() // �α� ����
    {
        // ���� ������ ����
        foreach (var ball in balls)
        {
            Destroy(ball);
        }
        balls.Clear();

        // ���ο� ���� ����
        for (int i = 0; i < numberOfBalls; i++)
        {
            CreateBall(GetRandomPosition());
        }
    }

    void DropdownValueChanged(Dropdown dropdown) // Dropdown �� ���� �� �α� ����
    {
        numberOfBalls = int.Parse(dropdown.options[dropdown.value].text);
        GenerateBalls();
    }

    GameObject CreateBall(Vector3 position)  // �� ���� �� �ʱ�ȭ
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        string binaryString = GenerateBinaryString(stringLength);
        BallData ballData = ball.AddComponent<BallData>(); // ���� ���ڿ��� ���� ���¸� �����ϴ� ������Ʈ �߰�
        ballData.binaryString = binaryString;
        ballData.fitness = CalculateFitness(binaryString);
        ball.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, ballData.fitness);
        balls.Add(ball);
        return ball;
    }

    Vector3 GetRandomPosition() // ���� ��ġ ����
    {
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(0.5f, yRange);
        float z = Random.Range(-zRange, zRange);
        return new Vector3(x, y, z);
    }

    string GenerateBinaryString(int length) // ���� ���ڿ� ����
    {
        string result = "";
        for (int i = 0; i < length; i++)
        {
            result += Random.Range(0, 2).ToString();
        }
        return result;
    }

    GameObject CreateBall(Vector3 position, string binaryString = null, int generation = 0)  // �� ���� �Լ� ���� (���� ���� ������Ʈ �� UI �ؽ�Ʈ ������Ʈ)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        BallData ballData = ball.AddComponent<BallData>();
        if (binaryString == null)
        {
            binaryString = GenerateBinaryString(stringLength);
        }
        ballData.binaryString = binaryString;
        ballData.fitness = CalculateFitness(binaryString);
        ballData.generation = generation;
        ball.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, ballData.fitness);

        // TextMeshPro ������Ʈ ã�� �� ���� ���� ������Ʈ
        var textMeshPro = ball.GetComponentInChildren<TextMeshPro>();
        if (textMeshPro != null)
        {
            textMeshPro.text = $"{generation}";
        }

        balls.Add(ball);
        return ball;
    }

    void UpdateGenerationText()    // ���� ���� ������Ʈ �� UI �ؽ�Ʈ ������Ʈ �Լ�
    {
        int minGeneration = GetMinGeneration();
        generationText.text = "Generation: " + minGeneration;
    }

    int GetMinGeneration()     // ���� ���� ���� ã��
    {
        if (balls.Count == 0)
        {
            return 0; // ���� ���� ��� 0����� ����
        }

        int minGen = int.MaxValue;
        foreach (var ball in balls)
        {
            int gen = ball.GetComponent<BallData>().generation;
            if (gen < minGen)
            {
                minGen = gen;
            }
        }
        return minGen;
    }

    // �ѤѤѤѤѤѤѤѤѤ� ���ڵ�/���յ� �ѤѤѤѤѤѤѤѤѤ�

    void EncodingLengthDropdownValueChanged(Dropdown dropdown) // ���ڵ� ���� Dropdown �� ���� �� ȣ��
    {
        stringLength = int.Parse(dropdown.options[dropdown.value].text);
    }

    float CalculateFitness(string binaryString) // ���յ� ���
    {
        int count = 0;
        foreach (char c in binaryString)
        {
            if (c == '1') count++;
        }
        return (float)count / binaryString.Length;
    }

    // �ѤѤѤѤѤѤѤѤѤ� ���� �ѤѤѤѤѤѤѤѤѤ�

    void ToggleBallSelection(GameObject ball)  // �� ���� ���� ���
    {
        int minGeneration = GetMinGeneration();
        BallData ballData = ball.GetComponent<BallData>();

        if (ballData.generation == minGeneration)
        {
            ballData.isSelected = !ballData.isSelected;
            Transform pinTransform = ball.transform.Find("SelectPin");

            if (ballData.isSelected)
            {
                // ���� ���õ� ���, �ش� ���� ���� ���ڿ��� UI �ؽ�Ʈ�� ǥ��
                selectedBallText.text = "" + ballData.binaryString;

                if (pinTransform == null) // selectPin�� ������ ����
                {
                    GameObject pin = Instantiate(selectPin, ball.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                    pin.name = "SelectPin";
                    pin.transform.SetParent(ball.transform);
                }
            }
            else
            {
                // ���� ���� ������ ���, UI �ؽ�Ʈ�� ���
                selectedBallText.text = "";

                if (pinTransform != null) // �̹� �ִ� selectPin ����
                {
                    Destroy(pinTransform.gameObject);
                }
            }
        }
    }

    void DeselectAllBalls() // ���� ����
    {
        foreach (var ball in balls)
        {
            BallData ballData = ball.GetComponent<BallData>();
            if (ballData.isSelected)
            {
                ballData.isSelected = false;
                Transform pinTransform = ball.transform.Find("SelectPin");
                if (pinTransform != null)
                    Destroy(pinTransform.gameObject);
            }
        }
        // ���õ� �� ǥ�� �ؽ�Ʈ ������Ʈ
        selectedBallText.text = "";
    }

    void AutoSelectBalls() // �ڵ� ���� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    {
        StartCoroutine(AutoSelectCoroutine());
    }

    IEnumerator AutoSelectCoroutine() // �귿 �� ������� �ڵ� ������ �����ϴ� �ڷ�ƾ
    {
        // ��ư�� ������ ���� �ּ� ���� ����
        int currentMinGeneration = GetMinGeneration();

        while (true)
        {
            // ��ư ��Ȱ��ȭ
            autoSelectButton.interactable = false;

            RouletteWheelSelection();
            yield return new WaitForSeconds(selectTime);

            List<GameObject> selectedBalls = GetSelectedBalls();
            if (selectedBalls.Count == 2)
            {
                PerformReplacementIfReady();
            }

            if (GetMinGenerationBallsSelected(currentMinGeneration))
            {
                // ��ư ��Ȱ��ȭ
                autoSelectButton.interactable = true;
                break; // �ڷ�ƾ �ߴ�
            }
        }
    }

    bool GetMinGenerationBallsSelected(int currentMinGeneration) // �ֽ� ������ ��� ���� ���õǾ����� Ȯ���ϴ� �Լ�
    {
        foreach (var ball in balls)
        {
            BallData ballData = ball.GetComponent<BallData>();
            if (ballData.generation == currentMinGeneration && !ballData.isSelected)
            {
                return false; // ���õ��� ���� ���� ������ false ��ȯ
            }
        }
        return true; // ��� ���� ���õǾ��ٸ� true ��ȯ
    }

    void RouletteWheelSelection() // �귿 �� ���� ��Ŀ����
    {
        float totalFitness = 0f;
        foreach (var ball in balls)
        {
            totalFitness += ball.GetComponent<BallData>().fitness;
        }

        float randomPoint = Random.value * totalFitness;
        float currentSum = 0f;

        foreach (var ball in balls)
        {
            currentSum += ball.GetComponent<BallData>().fitness;
            if (currentSum >= randomPoint)
            {
                // ���õ� �� ó��
                ToggleBallSelection(ball);
                break;
            }
        }
    }

    // �ѤѤѤѤѤѤѤѤѤ� ���� �ѤѤѤѤѤѤѤѤѤ�

    List<string> OnePointCrossover(string parent1, string parent2) // One-point crossover ����
    {
        int length = parent1.Length;
        int point = Random.Range(0, length);

        string child1 = parent1.Substring(0, point) + parent2.Substring(point);
        string child2 = parent2.Substring(0, point) + parent1.Substring(point);

        return new List<string> { child1, child2 };
    }

    List<string> TwoPointCrossover(string parent1, string parent2) // Two-point crossover ����
    {
        int length = parent1.Length;
        int point1 = Random.Range(0, length);
        int point2 = Random.Range(point1, length);

        string child1 = parent1.Substring(0, point1) + parent2.Substring(point1, point2 - point1) + parent1.Substring(point2);
        string child2 = parent2.Substring(0, point1) + parent1.Substring(point1, point2 - point1) + parent2.Substring(point2);

        return new List<string> { child1, child2 };
    }

    List<string> ThreePointCrossover(string parent1, string parent2)  // Three-point crossover ����
    {
        int length = parent1.Length;
        int point1 = Random.Range(0, length);
        int point2 = Random.Range(point1, length);
        int point3 = Random.Range(point2, length);

        string child1 = parent1.Substring(0, point1) + parent2.Substring(point1, point2 - point1) + parent1.Substring(point2, point3 - point2) + parent2.Substring(point3);
        string child2 = parent2.Substring(0, point1) + parent1.Substring(point1, point2 - point1) + parent2.Substring(point2, point3 - point2) + parent1.Substring(point3);

        return new List<string> { child1, child2 };
    }

    List<string> UniformCrossover(string parent1, string parent2)   // Uniform crossover ����
    {
        char[] child1 = new char[parent1.Length];
        char[] child2 = new char[parent2.Length];

        for (int i = 0; i < parent1.Length; i++)
        {
            bool takeFromParent1 = Random.value < 0.5f;
            child1[i] = takeFromParent1 ? parent1[i] : parent2[i];
            child2[i] = takeFromParent1 ? parent2[i] : parent1[i];
        }

        return new List<string> { new string(child1), new string(child2) };
    }

    // �ѤѤѤѤѤѤѤѤѤ� ���� �ѤѤѤѤѤѤѤѤѤ�

    void ApplyMutationToChildren()  // �ڽĿ� Mutation ����
    {
        float mutationRate = mutationRates[mutationDropdown.value];
        foreach (var ball in balls)
        {
            BallData ballData = ball.GetComponent<BallData>();
            if (ballData.generation == GetMinGeneration())
            {
                if (Random.Range(0f, 100f) < mutationRate)
                {
                    MutateBall(ballData);
                }
            }
        }
    }

    void MutateBall(BallData ballData)  // Mutation ó��
    {
        char[] binaryStringArray = ballData.binaryString.ToCharArray();
        int mutateIndex = Random.Range(0, binaryStringArray.Length);
        binaryStringArray[mutateIndex] = binaryStringArray[mutateIndex] == '0' ? '1' : '0';
        ballData.binaryString = new string(binaryStringArray);
        Debug.Log("Mutate!");
    }

    // �ѤѤѤѤѤѤѤѤѤ� ���� ��ü �ѤѤѤѤѤѤѤѤѤ�

    void PerformReplacementIfReady() // ��ü �غ�
    {
        List<GameObject> selectedBalls = GetSelectedBalls();
        if (selectedBalls.Count == 2 && AllBallsSameGeneration(selectedBalls, GetMinGeneration()))
        {
            string selectedMethod = replacementDropdown.options[replacementDropdown.value].text;
            switch (selectedMethod)
            {
                case "Generational":
                    PerformStandardReplacement(selectedBalls[0], selectedBalls[1]);
                    break;
                case "Steady-State":
                    PerformReplacementWithWorst(selectedBalls[0], selectedBalls[1]);
                    break;
            }
            // �ڽ� ���� �� ��� ���� ���� ���� ����
            DeselectAllBalls();

            // Mutation ����
            ApplyMutationToChildren();
        }
    }

    GameObject FindWorstBallInGeneration(int generation) // ���� ���յ� ���� �� ã��
    {
        GameObject worstBall = null;
        float lowestFitness = float.MaxValue;

        foreach (var ball in balls)
        {
            BallData ballData = ball.GetComponent<BallData>();
            if (ballData.generation == generation && ballData.fitness < lowestFitness)
            {
                worstBall = ball;
                lowestFitness = ballData.fitness;
            }
        }

        return worstBall;
    }

    void PerformReplacementWithWorst(GameObject parent1, GameObject parent2) // Worst�� ��ü
    {
        var children = TwoPointCrossover(parent1.GetComponent<BallData>().binaryString,
                                         parent2.GetComponent<BallData>().binaryString);
        int nextGeneration = Mathf.Max(parent1.GetComponent<BallData>().generation,
                                       parent2.GetComponent<BallData>().generation) + 1;

        foreach (var child in children)
        {
            CreateBall(GetRandomPosition(), child, nextGeneration);
        }

        // �������� �� �θ�� ���յ��� ���� ���� �� ����
        GameObject ballToRemove = Random.value < 0.5f ? parent1 : parent2;
        RemoveBall(ballToRemove);
        RemoveBall(FindWorstBallInGeneration(ballToRemove.GetComponent<BallData>().generation));
    }

    bool AllBallsSameGeneration(List<GameObject> selectedBalls, int generation)  // ��� ���õ� ���� ������ �������� Ȯ���ϴ� �Լ�
    {
        foreach (var ball in selectedBalls)
        {
            if (ball.GetComponent<BallData>().generation != generation)
            {
                return false;
            }
        }
        return true;
    }

    List<GameObject> GetSelectedBalls()   // ���õ� ������ �������� �Լ�
    {
        List<GameObject> selectedBalls = new List<GameObject>();
        foreach (var ball in balls)
        {
            if (ball.GetComponent<BallData>().isSelected)
            {
                selectedBalls.Add(ball);
            }
        }
        return selectedBalls;
    }

    void PerformStandardReplacement(GameObject parent1, GameObject parent2) // �θ� �� ���� �� ��ü ���� �Լ�
    {
        string parent1Binary = parent1.GetComponent<BallData>().binaryString;
        string parent2Binary = parent2.GetComponent<BallData>().binaryString;
        var children = TwoPointCrossover(parent1Binary, parent2Binary);
        int nextGeneration = Mathf.Max(parent1.GetComponent<BallData>().generation, parent2.GetComponent<BallData>().generation) + 1;

        foreach (var child in children)
        {
            CreateBall(GetRandomPosition(), child, nextGeneration);
        }

        RemoveBall(parent1);
        RemoveBall(parent2);
    }

    void RemoveBall(GameObject ball)  // �� ���� �Լ�
    {
        balls.Remove(ball);
        Destroy(ball);
        UpdateGenerationText(); // ���� ���� ������Ʈ
    }

}

// �� ���� ���� �����͸� �����ϴ� ������Ʈ
public class BallData : MonoBehaviour
{
    public string binaryString; // ���� ���ڿ�
    public float fitness; // ���յ�
    public bool isSelected = false; // ���� ����
    public int generation;
}
*/