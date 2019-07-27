using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;
using Thrift.Protocol;
using Thrift.Collections;
using System.Data;
using System.Reflection;

namespace MAPResClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TTransport tr = new TSocket("localhost", 9091);
                tr.Open();
                TProtocol prot = new TBinaryProtocol(tr);
                MAPResService.Client client = new MAPResService.Client(prot);
                Program pr = new Program();
                if (client.loginToServer("Adnan", "12345"))
                {
                    //Console.WriteLine(client.getDOEC("Sajeel"));
                    THashSet<string> ModSites = new THashSet<string>();
                    ModSites.Add("S");
                    ModSites.Add("T");
                    ModSites.Add("Y");
                    //THashSet<string> PseudoAminoSet = new THashSet<string>();
                    //PseudoAminoSet = pr.generatePseudoSet();
                    //client.setProjectProfileWithStandardAminoAcids("ABC","Sajeel",3,ModSites,"thisPath");
                    client.setProjectProfileWithStandardAminoAcids("FirstANalysis", "Sajeel", 10, ModSites);
                    client.openDatasetLocationForAnalysis(@"C:\Users\Sajeel\Desktop\Results\Carboxylation\XML Files\CarboxylEncoded.XML");
                    var list = client.getDeviationParameter();
                    //DataTable dt = ToDataTable(list);
                    client.getModsite("S");
                    client.runPreferenceEstimationProcess();
                    client.SaveProject();

                    Console.WriteLine(client.getDOEC());
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Improper User Name & Password");
                    Console.ReadKey();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public THashSet<string> generatePseudoSet()
        {

            var set = new THashSet<String>();
            String str = null;
            Console.WriteLine("Give Pseudo Set");
            str = Console.ReadLine();
            while (!string.Equals(str, "-1"))
            {
                set.Add(str.ToUpper());
                str = Console.ReadLine();
            }
            return set;
        }
    }
}
