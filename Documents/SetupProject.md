# プロジェクトのセットアップ

このドキュメントはGitHubからこのプロジェクトをクローンしたところから始めます。

## Unityで開く

Unity Hubを開き、`リストに追加`ボタンからこのプロジェクトのフォルダを選択します。

プロジェクト一覧にプロジェクトが出てくるはずなので、クリックしてプロジェクトを開きます。

![UnityHub_リストに追加](https://github.com/nitsc-proclub/PiccoRoboGame/blob/main/Documents/Images/UnityHub_リストに追加.png)

プロジェクトを開くと、コンパイルエラーが原因でロードが止まります。
(Photonのライブラリを個別でインポートする必要があり、最初の状態ではコンパイルに必要なライブラリが存在しないためです)
参考: https://forum.photonengine.com/discussion/17810/namespace-errors-when-others-pull-my-project-from-git

すると`Enter Safe Mode?`というダイアログが出てくるので、`Enter Safe Mode`をクリックしてセーフモードに入ります。

![Unity_EnterSafeMode](https://github.com/nitsc-proclub/PiccoRoboGame/blob/main/Documents/Images/Unity_EnterSafeMode.png)

## PUNのインポート

Unityエディターが開いたら、ウィンドウ上のバーから`Package Manager`を押してパッケージマネージャーを開きます。

![Unity_OpenPackageManager](https://github.com/nitsc-proclub/PiccoRoboGame/blob/main/Documents/Images/Unity_OpenPackageManager.png)

上のバーから`Packages: My Assets`を選択して、`PUN 2 - FREE`をインポートします。

![Unity_ImportPUN2](https://github.com/nitsc-proclub/PiccoRoboGame/blob/main/Documents/Images/Unity_ImportPUN2.png)

もしマイアセットにPUN2が無い場合は、下のリンクからPUN2をマイアセットに追加してください。	

https://assetstore.unity.com/packages/tools/network/pun-2-free-119922

`Script Updating Consent`というダイアログが出てきた場合は、
`Yes, for these and other files that might be found later`ボタンを押してファイルを更新します。

![Unity_ImportPUN2](https://github.com/nitsc-proclub/PiccoRoboGame/blob/main/Documents/Images/Unity_ImportPUN2.png)

これによりプロジェクトが正常に読み込めるようになるので、自動的にプロジェクトが読み込まれます。

`PUN Wizard`というダイアログが出てきた場合は、テキストボックスにアプリID(Discordにて連絡)を入力して`Setup Project`をクリックします。

![PUN2_Wizard](https://github.com/nitsc-proclub/PiccoRoboGame/blob/main/Documents/Images/PUN2_Wizard.png)

---

これでプロジェクトのセットアップは完了です。
