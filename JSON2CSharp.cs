using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Json2Csharpclass
{
    //根据JSON生成C#的类
    public class JSON2CSharp
    {
        private static string _allClass;

        public static string convert(JToken o)
        {
            return _genClassCode(o, "");
        }

        private static string _genClassCode(JToken obj, string name)
        {
            var clas = string.Format("public class {0}\r\n{{\r\n", string.IsNullOrEmpty(name) ? "Root" : name);
            foreach (JProperty o in obj)
            {
                string path = _split(o.Name);
                var v = obj[path];
                clas += string.Format("    {0}    public {1} {2} {{ get; set; }}\n", _genComment(v), _genTypeByProp(path, v), path);
            }
            clas += "}\n";
            _allClass += clas;
            _allClass += "\n";
            return _allClass;
        }

        private static object _genTypeByProp(string name, JToken val)
        {
            if (val is JObject)
            {
                name = _split(val.Path);
                name = name.Substring(0, 1).ToUpper() + name.Substring(1);

                _genClassCode(val, name);
                return name;
            }
            else if (val is JArray)
            {
                return string.Format("List <{0}> ", _genTypeByProp(name + "Item", val[0]));
            }
            else
            {
                return "string";
            }
        }

        private static string _split(string s)
        {
            int beg = s.LastIndexOf('.') + 1;
            int end = s.IndexOf('[') == -1 ? s.Length : s.IndexOf('[');
            return s.Substring(beg, end - beg);
        }
        private static object _genComment(JToken v)
        {
            return null;
        }
    }
}