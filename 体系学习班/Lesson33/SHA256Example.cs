//测试通过

#region

using System.Security.Cryptography;
using System.Text;

#endregion

namespace Algorithms.Lesson33;

public static class Sha256Example
{
    public static string ComputeSha256Hash(string input)
    {
        // 创建一个SHA256哈希算法的实例  
        using var sha256Hash = SHA256.Create();
        // 将输入字符串转换为字节数组  
        var data = Encoding.UTF8.GetBytes(input);

        // 计算哈希值  
        var hash = sha256Hash.ComputeHash(data);

        // 将字节数组转换为十六进制字符串  
        var sBuilder = new StringBuilder();

        foreach (var item in hash) sBuilder.Append(item.ToString("x2"));

        return sBuilder.ToString();
    }

    public static void Run()
    {
        var input = "Hello, World!";
        var hash = ComputeSha256Hash(input);

        Console.WriteLine($"The SHA256 hash of \"{input}\" is: {hash}");
    }
}