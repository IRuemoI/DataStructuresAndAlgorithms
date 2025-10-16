# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

这是一个基于左程云老师2021年课程体系学习班和大厂刷题班的C#数据结构与算法学习仓库。原Java代码已被转写为C#版本，使用.NET 8.0框架。

## 构建和运行命令

### 构建项目
```bash
# 构建整个解决方案
dotnet build DataStructuresAndAlgorithms.sln

# 构建特定项目
dotnet build 体系学习班/体系学习班.csproj
dotnet build 大厂刷题班/大厂刷题班.csproj
dotnet build Leetcode三阶段练习/Leetcode三阶段练习.csproj
dotnet build 个人练习/个人练习.csproj
dotnet build 通用代码/通用代码.csproj
```

### 运行项目
```bash
# 运行体系学习班（默认运行Lesson10）
dotnet run --project 体系学习班

# 运行Leetcode练习
dotnet run --project Leetcode三阶段练习

# 运行个人练习
dotnet run --project 个人练习

# 运行大厂刷题班
dotnet run --project 大厂刷题班
```

### 测试代码
- 项目使用自定义的`QuestionTemplate<TInput, TOutput>`测试框架
- 每个算法题目都有对应的测试类继承此模板
- 测试通过调用`RunTests()`方法执行，默认100次测试

## 代码架构

### 项目结构
1. **体系学习班** - 系统性学习数据结构与算法的核心实现
   - 按课程Lesson01-Lesson47组织
   - 每个Lesson包含一个或多个算法实现
   - 依赖通用代码项目

2. **通用代码** - 共享的数据结构和算法工具
   - `DataStructures/` - 基础数据结构实现（二叉树、图、堆、链表、前缀树、队列、栈、并查集等）
   - `Algorithms/` - 通用算法实现（搜索、排序）
   - `QuestionTemplate.cs` - 自动化测试框架模板

3. **Leetcode三阶段练习** - LeetCode题目练习
   - 按难度分类：简单、中等、困难
   - 按题型分类：数组和字符串、链表、树和图等

4. **大厂刷题班** - 针对大厂面试的高频题目
5. **个人练习** - 个人编程练习区域

### 重要约定
- Java到C#类型映射：
  - `HashMap` → `Dictionary`
  - `HashSet` → `HashSet`
  - `ArrayList` → `List`
  - `TreeMap` → `SortDictionary`
- 使用C# 8.0+语法特性：可空引用类型、模式匹配、切片语法等
- 矩阵数组使用`GetLength(维度)`获取尺寸，维度0为行，1为列

### 运行单个算法
要运行特定Lesson中的算法，修改`体系学习班/Program.cs`中的Main方法，将`UnRecursiveTraversalBinaryTree.Run()`替换为目标类的Run方法。

### 开发工具推荐
- 学习阶段：JetBrains Rider 2023.1.3+（个人开发者免费）
- 练习阶段：VSCode/记事本/Vim（禁用代码补全插件）