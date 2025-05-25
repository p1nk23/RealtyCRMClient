using RealtyCRMClient.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealtyCRMClient.Models
{
    public class ClientListItem : INotifyPropertyChanged
    {
        private long _id;
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _number;
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private int _status;
        public int Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusText));
                // Отправьте обновление на сервер
                //(Application.Current.MainWindow.DataContext as MainViewModel)?.UpdateClientStatus(Id, Status); ///////sdfgsdfgsdf/gsdfg/sdf/gsdfg
            }
        }

        private string _statusText;
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case 0: return "Не активен";
                    case 1: return "Активен";
                    default: return "Неизвестный статус";
                }
            }
        }

        private int? _cardObjId { get; set; }
        public int? CardObjId
        {
            get => _cardObjId;
            set
            {
                _cardObjId = value;
                OnPropertyChanged();
            }

        }

        private long? _taskObjId { get; set; }
        public long? TaskObjId
        {
            get => _taskObjId;
            set
            {
                _taskObjId = value;
                OnPropertyChanged();
            }

        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
