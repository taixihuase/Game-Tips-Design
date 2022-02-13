namespace JsonData
{
    public class JsonUtil
    {
        public static string AppendJsonName(string nameStr, string arrayStr)
        {
            return string.Format("{{\"{0}\":{1}}}", nameStr, arrayStr);
        }

        public static bool IsJsonFormat(string source)
        {
            //只是简单判空，未对内容格式正确性做解析以提升性能
            if (string.IsNullOrEmpty(source) || source == "[]")
            {
                return false;
            }

            return true;
        }
    }
}
