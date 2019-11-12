using LongManagerClient.Core.ClientDataBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace LongManagerClient.Core
{
    public class OutExcelHandle
    {
        private readonly string _fileName = "";
        private LongClientDbContext _longDBContext = new LongClientDbContext();
        private int _mailNOIndex = 0; //邮件号索引
        private int _orgNameIndex = 0; //寄达局索引
        private int _postDateIndex = 0; //收寄时间索引
        public OutExcelHandle(string fileName)
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
                        case "邮件号":
                            _mailNOIndex = i;
                            break;
                        case "寄达局":
                            _orgNameIndex = i;
                            break;
                        case "收寄时间":
                            _postDateIndex = i;
                            break;
                        default:
                            break;
                    }
                }

                if (_mailNOIndex == 0 || _orgNameIndex == 0)
                {
                    var warn = $"邮件号：{_mailNOIndex},寄达局：{_orgNameIndex},收寄时间：{_postDateIndex}";
                    MessageBox.Show(warn, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                for (int i = minRow + 1; i <= maxRow; i++)
                {
                    var mailNO = worksheet.Cells[i, _mailNOIndex].GetValue<string>();
                    var orgName = worksheet.Cells[i, _orgNameIndex].GetValue<string>();
                    var postDate = worksheet.Cells[i, _postDateIndex].GetValue<string>();
                    var count = _longDBContext.OutInfo.Where(x => x.MailNO == mailNO).Count();
                    if (!string.IsNullOrEmpty(mailNO) && count == 0)
                    {
                        var outInfo = new OutInfo
                        {
                            RowGuid = Guid.NewGuid().ToString(),
                            MailNO = mailNO,
                            AddDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            OrgName = orgName,
                            PostDate = postDate
                        };
                        _longDBContext.Add(outInfo);
                    }
                }

                _longDBContext.SaveChanges();
                MessageBox.Show("数据导入完成", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}
