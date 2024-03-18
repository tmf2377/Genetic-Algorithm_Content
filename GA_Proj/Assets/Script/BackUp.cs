using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
public class BallGenerator : MonoBehaviour
{
    public GameObject ballPrefab; // 공 프리팹
    public GameObject selectPin;

    public Dropdown encodingLengthDropdown; // 인코딩 길이 Dropdown
    public Dropdown numberDropdown; // 인구 생성 Dropdown
    public Button autoSelectButton; // 자동 선택 버튼
    public Dropdown crossoverMethodDropdown; // 교차 방식 선택 Dropdown
    public Dropdown mutationDropdown; // Mutation rate 선택 Dropdown
    public Dropdown replacementDropdown; // 대체 방식 선택 Dropdown

    public int numberOfBalls = 10; // 생성할 공의 수
    public int stringLength = 10; // 이진 문자열의 길이
    public float selectTime = 0.5f; // 자동 선택 시간
    public Text generationText; // 세대 정보를 표시할 UI 텍스트
    public Text selectedBallText; // 선택된 공의 이진 문자열을 표시할 UI 텍스트
    private float[] mutationRates = new float[] { 1f, 0.1f, 0.01f, 0.001f }; // Mutation 확률

    private int xRange = 10;
    private int yRange = 2;
    private int zRange = 10;
    private List<GameObject> balls = new List<GameObject>(); // 생성된 공들을 저장하는 리스트


    void Start()
    {
        InitializeDropdown();
        InitializeEncodingLengthDropdown(); // 인코딩 길이 Dropdown 초기화
        InitializeMutationDropdown(); // Mutation Dropdown 초기화
        InitializeReplacementDropdown(); // 대체 방식 Dropdown 초기화
        UpdateGenerationText(); // UI 텍스트 초기화

        // 버튼 클릭 리스너 추가
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
                    PerformReplacementIfReady(); // 선택 상태 업데이트 후 교차 가능 여부 확인
                }
            }
        }
    }

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 초기화 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    void InitializeDropdown() // 인구 Dropdown 초기화
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

    void InitializeEncodingLengthDropdown() // 인코딩 길이 Dropdown 초기화
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

    void InitializeReplacementDropdown() // 교차 방법 초기화
    {
        replacementDropdown.ClearOptions();
        List<string> options = new List<string> { "Steady-State", "Generational" };
        replacementDropdown.AddOptions(options);
    }

    void InitializeMutationDropdown() // Mutation Dropdown 초기화
    {
        mutationDropdown.ClearOptions();
        List<string> options = new List<string> { "1%", "0.1%", "0.01%", "0.001%" };
        mutationDropdown.AddOptions(options);
    }


    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 인구 생성 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    public void GenerateBalls() // 인구 생성
    {
        // 기존 공들을 제거
        foreach (var ball in balls)
        {
            Destroy(ball);
        }
        balls.Clear();

        // 새로운 공들 생성
        for (int i = 0; i < numberOfBalls; i++)
        {
            CreateBall(GetRandomPosition());
        }
    }

    void DropdownValueChanged(Dropdown dropdown) // Dropdown 값 변경 시 인구 생성
    {
        numberOfBalls = int.Parse(dropdown.options[dropdown.value].text);
        GenerateBalls();
    }

    GameObject CreateBall(Vector3 position)  // 공 생성 및 초기화
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        string binaryString = GenerateBinaryString(stringLength);
        BallData ballData = ball.AddComponent<BallData>(); // 이진 문자열과 선택 상태를 저장하는 컴포넌트 추가
        ballData.binaryString = binaryString;
        ballData.fitness = CalculateFitness(binaryString);
        ball.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, ballData.fitness);
        balls.Add(ball);
        return ball;
    }

    Vector3 GetRandomPosition() // 랜덤 위치 생성
    {
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(0.5f, yRange);
        float z = Random.Range(-zRange, zRange);
        return new Vector3(x, y, z);
    }

    string GenerateBinaryString(int length) // 이진 문자열 생성
    {
        string result = "";
        for (int i = 0; i < length; i++)
        {
            result += Random.Range(0, 2).ToString();
        }
        return result;
    }

    GameObject CreateBall(Vector3 position, string binaryString = null, int generation = 0)  // 공 생성 함수 수정 (세대 정보 업데이트 후 UI 텍스트 업데이트)
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

        // TextMeshPro 컴포넌트 찾기 및 세대 정보 업데이트
        var textMeshPro = ball.GetComponentInChildren<TextMeshPro>();
        if (textMeshPro != null)
        {
            textMeshPro.text = $"{generation}";
        }

        balls.Add(ball);
        return ball;
    }

    void UpdateGenerationText()    // 세대 정보 업데이트 및 UI 텍스트 업데이트 함수
    {
        int minGeneration = GetMinGeneration();
        generationText.text = "Generation: " + minGeneration;
    }

    int GetMinGeneration()     // 가장 작은 세대 찾기
    {
        if (balls.Count == 0)
        {
            return 0; // 공이 없을 경우 0세대로 가정
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

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 인코딩/적합도 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    void EncodingLengthDropdownValueChanged(Dropdown dropdown) // 인코딩 길이 Dropdown 값 변경 시 호출
    {
        stringLength = int.Parse(dropdown.options[dropdown.value].text);
    }

    float CalculateFitness(string binaryString) // 적합도 계산
    {
        int count = 0;
        foreach (char c in binaryString)
        {
            if (c == '1') count++;
        }
        return (float)count / binaryString.Length;
    }

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 선택 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    void ToggleBallSelection(GameObject ball)  // 공 선택 상태 토글
    {
        int minGeneration = GetMinGeneration();
        BallData ballData = ball.GetComponent<BallData>();

        if (ballData.generation == minGeneration)
        {
            ballData.isSelected = !ballData.isSelected;
            Transform pinTransform = ball.transform.Find("SelectPin");

            if (ballData.isSelected)
            {
                // 공이 선택된 경우, 해당 공의 이진 문자열을 UI 텍스트에 표시
                selectedBallText.text = "" + ballData.binaryString;

                if (pinTransform == null) // selectPin이 없으면 생성
                {
                    GameObject pin = Instantiate(selectPin, ball.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                    pin.name = "SelectPin";
                    pin.transform.SetParent(ball.transform);
                }
            }
            else
            {
                // 공이 선택 해제된 경우, UI 텍스트를 비움
                selectedBallText.text = "";

                if (pinTransform != null) // 이미 있는 selectPin 제거
                {
                    Destroy(pinTransform.gameObject);
                }
            }
        }
    }

    void DeselectAllBalls() // 선택 해제
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
        // 선택된 공 표시 텍스트 업데이트
        selectedBallText.text = "";
    }

    void AutoSelectBalls() // 자동 선택 버튼 클릭 시 호출되는 함수
    {
        StartCoroutine(AutoSelectCoroutine());
    }

    IEnumerator AutoSelectCoroutine() // 룰렛 휠 방식으로 자동 선택을 진행하는 코루틴
    {
        // 버튼을 눌렀을 때의 최소 세대 저장
        int currentMinGeneration = GetMinGeneration();

        while (true)
        {
            // 버튼 비활성화
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
                // 버튼 비활성화
                autoSelectButton.interactable = true;
                break; // 코루틴 중단
            }
        }
    }

    bool GetMinGenerationBallsSelected(int currentMinGeneration) // 최신 세대의 모든 공이 선택되었는지 확인하는 함수
    {
        foreach (var ball in balls)
        {
            BallData ballData = ball.GetComponent<BallData>();
            if (ballData.generation == currentMinGeneration && !ballData.isSelected)
            {
                return false; // 선택되지 않은 공이 있으면 false 반환
            }
        }
        return true; // 모든 공이 선택되었다면 true 반환
    }

    void RouletteWheelSelection() // 룰렛 휠 선택 메커니즘
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
                // 선택된 공 처리
                ToggleBallSelection(ball);
                break;
            }
        }
    }

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 교차 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    List<string> OnePointCrossover(string parent1, string parent2) // One-point crossover 구현
    {
        int length = parent1.Length;
        int point = Random.Range(0, length);

        string child1 = parent1.Substring(0, point) + parent2.Substring(point);
        string child2 = parent2.Substring(0, point) + parent1.Substring(point);

        return new List<string> { child1, child2 };
    }

    List<string> TwoPointCrossover(string parent1, string parent2) // Two-point crossover 구현
    {
        int length = parent1.Length;
        int point1 = Random.Range(0, length);
        int point2 = Random.Range(point1, length);

        string child1 = parent1.Substring(0, point1) + parent2.Substring(point1, point2 - point1) + parent1.Substring(point2);
        string child2 = parent2.Substring(0, point1) + parent1.Substring(point1, point2 - point1) + parent2.Substring(point2);

        return new List<string> { child1, child2 };
    }

    List<string> ThreePointCrossover(string parent1, string parent2)  // Three-point crossover 구현
    {
        int length = parent1.Length;
        int point1 = Random.Range(0, length);
        int point2 = Random.Range(point1, length);
        int point3 = Random.Range(point2, length);

        string child1 = parent1.Substring(0, point1) + parent2.Substring(point1, point2 - point1) + parent1.Substring(point2, point3 - point2) + parent2.Substring(point3);
        string child2 = parent2.Substring(0, point1) + parent1.Substring(point1, point2 - point1) + parent2.Substring(point2, point3 - point2) + parent1.Substring(point3);

        return new List<string> { child1, child2 };
    }

    List<string> UniformCrossover(string parent1, string parent2)   // Uniform crossover 구현
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

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 변이 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    void ApplyMutationToChildren()  // 자식에 Mutation 적용
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

    void MutateBall(BallData ballData)  // Mutation 처리
    {
        char[] binaryStringArray = ballData.binaryString.ToCharArray();
        int mutateIndex = Random.Range(0, binaryStringArray.Length);
        binaryStringArray[mutateIndex] = binaryStringArray[mutateIndex] == '0' ? '1' : '0';
        ballData.binaryString = new string(binaryStringArray);
        Debug.Log("Mutate!");
    }

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 세대 교체 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    void PerformReplacementIfReady() // 대체 준비
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
            // 자식 생성 후 모든 공의 선택 상태 해제
            DeselectAllBalls();

            // Mutation 적용
            ApplyMutationToChildren();
        }
    }

    GameObject FindWorstBallInGeneration(int generation) // 가장 적합도 낮은 공 찾기
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

    void PerformReplacementWithWorst(GameObject parent1, GameObject parent2) // Worst와 교체
    {
        var children = TwoPointCrossover(parent1.GetComponent<BallData>().binaryString,
                                         parent2.GetComponent<BallData>().binaryString);
        int nextGeneration = Mathf.Max(parent1.GetComponent<BallData>().generation,
                                       parent2.GetComponent<BallData>().generation) + 1;

        foreach (var child in children)
        {
            CreateBall(GetRandomPosition(), child, nextGeneration);
        }

        // 랜덤으로 한 부모와 적합도가 가장 낮은 공 제거
        GameObject ballToRemove = Random.value < 0.5f ? parent1 : parent2;
        RemoveBall(ballToRemove);
        RemoveBall(FindWorstBallInGeneration(ballToRemove.GetComponent<BallData>().generation));
    }

    bool AllBallsSameGeneration(List<GameObject> selectedBalls, int generation)  // 모든 선택된 공이 동일한 세대인지 확인하는 함수
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

    List<GameObject> GetSelectedBalls()   // 선택된 공들을 가져오는 함수
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

    void PerformStandardReplacement(GameObject parent1, GameObject parent2) // 부모 공 선택 및 대체 수행 함수
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

    void RemoveBall(GameObject ball)  // 공 제거 함수
    {
        balls.Remove(ball);
        Destroy(ball);
        UpdateGenerationText(); // 세대 정보 업데이트
    }

}

// 각 공에 대한 데이터를 저장하는 컴포넌트
public class BallData : MonoBehaviour
{
    public string binaryString; // 이진 문자열
    public float fitness; // 적합도
    public bool isSelected = false; // 선택 상태
    public int generation;
}
*/