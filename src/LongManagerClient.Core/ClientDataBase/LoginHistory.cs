using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    [Table("LoginHistory")]
    public class LoginHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), NonSerialized]
        public int _id;

        public int ID
        {
            get { return _id; }
            set { value = _id; }
        }

        public string RowGuid { get; set; }
        public string LoginDisplayName { get; set; }
        public string LoginUserName { get; set; }
        public string LoginDate { get; set; }
        public int IsPush { get; set; }
    }
}
