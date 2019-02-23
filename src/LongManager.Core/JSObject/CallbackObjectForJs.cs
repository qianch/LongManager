﻿using LongManager.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LongManager.Core.JSObject
{
    public class CallbackObjectForJs
    {
        private LongDbContext _longDBContext = new LongDbContext();
        public void showMsg(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void saveAddress(string mailNO, string address)
        {
            var mail = _longDBContext.Mails.Where(x => x.MailNO == mailNO).FirstOrDefault();
            if (mail != null && string.IsNullOrEmpty(mail.Address))
            {
                mail.Address = address;
                _longDBContext.Update(mail);
                _longDBContext.SaveChanges();
            }
        }
    }
}
