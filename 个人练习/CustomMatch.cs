namespace CustomTraining;

public static class CustomMatch
{
//实现一个简易的字符串通配符匹配函数，支持两种特殊的通配符#(长度非零的数字串)和*(任意长度字符串).
public static bool Match(string wild, string str)
{
    var wildLength = wild.Length;
    var strLength = str.Length;
    var iWild = 0;
    var iStr = 0;
    while (iWild < wildLength && iStr < strLength)
        switch (wild[iWild] /*第一空*/)
        {
            case '*':
            {
                iWild += 1;
                if (iWild >= wildLength) return true;

                for (var i = iStr; i < strLength; i++)
                    if (Match(wild.Substring(iWild), str.Substring(i)) /*第二空*/)
                        return true;
            }
                break;
            case '#':
            {
                if (iStr >= strLength || !char.IsDigit(str[iStr]) /*第三空*/) return false;

                while (iStr < strLength && str[iStr] >= '0' && str[iStr] <= '9') iStr += 1;
            }
                break;
            default:
            {
                if (wild[iWild] != str[iStr])
                    return false /*第三空*/;
                iWild += 1;
                iStr += 1;
                break;
            }
        }

    if (iWild < wildLength && iStr >= strLength)
    {
        if (wild[iWild] == '*') return true;
    }
    else
    {
        return false /*第三空*/;
    }

    return false;
}
}