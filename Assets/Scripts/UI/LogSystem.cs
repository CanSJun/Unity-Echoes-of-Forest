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
            //������ �ִ밪�̶�� �������� �ϳ��� �����ϴ� ������� ����.
            //�� ���� 20�̶�� 21��°�� ���ö� ������ ������ �ؽ�Ʈ ������Ʈ�� �ı��ϴ� �������� ������ ��.
            Destroy(_scroll.content.GetChild(0).gameObject);
        }
    }
}
