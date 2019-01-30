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
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Birthday { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
