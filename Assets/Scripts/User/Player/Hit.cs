using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hit : MonoBehaviour
{
    [SerializeField] private GameObject _Text;
    [SerializeField] private GameObject _Effect;
    [SerializeField] private GameObject _HPBAR;



    public void Hitted(ref float now,float  max, ref bool die, float damage, Transform pos)
    {
        if (die == true) return;
        GameObject T = Instantiate(_Text);
        T.transform.position = pos.position;
        T.GetComponent<TextControl>()._color = Color.red;
        T.GetComponent<TextControl>()._Size = 5;
        T.GetComponent<TextControl>()._Text = damage.ToString();

        if (now <= damage)
        {
            die = true;
            now = 0;
            _HPBAR.GetComponent<HPMP_Control>().ChangeValue(0);
            UserControl._instance._nowHP = 0;
            UserControl._instance._Action.SetTrigger("Die");
            UserControl._instance.Die.SetActive(true);
            UserControl._instance._CurrentState = UserControl.State.Die;
            return;
        }

        if (!UserControl._instance._isCheck)
        {
            UserControl._instance._isCheck = true;
            _Effect.SetActive(true);
            now -= damage;
            _HPBAR.GetComponent<HPMP_Control>().ChangeValue(damage / max);
            UserControl._instance._nowHP = now;
            StartCoroutine(Hitted());
        }
    }

    IEnumerator Hitted()
    {

        Image img = _Effect.GetComponent<Image>();
        Color color = img.color;
        yield return new WaitForSeconds(0.2f);
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0.13f, 0f, i / 0.5f);
            img.color = color;
            yield return null;
        }
        UserControl._instance._isCheck = false;
    }


}
