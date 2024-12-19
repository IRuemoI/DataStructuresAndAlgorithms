//测试通过

namespace Algorithms.Lesson06;

public class Comparator
{
    public static void Run()
    {
        int[] arr = [5, 4, 3, 2, 7, 9, 1, 0];

        Array.Sort(arr, new AComp());

        foreach (var t in arr) Console.WriteLine(t);

        Console.WriteLine("===========================");

        var student1 = new Student("A", 4, 40);
        var student2 = new Student("B", 4, 21);
        var student3 = new Student("C", 3, 12);
        var student4 = new Student("D", 3, 62);
        var student5 = new Student("E", 3, 42);
        // D E C A B

        Student[] students = [student1, student2, student3, student4, student5];
        Console.WriteLine("第一条打印");

        Array.Sort(students, new IdAscAgeDescOrder());
        foreach (var s in students) Console.WriteLine(s.Name + "," + s.Id + "," + s.Age);

        Console.WriteLine("第二条打印");
        var studentList = new List<Student>
        {
            student1,
            student2,
            student3,
            student4,
            student5
        };
        studentList.Sort(new IdAscAgeDescOrder());
        foreach (var s in studentList) Console.WriteLine(s.Name + "," + s.Id + "," + s.Age);

        // N * logN
        Console.WriteLine("第三条打印");
        student1 = new Student("A", 4, 40);
        student2 = new Student("B", 4, 21);
        student3 = new Student("C", 4, 12);
        student4 = new Student("D", 4, 62);
        student5 = new Student("E", 4, 42);
        var sortedDictionary = new SortedDictionary<Student, string>(new IdAscAgeDescOrder())
        {
            { student1, "我是学生1，我的名字叫A" },
            { student2, "我是学生2，我的名字叫B" },
            { student3, "我是学生3，我的名字叫C" },
            { student4, "我是学生4，我的名字叫D" },
            { student5, "我是学生5，我的名字叫E" }
        };
        foreach (var s in sortedDictionary.Keys) Console.WriteLine(s.Name + "," + s.Id + "," + s.Age);
    }

    private class Student(string name, int id, int age)
    {
        public readonly int Age = age;
        public readonly int Id = id;
        public readonly string Name = name;
    }

    // 任何比较器：
    // compare方法里，遵循一个统一的规范：
    // 返回负数的时候，认为第一个参数应该排在前面
    // 返回正数的时候，认为第二个参数应该排在前面
    // 返回0的时候，认为无所谓谁放前面
    private class IdAscAgeDescOrder : IComparer<Student>
    {
        public int Compare(Student? x, Student? y)
        {
            if (x != null && y != null) return x.Id != y.Id ? x.Id - y.Id : y.Age - x.Age;

            throw new AggregateException();
        }
    }

    private class AComp : IComparer<int>
    {
        // 如果返回负数，认为第一个参数应该拍在前面
        // 如果返回正数，认为第二个参数应该拍在前面
        // 如果返回0，认为谁放前面都行

        public int Compare(int arg0, int arg1)
        {
            return arg1 - arg0;
        }
    }
}