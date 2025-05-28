using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RealtyCRMClient.Models
{
    public class TaskListItem : INotifyPropertyChanged
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

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private int? _status;
        public int? Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private string _statusText;
        public string StatusText
        {
            get
            {
                switch (_status)
                {
                    case 0:
                        return "Очередь";
                    case 1:
                        return "В работе";
                    case 2:
                        return "Ожидание ответа";
                    case 3:
                        return "Готово";
                    default:
                        return "Очередь";
                }
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

        private long? _personalId;
        public long? PersonalId
        {
            get => _personalId;
            set
            {
                _personalId = value;
                OnPropertyChanged();
            }
        }

        private string _personalName;
        public string PersonalName
        {
            get => _personalName;
            set
            {
                _personalName = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
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