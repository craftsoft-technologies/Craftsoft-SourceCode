using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using APP.Common.Utilities;
using APP.Web.Common.Models;

namespace APP.Web.Common.Utilities
{
    public class DropdownListUtil
    {
        private const string DefaultLanguageCode = "en";
        private static ConcurrentDictionary<string, DropdownListCollections> _dic = new ConcurrentDictionary<string, DropdownListCollections>();


        public static void Load(string file)
        {
            int index = file.LastIndexOf('\\');

            if (index > 0)
            {
                string dir = file.Substring(0, index);
                string fileName = file.Substring(index + 1);

                index = fileName.LastIndexOf('.');

                if (index > 0)
                {
                    string[] files = Directory.GetFiles(dir, fileName.Insert(index, "*"), SearchOption.TopDirectoryOnly);

                    Regex regex = new Regex(fileName.Insert(index, @"\.(.*)\"), RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    foreach (var item in files)
                    {
                        string lang = regex.Match(item).Groups[1].Value.ToLower();
                        _dic.AddOrUpdate(string.IsNullOrEmpty(lang) ? DefaultLanguageCode : lang, Deserialize(item), (key, i) => i);
                    }
                }
            }
        }

        public static DropdownList Get(string langCode, DropdownListType type)
        {
            DropdownList returnValue = InternallyGet(langCode, type) ?? InternallyGet(DefaultLanguageCode, type);

            if (returnValue == null)
            {
                throw new Exception(string.Format("Cannot Get DropdownList, Type {0}, Language Code: {1}", type.ToString(), langCode));
            }

            return returnValue;
        }

        public static DropdownList Get(string langCode, params DropdownListType[] types)
        {
            DropdownList result = new DropdownList("CombinedDropdownList");
            DropdownList next = null;
            var e = types.GetEnumerator();

            while (e.MoveNext())
            {
                next = Get(langCode, (DropdownListType)e.Current);
                result.Items.AddRange(next.Items);
            }

            return result;
        }

        public static DropdownList GetForSearch(string langCode, DropdownListType type, string defaultAllValue = "0")
        {
            return GetWithTitle(langCode, type, APP.Web.Common.Properties.CommonText.DropdownList_All, defaultAllValue);
        }

        public static DropdownList GetForEmpty(string langCode, DropdownListType type, string defaultAllValue = "0")
        {
            return GetWithTitle(langCode, type, APP.Web.Common.Properties.CommonText.DropdownList_Empty, defaultAllValue);
        }

        public static DropdownList GetForSelect(string langCode, DropdownListType type, string defaultAllValue = "0")
        {
            return GetWithTitle(langCode, type, APP.Web.Common.Properties.CommonText.DropdownList_Select, defaultAllValue);
        }

        public static DropdownList GetWithTitle(string langCode, DropdownListType type, string text, string value)
        {
            DropdownList list = Get(langCode, type);

            DropdownList returnValue = new DropdownList(type.ToString());

            returnValue.Items.Add(new DropdownListItem { Text = text, Value = value });
            foreach (var item in list.Items)
            {
                returnValue.Items.Add(item);
            }

            return returnValue;
        }

        #region Get Localized Text

        public static string GetText(DropdownListType type, int value)
        {
            return GetText(System.Globalization.CultureInfo.CurrentCulture.Name.ToLower(), type, value);
        }

        public static string GetText(string langCode, DropdownListType type, int value)
        {
            return GetText(langCode, type, value.ToString());
        }

        public static string GetText(string langCode, DropdownListType type, string value)
        {
            DropdownList list = Get(langCode, type);

            return GetText(list, value);
        }

        public static string GetText(DropdownList list, string value)
        {
            if (list != null)
            {
                foreach (var item in list.Items)
                {
                    if (string.Equals(value, item.Value, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return item.Text;
                    }
                }
            }

            return string.Format("[{0}]", value);
        }

        public static string GetText(DropdownList list, int value)
        {
            return GetText(list, value.ToString());
        }

        #endregion

        public static SelectList GetMvcDropdownListBy<TEnum>(string prefix, Type resourceType)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return new SelectList(GetMvcDropdownListBy<TEnum>(prefix, resourceType, null), "Value", "Text");
        }

        public static SelectList GetMvcDropdownListBy<TEnum>(string prefix, Type resourceType, string defaultText, object defaultValue)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return new SelectList(
                System.Linq.Enumerable.Concat(
                    first: new SelectListItem[1] { new SelectListItem { Text = defaultText, Value = Convert.ToString(defaultValue) } },
                    second: GetMvcDropdownListBy<TEnum>(prefix, resourceType, defaultValue)
                ),
                "Value", "Text"
            );
        }

        public static SelectList GetMvcDropdownListBy<TEnum>(string prefix, Type resourceType, object selectedValue)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            return new SelectList(GetMvcDropdownListBy<TEnum>(prefix, resourceType, Convert.ToString(selectedValue)), "Value", "Text");
        }

        public static SelectList GetMvcDropdownList(string langCode, DropdownListType type)
        {
            return new SelectList(DropdownListUtil.Get(langCode, type).Items, "Value", "Text");
        }

        public static SelectList GetMvcDropdownList(IEnumerable<DropdownListItem> listItems, object selectedValue = null)
        {
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public static SelectList GetMvcDropdownList(string langCode, DropdownListType type, string selectedValue)
        {
            return new SelectList(DropdownListUtil.Get(langCode, type).Items, "Value", "Text", selectedValue);
        }

        public static SelectList GetMvcDropdownListForSearch(string langCode, DropdownListType type, string defaultAllValue = "0")
        {
            return new SelectList(DropdownListUtil.GetForSearch(langCode, type, defaultAllValue).Items, "Value", "Text");
        }

        public static SelectList GetMvcDropdownListForEmpty(string langCode, DropdownListType type, string defaultAllValue = "")
        {
            return new SelectList(DropdownListUtil.GetForEmpty(langCode, type, defaultAllValue).Items, "Value", "Text");
        }

        public static SelectList GetMvcDropdownListForSelect(string langCode, DropdownListType type, string defaultAllValue = "0")
        {
            return new SelectList(DropdownListUtil.GetForSelect(langCode, type, defaultAllValue).Items, "Value", "Text");
        }

        public static SelectList GetMvcDropdownListWithTitle(string langCode, DropdownListType type, string text, string value)
        {
            return new SelectList(DropdownListUtil.GetWithTitle(langCode, type, text, value).Items, "Value", "Text");
        }

        private static IEnumerable<SelectListItem> GetMvcDropdownListBy<TEnum>(string prefix, Type resourceType, string selectedValue)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (typeof(TEnum).IsEnum)
            {
                var enumList = Enum.GetValues(typeof(TEnum));
                var resMan = new System.Resources.ResourceManager(resourceType);

                string text, value;
                bool @selected = false;

                foreach (TEnum e in enumList)
                {
                    text = resMan.GetString(prefix + Enum.GetName(typeof(TEnum), e));
                    value = Convert.ToString((int)Convert.ChangeType(e, typeof(int)));
                    @selected = (value == selectedValue);

                    if (String.IsNullOrEmpty(text))
                    {
#if DEBUG
                        throw new KeyNotFoundException(String.Format(
                            "Please make sure this resource named \"{0}\" actually exists in the collection.", text
                        ));
#else
                        yield break;
#endif
                    }

                    yield return new SelectListItem
                    {
                        Text = text,
                        Value = value,
                        Selected = @selected
                    };
                }
            }

            yield break;
        }

        private static DropdownListCollections Deserialize(string file)
        {
            // 1) in this way, cannot ensure the DropdownList in DropdownListCollections in order by DrowdownListType, 
            // except we put DropdownList in the config file in order by DrowdownListType
            //DropdownListCollections collections = XmlSerializerHelper.ToObjFromFile<DropdownListCollections>(file);

            #region 2) deserialize object DropdownList one by one

            DropdownListCollections collections = new DropdownListCollections();

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodes = doc.SelectNodes("//DropdownList");

            DropdownListType type;
            foreach (XmlNode node in nodes)
            {
                DropdownList list = XmlSerializerHelper.ToObj<DropdownList>(node.OuterXml);
                if (Enum.TryParse<DropdownListType>(list.EntityType, out type))
                {
                    collections[(int)type] = list;
                }
            }

            #endregion

            return collections;
        }

        private static DropdownList InternallyGet(string langCode, DropdownListType type)
        {
            DropdownListCollections collection;

            if (_dic.TryGetValue(langCode, out collection))
            {
                return collection[(int)type];
            }

            return null;
        }

    }
}
