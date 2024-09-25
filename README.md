# Unity-MysticLauncher
Unityエディタ ランチャーツール

![portal](https://raw.githubusercontent.com/tyanmahou/Unity-MysticLauncher/main/Docs/0.png)

## 概要
`Unity-MysticLauncher`は、Unityエディタ拡張のランチャーツールです。  
カスタマイズ可能なポータルページの作成や、アセットのお気に入り機能や、履歴機能を備えています。

## インストール

#### UPM

> Window > Package Manager  
> Add package from git URL...

```
https://github.com/tyanmahou/Unity-MysticLauncher.git?path=Assets/MysticLauncher
```

## 使用方法
### ランチャーの表示
ランチャーはUnityエディタメニューの `Window > Mystic Launcher` をクリックするか、  
`Ctrl + L`のショートカットキーから表示できます。


### ランチャーのカスタマイズ
ランチャーの表示内容は、`Project Settings > Mytic Launcher` からカスタマイズ可能です。  

詳細は [プロジェクト設定](https://github.com/tyanmahou/Unity-MysticLauncher/wiki/%E3%83%97%E3%83%AD%E3%82%B8%E3%82%A7%E3%82%AF%E3%83%88%E8%A8%AD%E5%AE%9A) 

### ユーザー設定
プロジェクト共通の設定とは別に、ユーザー毎に個別のページを作成することをサポートしています。  
これにより、チーム開発では個々に自由にレイアウトを作成することが可能です。

ユーザー設定は、`Preference > Mytic Launcher` からカスタマイズ可能です。 

#### Terminal Path
`RepositoryElement`等でターミナル表示する際に使用する既定のシステムを指定できます。

#### Env
環境変数の設定をできます。  
Mystic Launcherではパスの指定する際に、環境変数を利用することが可能です。    
これは、`RepositoryElement`等を使用する際に、設定するパスがチーム全体で共通化できない場合に活用できます。  
環境変数は、こちらのインスペクタから登録したものだけではなく、個々のPCに設定されているものも対象となります。
