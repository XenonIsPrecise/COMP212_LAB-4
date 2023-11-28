// See https://aka.ms/new-console-template for more information
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;



namespace Q2Lab4
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Question 02 - Lab 04");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Provide the file paths to your Excel files
            string titlesFilePath = "/Users/sujalxenon/Downloads/Lab04 - Solution Template/Q2Lab4/obj/Debug/dbo.Titles.xlsx";
            string authorsFilePath = "/Users/sujalxenon/Downloads/Lab04 - Solution Template/Q2Lab4/obj/Debug/dbo.Authors.xlsx";
            string authorISBNFilePath = "/Users/sujalxenon/Downloads/Lab04 - Solution Template/Q2Lab4/obj/Debug/dbo.AuthorISBN.xlsx";

            var titles = LoadTitlesFromExcel(titlesFilePath);
            var authors = LoadAuthorsFromExcel(authorsFilePath);
            var authorISBN = LoadAuthorISBNFromExcel(authorISBNFilePath);

            Question2_1(titles, authors, authorISBN);
            Question2_2(titles, authors, authorISBN);
            Question2_3(titles, authors, authorISBN);

            Console.ReadLine();
        }

        // Load titles from Excel file
        static List<Title> LoadTitlesFromExcel(string filePath)
        {
            var titles = new List<Title>();
            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        titles.Add(new Title
                        {
                            ISBN = worksheet.Cells[row, 1].Value?.ToString(),
                            TitleName = worksheet.Cells[row, 2].Value?.ToString(),
                            EditionNumber = int.Parse(worksheet.Cells[row, 3].Value?.ToString() ?? "0"),
                            Copyright = int.Parse(worksheet.Cells[row, 4].Value?.ToString() ?? "0")
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No worksheet found for titles in the Excel file.");
                }
            }
            return titles;
        }

        // Load authors from Excel file
        static List<Author> LoadAuthorsFromExcel(string filePath)
        {
            var authors = new List<Author>();
            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        authors.Add(new Author
                        {
                            AuthorID = int.Parse(worksheet.Cells[row, 1].Value?.ToString() ?? "0"),
                            FirstName = worksheet.Cells[row, 2].Value?.ToString(),
                            LastName = worksheet.Cells[row, 3].Value?.ToString()
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No worksheet found for authors in the Excel file.");
                }
            }
            return authors;
        }

        // Load AuthorISBN from Excel file
        static List<AuthorISBN> LoadAuthorISBNFromExcel(string filePath)
        {
            var authorISBN = new List<AuthorISBN>();
            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        authorISBN.Add(new AuthorISBN
                        {
                            AuthorID = int.Parse(worksheet.Cells[row, 1].Value?.ToString() ?? "0"),
                            ISBN = worksheet.Cells[row, 2].Value?.ToString()
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No worksheet found for AuthorISBN in the Excel file.");
                }
            }
            return authorISBN;
        }


        // 1. Get a list of all the titles and the authors who wrote them. Sort the results by title. [2 marks]
        static void Question2_1(List<Title> titles, List<Author> authors, List<AuthorISBN> authorISBN)
        {
            var result = from title in titles
                         join ai in authorISBN on title.ISBN equals ai.ISBN
                         join author in authors on ai.AuthorID equals author.AuthorID
                         orderby title.TitleName
                         select new
                         {
                             Title = title.TitleName,
                             Author = author.FirstName + " " + author.LastName
                         };

            Console.WriteLine("1. Titles and authors sorted by title:");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Title} - {item.Author}");
            }
        }

        // 2. Get a list of all the titles and the authors who wrote them. Sort the results by title. Each title sorts the authors alphabetically by last name, then first name [4 marks]
        static void Question2_2(List<Title> titles, List<Author> authors, List<AuthorISBN> authorISBN)
        {
            var result = from title in titles
                         join ai in authorISBN on title.ISBN equals ai.ISBN
                         join author in authors on ai.AuthorID equals author.AuthorID
                         orderby title.TitleName, author.LastName, author.FirstName
                         select new
                         {
                             Title = title.TitleName,
                             Author = author.FirstName + " " + author.LastName
                         };

            Console.WriteLine("\n2. Titles and authors sorted by title, then by authors:");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Title} - {item.Author}");
            }
        }

        // 3. Get a list of all the authors grouped by title, sorted by title; for a given title sort the author names alphabetically by last name then first name [4 marks]
        static void Question2_3(List<Title> titles, List<Author> authors, List<AuthorISBN> authorISBN)
        {
            var result = from title in titles
                         join ai in authorISBN on title.ISBN equals ai.ISBN
                         join author in authors on ai.AuthorID equals author.AuthorID
                         orderby title.TitleName, author.LastName, author.FirstName
                         group author by title.TitleName into grp
                         select new
                         {
                             Title = grp.Key,
                             Authors = grp.Select(a => a.FirstName + " " + a.LastName).OrderBy(a => a)
                         };

            Console.WriteLine("\n3. Authors grouped by title, sorted by title, and by authors:");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Title}:");
                foreach (var author in item.Authors)
                {
                    Console.WriteLine($"- {author}");
                }
            }
        }
    }

    // Define classes for entities
    public class Title
    {
        public string ISBN { get; set; }
        public string TitleName { get; set; }
        public int EditionNumber { get; set; }
        public int Copyright { get; set; }
    }

    public class Author
    {
        public int AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AuthorISBN
    {
        public int AuthorID { get; set; }
        public string ISBN { get; set; }
    }
}
