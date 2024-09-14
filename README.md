# AIClientAPI

Azure OpenAI を使う dll を用意し Web アプリで使うサンプルです。
NuGet の Azure.AI.OpenAI というプレリリースのパッケージを使用してとても簡単に AI チャットを実装しています。

AIClientAPI プロジェクトの dll を TestWebApp で読み込んでいます。AI を扱うコントローラーは AIClientAPI にありますが、TestWebApp で Web API として呼び出せる。そういうサンプルです。
