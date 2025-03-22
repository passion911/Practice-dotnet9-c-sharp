using Newtonsoft.Json.Linq;

namespace Practice.TestForSomething;

public class CleanObjectProcessor
{
    public static void CleanJsonData(string jsonString)
    {
        try
        {
            JToken json = JToken.Parse(jsonString);
            CleanToken(json);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    static void CleanObject(JObject obj)
    {
        var properties = obj.Properties().ToList();
        foreach (var prop in properties)
        {
            if (IsRemovableValue(prop.Value))
            {
                prop.Remove();
            }
            else
            {
                CleanToken(prop.Value);
            }
        }
    }

    static void CleanArray(JArray arr)
    {
        for (int i = arr.Count - 1; i >= 0; i--)
        {
            var item = arr[i];
            if (IsRemovableValue(item))
            {
                arr.RemoveAt(i);
            }
            else
            {
                CleanToken(item);
            }
        }
    }

    public static void CleanToken(JToken token)
    {
        switch (token)
        {
            case JObject obj:
                CleanObject(obj);
                break;
            case JArray arr:
                CleanArray(arr);
                break;
        }
    }

    static bool IsRemovableValue(JToken token)
    {
        return token.Type == JTokenType.String && (token.ToString() == "N/A" || token.ToString() == "-" || token.ToString() == string.Empty);
    }
}
