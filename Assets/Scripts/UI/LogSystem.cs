using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogSystem : MonoBehaviour
{
    private static LogSystem my;
    public static LogSystem _instance { get { return my; } }

    [SerializeField] GameObject _logText;
    [SerializeField] ScrollRect _scroll;
    [SerializeField] int MAX = 20;
    private void Awake() => my = this; 

    public void UpdateLog(string logText)
    {
        
        GameObject txt = Instantiate(_logText);
        txt.transform.SetParent(transform);
        txt.GetComponent<Text>().text = logText;
        Canvas.ForceUpdateCanvases();
        StartCoroutine(AutoScrollDown());
        CheckChild();
    }
    IEnumerator AutoScrollDown()
    {
        yield return new WaitForEndOfFrame();
        _scroll.verticalNormalizedPosition = 0f; 
    }
    private void CheckChild()
    {
        if(_scroll.content.childCount > MAX)
        {
            //설정한 최대값이라면 맨위부터 하나씩 삭제하는 방식으로 구현.
            //즉 현재 20이라면 21번째가 들어올때 맨위에 적었던 텍스트 오브젝트를 파괴하는 형식으로 구현을 함.
            Destroy(_scroll.content.GetChild(0).gameObject);
        }
    }
}
