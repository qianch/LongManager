using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManager.Core.DataBase
{
    [Table("FrameUser")]
    public class FrameUser : INotifyPropertyChanged
    {
        private int _id;
        private string _rowGuid;
        private string _userName;
        private string _password;
        private string _displayName;
        private string _mobile;
        private string _address;
        private string _birthday;
        [Key]
        public int ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    Notify("ID");
                }
            }
        }

        public string RowGuid
        {
            get { return _rowGuid; }
            set
            {
                if (value != RowGuid)
                {
                    _rowGuid = value;
                    Notify("RowGuid");
                }
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    Notify("UserName");
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    Notify("Password");
                }
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (value != _displayName)
                {
                    _displayName = value;
                    Notify("DisplayName");
                }
            }
        }

        public string Mobile
        {
            get { return _mobile; }
            set
            {
                if (value != _mobile)
                {
                    _mobile = value;
                    Notify("Mobile");
                }
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (value != _address)
                {
                    _address = value;
                    Notify("address");
                }
            }
        }

        public string Birthday
        {
            get { return _birthday; }
            set
            {
                if (value != _birthday)
                {
                    _birthday = value;
                    Notify("Birthday");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
