﻿using LongManagerClient.Core.ClientDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LongManagerClient.Core.CEF.JSObject
{
    public class CallbackObjectForJs
    {
        private LongClientDbContext _longDBContext = new LongClientDbContext();
        public void showMsg(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void saveOutAddress(string mailNO, string address)
        {
            var mail = _longDBContext.OutInfo.Where(x => x.MailNO == mailNO).FirstOrDefault();
            if (mail != null && string.IsNullOrEmpty(mail.Address))
            {
                mail.Address = address;
                _longDBContext.Update(mail);
                _longDBContext.SaveChanges();
            }
        }
    }
}
