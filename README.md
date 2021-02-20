# Unity-Barracuda-MobileNet-WebGL-Sample
Unity Barracudaを用いてMobileNet(画像クラス分類)をWebGL上で推論するサンプルです。<br>
現時点(2021/02/09)でUnityのWebGLはCPU推論のみのサポートです。<br>
GPU推論と比較しパフォーマンスは出ません。GPU推論やAndroid上での実行時の参考FPSを後述しています。<br>
![v9g8l-5fqfl](https://user-images.githubusercontent.com/37477845/107378942-d1a7c480-6b2f-11eb-9e4f-ff17a466685e.gif)

# Demo
動作確認用ページは以下。<br>
MobileNetV1のデモです。<br>
[https://kazuhito00.github.io/Unity-Barracuda-MobileNet-WebGL-Sample/WebGL-Build](https://kazuhito00.github.io/Unity-Barracuda-MobileNet-WebGL-Sample/WebGL-Build/index.html)

# FPS(参考値)
WebCamController.cs の Update()の呼び出し周期を計測したものです。<br>
以下のように動作は基本的に非同期処理のため、FPSは見かけ上のFPSであり、推論自体のFPSではありません。<br>
　CSharpBurst：非同期<br>
　CSharpRef：同期<br>
　ComputePrecompiled：非同期
|  | MobileNetV1 | MobileNetV2 |
| - | :- | :- |
| WebGL<br>CPU：Core i7-8750H CPU @2.20GHz | 約2.2FPS<br>CSharpBurst | 約0.08FPS<br>CSharpRef<br>※CSharpBurstで動作せず |
| WebGL<br>CPU：Core i5-5200U CPU @2.20GHz | 約1.1FPS<br>CSharpBurst | 未計測 |
| Android<br>Google Pixel4a(Snapdragon 730G) | 約1.9FPS<br>ToDo:Vulkan設定確認後に再計測<br>ComputePrecompiled | 約1.8FPS<br>ToDo:Vulkan設定確認後に再計測<br>ComputePrecompiled |
| Unity Editor<br>GPU：GTX 1050 Ti Max-Q(4GB) | 約45FPS<br>ComputePrecompiled | 約41FPS<br>ComputePrecompiled |

# Requirement (Unity)
* Unity 2020.1.6f1 or later
* Barracuda 1.3.0 or later

# Requrement (Python) ※ONNX変換をする場合のみ
* Tensorflow 2.4.0 or later
* tf2onnx 1.8.2 or later
* onnxruntime 1.6.0 or later(※ONNX変換後の推論をテストする場合のみ)

# Reference
* [Barracuda 1.3.0 preview](https://docs.unity3d.com/Packages/com.unity.barracuda@1.3/manual/index.html)
* [【Unity】WebGLで日本語テキストが表示されない問題について](https://chiritsumo-blog.com/unity-webgl-japanese/)
* [Texutre2Dのサイズ変更【Unity】](https://kan-kikuchi.hatenablog.com/entry/TextureScale)
* [【Unity】FPS を計測するスクリプト](https://baba-s.hatenablog.com/entry/2019/05/04/220500)

# ToDo
- [x] ~~Androidビルドの設定確認<br>参考：[Unity で Vulkan API を使用する](http://nakamura001.hatenablog.com/entry/20170621/1498030760)、[barracuda-release:issues85](https://github.com/Unity-Technologies/barracuda-release/issues/85)、[Unityで取得できるシステム情報](https://qiita.com/MARQUE/items/fd08fe57cbe4e0b53aa8)~~
- [x] ~~[Androidの画面向きを横画面に固定する](https://deve-cat.com/unity-screen-rotation/)~~

# Author
高橋かずひと(https://twitter.com/KzhtTkhs)
 
# License 
Unity-Barracuda-MobileNet-WebGL-Sample is under [Apache-2.0 License](LICENSE).

# Licence(Font)
[Noto Sans JP fonts](https://fonts.google.com/specimen/Noto+Sans+JP) is under [Open Font License](https://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL).
