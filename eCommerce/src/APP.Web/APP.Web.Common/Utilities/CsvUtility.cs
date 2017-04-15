using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace APP.Web.Common.Utilities
{
    /// <summary>
    /// Incredibly simple CSV parsing utility, written by yomanto.taro@xuenn.com.
    /// <para>Consider using the more dedicated one as you scale. See <see cref="https://www.nuget.org/packages/CsvHelper/" />.</para>
    /// </summary>
    public class CsvUtility
    {
        string _defHeaderSig = "Domain,Affiliate code";
        char _delimiter = ',';

        public virtual string HeaderSignature
        {
            get { return _defHeaderSig; }
            set { _defHeaderSig = value; }
        }
        public char Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }
        public string FilePath { get; set; }

        public CsvUtility(string physicalFilePath, char delimiter)
            : this(physicalFilePath)
        {
            this.Delimiter = delimiter;
        }
        public CsvUtility(string physicalFilePath)
        {
            this.FilePath = physicalFilePath;

            if (!File.Exists(FilePath))
                throw new FileNotFoundException("physicalFilePath");
        }

        public IDictionary<string, string> AsDictionary()
        {
            return this.Lines().ToDictionary(k => k.Key, v => v.Value);
        }

        public bool FindValue(string keyName, ref string value, bool ignoreCase = true)
        {
            foreach (var item in this.Lines())
            {
                if (String.Compare(item.Key, keyName, ignoreCase) == 0)
                {
                    value = item.Value;
                    return true;
                }
            }

            return false;
        }

        IEnumerable<KeyValuePair<string, string>> Lines()
        {
            using (var file = new StreamReader(FilePath))
            {
                string line = string.Empty;

                while ((line = file.ReadLine()) != null)
                {
                    if (line != this.HeaderSignature)
                    {
                        var cols = line.Split(this.Delimiter);

                        yield return new KeyValuePair<string, string>(cols[0], cols[1]);
                    }
                }
            }
        }

    }
}
