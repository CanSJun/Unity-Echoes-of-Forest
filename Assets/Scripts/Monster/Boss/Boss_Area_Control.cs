using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Boss_Area_Control : MonoBehaviour
{
    [SerializeField] Transform _Boss_Transform;
    [SerializeField] GameObject _HP;
    [SerializeField] Animator _Action;
    [SerializeField] Transform Point;
    [SerializeField] float speed = 1.0f;

    [SerializeField] AudioClip Scream;
    [SerializeField] AudioClip Fly;
    UserControl player;
    private Camera camera;
    private void OnTriggerEnter(Collider other)
    {
            #if UNITY_EDITOR
                    speed = 5.0f;
            #endif
        if (other.CompareTag("Player")) {

            player = other.GetComponent<UserControl>();
            camera = Camera.main;
            player._CurrentState = UserControl.State.Boss;

            player._Action.SetFloat("X", 0);
            player._Action.SetFloat("Y", 0);

            player._Action.Play("Idle,Walk");
            player._skilltype = 0;
            StartCoroutine(Moving());
        }
    }

    IEnumerator Moving()
    {

        while (Vector3.Distance(camera.transform.position, Point.position) > 0.1f)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, Point.position, Time.deltaTime * speed);
            camera.transform.LookAt(_Boss_Transform);
            yield return null;
        }
        SoundManager._instance.PrivatePlaySound(Fly, player.transform);
        _Action.SetTrigger("Jump");
        yield return new WaitForSeconds(0.5f);
        while (_Action.GetCurrentAnimatorStateInfo(0).IsName("Jump")) yield return null;
      
        _Action.SetTrigger("Scream");
        yield return new WaitForSeconds(0.75f);
        SoundManager._instance.PrivatePlaySound(Scream, player.transform, 0.7f);
        Vector3 pos = camera.transform.localPosition;
        float current = 0f;
        while (current < 2)
        {
            camera.transform.localPosition = pos + Random.insideUnitSphere * 0.35f;
            current += Time.deltaTime;
            yield return null;
        }
        while (_Action.GetCurrentAnimatorStateInfo(0).IsName("Scream")) yield return null; // Jump 애니메이션이 끝날 때까지 대기
        camera.transform.localPosition = pos;
        UserControl._instance._CurrentState = UserControl.State.Idle;
        _HP.gameObject.SetActive(true);
        Destroy(gameObject);

        yield return null;
    }


}
