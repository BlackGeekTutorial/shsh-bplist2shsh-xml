using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlistCS;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace shsh_bplist2shsh_xml
{
    class Program
    {
        public static void bplist_to_xml(string bplist, string xml)
        {
            Dictionary<string, object> dictionary = (Dictionary<string, object>)Plist.readPlist(bplist);
            Plist.writeXml(dictionary, xml);
            dictionary.Clear();
        }

        public static string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder builder = new StringBuilder(arrInput.Length);
            int num = arrInput.Length - 1;
            for (int i = 0; i <= num; i++)
            {
                builder.Append(arrInput[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }

        static void Main(string[] args)
        {

            Console.WriteLine("CREDITS: iH8Sn0w");
            Console.WriteLine("Usage: shsh-bplist2shsh-xml.exe -i [infile.shsh] -o [outfile.xml]");
            string bplist = args[1];
            string xml = args[3];
            BinaryReader reader = new BinaryReader(System.IO.File.Open(bplist, FileMode.Open));
            string bytes = ByteArrayToString(reader.ReadBytes(4));
            reader.Close();
            switch (bytes)
            {
                case "1F8B0800":
                    using (FileStream stream = new FileStream(bplist, FileMode.Open, FileAccess.Read))
                    {
                        using (GZipInputStream stream2 = new GZipInputStream(stream))
                        {
                            using (FileStream stream3 = new FileStream(bplist + ".copy.shsh", FileMode.Create, FileAccess.Write))
                            {
                                int num = 0;
                                byte[] buf = new byte[0x1000];
                                while (InlineAssignHelper<int>(ref num, stream2.Read(buf, 0, buf.Length)) != 0)
                                {
                                    stream3.Write(buf, 0, num);
                                }
                            }
                        }
                    }
                    bplist_to_xml(bplist + ".copy.shsh", xml);
                    break;
                case "3C3F786D":
                    break;
                case "62706C69":
                    {
                        string str5 = Path.GetFileName(bplist);
                        bplist_to_xml(bplist, xml);
                    }
                    break;
            }
            return;
            File.Delete(bplist + ".copy.shsh");

        }
    }
}