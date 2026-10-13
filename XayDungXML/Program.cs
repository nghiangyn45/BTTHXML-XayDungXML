using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Thiết lập mã hóa console để hiển thị tiếng Việt chính xác
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string filePath = "bookstore.xml";

        // 1. Viết code xây dựng cây XML và export ra file
        CreateAndExportXml(filePath);

        // 3. Hiển thị các thông tin
        DisplayXmlInfo(filePath);

        Console.ReadKey();
    }

    static void CreateAndExportXml(string filePath)
    {
        // Xây dựng cấu trúc cây XML theo đề bài và thêm một số sách mẫu
        XDocument doc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("bookstore",
                new XElement("book",
                    new XAttribute("category", "cooking"),
                    new XElement("title", new XAttribute("lang", "en"), "Everyday Italian"),
                    new XElement("author", "Giada De Laurentiis"),
                    new XElement("year", "2005"),
                    new XElement("price", "30.00")
                ),
                // 2. Thêm các phần tử con bổ sung (để kiểm tra min/max giá)
                new XElement("book",
                    new XAttribute("category", "children"),
                    new XElement("title", new XAttribute("lang", "en"), "Harry Potter"),
                    new XElement("author", "J.K. Rowling"),
                    new XElement("year", "2005"),
                    new XElement("price", "29.99")
                ),
                new XElement("book",
                    new XAttribute("category", "web"),
                    new XElement("title", new XAttribute("lang", "en"), "Learning XML"),
                    new XElement("author", "Erik T. Ray"),
                    new XElement("year", "2003"),
                    new XElement("price", "39.95")
                )
            )
        );

        // Lưu ra file XML
        doc.Save(filePath);
        Console.WriteLine($"-> Đã xây dựng và export file '{filePath}' thành công!\n");
    }

    static void DisplayXmlInfo(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Không tìm thấy file XML.");
            return;
        }

        XDocument doc = XDocument.Load(filePath);
        XElement root = doc.Root;

        Console.WriteLine("===== KẾT QUẢ HIỂN THỊ THÔNG TIN (C#) =====");

        // a. Phần tử gốc
        Console.WriteLine($"a. Phần tử gốc: <{root.Name}>");

        // b. Số phần tử con, cháu
        int directChildrenCount = root.Elements().Count(); // Số thẻ <book> trực tiếp
        int totalDescendantsCount = root.Descendants().Count(); // Tổng số phần tử con, cháu trong cây
        Console.WriteLine($"b. Số phần tử con trực tiếp của gốc: {directChildrenCount}");
        Console.WriteLine($"   Tổng số phần tử con, cháu trong cây: {totalDescendantsCount}");

        // c. Tìm phần tử có giá thấp nhất và cao nhất
        var books = root.Elements("book").Select(b => new {
            Title = b.Element("title")?.Value,
            Price = decimal.Parse(b.Element("price")?.Value ?? "0")
        }).ToList();

        if (books.Count > 0)
        {
            var minBook = books.OrderBy(b => b.Price).First();
            var maxBook = books.OrderByDescending(b => b.Price).First();

            Console.WriteLine($"c. Sách có giá thấp nhất: '{minBook.Title}' (${minBook.Price})");
            Console.WriteLine($"   Sách có giá cao nhất: '{maxBook.Title}' (${maxBook.Price})");
        }
        else
        {
            Console.WriteLine("c. Không có dữ liệu sách trong hệ thống.");
        }
    }
}