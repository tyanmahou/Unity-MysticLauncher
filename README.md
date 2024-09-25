# Unity-MysticLauncher
Unityエディタ ランチャーツール

![portal](https://raw.githubusercontent.com/tyanmahou/Unity-MysticLauncher/main/Docs/0.png)

# 概要
`Unity-MysticLauncher`は、Unityエディタ拡張のランチャーツールです。  
カスタマイズ可能な、ポータルページの作成や、アセットのお気に入り機能や、履歴機能を備えています。

# インストール

UPM

> Window > Package Manager  
> Add package from git URL...

```
https://github.com/tyanmahou/Unity-MysticLauncher.git?path=Assets/MysticLauncher
```

# ランチャーの表示
ランチャーはUnityエディタメニューの `Window > Mystic Launcher` をクリックするか、`Ctrl + L`のショートカットキーから表示できます。


# ランチャーのカスタマイズ
ランチャーの表示内容は、`Project Settings > Mytic Launcher` からカスタマイズ可能です。  

## Project Info
ランチャーウィンドウのヘッダー部分になります。  
`CustomHeader`を設定することで、自由なコンテンツを作成することも可能です。  
`CustomHeader`は、`IElement`を実装したクラスを指定することが可能です。  
(`IElement`に関しては、後述)

## Portal Layout
ランチャーのポータルページの設定です。
`IElement`を実装したクラスを指定することで自由にカスタマイズすることが可能です。

### 組み込みの `IElement` 

#### ActionElement
#### AssetElement
#### CategoryElement
#### FolderElement
#### HorizontalElement
#### MenuItemElement
#### ProcessElement
#### RepositoryElement
#### SettingServiceElement
#### TextElement
#### ToolNaviElement
#### TreeElement
#### URLElement

### 独自の`IElement`の実装

## Project Tabs
プロジェクト共通の、任意の `ITabLayout` を指定して、タブの追加をする事が可能です。

# ユーザー設定
プロジェクト共通の設定とは別に、ユーザー毎に個別のページを作成することをサポートしています。  
これにより、チーム開発では個々に自由にレイアウトを作成することが可能です。

ユーザー設定は、`Preference > Mytic Launcher` からカスタマイズ可能です。 

## Terminal Path
`RepositoryElement`等でターミナル表示する際に使用する既定のシステムを指定できます。

## Env
環境変数の設定をできます。
Mystic Launcherではパスの指定する際に、環境変数を利用することが可能です。  
これは、`RepositoryElement`等を使用する際に、設定するパスがチーム全体で共通化できない場合に活用できます。
環境変数は、こちらのインスペクタから登録したものだけではなく、個々のPCに設定されているものも対象となります。
