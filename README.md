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
|Dictionary|1,594||
|FileDictionary|691,311|←まじすごい|


|Remove()|処理時間(msec)||
|:---|:---|:---|
|Dictionary|376||
|FileDictionary|280,426|←まじす(ry|


|Get(ｲﾝﾃﾞｸｻー100万ｲﾃﾚーﾄ)|処理時間(msec)||
|:---|:---|:---|
|Dictionary|369||
|FileDictionary|160,698|←まじ(ry|


|foreachｲﾃﾚーｼｮﾝ|処理時間(msec)||
|:---|:---|:---|
|Dictionary|12||
|FileDictionary|218,873|←ま(ry|

System.Collections.Generic.Dictionary比430倍の処理時間で200分の1のﾒﾓﾘー消費という脅威の性能
## すごい！

## ﾗｲｾﾝｽ
MIT