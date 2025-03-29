//pass

namespace AdvancedTraining.Lesson01;

public class CountFiles
{
    // 注意这个函数也会统计隐藏文件  
    private static int GetFileNumber(string folderPath)
    {
        var root = new DirectoryInfo(folderPath);
        if (!root.Exists) return 0;

        if ((File.GetAttributes(folderPath) & FileAttributes.Directory) !=
            FileAttributes.Directory) return 1; // 如果路径指向的是一个文件，返回1  


        var stack = new Stack<DirectoryInfo>();
        stack.Push(root);
        var files = 0;
        while (stack.Count > 0)
        {
            var folder = stack.Pop();
            var filesInfo = folder.GetFiles();
            // foreach (var file in filesInfo)
            // {
            //     files++;
            // }

            files += filesInfo.Length;

            var dirsInfo = folder.GetDirectories();
            foreach (var dir in dirsInfo) stack.Push(dir);
        }

        return files;
    }

    public static void Run()
    {
        // 你可以自己更改目录  
        const string path = @"D:\Library\Documents\MyArchive\Vim";
        Console.WriteLine($"该目录共有文件{GetFileNumber(path)}个");
    }
}