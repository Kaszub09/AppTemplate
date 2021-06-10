using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI {
    public class Program {
        public class Book {
            public String title;
            public string title2="asd";
            public double b = 123.3123;
            public int title3=23;
            public DateTime Date = DateTime.Now;
        }

        public static void Main(string[] args) {
            //for faster Program testing
               
    
  
    
        Book overview = new Book();  
        overview.title = "Serialization Overview";  
        System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Book));  
  
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationOverview.xml";  
        System.IO.FileStream file = System.IO.File.Create(path);  
  
        writer.Serialize(file, overview);  
        file.Close();  
    
        }
    }
}
