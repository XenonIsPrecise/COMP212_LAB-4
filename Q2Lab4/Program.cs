using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string filePathOfAuthors = "/Users/sujalxenon/Downloads/Lab04 - Solution Template/Q2Lab4/obj/Debug/dbo.Authors.xlsx";
            string filePathOfTitles = "/Users/sujalxenon/Downloads/Lab04 - Solution Template/Q2Lab4/obj/Debug/dbo.Titles.xlsx";
            string filePathOfAuthorISBN = "/Users/sujalxenon/Downloads/Lab04 - Solution Template/Q2Lab4/obj/Debug/dbo.AuthorISBN.xlsx";

            // Ensure the file exists
            if (!File.Exists(filePathOfAuthors))
            {
                Console.WriteLine("File of Authors not found.");
                return;
            }
            if (!File.Exists(filePathOfTitles))
            {
                Console.WriteLine("File of Titles not found.");
                return;
                
            }
            if (!File.Exists(filePathOfAuthorISBN))
            {
                Console.WriteLine("File of Titles not found.");
                return;

            }

            var authorsData = ReadExcelFile(filePathOfAuthors);
            var booksData = ReadExcelFile(filePathOfAuthorISBN);
            var titlesData = ReadExcelFile(filePathOfTitles);

            Console.WriteLine("Hello World!");
            Console.WriteLine("Question 02 - Lab 04");

            // Invokes methods
            Question2_1();
            Question2_2();
            Question2_3();

            //1.Get a list of all the titles and the authors who wrote them. Sort the results by title. [2 marks]
            void Question2_1()
            {
                var query = titlesData
             .OrderBy(title => title["Title"])
             .Select(title =>
             {
                 var authorIDs = booksData
                     .Where(book => book["ISBN"] == title["ISBN"])
                     .Select(book => book["AuthorID"])
                     .Distinct();

                 var authors = authorsData
                     .Where(author => authorIDs.Contains(author["AuthorID"]))
                     .Select(author => $"{author["FirstName"]} {author["LastName"]}");

                 return new
                 {
                     Title = title["Title"],
                     Authors = string.Join(", ", authors)
                 };
             });

                // Display the results
                Console.WriteLine("List of all titles and the authors who wrote them (sorted by title):");
                foreach (var result in query)
                {
                    Console.WriteLine($"{result.Title}: {result.Authors}");
                }


            }

            //2.Get a list of all the titles and the authors who wrote them. Sort the results by title.  Each title sort the authors alphabetically by last name, then first name[4 marks]
            void Question2_2()
            {
                Console.WriteLine("=============================================");
                Console.WriteLine("list of all the titles and the authors who wrote them sorted by title with last name and first");

                var query = titlesData
           .OrderBy(title => title["Title"])
           .Select(title =>
           {
               var authorIDs = booksData
                   .Where(book => book["ISBN"] == title["ISBN"])
                   .Select(book => book["AuthorID"])
                   .Distinct();

               var authors = authorsData
                   .Where(author => authorIDs.Contains(author["AuthorID"]))
                   .OrderBy(author => author["LastName"])
                   .ThenBy(author => author["FirstName"])
                   .Select(author => $"{author["FirstName"]} {author["LastName"]}");

               return new
               {
                   Title = title["Title"],
                   Authors = string.Join(", ", authors)
               };
           });

                // Display the results
                Console.WriteLine("List of all titles and the authors who wrote them (sorted by title and authors):");
                foreach (var result in query)
                {
                    Console.WriteLine($"{result.Title}: {result.Authors}");
                }

            }

            //3.Get a list of all the authors grouped by title, sorted by title; for a given title sort the author names alphabetically by last name then first name.[4 marks]
            void Question2_3()
            {
                Console.WriteLine("============================");
                var query = titlesData
          .OrderBy(title => title["Title"])
          .Select(title =>
          {
              var authorIDs = booksData
                  .Where(book => book["ISBN"] == title["ISBN"])
                  .Select(book => book["AuthorID"])
                  .Distinct();

              var authors = authorsData
                  .Where(author => authorIDs.Contains(author["AuthorID"]))
                  .OrderBy(author => author["LastName"])
                  .ThenBy(author => author["FirstName"])
                  .Select(author => $"{author["FirstName"]} {author["LastName"]}");

              return new
              {
                  Title = title["Title"],
                  Authors = authors.ToList() // Convert to list to materialize the query
              };
          })
          .OrderBy(group => group.Title);

                // Display the results
                Console.WriteLine("List of all authors grouped by title, sorted by title:");
                foreach (var result in query)
                {
                    Console.WriteLine($"{result.Title}: {string.Join(", ", result.Authors)}");
                }

            }
            Console.ReadKey();
        }
        static List<Dictionary<string, string>> ReadExcelFile(string filePath)
        {
            var result = new List<Dictionary<string, string>>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet != null)
                {
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var rowValues = new Dictionary<string, string>();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            var columnName = worksheet.Cells[1, col].Value.ToString();
                            var cellValue = worksheet.Cells[row, col].Value.ToString();
                            rowValues[columnName] = cellValue;
                        }
                        result.Add(rowValues);
                    }
                }
            }

            return result;
        }

    }

}


