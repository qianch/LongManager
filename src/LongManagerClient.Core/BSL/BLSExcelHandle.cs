using LongManagerClient.Core.ClientDataBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace LongManagerClient.Core.BSL
{
    public class BLSExcelHandle
    {
        private readonly string _fileName = "";
        private LongClientDbContext _longDBContext = new LongClientDbContext();
        private int _mailNOIndex = 0; //运单号索引
        private int _consigneeIndex = 0; //收件人索引
        private int _orgNameIndex = 0; //寄达地区索引
        private int _addressIndex = 0; //详细地址索引
        public BLSExcelHandle(string fileName)
        {
            _fileName = fileName;
        }

        public void Save()
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(_fileName)))
            {
                var worksheet = package.Workbook.Worksheets[1];
                var minColumn = worksheet.Dimension.Start.Column;
                var maxColumn = worksheet.Dimension.End.Column;
                var minRow = worksheet.Dimension.Start.Row;
                var maxRow = worksheet.Dimension.End.Row;

                //查找有效列的索引位置
                for (int i = minRow; i <= maxColumn; i++)
                {
                    var title = worksheet.Cells[minRow, i].GetValue<string>();
                    switch (title)
                    {
                        case "运单号":
                            _mailNOIndex = i;
                            break;
                        case "收件人":
                            _consigneeIndex = i;
                            break;
                        case "寄达地区":
                            _orgNameIndex = i;
                            break;
                        case "详细地址":
                            _addressIndex = i;
                            break;
                        default:
                            break;
                    }
                }

                if (_mailNOIndex == 0 || _consigneeIndex == 0 || _orgNameIndex == 0 || _addressIndex == 0)
                {
                    var warn = $"运单号：{_mailNOIndex}，收件人：{_consigneeIndex},寄达地区：{_orgNameIndex}，详细地址：{_addressIndex}";
                    MessageBox.Show(warn, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                for (int i = minRow + 1; i <= maxRow; i++)
                {
                    var mailNO = worksheet.Cells[i, _mailNOIndex].GetValue<string>();
                    var consignee = worksheet.Cells[i, _consigneeIndex].GetValue<string>();
                    var orgName = worksheet.Cells[i, _orgNameIndex].GetValue<string>();
                    var address = worksheet.Cells[i, _addressIndex].GetValue<string>();
                    var count = _longDBContext.BLSInfo.Where(x => x.MailNO == mailNO).Count();
                    if (!string.IsNullOrEmpty(mailNO) && count == 0)
                    {
                        var bls = new BLSOutInfo
                        {
                            RowGuid = Guid.NewGuid().ToString(),
                            MailNO = mailNO,
                            AddDate = DateTime.Now,
                            OrgName = orgName,
                            Consignee = consignee,
                            Address = address
                        };
                        _longDBContext.Add(bls);
                    }
                }

                _longDBContext.SaveChanges();
                MessageBox.Show("倍乐生数据导入完成", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}
