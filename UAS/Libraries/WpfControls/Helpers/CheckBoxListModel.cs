using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace WpfControls.Helpers
{
    public class CheckBoxListModel : ViewModelBase
    {
        private string checkedItemsString;
        public CheckBoxListModel(ObservableCollection<CheckBoxItem> itemList)
        {
            this.ItemsSource = itemList;
            this.GetCheckedItemsCommand = new DelegateCommand(OnGetCheckedItemsCommandExecute);
        }
        public CheckBoxListModel()
        {
            ObservableCollection<CheckBoxItem> itemList = new ObservableCollection<CheckBoxItem>();
            this.ItemsSource = itemList;
            this.GetCheckedItemsCommand = new DelegateCommand(OnGetCheckedItemsCommandExecute);
        }
        private void OnGetCheckedItemsCommandExecute(object obj)
        {
            var sb = new StringBuilder();
            var checkedItems = this.ItemsSource.Where(i => i.IsChecked);
            foreach (CheckBoxItem item in checkedItems)
            {
                sb.AppendLine(item.Name);
            }

            this.CheckedItemsString = sb.ToString();
        }

        public ObservableCollection<CheckBoxItem> ItemsSource { get; set; }
        public ICommand GetCheckedItemsCommand { get; set; }

        public string CheckedItemsString
        {
            get
            {
                return this.checkedItemsString;
            }

            set
            {
                if (this.checkedItemsString != value)
                {
                    this.checkedItemsString = value;
                    this.OnPropertyChanged(() => this.CheckedItemsString);
                }
            }
        }
    }
}
