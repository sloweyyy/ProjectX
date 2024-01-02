using System;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectX.Views
{
    public class User : INotifyPropertyChanged
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string useraccountname { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        private string _email;
        public string email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(email));
                }
            }
        }


        private string _zaloapi;
        public string zaloapi
        {
            get { return _zaloapi; }
            set
            {
                if (_zaloapi != value)
                {
                    _zaloapi = value;
                    OnPropertyChanged(nameof(zaloapi));
                }
            }
        }

        private string _fptapi;
        public string fptapi
        {
            get { return _fptapi; }
            set
            {
                if (_fptapi != value)
                {
                    _fptapi = value;
                    OnPropertyChanged(nameof(fptapi));
                }
            }
        }

        public bool __v { get; set; }
        public DateTime last_used_at { get; set; }
        public DateTime created_at { get; set; }
        public bool premium { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}