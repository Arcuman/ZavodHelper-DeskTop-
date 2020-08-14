﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;
namespace ZavodHelper
{
    public class Excel
    {
        string path = "";

        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;

        public Excel(string path, int Sheet)
        {
            this.path = Directory.GetCurrentDirectory() +"\\" +path;
            wb = excel.Workbooks.Open(this.path);
            ws = wb.Worksheets[Sheet];
        }

        public string ReadCell(int i, int j)
        {
            i++;
            j++;
            if (ws.Cells[i, j].Value2 != null)
                return ws.Cells[i, j].Value2.ToString();
            else return "";
        }
    }
}
