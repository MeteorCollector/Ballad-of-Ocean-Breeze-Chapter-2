using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;

public class RotatingCam : MonoBehaviour
{
    public float rotateTime = 0.2f;
    public GameObject player;
    public Transform camPos;
    private Transform trans;
    public bool isInnerRing = false;
    private bool isRotating = false;
    public bool isFighting = false;
    private bool onFx = false;
    public PostProcessVolume myVolume;//不再是PostProcessVolume myVolume;
    private ChromaticAberration chro;
    public AudioClip IdleMusic;
    public AudioClip FightMusic;
    public AudioClip intoFight;
    public AudioSource Source;
    public GameObject FightUI;
    void Start()
    {
        trans = player.GetComponent<Transform>();// may be modified when multiplayer
        chro = myVolume.profile.GetSetting<ChromaticAberration>();
    }

    void Update()
    {
        if(!isInnerRing) transform.position = trans.position;
        Rotate();
    }

    void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isRotating && !isInnerRing)
        {
            StartCoroutine(RotateAround(-45, rotateTime));
        }
        if (Input.GetKeyDown(KeyCode.E) && !isRotating && !isInnerRing)
        {
            StartCoroutine(RotateAround(45, rotateTime));
        }
        if (Input.GetKeyDown(KeyCode.P) && isInnerRing)// 调试功能
        {
            //switchMode(isFighting);
            StartCoroutine(CameraShake(1));
        }
    }

    public void switchMode(bool toIdle)
    {
        if (!isRotating && isInnerRing)
        {
            if (toIdle)
            {
                FightUI.SetActive(false);
                StartCoroutine(RotateAround(-40, rotateTime));
                Source.clip = IdleMusic;
                Source.Play();
                chro.intensity.value = 0;
            }
            else
            {
                FightUI.SetActive(true);
                StartCoroutine(RotateAround(40, rotateTime));
                Source.clip = FightMusic;
                AudioSource.PlayClipAtPoint(intoFight, trans.position);
                Source.Play();
                if (!onFx) { StartCoroutine(FightFX(rotateTime)); }
            }
        }
    }

    IEnumerator RotateAround(float angle, float time)
    {
        float number = 60 * time;
        float nextAngle = angle / number;
        isRotating = true;

        for (int i = 0; i < number; i++)
        {
            if (isInnerRing) transform.Rotate(new Vector3(nextAngle, 0, 0));
            else transform.Rotate(new Vector3(0, 0, nextAngle));
            yield return new WaitForFixedUpdate();
        }

        isFighting = !isFighting;
        isRotating = false;
    }

    public IEnumerator FightFX(float time)// InnerRing
    {
        float number = 60 * time;
        onFx = true;
        for (int i = 0; i < number; i++)
        {
            chro.intensity.value = i / number;
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < number * 15; i++)
        {
            chro.intensity.value = (number * 8) / (i + number);
            yield return new WaitForFixedUpdate();
        }
        onFx = false;
        if (!isFighting) { chro.intensity.value = 0; }
    }

    public IEnumerator CameraShake(float shake)
    {
        //Vector3 originalPos = camPos.position;
        Debug.Log("shaking");
        while (shake >= 0.5f)
        {
            transform.position = new Vector3(
            UnityEngine.Random.Range(0f, shake * 2f) - shake + camPos.position.x,
            UnityEngine.Random.Range(0f, shake * 1f) - shake + camPos.position.y,
            camPos.position.z);
            shake = shake / 1.05f;
            yield return new WaitForFixedUpdate();
        }
        shake = 0;
        transform.position = camPos.position;
        yield return null;
    }
}