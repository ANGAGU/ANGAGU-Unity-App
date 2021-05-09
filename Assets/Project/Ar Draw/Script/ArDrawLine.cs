using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArDrawLine : MonoBehaviour
{
    public Transform _pivotPoint; // 위치 정보 가져올 변수

    public GameObject _lineRenderePrefabs; // Line Prefab 가져올

    private LineRenderer _lineRenderer; // 현재 제작되어 사용할 라인 렌더러 저장할 변수

    public List<LineRenderer> _lineList = new List<LineRenderer>(); // 그려지는 라인들을 저장해서 이 후 CRUD 가능하도록

    public Transform _linePool; // 실제로 라인들이 위치하고 있는 위치들

    public bool _use; // 해당 코드가 사용되고 있는지 아닌지

    public bool _startLIne; // 만들어준 라인 랜더러가 사용중인지 아닌지
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_use && _startLIne)
        {
            DrawLineConintue();
        }
    }

    public void MakeLineRenderer()
    {
        GameObject tLine = Instantiate(_lineRenderePrefabs);
        // prefab 을 오브젝트로 만들어주고
        tLine.transform.SetParent(_linePool);
        // SetParent 를 이요해서 linePool 에 저장
        tLine.transform.position = Vector3.zero;
        tLine.transform.localScale = new Vector3(1, 1, 1);
        // position 과 localScale 을 초기화

        _lineRenderer = tLine.GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _pivotPoint.position);
        // 초기값 연동

        _startLIne = true;
        _lineList.Add(_lineRenderer);
    }

    public void DrawLineConintue() // 그려주는 거 
    {
        _lineRenderer.positionCount = _lineRenderer.positionCount + 1;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _pivotPoint.position);
        // 새로 그려지는 포인트 처리
        
        
    }

    public void StartDrawLine()
    {
        _use = true;
        if (!_startLIne)
        {
            MakeLineRenderer();
        }
    }

    public void StopDrawLine() // 그리기 종료
    {
        _use = false;
        _startLIne = false;
        _lineRenderer = null;
    }
}
