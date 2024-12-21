using System.Text;

namespace CustomTraining.Leetcode;
//leetcode:https://leetcode.cn/problems/ti-huan-kong-ge-lcof/
public class PathEncryption
{
    private string PathEncryptionCode(string path) {
        var sb = new StringBuilder();
        foreach (var charItem in path)
        {
            sb.Append(charItem == '.' ? ' ' : charItem);
        }

        return sb.ToString();
    }
}