Haceau.JSON
===========
![Language](https://img.shields.io/badge/Language-C%23-blue.svg?style=flat-square) ![.Net Core](https://img.shields.io/badge/.Net&nbsp;Core-3.1-blue.svg?style=flat-square)

[![Gitee](https://img.shields.io/badge/Gitee-辰落火辉Haceau-red.svg?style=flat-square)](https://gitee.com/haceau/Haceau.JSON) [![Github](https://img.shields.io/badge/Github-HaceauZoac-blue.svg?style=flat-square)](https://github.com/Haceau-Zoac/Haceau.JSON)

介绍
---
Haceau.JSON是一个类库，用于解析JSON字符串并返回Dictionary\<string, object>或List\<object>类型的值。

测试
---
|字符串|运行一次用时|运行一百次用时|
|---|---|---|
|{\"Array\": [\"字符串\", false], \"number\": 123, \"double\": 999.888, \"dgfsrfe\": null}|19ms|92ms|
|{\"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Array\":[&nbsp;&nbsp;\"字符串\",&nbsp;false],&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\"number\":&nbsp;&nbsp;&nbsp;\n\n\n\t\r123,&nbsp;\"dgfsrfe\":&nbsp;null}|0ms|64ms|
|[true]|1ms|9ms|
|{\"st\tring\": \"\t\t\\t\u1234\"}|0ms|8ms|
* test项目用来测试。
* 用时值为测试十次的平均值。
* 用时可能会有较大误差。
* 我知道数据很迷，所以大佬帮我康康呗awa

使用
---
1. 导入dll文件（在Release里）或下载nuget包（Haceau.JSON）
2. 添加using Haceau.JSON
3. 创建ReadJSON类的实例（例：ReadJSON readJSON = new ReadJSON();）
4. 使用Parser方法读取JSON字符串（例：readJSON.Parser("{"键": ["值"]}";)）
5. 更多见[文档](https://github.com/Haceau-Zoac/Haceau.JSON/wiki/Document)

文档
---
[中文文档](https://github.com/Haceau-Zoac/Haceau.JSON/wiki/Document)

待办清单
---
|完成版本|内容|
|---|---|
|1.1.0|导出JSON字符串|
|1.0.x|长期维护|