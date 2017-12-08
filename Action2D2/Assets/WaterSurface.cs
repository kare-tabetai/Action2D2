using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour {

    class InvertedSprite//wip タイエルマップレンダラーに対応する
    {
        readonly Vector2 pivot;
        readonly GameObject original;
        readonly SpriteRenderer originalRenderer;
        public GameObject inverted;
        SpriteRenderer invertedRenderer;
        public InvertedSprite(GameObject origin,Vector2 pv)
        {
            pivot = pv;
            original = origin;
            originalRenderer = original.GetComponent<SpriteRenderer>();

            inverted = new GameObject();
            var ydist = original.transform.position.y - pivot.y;
            var pos = pivot + Vector2.down * ydist;
            inverted.transform.position = pos;
            inverted.transform.localScale = original.transform.lossyScale;

            invertedRenderer = inverted.AddComponent<SpriteRenderer>();
            invertedRenderer.sprite = originalRenderer.sprite;
            invertedRenderer.flipY = !originalRenderer.flipY;
        }

        public void Update()
        {
            var ydist = original.transform.position.y - pivot.y;
            var pos = new Vector2(original.transform.position.x,pivot.y - ydist);
            inverted.transform.position = pos;

            invertedRenderer.sprite = originalRenderer.sprite;
        }
        public bool DestroyCheck(GameObject gameObject)
        {
            if (original == gameObject)
            {
                Destroy(inverted);
                return true;
            }
            return false;
        }
    }

    List<InvertedSprite> sprites = new List<InvertedSprite>(); 
	void Start () {
    }
	
	void Update () {
        sprites.ForEach( a => a.Update());
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        CreateInvertedSprite(col.gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        sprites.RemoveAll(a => a.DestroyCheck(col.gameObject));
    }

    GameObject CreateInvertedSprite(GameObject origin)
    {
        if (!origin.GetComponent<SpriteRenderer>()) return null;
        var invertedSprite = new InvertedSprite(origin,transform.position);
        sprites.Add(invertedSprite);
        return invertedSprite.inverted;
    }
}
