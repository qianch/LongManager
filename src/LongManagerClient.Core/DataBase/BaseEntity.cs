using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LongManagerClient.Core.DataBase
{
    public class BaseEntity : INotifyPropertyChanged
    {
        public int _id;
        public string _rowGuid;
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
            set { _rowGuid = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
