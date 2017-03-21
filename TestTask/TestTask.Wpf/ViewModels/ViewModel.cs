using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TestTask.Wpf.Entities;
using TestTask.Wpf.Models;

namespace TestTask.Wpf.ViewModels
{
    public class ViewModel : ViewModelBase
    {
        private IModel model;

        private string selectedCity;
        private DateTime? selectedDate;
        private Possibility selectedPossibility;
        private string infoMessage;
        private string userNameText;

        public ObservableCollection<MeteringRecord> MeteringRecords { get; set; }
        public ICollection<Possibility> Possibilitys { get; set; }
        public ICollection<string> PossibilitysCity { get; set; }
        public ObservableCollection<Possibility> SelectedPossibilitys { get; set; }
        public MeteringRecord SelectedRecord { get; set; }
        public string UserNameText
        {
            get
            {
                return userNameText;
            }
            set
            {
                userNameText = value;
                var res = model.GetByUserPartName(userNameText);
                MeteringRecords.Clear();
                foreach (var item in res)
                {
                    MeteringRecords.Add(item);
                }
            }
        }
        public DateTime? SelectedDate
        {
            get
            {
                return selectedDate;
            }
            set
            {
                selectedDate = value;
                selectedPossibility = SelectedPossibilitys.FirstOrDefault(x => x.Date.Date == selectedDate.Value.Date);
                RaisePropertyChanged(() => SelectedDate);
                RaisePropertyChanged(() => SelectedPossibility);
            }
        }
        public Possibility SelectedPossibility
        {
            get
            {
                return selectedPossibility;
            }
            set
            {
                selectedPossibility = value;
                selectedDate = SelectedPossibility?.Date;
                RaisePropertyChanged(() => SelectedDate);
                RaisePropertyChanged(() => SelectedPossibility);
            }
        }
        public string SelectedCity
        {
            get
            {
                return selectedCity;
            }
            set
            {
                selectedCity = value;
                //TODO Заменить это убожество!!!
                LoadSelectedPossibilitys();
                //var res = model.GetByCity(selectedCity);
                //SelectedPossibilitys.Clear();
                //foreach(var item in res)
                //{
                //    SelectedPossibilitys.Add(item);
                //}
                //SelectedPossibilitys = new ObservableCollection<Possibility>(model.GetByCity(selectedCity));
            }
        }
        public string InfoMessage
        {
            get
            {
                return infoMessage;
            }
            set
            {
                infoMessage = value;
                RaisePropertyChanged(() => InfoMessage);
            }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand LoadWithoutDateCommand { get; private set; }
        public ICommand LoadAllRecordsCommand { get; private set; }

        public ViewModel()
        {
            model = new Model();
            MeteringRecords = new ObservableCollection<MeteringRecord>(model.GetAllRecords());
            Possibilitys = model.GetAllPossibilitys();
            PossibilitysCity = Possibilitys.Select(x => x.City).Distinct().ToList();
            SelectedPossibilitys = new ObservableCollection<Possibility>();
            SaveCommand = new RelayCommand(Save);
            LoadWithoutDateCommand = new RelayCommand(LoadRecordsWithoutDate);
            LoadAllRecordsCommand = new RelayCommand(LoadSelectedRecords);
        }

        private void Save()
        {
            var result = model.SetDate(SelectedRecord?.Number, SelectedDate, SelectedCity);
            switch (result)
            {
                case ESetDateResult.Ok:
                    InfoMessage = "Запись произведена";
                    LoadSelectedPossibilitys();
                    LoadSelectedRecords();
                    break;
                case ESetDateResult.DateIsNull:
                    InfoMessage = "Выберите дату";
                    break;
                case ESetDateResult.NumberIsNull:
                    InfoMessage = "Выберите запись";
                    break;
                case ESetDateResult.NotFoundRecord:
                    InfoMessage = "Запись с таким номером не найдена";
                    break;
                case ESetDateResult.NotFoundPossibility:
                    InfoMessage = string.Format("В городе {0} на {1} запись не производится", SelectedCity, SelectedDate.Value.Date.ToShortDateString());
                    break;
                case ESetDateResult.LimitIsOver:
                    InfoMessage = string.Format("В городе {0} на {1} лимит исчерпан", SelectedCity, SelectedDate.Value.Date.ToShortDateString());
                    break;
                default:
                    break;
            }
        }

        private void LoadSelectedPossibilitys()
        {
            var res = model.GetByCity(selectedCity);
            SelectedPossibilitys.Clear();
            foreach (var item in res)
            {
                SelectedPossibilitys.Add(item);
            }
        }

        private void LoadSelectedRecords()
        {
            var res = model.GetAllRecords();
            MeteringRecords.Clear();
            foreach (var item in res)
            {
                MeteringRecords.Add(item);
            }
        }

        private void LoadRecordsWithoutDate()
        {
            var res = model.GetWithoutDate();
            MeteringRecords.Clear();
            foreach (var item in res)
            {
                MeteringRecords.Add(item);
            }
        }
    }
}
