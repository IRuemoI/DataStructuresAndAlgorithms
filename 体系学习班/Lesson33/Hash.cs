//通过

namespace Algorithms.Lesson33;

public class Hash
{
    public static void Run()
    {
        Console.WriteLine("=======");

        const string input1 = "zuochengyunzuochengyun1";
        const string input2 = "zuochengyunzuochengyun2";
        const string input3 = "zuochengyunzuochengyun3";
        const string input4 = "zuochengyunzuochengyun4";
        const string input5 = "zuochengyunzuochengyun5";
        Console.WriteLine(Sha256Example.ComputeSha256Hash(input1));
        Console.WriteLine(Sha256Example.ComputeSha256Hash(input2));
        Console.WriteLine(Sha256Example.ComputeSha256Hash(input3));
        Console.WriteLine(Sha256Example.ComputeSha256Hash(input4));
        Console.WriteLine(Sha256Example.ComputeSha256Hash(input5));
    }
}