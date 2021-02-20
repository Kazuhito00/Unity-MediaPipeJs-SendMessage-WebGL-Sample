using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawHands : MonoBehaviour
{
    // 描画エリアサイズ
    const int width = 320;
    const int height = 180;

    // ペイント関連
    Color[] drawBuffer;
    Texture2D drawTexture;
    Vector2 prevPoint;

    void Start()
    {
        //  描画用バッファ
        drawBuffer = new Color[width * height];

        // 描画用テクスチャ準備
        drawTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        drawTexture.filterMode = FilterMode.Point;
        
        ClearCanvas();
    }

    void Update()
    {
        // 描画バッファをテクスチャへ反映
        drawTexture.SetPixels(drawBuffer);
        drawTexture.Apply();
        GetComponent<Renderer>().material.mainTexture = drawTexture;
    }

    private void DrawPoint(Vector2 point, Color color, double brushSize = 1.5f)
    {
        // 点描画
        point.x = (int)point.x;
        point.y = (int)point.y;

        int start_x = Mathf.Max(0, (int)(point.x - (brushSize - 1)));
        int end_x = Mathf.Min(drawTexture.width, (int)(point.x + (brushSize + 1)));
        int start_y =  Mathf.Max(0, (int)(point.y - (brushSize - 1)));
        int end_y = Mathf.Min(drawTexture.height, (int)(point.y + (brushSize + 1)));

        for (int x = start_x; x < end_x; x++) {
            for (int y = start_y; y < end_y; y++) {
                double length = Mathf.Sqrt(Mathf.Pow(point.x - x, 2) + Mathf.Pow(point.y - y, 2));
                if (length < brushSize) {
                    drawBuffer.SetValue(color, x + drawTexture.width * y);
                }
            }
        }
    }

    private void DrawLine(Vector2 point1, Vector2 point2, Color color, double brushSize = 1.5f, int lerpNum = 50)
    {
        // 線描画
        for(int i=0; i < lerpNum + 1; i++) {
            var point = Vector2.Lerp(point1, point2, i * (1.0f / lerpNum));
            DrawPoint(point, color, brushSize);
        }
    }

    public void ClearCanvas()
    {
        // キャンバスを白色で初期化
        int color_r = 255;
        int color_g = 255;
        int color_b = 255;
        Color color = new Color((float)(color_r/255f), (float)(color_g/255f), (float)(color_b/255f));
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var drawPoint = new Vector2(x, y);
                DrawPoint(drawPoint, color, 1.0f);
            }
        }
    }

    public void DrawHandsLandmark(string landmarkJson)
    {
        int color_r = 0;
        int color_g = 0;
        int color_b = 0;
        Color color = new Color((float)(color_r/255f), (float)(color_g/255f), (float)(color_b/255f));

        Landmark[] landmarks = JsonHelper.FromJson<Landmark>(landmarkJson);

        // 掌
        var drawPoint1 = new Vector2((int)(landmarks[0].x * width), (int)(landmarks[0].y * height));
        var drawPoint2 = new Vector2((int)(landmarks[1].x * width), (int)(landmarks[1].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        drawPoint1 = new Vector2((int)(landmarks[0].x * width), (int)(landmarks[0].y * height));
        drawPoint2 = new Vector2((int)(landmarks[5].x * width), (int)(landmarks[5].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        drawPoint1 = new Vector2((int)(landmarks[5].x * width), (int)(landmarks[5].y * height));
        drawPoint2 = new Vector2((int)(landmarks[9].x * width), (int)(landmarks[9].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        drawPoint1 = new Vector2((int)(landmarks[9].x * width), (int)(landmarks[9].y * height));
        drawPoint2 = new Vector2((int)(landmarks[13].x * width), (int)(landmarks[13].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        drawPoint1 = new Vector2((int)(landmarks[13].x * width), (int)(landmarks[13].y * height));
        drawPoint2 = new Vector2((int)(landmarks[17].x * width), (int)(landmarks[17].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        drawPoint1 = new Vector2((int)(landmarks[17].x * width), (int)(landmarks[17].y * height));
        drawPoint2 = new Vector2((int)(landmarks[0].x * width), (int)(landmarks[0].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        // 親指
        drawPoint1 = new Vector2((int)(landmarks[1].x * width), (int)(landmarks[1].y * height));
        drawPoint2 = new Vector2((int)(landmarks[2].x * width), (int)(landmarks[2].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[2].x * width), (int)(landmarks[2].y * height));
        drawPoint2 = new Vector2((int)(landmarks[3].x * width), (int)(landmarks[3].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[3].x * width), (int)(landmarks[3].y * height));
        drawPoint2 = new Vector2((int)(landmarks[4].x * width), (int)(landmarks[4].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        // 人差指
        drawPoint1 = new Vector2((int)(landmarks[5].x * width), (int)(landmarks[5].y * height));
        drawPoint2 = new Vector2((int)(landmarks[6].x * width), (int)(landmarks[6].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[6].x * width), (int)(landmarks[6].y * height));
        drawPoint2 = new Vector2((int)(landmarks[7].x * width), (int)(landmarks[7].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[7].x * width), (int)(landmarks[7].y * height));
        drawPoint2 = new Vector2((int)(landmarks[8].x * width), (int)(landmarks[8].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        // 中指
        drawPoint1 = new Vector2((int)(landmarks[9].x * width), (int)(landmarks[9].y * height));
        drawPoint2 = new Vector2((int)(landmarks[10].x * width), (int)(landmarks[10].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[10].x * width), (int)(landmarks[10].y * height));
        drawPoint2 = new Vector2((int)(landmarks[11].x * width), (int)(landmarks[11].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[11].x * width), (int)(landmarks[11].y * height));
        drawPoint2 = new Vector2((int)(landmarks[12].x * width), (int)(landmarks[12].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        // 薬指
        drawPoint1 = new Vector2((int)(landmarks[13].x * width), (int)(landmarks[13].y * height));
        drawPoint2 = new Vector2((int)(landmarks[14].x * width), (int)(landmarks[14].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[14].x * width), (int)(landmarks[14].y * height));
        drawPoint2 = new Vector2((int)(landmarks[15].x * width), (int)(landmarks[15].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[15].x * width), (int)(landmarks[15].y * height));
        drawPoint2 = new Vector2((int)(landmarks[16].x * width), (int)(landmarks[16].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        // 小指
        drawPoint1 = new Vector2((int)(landmarks[17].x * width), (int)(landmarks[17].y * height));
        drawPoint2 = new Vector2((int)(landmarks[18].x * width), (int)(landmarks[18].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[18].x * width), (int)(landmarks[18].y * height));
        drawPoint2 = new Vector2((int)(landmarks[19].x * width), (int)(landmarks[19].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);
        
        drawPoint1 = new Vector2((int)(landmarks[19].x * width), (int)(landmarks[19].y * height));
        drawPoint2 = new Vector2((int)(landmarks[20].x * width), (int)(landmarks[20].y * height));
        DrawLine(drawPoint1, drawPoint2, color, 1.0f);

        // キーポイント
        for (int i = 0; i < landmarks.Length; i++) {
            var drawPoint = new Vector2((int)(landmarks[i].x * width), (int)(landmarks[i].y * height));
            DrawPoint(drawPoint, color, 2.0f);
        }
    }
}

[System.Serializable]
public class Landmark
{
    public float x;
    public float y;
    public float z;
}

public static class JsonHelper
{
    // 参考：https://takap-tech.com/entry/2021/02/02/222406

    /// <summary>
    /// 指定した string を Root オブジェクトを持たない JSON 配列と仮定してデシリアライズします。
    /// </summary>
    public static T[] FromJson<T>(string json)
    {
        // ダミーノード追加
        string dummy_json = $"{{\"{DummyNode<T>.ROOT_NAME}\": {json}}}";

        // ダミーのルートにデシリアライズしてから中身の配列を返す
        var obj = JsonUtility.FromJson<DummyNode<T>>(dummy_json);
        return obj.array;
    }

    /// <summary>
    /// 指定した配列やリストなどのコレクションを Root オブジェクトを持たない JSON 配列に変換します。
    /// </summary>
    public static string ToJson<T>(IEnumerable<T> collection)
    {
        string json = JsonUtility.ToJson(new DummyNode<T>(collection)); // ダミールートごとシリアル化する
        int start = DummyNode<T>.ROOT_NAME.Length + 4;
        int len = json.Length - start - 1;
        return json.Substring(start, len); // 追加ルートの文字を取り除いて返す
    }

    // 内部で使用するダミーのルート要素
    [System.Serializable]
    private struct DummyNode<T>
    {
        // JSONに付与するダミールートの名称
        public const string ROOT_NAME = nameof(array);
        // 疑似的な子要素
        public T[] array;
        // コレクション要素を指定してオブジェクトを作成する
        public DummyNode(IEnumerable<T> collection) => this.array = collection.ToArray();
    }
}
