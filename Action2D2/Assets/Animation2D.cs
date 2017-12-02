using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation2D : MonoBehaviour {
    [SerializeField]
    string memo;
    public bool autoDestroy;
    public Sprite[] sprites;
    public float animationRate;

    [System.NonSerialized]
    public int nowSprite = 0;
    SpriteRenderer sprRenderer;
    float timer = 0;
    float fadeTime = -1;
    float fadeTimer = 0;
    void Start () {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        Animation();
        if (0 < fadeTime) FadeOutAnimation();
    }

    void Animation()
    {
        timer += Time.deltaTime;
        if (timer < animationRate) return;

        timer = 0;
        nowSprite++;
        if (sprites.Length <= nowSprite && !autoDestroy) nowSprite = 0;
        if (sprites.Length <= nowSprite && autoDestroy)
        {
            Destroy(gameObject);
            return;
        }
        sprRenderer.sprite = sprites[nowSprite];
    }

    void FadeOutAnimation()
    {
        fadeTimer += Time.deltaTime;
        if(fadeTime < fadeTimer)
        {
            Destroy(gameObject);
            return;
        }

        Color col = sprRenderer.color;
        col.a = (fadeTime - fadeTimer) / fadeTime;
        sprRenderer.color = col;
    }

    //外部からこのメソッドを呼び出すことによってのみFade outする
    public void FadeOut(float fadeTime = 5.0f)
    {
        if (0 < this.fadeTime) Debug.LogError("FadeOut()にてすでにFade中です");
        if (fadeTime <= 0) Debug.LogError("FadeOut()に不正な値が入力されました");
        this.fadeTime = fadeTime;
    }

    public void ReSet()
    {
        timer = 0;
        fadeTimer = 0;
        fadeTime = -1;
        nowSprite = 0;
    }
}
