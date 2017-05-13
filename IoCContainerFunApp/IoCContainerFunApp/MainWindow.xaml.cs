using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace IoCContainerFunApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private ObservableCollection<Type> _parts = new ObservableCollection<Type>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Type> Parts
        {
            get { return _parts; }
            set
            {
                _parts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Parts"));
            }
        }

        public void InjectParts(IEnumerable<Type> parts)
        {
            Dispatcher.InvokeAsync(() => Parts = new ObservableCollection<Type>(parts));
        }
    }
}
