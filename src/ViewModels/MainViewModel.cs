using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BrowserChoice
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private RelayCommand registerCommand;
        private RelayCommand saveRulesCommand;
        private RelayCommand<Rule> deleteRuleCommand;
        private RelayCommand newRuleCommand;
        private ObservableCollection<Rule> rules;

        public MainViewModel()
        {
            rules = new ObservableCollection<Rule>(BrowserChoice.Rules.Instance.List);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        
        public ObservableCollection<Rule> Rules => rules;
        public String Title => "Rodkulman's Browser Choice Settings";

        public RelayCommand RegisterCommand => registerCommand ?? (registerCommand = new RelayCommand(RegistryHelper.RegisterApplication));
        public RelayCommand SaveRulesCommand => saveRulesCommand ?? (saveRulesCommand = new RelayCommand(SaveRules));
        public RelayCommand NewRuleCommand => newRuleCommand ?? (newRuleCommand = new RelayCommand(NewRule));
        public RelayCommand<Rule> DeleteRuleCommand => deleteRuleCommand ?? (deleteRuleCommand = new RelayCommand<Rule>(DeleteRule));

        private void NewRule()
        {
            rules.Add(new Rule()
            {
                SearchType = SearchType.Domain
            });
        }        

        private void DeleteRule(Rule rule)
        {
            rules.Remove(rule);
        }

        private void SaveRules()
        {
            BrowserChoice.Rules.Instance.Save(rules);
        }
    }
}