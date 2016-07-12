using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Festo.Config.Api.Model
{
    public class Motor : INotifyPropertyChanged
    {
        private int id;
        [JsonProperty(PropertyName = "id")]
        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public string Description { get; set; }

        public float MaxCurrent { get; set; }

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
