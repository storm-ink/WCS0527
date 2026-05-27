using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Wcs.App.Plugins.Reports
{
    public static class ExportExcelHelper
    {
        const string excel_xml_temp = @"<?xml version=""1.0""?>
<?mso-application progid=""Excel.Sheet""?>
<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""
 xmlns:o=""urn:schemas-microsoft-com:office:office""
 xmlns:x=""urn:schemas-microsoft-com:office:excel""
 xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""
 xmlns:html=""http://www.w3.org/TR/REC-html40"">
 <DocumentProperties xmlns=""urn:schemas-microsoft-com:office:office"">
  <Author>paopao</Author>
  <LastAuthor>paopao</LastAuthor>
  <Created>2014-03-26T00:54:41Z</Created>
  <Company>Microsoft</Company>
  <Version>14.00</Version>
 </DocumentProperties>
 <OfficeDocumentSettings xmlns=""urn:schemas-microsoft-com:office:office"">
  <AllowPNG/>
 </OfficeDocumentSettings>
 <ExcelWorkbook xmlns=""urn:schemas-microsoft-com:office:excel"">
  <WindowHeight>9405</WindowHeight>
  <WindowWidth>20475</WindowWidth>
  <WindowTopX>600</WindowTopX>
  <WindowTopY>90</WindowTopY>
  <ProtectStructure>False</ProtectStructure>
  <ProtectWindows>False</ProtectWindows>
 </ExcelWorkbook>
 <Styles>
  <Style ss:ID=""Default"" ss:Name=""Normal"">
   <Alignment ss:Vertical=""Center""/>
   <Borders/>
   <Font ss:FontName=""宋体"" x:CharSet=""134"" ss:Size=""11"" ss:Color=""#000000""/>
   <Interior/>
   <NumberFormat/>
   <Protection/>
  </Style>
 </Styles>
</Workbook>";

       static XmlDocument CreateDocument(out XmlNamespaceManager nsp)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(excel_xml_temp);
            nsp = new XmlNamespaceManager(doc.NameTable);
            nsp.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsp.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");
            nsp.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            nsp.AddNamespace("html", "http://www.w3.org/TR/REC-html40");
            return doc;
        }

       static void FillSheet(XmlNamespaceManager nsp,XmlElement sheetElement, DataGridView dataGridView)
       {
           var doc = sheetElement.OwnerDocument;

           var tableElement = sheetElement.SelectSingleNode("ss:Table", nsp);
           var ssNamespace = "urn:schemas-microsoft-com:office:spreadsheet";
           var columnsRowElement = doc.CreateElement("Row", ssNamespace);
           foreach (DataGridViewColumn column in dataGridView.Columns)
           {
               var colElement = doc.CreateElement("Cell", ssNamespace);
               var dataElement = doc.CreateElement("Data", ssNamespace);
               var dataTypeAttribute = doc.CreateAttribute("ss", "Type", ssNamespace);
               dataTypeAttribute.Value = "String";
               dataElement.Attributes.Append(dataTypeAttribute);
               dataElement.InnerText = column.HeaderText;
               colElement.AppendChild(dataElement);
               columnsRowElement.AppendChild(colElement);
           }
           tableElement.AppendChild(columnsRowElement);

           foreach (DataGridViewRow valueRow in dataGridView.Rows)
           {
               var rowElement = doc.CreateElement("Row", ssNamespace);
               foreach (DataGridViewCell cell in valueRow.Cells)
               {
                   var cellElement = doc.CreateElement("Cell", ssNamespace);
                   var dataElement = doc.CreateElement("Data", ssNamespace);
                   var dataTypeAttribute = doc.CreateAttribute("ss", "Type", ssNamespace);
                   dataTypeAttribute.Value = "String";
                   dataElement.Attributes.Append(dataTypeAttribute);
                   dataElement.InnerText = Convert.ToString(cell.Value) + "";
                   cellElement.AppendChild(dataElement);
                   rowElement.AppendChild(cellElement);
               }

               tableElement.AppendChild(rowElement);
           }
       }

       static XmlElement AddSheet(XmlDocument doc,XmlNamespaceManager nsp, String newSheetName = "Sheet1")
       {
           var tableElement = doc.SelectSingleNode("ss:Workbook/ss:Worksheet[@ss:Name='" + newSheetName + "']", nsp);
           if (tableElement != null)
           {
               throw new InvalidOperationException(string.Format("已存在名为“{0}”的工作表", newSheetName));
           }

           var sheetNamespace = "urn:schemas-microsoft-com:office:spreadsheet";
           var newElement = doc.CreateElement("Worksheet", sheetNamespace);
           var newElementNameAttribute = doc.CreateAttribute("ss", "Name", sheetNamespace);
           newElementNameAttribute.Value = newSheetName;
           newElement.Attributes.Append(newElementNameAttribute);

           tableElement = doc.CreateElement("Table", sheetNamespace);
           var expandedColumnCountAttribute = doc.CreateAttribute("ss", "ExpandedColumnCount", sheetNamespace);
           expandedColumnCountAttribute.Value = "65535";
           tableElement.Attributes.Append(expandedColumnCountAttribute);
           var expandedRowCountAttribute = doc.CreateAttribute("ss", "ExpandedRowCount", sheetNamespace);
           expandedRowCountAttribute.Value = "65535";
           tableElement.Attributes.Append(expandedRowCountAttribute);
           var fullColumns = doc.CreateAttribute("ss", "FullColumns", sheetNamespace);
           fullColumns.Value = "1";
           tableElement.Attributes.Append(fullColumns);
           var fullRows = doc.CreateAttribute("ss", "FullRows", sheetNamespace);
           fullRows.Value = "1";
           tableElement.Attributes.Append(fullColumns);
           var defaultColumnWidth = doc.CreateAttribute("ss","DefaultColumnWidth", sheetNamespace);
           defaultColumnWidth.Value = "54";
           tableElement.Attributes.Append(defaultColumnWidth);
           var defaultRowHeight = doc.CreateAttribute("ss","DefaultRowHeight", sheetNamespace);
           defaultRowHeight.Value = "13.5";
           tableElement.Attributes.Append(defaultColumnWidth);
           newElement.AppendChild(tableElement);

           var WorksheetOptionsNamespace = "urn:schemas-microsoft-com:office:excel";
           var worksheetOptions = doc.CreateElement("WorksheetOptions", WorksheetOptionsNamespace);
           var pageSetup = doc.CreateElement("PageSetup", WorksheetOptionsNamespace);
           var header = doc.CreateElement("Header", WorksheetOptionsNamespace);
           var header_Margin = doc.CreateAttribute("x", "Margin", WorksheetOptionsNamespace);
           header_Margin.Value = "0.3";
           header.Attributes.Append(header_Margin);
           pageSetup.AppendChild(header);
           var footer = doc.CreateElement("Footer", WorksheetOptionsNamespace);
           var footer_Margin = doc.CreateAttribute("x", "Margin", WorksheetOptionsNamespace);
           footer_Margin.Value = "0.3";
           footer.Attributes.Append(footer_Margin);
           pageSetup.AppendChild(footer);
           worksheetOptions.AppendChild(pageSetup);
           var protectObjects = doc.CreateElement("ProtectObjects");
           protectObjects.InnerText = "False";
           worksheetOptions.AppendChild(protectObjects);
           var protectScenarios = doc.CreateElement("ProtectScenarios");
           protectScenarios.InnerText = "False";
           worksheetOptions.AppendChild(protectScenarios);
           newElement.AppendChild(worksheetOptions);

           doc.SelectSingleNode("ss:Workbook", nsp).AppendChild(newElement);

           return newElement;
       }

        /// <summary>
        /// 导出当前显示的内容为 excel 格式
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void ExportAsExcel(this DataGridView dataGridView,string fileName)
        {
            XmlNamespaceManager nsp;
            var doc = CreateDocument(out nsp);
            var sheet = AddSheet(doc, nsp,"Sheet1");
            FillSheet(nsp, sheet, dataGridView);
            doc.Save(fileName);
        }

        /// <summary>
        /// 将指定的所有 DataGridView 导出为 excel 格式
        /// </summary>
        /// <param name="dataGridViews">Key 为工作表名称,Value 为 DataGridView</param>
        /// <param name="fileName"></param>
        public static void ExportAsExcel(this Dictionary<String, DataGridView> dataGridViews, string fileName)
        {
            XmlNamespaceManager nsp;
            var doc = CreateDocument(out nsp);
            foreach (var item in dataGridViews)
            {
                var sheet = AddSheet(doc,nsp, item.Key);
                FillSheet(nsp, sheet, item.Value);                
            }
            doc.Save(fileName);
        }

    }
}
