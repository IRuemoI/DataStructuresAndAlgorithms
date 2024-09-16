# 自述文件

## 前言

网上关于C#的数据结构教学比较少，仓库的内容是根据左程云老师2021年结课的[体系学习班](https://www.bilibili.com/list/ml3257668072)和大厂刷题班课程内的Java代码转写的C#代码。推荐大学期间想提升数据结构和算法的且**基础较好**的同学可以跟左老师的课程学习，提升很快。如果在Unity开发岗想提升数据结构和算法水平的话，也是不错的选择（也是本人的情况）。学算法重要的是逻辑，与语言的关系不大。

## 运行环境

课程学习阶段:推荐使用JetBrains Rider 2023.1.3和.Net SDK7及以上版本，可以直接点击每个类的Run函数行号的旁边的运行按钮即可，非常方便。

动手练习阶段:推荐使用不安装插件或者禁用代码补全插件的VSCode、记事本或者Vim。在一口气写完之后启用粘贴到Rider中运行结果。可以快速提升编码能力。

## 仓库代码结构

* **AdvancedTraining**项目：对应大厂刷题班
* **Algorithms**项目：对应体系学习班
* **Common**项目：数据结构和算法整理以及为其他项目提供通用功能
* **CustomTraining**项目：刷题练习
* **Ruemo's Algorithm Note**文件夹：目前是体系学习班笔记
* **java代码.zip**文件：大厂刷题班和对应体系学习班原版Java代码

## 注意事项

1. 由于C#与Java的语言差异需要说明关于两种语言之间的大致的类型替代：

   | Java类型  |     C#类型     |
   | :-------: | :------------: |
   |  HashMap  |   Dictionary   |
   |  HashSet  |    HashSet     |
   | ArrayList |      List      |
   |  TreeMap  | SortDictionary |

2. 关于新版.Net的一些语法糖
   * 声明类型Node? a；表示a为可空的Node类型。
   * 对于C#的矩阵数组比如 int[,] a;获取函数使用a.GetLength(维度)，获取行数维度为0，获取列数维度为1
   * cur=cur?.next 等价于 if(cur!=null) cur=cur?.next
   * cur?? throw new Exception; 等价于如果当前指针为空那么抛出异常
   * 构造函数的语法糖：可以将集合的成员直接在构造时天机
   * 使用[模式匹配](https://learn.microsoft.com/zh-cn/dotnet/csharp/fundamentals/functional/pattern-matching)进行判断：cur{ lchild is not null , rchild is not null}表示cur左右子节点都不为空的条件表达式
   * C#也有类似Python的切片语法：比如arr\[^1\]表示arr的倒数第一个元素
   * 集合的Floor函数可以使用[LINQ](https://learn.microsoft.com/zh-cn/dotnet/csharp/linq/get-started/introduction-to-linq-queries)代替（可能性能稍差）
   * 更多语法糖可以查阅微软的官方文档，这些以上语法都是在Rider的默认配置下提示的改进写法
3. 其他需要注意的就是C#的比较器与Java比较器的区别

## 其他说明

1.出现项目中代码与笔记中代码不一致的情况：项目中的代码是对Java代码的转写，笔记中给出的代码更符合C#的API或者编码习惯。
