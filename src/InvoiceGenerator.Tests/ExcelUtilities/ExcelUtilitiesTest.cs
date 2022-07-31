using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace InvoiceGenerator.Tests
{
    [TestClass]
    public class ExcelUtilitiesTest
    {
        /// <summary>
        /// Name of workbook.
        /// </summary>
        private const string TempFileName = "test2.xlsx";

        /// <summary>
        /// Name of excel sheet.
        /// </summary>
        private const string SheetName = "Sheet2";

        /// <summary>
        /// Workbook object used in tests.
        /// </summary>
        private XSSFWorkbook wb;

        /// <summary>
        /// Excel utilities.
        /// </summary>
        private readonly ExcelUtilities utilities = new ExcelUtilities(
                new Mock<ILogger<ExcelUtilities>>().Object);

        [TestMethod]
        public void ReadAndWriteNumberTest()
        {
            wb = new XSSFWorkbook();
            ISheet sheet = wb.CreateSheet(SheetName);
            double value = DateTime.UtcNow.Ticks / 100.0;
            utilities.SetCellValue(sheet, 10, 5, value);
            Assert.IsTrue(value == utilities.GetNumericalCellValue(sheet, 10, 5));
        }

        [TestMethod]
        public void ReadAndWriteStringTest()
        {
            wb = new XSSFWorkbook();
            ISheet sheet = wb.CreateSheet(SheetName);
            string value = DateTime.UtcNow.DayOfWeek.ToString();
            utilities.SetCellValue(sheet, 10, 5, value);
            Assert.IsTrue(value == utilities.GetStringCellValue(sheet, 10, 5));
        }

        [TestMethod]
        public void SaveAndReadFileTest()
        {
            wb = new XSSFWorkbook();
            ISheet sheet = (XSSFSheet)wb.CreateSheet(SheetName);
            double value = DateTime.UtcNow.Ticks / 100.0;
            utilities.SetCellValue(sheet, 10, 5, value);
            utilities.SaveAndCloseExcelFile(wb, TempFileName);

            wb = utilities.OpenExcelWorkbook(TempFileName);
            sheet = wb.GetSheet(SheetName);
            Assert.IsTrue(value == utilities.GetNumericalCellValue(sheet, 10, 5));
        }

        [TestMethod]
        public void ExcelFileParserTest()
        {
            List<ParserTestContract> items = new List<ParserTestContract>
            {
                new ParserTestContract()
                {
                    Name = "John",
                    Age = 21
                },
                new ParserTestContract()
                {
                    Name = "Jim",
                    Age = 54
                }
            };

            items.Sort();

            wb = new XSSFWorkbook();
            ISheet sheet = (XSSFSheet)wb.CreateSheet(SheetName);
            int rowIndex = 0;
            utilities.SetCellValue(sheet, rowIndex, 0, nameof(ParserTestContract.Name));
            utilities.SetCellValue(sheet, rowIndex, 1, nameof(ParserTestContract.Age));

            items.ForEach(item =>
            {
                rowIndex++; 
                utilities.SetCellValue(sheet, rowIndex, 0, item.Name);
                utilities.SetCellValue(sheet, rowIndex, 1, item.Age);
            });

            utilities.SaveAndCloseExcelFile(wb, TempFileName);

            var parsedItems = utilities.ParseExcelFile(
                TempFileName,
                (IRow row) =>
                {
                    return new ParserTestContract()
                    {
                        Name = utilities.GetStringCellValue(row, 0),
                        Age = utilities.GetNumericalCellValue(row, 1)
                    };
                });

            parsedItems.Sort();
            Assert.IsTrue(items.Count == parsedItems.Count, "Invalid number of items.");
            for (int i = 0; i < parsedItems.Count; i++)
            {
                Assert.IsTrue(parsedItems[i].Equals(items[i]));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            wb?.Close();
            if (File.Exists(TempFileName))
            {
                File.Delete(TempFileName);
            }
        }
    }
}