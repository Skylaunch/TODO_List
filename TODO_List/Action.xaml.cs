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
    /// Логика взаимодействия для Action.xaml
    /// </summary>
    public partial class Action : UserControl
    {
        private StackPanel myStackPanel;
        private Grid myGrid;
        private ActionList myActionList;

        //Конструктор с текстом из текстблока в качестве аргумента

        public Action(string ActionText)
        {
            InitializeComponent();
            ActionDescription.Text = ActionText;

            DeleteActionImage.Visibility = Visibility.Hidden;
        }

        private void DeleteActionImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //Через пододную конструктору получаем ссылки на панель-родитель элемента и объект класса ActionList

            myStackPanel = (StackPanel)this.Parent;
            myGrid = (Grid)myStackPanel.Parent;
            myActionList = (ActionList)myGrid.Parent;

            myStackPanel.Children.Remove(this);
            myActionList.allActions.Remove(this);

            if (myStackPanel.Children.Count == 0)
            {
                myActionList.HideElements();
            }

            if (IsDone.IsChecked == false)
            {
                myActionList.countActions--;
                myActionList.CountActionsLabel.Content = "Actions left: " +
                    (myActionList.countActions - myActionList.countCompletedActions);
            }
        }

        public void IsDone_Checked(object sender, RoutedEventArgs e)
        {
            myStackPanel = (StackPanel)this.Parent;
            myGrid = (Grid)myStackPanel.Parent;
            myActionList = (ActionList)myGrid.Parent;

            myActionList.countCompletedActions++;
            myActionList.CountActionsLabel.Content = "Actions left: " +
                (myActionList.countActions - myActionList.countCompletedActions);

            //Активация перечёркнутого текста

            ActionDescription.TextDecorations = TextDecorations.Strikethrough;

            /*Тут идет проверка на то, не сортируется ли панель. Без неё при активной SelectedActiveRadioButton 
            цикл падает из-за того, что коллекция изменяется во время его действия*/
            if (myActionList.actionsSorting == false)
            {
                myActionList.FilterRadioButton_Checked(new RadioButton(), e);
            }
        }

        private void IsDone_Unchecked(object sender, RoutedEventArgs e)
        {
            //Данная строка нужна для того, чтобы действие мгновенно уходило в нужную категорию

            myActionList.FilterRadioButton_Checked(new RadioButton(), e);

            myActionList.countCompletedActions--;
            myActionList.CountActionsLabel.Content = "Actions left: " +
                (myActionList.countActions - myActionList.countCompletedActions);

            //Деактивация перечёркнутого текста

            ActionDescription.TextDecorations = null;
        }

        // Показ/Скрытие кнопки удаления при наведении на действие

        private void ActionGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            DeleteActionImage.Visibility = Visibility.Visible;
        }

        private void ActionGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            DeleteActionImage.Visibility = Visibility.Hidden;
        }
    }
}
