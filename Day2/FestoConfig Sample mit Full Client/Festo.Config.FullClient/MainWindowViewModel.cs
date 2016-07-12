using Festo.Config.Api.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Festo.Config.FullClient
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Motor> motors;

        public MainWindowViewModel()
        {
            this.RefreshCommand = new DelegateCommand(
                async () => await this.DoRefreshAsync(),
                () => true,
                this);
        }

        private class ODataResult<T>
        {
            public IEnumerable<T> value { get; set; }
        }

        private async Task DoRefreshAsync()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(new Uri("http://localhost:1234/odata/Motor"));
                result.EnsureSuccessStatusCode();
                var motors = await result.Content.ReadAsAsync<ODataResult<Motor>>();
                this.Motors.Clear();
                foreach (var m in motors.value)
                {
                    this.Motors.Add(m);
                }
            }
        }

        public ObservableCollection<Motor> Motors { get; } = new ObservableCollection<Motor>();

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RefreshCommand { get; }
    }
}
