# 酔った勢いで作ったﾒﾓﾘーを全然使わないDictionary
すごい！

[![Build Status](https://travis-ci.org/sazae657/FileDictionary.svg?branch=master)](https://travis-ci.org/sazae657/FileDictionary)


# System.Collections.Generic.Dictionaryとの性能比較

環境

CPU: Opteron 6272x2(2.1GHz/32Core)

RAM: 192GB

ﾃﾞｨｽｸ: Crucial CT275MX300SSD1

OS: Windows10 Pro(ver 1709)

Key/Valueともに100byteのユニークな文字列を100万件


|100万Add後のﾋーﾌﾟｻｲｽﾞ|ｻｲｽﾞ(MB)||
|:---|:---|:---|
|Dictionary|682.5||
|FileDictionary|3.5|←まじすごい|


|Add()|処理時間(msec)||
|:---|:---|:---|
|Dictionary|1,567||
|FileDictionary|836,617|←まじすごい|


|Remove()|処理時間(msec)||
|:---|:---|:---|
|Dictionary|369||
|FileDictionary|389,859|←まじす(ry|


|Get(ｲﾝﾃﾞｸｻー100万ｲﾃﾚーﾄ)|処理時間(msec)||
|:---|:---|:---|
|Dictionary|369||
|FileDictionary|204,582|←まじ(ry|


|Keys()ｲﾃﾚーｼｮﾝ|処理時間(msec)||
|:---|:---|:---|
|Dictionary|2||
|FileDictionary|7,118|←ま(ry|

System.Collections.Generic.Dictionary比500倍の処理時間で200分の1のﾒﾓﾘー消費という脅威の性能
## すごい！

## ﾗｲｾﾝｽ
MIT