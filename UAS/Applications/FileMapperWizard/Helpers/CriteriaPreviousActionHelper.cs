using System;
using System.Windows;
using System.Windows.Controls;
using Core_AMS.Utilities;

namespace FileMapperWizard.Helpers
{
    public class CriteriaPreviousActionHelper
    {
        private const string AskToLeave = "You have not saved.  Are you sure you want to leave?";

        private readonly string _userMessage;
        private readonly Action _previousAction;
        private readonly RadioButton _yesRadioButton;
        private readonly RadioButton _noRadioButton;

        public CriteriaPreviousActionHelper(
            string userMessage,
            RadioButton yesRadioButton,
            RadioButton noRadioButton,
            Action previousAction)
        {
            _userMessage = userMessage;
            _yesRadioButton = yesRadioButton;
            _noRadioButton = noRadioButton;
            _previousAction = previousAction;
        }

        public void SetYesRadioButtonChecked()
        {
            if (_yesRadioButton != null)
            {
                _yesRadioButton.IsChecked = true;
            }
        }

        public void SetNoRadioButtonChecked()
        {
            if (_noRadioButton != null)
            {
                _noRadioButton.IsChecked = true;
            }
        }

        public void CallPreviousAction()
        {
            _previousAction?.Invoke();
        }
        
        public bool CallPreviousAction(bool isSaved, bool isYesNo)
        {
            if (!isYesNo)
            {
                var messageBoxResult = WPF.MessageResult(_userMessage, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SetYesRadioButtonChecked();
                }
                else
                {
                    SetNoRadioButtonChecked();
                    return true;
                }
            }
            else if (!isSaved)
            {
                var messageBoxResult = WPF.MessageResult(AskToLeave, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    CallPreviousAction();
                }
            }
            else
            {
                CallPreviousAction();
            }

            return false;
        }
    }
}
