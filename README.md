# VoiceConsoroid

## 概要

Voiceroidをコンソール上で喋らせることを目的にしたプログラムです。

現在、結月ゆかりと民安ともえに対応しています。

大雑把に以下の処理ができるような。

1. GUI上でしゃべらせる.
2. 指定したパスにwavファイルを保存する.
3. 指定したパスにwavファイルを保存すると同時に保存したwavファイルを再生する.

### 動作確認環境

#### OS

まさかのWindows8.1。32bit版/64bit版。Windows7持ってないんだもん...。

#### Voiceroid+

* 結月ゆかり パッケージ版
* 民安ともえ ダウンロード版

パッケージ版と変わらないとは思いますが念のため。


### 使い方


* git cloneで一式を取得してください。

```
$ git clone https://github.com/yukaary/VoiceConsoroid.git
```

* `product/VoiceConsoroid.exe`がコンパイル済みの実行ファイルです。
  - パスを通すなりなんなりしてください。
* ボイスロイドを起動します。Windows8の人は怒りの連続クリックだ！
* おもむろにコマンドを打ちます。

#### コマンド

```
VoiceConsoroid <command> <message> -v <voiceroid> -f <filepath>
```

* command
  - talk 再生ボタンを押してしゃべってもらう
  - save wavを指定したファイルに保存する
  - record wavを指定したファイルに保存すると同時に保存後にwavを再生
* message
  - しゃべってもらう内容。`"`で括りましょう。
* voiceroid
  - ボイスロイドの指定。 `yuakri` または `maki` を指定してください。
* filepath
  - 保存先のファイルパス。

`save`, `record`コマンドは`-f`で保存先を指定してください。未指定の場合はデフォルトで`voice.wav`(相対パス)に出力します。

#### サンプル

##### 単純にしゃべらせる.

```
$ VoiceConsoroid talk "こんにちわー" -v yukari
```

##### 保存する。

```
$ VoiceConsoroid save "やったぜ" -v maki -f yatta.wav
```

##### 保存後に再生。

```
$ VoiceConsoroid record "なんで元からコンソールサポートしてないのん？" -v yukari -f yatta.wav
```

## 注意点

* Win32APIを使って機械的にGUIを叩いている、そんなプログラムなので期待通りに動かないことが多々あります。
* `record`コマンドは保存処理が3秒程度で終わることを期待して書かれています。
* 上記はそのうちパラメータとして待機時間を指定可能にするかも。
