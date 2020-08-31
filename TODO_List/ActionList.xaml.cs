using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TODO_List
{
    /// <summary>
    /// Логика взаимодействия для ActionList.xaml
    /// </summary>
    public partial class ActionList : UserControl
    {
        public ActionList()
        {
            InitializeComponent();
        }

        //Поле для проверки сортировки ВСЕЙ панели
        public bool actionsSorting = false;

        //Лист со всеми действиями для организации фильтрации
        public List<Action> allActions = new List<Action>();

        private void ShowElements()
        {
            SelectAllImage.Visibility = Visibility.Visible;
            ListStackPanel.Visibility = Visibility.Visible;
            CountActionsLabel.Visibility = Visibility.Visible;
            SelectActiveRadioButton.Visibility = Visibility.Visible;
            SelectAllRadioButton.Visibility = Visibility.Visible;
            SelectCompletedRadioButton.Visibility = Visibility.Visible;
        }

        public void HideElements()
        {
            SelectAllImage.Visibility = Visibility.Hidden;
            ListStackPanel.Visibility = Visibility.Hidden;
            CountActionsLabel.Visibility = Visibility.Hidden;
            SelectActiveRadioButton.Visibility = Visibility.Hidden;
            SelectAllRadioButton.Visibility = Visibility.Hidden;
            SelectCompletedRadioButton.Visibility = Visibility.Hidden;
        }

        public int countActions = 0;
        public int countCompletedActions = 0;

        private void ActionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ActionTextBox.Text.Length != 0)
            {
                Action newAction = new Action(ActionTextBox.Text);
                ListStackPanel.Children.Add(newAction);
                ActionTextBox.Text = "";

                allActions.Add(newAction);

                ShowElements();

                countActions++;
                CountActionsLabel.Content = "Actions left:" + (countActions - countCompletedActions);
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HideElements();
        }

        private void SelectAllImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Вот место, где происходит сортировка ВСЕЙ панели

            actionsSorting = true;

            foreach (Action action in ListStackPanel.Children)
            {
                action.IsDone.IsChecked = true;
            }

            actionsSorting = false;

            //Так как во время цилка в классе Action из-за actionSorting == true фильтрации не происходит, то 
            //её нужно запустить после цикла вручную
            FilterRadioButton_Checked(new RadioButton(), e);
        }

        public void FilterRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //Проверка на наличия хотя бы одного действия
            if(allActions.Count == 0)
            {
                return;
            }

            ListStackPanel.Children.Clear();

            //Фильтрация панели исходя из активной RadioButton

            if (SelectAllRadioButton.IsChecked == true)
            {
                foreach (Action action in allActions)
                {
                    ListStackPanel.Children.Add(action);
                }
            }
            else if (SelectActiveRadioButton.IsChecked == true)
            {
                foreach (Action action in allActions)
                {
                    if (action.IsDone.IsChecked == false)
                    {
                        ListStackPanel.Children.Add(action);
                    }
                }
            }
            else
            {
                foreach (Action action in allActions)
                {
                    if (action.IsDone.IsChecked == true)
                    {
                        ListStackPanel.Children.Add(action);
                    }
                }
            }
        }
    }
}
