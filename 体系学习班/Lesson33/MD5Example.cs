//测试通过

#region

using System.Security.Cryptography;
using System.Text;

#endregion

namespace Algorithms.Lesson33;

public class Md5Example
{
    private static string ComputeMd5Hash(string input)
    {
        // 创建一个MD5哈希算法的实例  
        using var md5Hash = MD5.Create();
        // 将输入字符串转换为字节数组  
        var data = Encoding.UTF8.GetBytes(input);

        // 计算哈希值  
        var hash = md5Hash.ComputeHash(data);

        // 将字节数组转换为十六进制字符串  
        var sBuilder = new StringBuilder();

        foreach (var element in hash) sBuilder.Append(element.ToString("x2"));

        return sBuilder.ToString();
    }

    public static void Run()
    {
        var input = "Hello, World!";
        var hash = ComputeMd5Hash(input);

        Console.WriteLine($"The MD5 hash of \"{input}\" is: {hash}");
    }
}